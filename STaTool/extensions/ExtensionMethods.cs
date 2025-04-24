using System.Text;
using ClosedXML.Excel;
using log4net;

namespace STaTool.Extensions {
    public static class ExtenstionMethods {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExtenstionMethods));

        // Save data to Txt file using IEnumerable
        public static async Task ExportToTextFileAsync<T>(
            this IEnumerable<T> data,
            List<string>? headers,
            string filePath,
            string columnSeparator = "\t",
            CancellationToken cancellationToken = default) {
            ArgumentNullException.ThrowIfNull(data);
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            try {
                // Use tab as column separator if not provided
                string separator = columnSeparator ?? "\t";

                // Ensure directory exists
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                // Check if file already contains data
                bool fileHasData = File.Exists(filePath) && new FileInfo(filePath).Length > 0;

                // Open stream of the file for writing
                await using FileStream fileStream = File.Exists(filePath)
                    ? new FileStream(
                        filePath,
                        FileMode.Append,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        FileOptions.Asynchronous) // Enable asynchronous mode
                    : new FileStream(
                        filePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        FileOptions.Asynchronous);

                await using StreamWriter streamWriter = new(fileStream, new UTF8Encoding(false));

                // Write headers only if the file is empty
                if (!fileHasData && headers != null) {
                    await streamWriter.WriteLineAsync(string.Join(separator, headers)).ConfigureAwait(false);
                }

                // Write data to file line by line
                foreach (T row in data) {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (row == null) continue;

                    var rowData = row.GetType().GetProperties()
                                     .Select(property => property.GetValue(row)?.ToString() ?? "")
                                     .ToList();

                    await streamWriter.WriteLineAsync(string.Join(separator, rowData)).ConfigureAwait(false);
                }

                // Ensure all data is written to disk before closing the stream
                await streamWriter.FlushAsync().ConfigureAwait(false);
            } catch (OperationCanceledException) {
                log.Debug("Export operation was cancelled.");
                throw;
            } catch (Exception ex) {
                log.Debug($"Failed to export to text file: {ex}");
                throw;
            }
        }

        // Store data to Excel file using IEnumerable
        public static async Task ExportToExcelFile<T>(
            this IEnumerable<T> data,
            List<string>? headers,
            string filePath) {
            // Generate Mutex name based on file path (to ensure validity)
            string mutexName = "Global\\" + filePath.Replace('\\', '_').Replace(':', '_');
            using var mutex = new Mutex(false, mutexName);

            await Task.Run(async () => {
                XLWorkbook? xLWorkbook = null;
                bool mutexAcquired = false;
                try {
                    // Try to acquire Mutex (with a timeout of 1 second)
                    mutexAcquired = mutex.WaitOne(TimeSpan.FromSeconds(1));
                    if (!mutexAcquired) {
                        log.Error("Failed to acquire file access lock.");
                        return;
                    }

                    const string sheetName = "Sheet1";

                    // 1. Initialize workbook (with file existence check)
                    xLWorkbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook();

                    // 2. Get or create worksheet
                    var worksheet = xLWorkbook.Worksheets.Contains(sheetName)
                        ? xLWorkbook.Worksheet(sheetName)
                        : xLWorkbook.Worksheets.Add(sheetName);

                    // 3. Check if the worksheet already contains data
                    int rowCount = worksheet.LastRowUsed()?.RowNumber() ?? 0;

                    // 4. Write headers only if the worksheet is empty
                    if (rowCount == 0 && headers != null) {
                        worksheet.Cell(rowCount + 1, 1).InsertData(new List<List<string>> { headers });
                        rowCount++;
                    }

                    // 5. Write data to Excel
                    var dataRows = data.Select(row => row.GetType().GetProperties()
                                                        .Select(property => property.GetValue(row)?.ToString() ?? "")
                                                        .ToList())
                                       .ToList();

                    worksheet.Cell(rowCount + 1, 1).InsertData(dataRows);

                    // 6. Save (with retry and lock mechanism)
                    await SaveWithRetryAsync(xLWorkbook, filePath, mutex);
                } catch (Exception ex) {
                    log.Error($"Export failed: {ex}");
                } finally {
                    if (mutexAcquired) {
                        mutex.ReleaseMutex();
                    }
                    xLWorkbook?.Dispose();
                }
            });
        }

        private static async Task SaveWithRetryAsync(XLWorkbook book, string path, Mutex mutex) {
            int retryCount = 0;
            while (retryCount < 5) {
                try {
                    // Reacquire Mutex before saving
                    if (!mutex.WaitOne(TimeSpan.FromMilliseconds(200))) {
                        retryCount++;
                        continue;
                    }

                    book.SaveAs(path);
                    return;
                } catch (IOException ex) when (ex.Message.Contains("in use")) // More accurate file lock detection
                  {
                    log.Debug($"File locked (attempt {retryCount + 1}): {ex.Message}");
                } finally {
                    mutex.ReleaseMutex();
                }

                await Task.Delay(200 * (retryCount + 1)); // Exponential Backoff
                retryCount++;
            }
            log.Error("Failed to save after 5 retries.");
        }
    }
}
