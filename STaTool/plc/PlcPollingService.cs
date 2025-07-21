using log4net;

namespace STaTool.plc {

    public class PlcPollingService {
        private ILog log = LogManager.GetLogger(typeof(PlcPollingService));
        private readonly Fx5uModbusClient _client;
        private bool _lastHeartbeatValue; // 记录上一次心跳值

        public PlcPollingService(Fx5uModbusClient client) {
            _client = client;
            _lastHeartbeatValue = false; // 初始心跳值为0
        }

        /// <summary>
        /// 异步死循环读取 PLC 数据并发送心跳信号
        /// </summary>
        public async Task StartPollingAsync(
            int pollingIntervalMs,
            int targetAddress,
            int heartBeatAddress,
            Action<bool> setIsTargetReached,
            CancellationToken token) {
            try {
                while (!token.IsCancellationRequested) {
                    // ===== 心跳逻辑 =====
                    try {
                        // 交替写入 1 和 0 (true/false)
                        _lastHeartbeatValue = !_lastHeartbeatValue;
                        _client.WriteCoil(heartBeatAddress, _lastHeartbeatValue);

                        log.Debug($"发送心跳信号: {heartBeatAddress} = {_lastHeartbeatValue}");
                    } catch (Exception hbEx) {
                        log.Error($"心跳写入失败: {hbEx.Message}", hbEx);
                        // 心跳失败仍继续尝试主逻辑
                    }

                    // ===== 主业务逻辑 =====
                    try {
                        log.Info("读取到标识，开始执行指定逻辑...");
                        setIsTargetReached(_client.ReadCoil(targetAddress));
                    } catch (Exception readEx) {
                        log.Error($"数据读取失败: {readEx.Message}", readEx);
                    }

                    // ===== 等待下次轮询 =====
                    try {
                        await Task.Delay(pollingIntervalMs, token);
                    } catch (TaskCanceledException) {
                        // 正常取消不记录错误
                        break;
                    }
                }
            } catch (OperationCanceledException) {
                log.Info("轮询被正常取消");
            } catch (Exception ex) {
                log.Error($"轮询致命错误: {ex.Message}", ex);
            } finally {
                // 清理时重置心跳
                try {
                    _client.WriteCoil(heartBeatAddress, false);
                    log.Info("服务停止，心跳重置为0");
                } catch {
                    // 尽力清理，忽略失败
                }
            }
        }
    }
}
