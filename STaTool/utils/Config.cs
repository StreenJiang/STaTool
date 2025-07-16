namespace STaTool.utils {
    public class Config {
        public Queue<string> Ip { get; set; } = new Queue<string>();
        public Queue<int> Port { get; set; } = new Queue<int>();
        public Queue<string> UpdateBtnImg { get; set; } = new Queue<string>();
        public Queue<string> CrvHeaderImg { get; set; } = new Queue<string>();
        public Queue<string> ExportBtnImg { get; set; } = new Queue<string>();
        public Queue<string> BlmBtnImg { get; set; } = new Queue<string>();
        public Queue<string> SaveBtnImg { get; set; } = new Queue<string>();
        public Queue<string> YesBtnImg { get; set; } = new Queue<string>();
        public Queue<string> OkBtnImg { get; set; } = new Queue<string>();
        public Queue<string> CloseBtnImg { get; set; } = new Queue<string>();
        public int FetchInterval { get; set; } = 0;
        public int ClickInterval { get; set; } = 0;
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tightening data");
    }
}
