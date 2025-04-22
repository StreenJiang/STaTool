using System.Net;
using System.Net.Sockets;
using System.Text;
using log4net;
using STaTool.utils;

namespace STaTool.tasks {

    public class Sta6000PlusTask {
        #region Fields
        private readonly ILog log;
        private const string COMMUNICATION_START = "00200001000         \x00";
        private const string COMMUNICATION_STOP = "00200003000         \x00";
        private const string KEEP_ALIVE = "00209999001         \x00";
        private const string LAST_DATA_SUBSCRIBE = "002000600011        \x00";
        private readonly int RECEIVE_TIME_OUT = 500;
        private readonly int HEART_BEAT_COUNT = 20;

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

            // Add command: last_data_subscribe
            commands.Enqueue(LAST_DATA_SUBSCRIBE);

            // Set connected to true
            isConnected = true;

            RunTask();
        }

        private async void RunTask() {
            await Task.Run(async () => {
                log.Info($"Task is started...");
                WidgetUtils.AppendMsg("正在连接仪器...");

                int count = 0;
                while (isConnected) {
                    if (socket == null || !socket.Connected || !IsSocketConnected()) {
                        await OpenConnection();
                        count = 0;
                        await Task.Delay(RECEIVE_TIME_OUT);
                        continue;
                    }

                    try {
                        // Check and send command
                        if (commands.TryDequeue(out string? currentCommand)) {
                            socket.Send(Encoding.ASCII.GetBytes(currentCommand));
                            await Task.Delay(RECEIVE_TIME_OUT);
                            count = 0;
                        }

                        // Receive data
                        byte[] msgBytes = new byte[1024 * 1024];
                        int msgLen = socket.Receive(new ArraySegment<byte>(msgBytes), SocketFlags.None);
                        if (msgLen > 0) {
                            string dataMessage = Encoding.ASCII.GetString(msgBytes, 0, msgLen);
                            log.Info($"Receiving message: [{dataMessage}]");
                            if (GetMid(dataMessage) == "0061") {
                                log.Info($"Analyzing data: [{dataMessage}]");
                                WidgetUtils.AppendMsg($"收到数据：[{dataMessage}]");

                                // TODO: analyzing data
                                log.Debug($"Tightening status: {dataMessage.Substring(108, 1)}");
                                log.Debug($"Torque status: {dataMessage.Substring(111, 1)}");
                                log.Debug($"Angle status: {dataMessage.Substring(114, 1)}");
                                log.Debug($"Torque: {dataMessage.Substring(141, 6)}");
                                log.Debug($"Angle: {dataMessage.Substring(170, 5)}");
                            }
                            count = 0;
                        }
                    } catch {
                        count++;
                        if (count >= HEART_BEAT_COUNT) {
                            // Add command: keep_alive_message
                            commands.Enqueue(KEEP_ALIVE);
                        }
                    }
                }

                WidgetUtils.AppendMsg("正在断开连接...");
                log.Info($"Task is stopped...");

                await Task.Delay(RECEIVE_TIME_OUT);
                if (socket != null) {
                    try {
                        // Send communication stop
                        socket.Send(Encoding.ASCII.GetBytes(COMMUNICATION_STOP));
                    } catch (Exception) {
                    } finally {
                        socket.Close();
                        socket = null;
                    }
                }
                WidgetUtils.AppendMsg("连接已断开...");
            });
        }

        private async Task OpenConnection() {
            try {
                // Create socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {
                    ReceiveTimeout = RECEIVE_TIME_OUT
                };
                socket.Connect(IPAddress.Parse(Ip), Port);

                // Send communication stop to esure it's okay to connect
                socket.Send(Encoding.ASCII.GetBytes(COMMUNICATION_STOP));
                await Task.Delay(RECEIVE_TIME_OUT);

                // Send communication start
                socket.Send(Encoding.ASCII.GetBytes(COMMUNICATION_START));
                await Task.Delay(RECEIVE_TIME_OUT);
                byte[] msgBytes = new byte[1024 * 1024];
                int msgLen = socket.Receive(new ArraySegment<byte>(msgBytes), SocketFlags.None);
                if (msgLen > 0) {
                    string dataMessage = Encoding.ASCII.GetString(msgBytes, 0, msgLen);
                    log.Info($"Get response after connecting: [{dataMessage}]");
                    if (msgLen == 1 || (msgLen > 8 && GetMid(dataMessage) == "0005")) {
                        log.Info($"Trying to connect...");
                        WidgetUtils.AppendMsg("已连接仪器，持续接收数据中...");
                    } else {
                        socket.Close();
                        socket = null;
                        log.Warn($"Communication failed to start, result = [{dataMessage}]");
                        WidgetUtils.AppendMsg("无法连接到仪器，正在重新尝试...");
                    }
                }
            } catch (Exception e) {
                log.Warn($"Open connection failed, e = {e}");
                WidgetUtils.AppendMsg("无法连接到仪器，正在重新尝试...");
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
                    // isAlive = !(socket.Poll(0, SelectMode.SelectRead) || socket.Available > 0);
                    isAlive = !(socket.Poll(0, SelectMode.SelectRead) && socket.Available == 0);
                }
            } catch (SocketException e) {
                log.Warn($"Connection of socket is lost. Error: {e}");
            }

            return isAlive;
        }

    }
}
