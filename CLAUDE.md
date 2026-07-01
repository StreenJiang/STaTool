# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

STaTool 是一个 .NET 6 Windows Forms 桌面应用程序，用于与 **STa6000+ 扭力扳手** 通信，接收拧紧数据并存储到 SQLite，导出为 Excel/CSV/TXT。同时通过 **三菱 FX5U PLC (Modbus TCP)** 接收抓取指令，利用 **OpenCV 图像识别** 自动操作扭力工具配套软件界面来抓取曲线数据。

## 构建与运行

```bash
# 还原依赖并构建
dotnet restore
dotnet build

# Release 构建
dotnet build -c Release

# 运行
dotnet run --project STaTool/STaTool.csproj
```

解决方案文件: `STaTool.sln` (Visual Studio 2022)。安装器项目 `StaToolSetup/StaToolSetup.vdproj` 需要 Visual Studio Setup Project 扩展。

## 核心架构

### 通信层（双协议）

- **`Sta6000PlusTask`** (`tasks/Sta6000PlusTask.cs`) — 通过原生 TCP Socket 与 STa6000+ 仪器通信，使用专有 ASCII 协议（MID 消息标识）。连接后发送 `LAST_DATA_SUBSCRIBE` 订阅实时拧紧数据，收到 `0061` 消息后解析并存储。
- **`Fx5uModbusClient`** (`plc/Fx5uModbusClient.cs`) — 封装 EasyModbusTCP，连接三菱 FX5U PLC 的 Modbus TCP 服务。提供读写寄存器的基础操作。
- **`PlcPollingService`** (`plc/PlcPollingService.cs`) — 在后台轮询 PLC 的特定寄存器地址，用交替心跳（1/0）维持连接，检测到标识位为 1 时触发曲线抓取。

### 自动化抓取流程

**`ImageButtonClickerToolForm`** (`utils/ImageButtonClickerToolForm.cs`) 是曲线数据自动抓取的核心编排器：

1. 连接 PLC，启动 `PlcPollingService` 轮询
2. 检测到 PLC 指令后，按顺序对 8 个按钮执行图像识别点击（更新→曲线表头→导出→BLM→保存→是否替换→确认→关闭）
3. 特定按钮有键盘快捷键回退（Alt+S 保存、Alt+Y 确认）
4. 每轮点击完成后通过 `FinishType`（OK=2/ERROR=3）写回 PLC 寄存器
5. 全局键盘钩子：F10 可中止抓取

**`ImageRecognitionClicker`** (`utils/ImageRecognitionClicker.cs`) — 使用 OpenCvSharp4 的 `MatchTemplate`（`CCoeffNormed`）进行屏幕截图与模板匹配。每秒重试 5 次，超时 5 秒。

### 数据层

- **`TighteningData`** (`db/models/TighteningData.cs`) — 数据模型，映射到 SQLite `tightening_data` 表。使用 `[FieldName]` 自定义属性标记导出列名、顺序和枚举类型映射。
- **`TighteningDataDao`** (`db/dao/TighteningDataDao.cs`) — 基于 Dapper.Contrib 的 CRUD，构造函数自动建表。
- **`DbUtils`** (`db/DbUtils.cs`) — SQLite 连接工厂，数据库文件位于 `{BaseDirectory}/db/database.db`。

### 数据导出

**`ExtensionMethods`** (`extensions/ExtensionMethods.cs`) — 泛型扩展方法，利用反射读取 `[FieldName]` 属性自动生成表头和数据行：
- `ExportToExcelFile<T>()` — ClosedXML，带 Mutex 文件锁和重试（最多 5 次）
- `ExportToCsvFileAsync<T>()` / `ExportToTextFileAsync<T>()` — 流式写入，追加模式

### 配置系统

**`Config`** (`utils/Config.cs`) — 所有配置以 JSON 序列化到 `config.json`。使用 `Queue<T>` 存储最近使用的 IP/端口/图片名（最多 5 条），新连接成功的项插入队首。

关键配置项：仪器 IP/端口、PLC IP/端口/寄存器地址、8 种按钮图片名、重复次数/点击间隔/检查间隔、存储路径、自启动开关。

### UI 组件

- **`MainForm`** — 主界面：连接仪器、配置按钮图片（通过 `ScreenRegionSelector` 截图捕获）、配置 PLC 连接、触发抓取
- **`ScreenRegionSelector`** — 全屏半透明遮罩，拖拽选择屏幕区域截图
- **`ImageReviewInputBox`** — 截图预览和命名弹窗

## 关键依赖

| 包 | 用途 |
|---|---|
| `EasyModbusTCP` | Modbus TCP 通信（PLC） |
| `OpenCvSharp4` | 屏幕截图和模板匹配 |
| `InputSimulator` | 模拟鼠标点击和键盘输入 |
| `System.Data.SQLite` | SQLite 数据库 |
| `Dapper.Contrib` | ORM（CRUD 操作） |
| `ClosedXML` | Excel 文件读写 |
| `Newtonsoft.Json` | JSON 序列化（配置） |
| `log4net` | 日志框架 |

## 注意事项

- 应用要求 **管理员权限**（`app.manifest` 中 `requestedExecutionLevel` 设为 `requireAdministrator`），因为需要操作注册表实现自启动和全局键盘钩子。
- TCP 通信使用固定长度 ASCII 协议，MID 位于消息第 4-8 位。`AnalyzeData()` 中解析 `0061` 消息的字段位置是硬编码的，修改协议时需要精确对齐偏移量。
- 图像识别依赖屏幕分辨率 — 按钮模板图片必须在与实际运行时相同的分辨率和 DPI 下截取。
- 安装器项目 `StaToolSetup.vdproj` 只能在 Windows 上使用 Visual Studio 构建，`dotnet build` 不包含它。
