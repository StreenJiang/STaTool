using EasyModbus;
using log4net;

namespace STaTool.plc {

    /// <summary>
    /// 封装与三菱 FX5U PLC 通过 Modbus TCP 通信的客户端类
    /// </summary>
    public class Fx5uModbusClient {
        private ILog log = LogManager.GetLogger(typeof(Fx5uModbusClient));
        private ModbusClient _client;

        /// <summary>
        /// 构造函数：初始化 ModbusClient 实例
        /// </summary>
        public Fx5uModbusClient() {
            _client = new ModbusClient();
        }

        /// <summary>
        /// 连接到 FX5U PLC (Modbus TCP)
        /// </summary>
        /// <param name="ipAddress">PLC 的 IP 地址</param>
        /// <param name="port">PLC 的端口号</param>
        public void Connect(string ipAddress, int port) {
            try {
                _client.IPAddress = ipAddress;
                _client.Port = port;
                _client.Connect();
            } catch (Exception ex) {
                log.Warn($"连接PLC失败：{ex.Message}", ex);
                throw new Exception($"连接PLC失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 断开与 PLC 的连接
        /// </summary>
        public void Disconnect() {
            try {
                _client.Disconnect();
            } catch (Exception ex) {
                // 可选：记录异常或忽略
                log.Warn($"断开连接时发生错误：{ex.Message}", ex);
                throw new Exception($"断开连接时发生错误：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取单个位（Coil）值
        /// </summary>
        /// <param name="address">线圈地址</param>
        /// <returns>线圈状态 (true 或 false)</returns>
        public bool ReadCoil(int address) {
            try {
                bool[] coils = _client.ReadCoils(address, 1);
                return coils[0];
            } catch (Exception ex) {
                log.Warn($"读取单个位失败：{ex.Message}", ex);
                throw new Exception($"读取单个位失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取多个位（Coils）值
        /// </summary>
        /// <param name="startAddress">起始线圈地址</param>
        /// <param name="count">读取线圈数量</param>
        /// <returns>返回读取到的线圈状态数组</returns>
        public bool[] ReadCoils(int startAddress, int count) {
            try {
                return _client.ReadCoils(startAddress, count);
            } catch (Exception ex) {
                log.Warn($"读取多个位失败：{ex.Message}", ex);
                throw new Exception($"读取多个位失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取单个寄存器 (16位整数)
        /// </summary>
        /// <param name="address">寄存器地址</param>
        /// <returns>返回寄存器值</returns>
        public int ReadRegister(int address) {
            try {
                int[] regs = _client.ReadHoldingRegisters(address, 1);
                return regs[0];
            } catch (Exception ex) {
                log.Warn($"读取单个寄存器失败：{ex.Message}", ex);
                throw new Exception($"读取单个寄存器失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取多个寄存器值
        /// </summary>
        /// <param name="startAddress">起始寄存器地址</param>
        /// <param name="count">读取寄存器数量</param>
        /// <returns>返回读取到的寄存器值数组</returns>
        public int[] ReadRegisters(int startAddress, int count) {
            try {
                return _client.ReadHoldingRegisters(startAddress, count);
            } catch (Exception ex) {
                log.Warn($"读取多个寄存器失败：{ex.Message}", ex);
                throw new Exception($"读取多个寄存器失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取浮点数 (占用两个连续寄存器，先低后高)
        /// </summary>
        /// <param name="startAddress">起始寄存器地址（低寄存器地址）</param>
        /// <returns>返回浮点值</returns>
        public float ReadFloat(int startAddress) {
            try {
                int[] regs = _client.ReadHoldingRegisters(startAddress, 2);
                return ModbusClient.ConvertRegistersToFloat(regs);
            } catch (Exception ex) {
                log.Warn($"读取浮点数失败：{ex.Message}", ex);
                throw new Exception($"读取浮点数失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入单个位（Coil）
        /// </summary>
        /// <param name="address">线圈地址</param>
        /// <param name="value">要写入的布尔值</param>
        public void WriteCoil(int address, bool value) {
            try {
                _client.WriteSingleCoil(address, value);
            } catch (Exception ex) {
                log.Warn($"写入单个位失败：{ex.Message}", ex);
                throw new Exception($"写入单个位失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入多个位（Coils）
        /// </summary>
        /// <param name="startAddress">起始线圈地址</param>
        /// <param name="values">要写入的布尔值数组</param>
        public void WriteCoils(int startAddress, bool[] values) {
            try {
                _client.WriteMultipleCoils(startAddress, values);
            } catch (Exception ex) {
                log.Warn($"写入多个位失败：{ex.Message}", ex);
                throw new Exception($"写入多个位失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入单个寄存器 (16位整数)
        /// </summary>
        /// <param name="address">寄存器地址</param>
        /// <param name="value">要写入的整数值</param>
        public void WriteRegister(int address, int value) {
            try {
                _client.WriteSingleRegister(address, value);
            } catch (Exception ex) {
                log.Warn($"写入单个寄存器失败：{ex.Message}", ex);
                throw new Exception($"写入单个寄存器失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入多个寄存器值
        /// </summary>
        /// <param name="startAddress">起始寄存器地址</param>
        /// <param name="values">要写入的整数数组</param>
        public void WriteRegisters(int startAddress, int[] values) {
            try {
                _client.WriteMultipleRegisters(startAddress, values);
            } catch (Exception ex) {
                log.Warn($"写入多个寄存器失败：{ex.Message}", ex);
                throw new Exception($"写入多个寄存器失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入浮点数 (占用两个连续寄存器，先低后高)
        /// </summary>
        /// <param name="startAddress">起始寄存器地址（低寄存器地址）</param>
        /// <param name="value">要写入的浮点值</param>
        public void WriteFloat(int startAddress, float value) {
            try {
                int[] regs = ModbusClient.ConvertFloatToRegisters(value);
                _client.WriteMultipleRegisters(startAddress, regs);
            } catch (Exception ex) {
                log.Warn($"写入浮点数失败：{ex.Message}", ex);
                throw new Exception($"写入浮点数失败：{ex.Message}", ex);
            }
        }
    }
}
