namespace STaTool.utils {
    public class Config {
        public Queue<string> Ip { get; set; } = new Queue<string>();
        public Queue<int> Port { get; set; } = new Queue<int>();
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tightening data");
    }
}
