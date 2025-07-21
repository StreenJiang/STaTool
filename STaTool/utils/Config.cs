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
        public Queue<string> PlcIp { get; set; } = new Queue<string>();
        public Queue<int> PlcPort { get; set; } = new Queue<int>();
        public int PlcFlagPos { get; set; } = 0;
        public int PlcHeartBeatPos { get; set; } = 0;
        public int RepeatTimes { get; set; } = 0;
        public int ClickInterval { get; set; } = 0;
        public int CheckInterval { get; set; } = 0;
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tightening data");
    }
}
