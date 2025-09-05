using log4net;
using STaTool.Properties;

namespace STaTool.utils {
    /// <summary>
    /// 资源图片管理器 - 处理从Resources复制图片到Button Images文件夹
    /// </summary>
    public static class ResourceImageManager {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResourceImageManager));

        /// <summary>
        /// 检查Button Images文件夹是否为空，如果为空则从Resources复制默认图片
        /// </summary>
        /// <returns>是否执行了复制操作</returns>
        public static bool InitializeDefaultImages() {
            try {
                string buttonImagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileUtil.GetImageDirectory());

                // 检查Button Images文件夹是否存在且为空
                if (!Directory.Exists(buttonImagesPath) || !Directory.GetFiles(buttonImagesPath, "*.png").Any()) {
                    log.Info("Button Images文件夹为空，开始从Resources复制默认图片");

                    // 使用FileUtil确保Button Images文件夹存在
                    FileUtil.CheckAndCreateFolder(buttonImagesPath);
                    log.Info($"创建Button Images文件夹: {buttonImagesPath}");

                    // 复制所有资源图片
                    CopyResourceImages(buttonImagesPath);

                    log.Info("默认图片复制完成");
                    return true;
                } else {
                    log.Info("Button Images文件夹已存在图片，跳过默认图片复制");
                    return false;
                }
            } catch (Exception ex) {
                log.Error($"初始化默认图片失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 从Resources复制图片到指定文件夹
        /// </summary>
        /// <param name="targetPath">目标文件夹路径</param>
        private static void CopyResourceImages(string targetPath) {
            // 定义资源名称和对应的文件名映射
            var resourceMappings = new Dictionary<string, string> {
                { "BLM按钮图片", "BLM按钮图片" },
                { "保存按钮图片", "保存按钮图片" },
                { "关闭按钮图片", "关闭按钮图片" },
                { "导出按钮图片", "导出按钮图片" },
                { "是否替换按钮图片", "是否替换按钮图片" },
                { "曲线表头图片", "曲线表头图片" },
                { "更新按钮图片", "更新按钮图片" },
                { "确认按钮图片", "确认按钮图片" }
            };

            foreach (var mapping in resourceMappings) {
                try {
                    // 从Resources获取图片
                    var resourceImage = GetResourceImage(mapping.Key);
                    if (resourceImage != null) {
                        // 使用FileUtil.SaveImage保存图片
                        FileUtil.SaveImage(resourceImage, mapping.Value);
                        log.Info($"复制图片: {mapping.Key} -> {mapping.Value}");
                    } else {
                        log.Warn($"无法获取资源图片: {mapping.Key}");
                    }
                } catch (Exception ex) {
                    log.Error($"复制图片失败 {mapping.Key}: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 从Resources获取指定名称的图片
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>图片对象，如果获取失败返回null</returns>
        private static Image? GetResourceImage(string resourceName) {
            try {
                // 使用反射获取Resources中的图片
                var resourceManager = Resources.ResourceManager;
                var image = resourceManager.GetObject(resourceName) as Image;
                return image;
            } catch (Exception ex) {
                log.Error($"获取资源图片失败 {resourceName}: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取Button Images文件夹中的所有图片文件路径
        /// </summary>
        /// <returns>图片文件路径列表</returns>
        public static List<string> GetButtonImagePaths() {
            try {
                string buttonImagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileUtil.GetImageDirectory());

                if (!Directory.Exists(buttonImagesPath)) {
                    return new List<string>();
                }

                var imageFiles = Directory.GetFiles(buttonImagesPath, "*.png")
                    .OrderBy(f => Path.GetFileName(f))
                    .ToList();

                log.Info($"找到 {imageFiles.Count} 个按钮图片文件");
                return imageFiles;
            } catch (Exception ex) {
                log.Error($"获取按钮图片路径失败: {ex.Message}", ex);
                return new List<string>();
            }
        }
    }
}
