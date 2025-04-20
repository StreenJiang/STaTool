using System.Net;
using System.Net.Sockets;
using System.Text;
using log4net;

namespace STaTool.tasks {

    public class Sta6000PlusTask {
        #region Fields
        private readonly ILog log;
        private readonly int RECEIVE_TIME_OUT = 500;
        private readonly int HEART_BEAT_COUNT = 10;

        private Socket? socket = null;
        private string ip;
        private int port;
        private readonly Queue<string> commands = new();
        private bool isConnected = false;
        #endregion

        #region Properties
        public Socket? SocketClient { get => socket; set => socket = value; }
        public string Ip { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public bool IsConnected { get => isConnected; set => isConnected = value; }
        #endregion

        public Sta6000PlusTask(string ip, int port) {
            // Initialize log
            log = LogManager.GetLogger(GetType());

            Ip = ip;
            Port = port;

            // TODO: Last_data_subscribe
            commands.Enqueue("002000600011        \x00");

            // Set connected to true
            isConnected = true;

            RunTask();
        }

        private async void RunTask() {
            await Task.Run(() => {
                int count = 0;
                while (isConnected) {
                    if (socket == null || !socket.Connected || !IsSocketConnected()) {
                        OpenConnection();
                        count = 0;
                        Task.Delay(RECEIVE_TIME_OUT);
                        continue;
                    }

                    try {
                        // Check and send command
                        if (commands.TryDequeue(out string? currentCommand)) {
                            socket.Send(Encoding.ASCII.GetBytes(currentCommand));
                            Task.Delay(RECEIVE_TIME_OUT);
                            count = 0;
                        }

                        // Receive data
                        byte[] msgBytes = new byte[1024 * 1024];
                        int msgLen = socket.Receive(new ArraySegment<byte>(msgBytes), SocketFlags.None);
                        if (msgLen > 0) {
                            string dataMessage = Encoding.ASCII.GetString(msgBytes);
                            log.Info($"Analyzing data : [{dataMessage}]");
                            if (GetMid(dataMessage) == "0005") {
                                // TODO: analyzing data
                            }
                            count = 0;
                        }
                    } catch (Exception e) {
                        log.Error($"Send or receive message failed, e = {e}");

                        count++;
                        if (count >= HEART_BEAT_COUNT) {
                            // TODO: keep_alive_message
                            commands.Enqueue("00209999001         \x00");
                        }
                    }
                }
            });
        }

        private void OpenConnection() {
            try {
                // Create socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {
                    ReceiveTimeout = RECEIVE_TIME_OUT
                };
                socket.Connect(IPAddress.Parse(Ip), Port);

                // Send communication stop to esure it's okay to connect
                // TODO: communication_stop
                socket.Send(Encoding.ASCII.GetBytes("00200003000         \x00"));
                Task.Delay(RECEIVE_TIME_OUT);

                // Send communication start
                // TODO: comminucation_start
                socket.Send(Encoding.ASCII.GetBytes("00200001000         \x00"));
                Task.Delay(RECEIVE_TIME_OUT);
                byte[] msgBytes = new byte[1024 * 1024];
                int msgLen = socket.Receive(new ArraySegment<byte>(msgBytes), SocketFlags.None);
                if (msgLen > 0) {
                    string dataMessage = Encoding.ASCII.GetString(msgBytes);
                    log.Info($"Analyzing data : [{dataMessage}]");
                    if (GetMid(dataMessage) != "0005") {
                        // TODO: show message
                    } else {
                        socket.Close();
                        socket = null;
                        log.Warn($"Communication failed to start, result = [{dataMessage}]");
                        // TODO: show message
                    }
                }
            } catch (Exception e) {
                log.Warn($"Open connection failed, e = {e}");
            }
        }

        private string GetMid(string result) {
            string mid = "";
            try {
                mid = result.Substring(4, 4);
            } catch (Exception e) {
                log.Error($"Get mid failed from result message = {result}, e = {e}");
            }
            return mid;
        }

        private bool IsSocketConnected() {
            bool isAlive = false;
            try {
                if (socket != null) {
                    isAlive = !(socket.Poll(0, SelectMode.SelectRead) || socket.Available > 0);
                }
            } catch (SocketException e) {
                log.Warn($"Connection of socket is lost. Error: {e}");
            }

            return isAlive;
        }

    }
}
