# CONTEXT.md — STaTool 领域词汇表

## 核心概念

- **扭力扳手 (Torque Wrench)** — STa6000+ 智能扭力扳手，通过 TCP 发送 ASCII 协议消息。应用连接后订阅 `LAST_DATA_SUBSCRIBE`，接收到 `0061` 消息时解析拧紧数据并存储。

- **拧紧数据 (Tightening Data)** — 单次拧紧操作的测量记录，包含扭矩值、角度值、拧紧状态、VIN 号、程序号等字段。存储到 SQLite `tightening_data` 表，同时导出为 Excel/CSV/TXT。

- **曲线数据 (Curve Data)** — 扭力工具配套软件导出的扭矩-角度曲线图文件（`.crv` 格式）。两个文件：`Curves graph_ang.crv`（角度曲线）和 `Curves graph.crv`（扭矩曲线）。

- **PLC** — 三菱 FX5U 可编程逻辑控制器。通过 Modbus TCP 与应用程序通信，负责发出抓取指令并接收抓取结果反馈。

- **抓取 (Fetch / Capture)** — 通过 OpenCV 图像识别自动操作扭力工具配套软件界面，依次点击"更新→曲线表头→导出→BLM→保存→关闭"按钮，获取曲线数据的完整流程。每轮抓取由 PLC 标识位触发。

- **按钮图片 (Button Images)** — 用于 OpenCV `MatchTemplate` 的屏幕按钮截图模板。必须在与实际运行时相同的分辨率和 DPI 下截取。

## 通信协议

- **MID (Message ID)** — STA6000+ ASCII 协议的消息标识，位于消息第 4-8 位。`0061` = 拧紧数据, `0005` = 心跳确认, `9999` = 等待数据。

- **标识位 (Flag Position)** — PLC 寄存器地址，应用轮询此位。值为 `1` 时触发抓取；完成后写回 `2`(OK) 或 `3`(ERROR)。

- **心跳位 (Heartbeat Position)** — PLC 寄存器地址，应用交替写 `1`/`0` 维持连接存活性。

## 配置与存储

- **拧紧数据存储路径** — 拧紧数据 Excel/CSV/TXT 文件的输出目录。按年月组织子目录（如 `2026-07/2026-07-01.xlsx`）。

- **曲线数据存储路径** — 曲线 `.crv` 文件的所在目录。每轮抓取前先删除旧文件（避免导出时弹出"是否替换"对话框），然后由扭力工具软件重新生成。
