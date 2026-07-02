# 曲线数据存储路径 & 移除"是否替换"/"确认"步骤

**日期:** 2026-07-01
**状态:** 已确认（grilled）

---

## 概述

1. 新增"曲线数据存储路径"配置项，抓取每轮开始前删除该目录下的 `Curves graph_ang.crv` 和 `Curves graph.crv`
2. 从按钮点击循环中移除"是否替换"和"确认"两个步骤（及其 UI 配置）
3. 文件删除失败时弹窗警告操作员，跳过本轮并向 PLC 报 ERROR
4. 新增 `FetchConfig` 类打包 PLC/曲线路径配置，避免构造函数参数膨胀

---

## 一、新增文件 (`STaTool/utils/FetchConfig.cs`)

```csharp
namespace STaTool.utils {
    public class FetchConfig {
        public string PlcIp { get; set; }
        public int PlcPort { get; set; }
        public string CurveStoragePath { get; set; }
    }
}
```

---

## 二、Config 变更 (`STaTool/utils/Config.cs`)

| 操作 | 属性 | 类型 | 默认值 |
|------|------|------|--------|
| 新增 | `CurveStoragePath` | `string` | `""` |
| 删除 | `YesBtnImg` | `Queue<string>` | — |
| 删除 | `OkBtnImg` | `Queue<string>` | — |

旧 `config.json` 中残留的 `YesBtnImg`/`OkBtnImg` 字段会被 Newtonsoft.Json 忽略，不报错。

---

## 三、MainForm UI 变更 (`STaTool/MainForm.Designer.cs`)

### 3.1 存储配置区域 (`groupBox_storage_settings`)

- `label_storage_path` 文本: `"存储路径"` → `"拧紧数据存储路径"`
- 高度增加以容纳新行
- 新增控件（代码给出初始位置，用户在设计器中手动微调）:
  - `label_curve_storage_path` — `"曲线数据存储路径"`
  - `textBox_curve_storage_path` — 文本框，默认空
  - `button_browse_curve` — 浏览按钮

### 3.2 曲线抓取配置区域 (`groupBox_curve_settings`)

**删除以下控件：**
- `label_yes_btn_img`、`comboBox_yes_btn_img`、`button_yes_btn_img`
- `label_ok_btn_img`、`comboBox_ok_btn_img`、`button_ok_btn_img`

剩余按钮图片控件重新排列（从 8 行变为 6 行）：
1. 更新
2. 曲线表头
3. 导出到crv
4. BLM Curve
5. 保存
6. 关闭

---

## 四、MainForm 代码逻辑变更 (`STaTool/MainForm.cs`)

### 4.1 构造函数

- 读取 `config.CurveStoragePath` → `textBox_curve_storage_path.Text`
- 删除 `_loadImages(config.YesBtnImg, ...)` 和 `_loadImages(config.OkBtnImg, ...)`
- 删除 `button_yes_btn_img.Click` 和 `button_ok_btn_img.Click` 事件绑定
- 新增 `button_browse_curve.Click` 事件绑定

### 4.2 新增曲线路径浏览事件

逻辑与 `ButtonBrowse_Click` 一致：`FolderBrowserDialog` 选目录 → 校验 → 填入 `textBox_curve_storage_path`

### 4.3 ButtonStartFetch_Click

- **在打开子窗口之前**校验曲线路径必填（为空则 `ShowWarningPopUp` 并 `return`）
- 构造 `FetchConfig` 对象传入 `ImageButtonClickerToolForm`
- 按钮列表从 8 减为 6（去掉 yes/ok）
- 保存 `config.CurveStoragePath`

### 4.4 ButtonDisConnect_Click

- 恢复 `textBox_curve_storage_path.Enabled = true`

### 4.5 AutoStartFetch

- 删除对 `yesBtnImgName`/`okBtnImgName` 的空值检查

---

## 五、抓取流程变更 (`STaTool/utils/ImageButtonClickerToolForm.cs`)

### 5.1 构造函数

使用 `FetchConfig` 对象替代独立参数 `plcIp`、`plcPort`、`curveStoragePath`：

```csharp
public ImageButtonClickerToolForm(MainForm mainForm,
                                  List<string> imageButtons,
                                  FetchConfig fetchConfig,
                                  Action resetControls)
```

### 5.2 主循环

PLC 触发后、按钮循环之前，插入文件删除：

```
PLC触发 → 删除crv文件 → 成功则按钮循环(6步) → 重置PLC标志
                   → 失败则弹窗 + 报ERROR + 跳过本轮
```

删除逻辑：
- 目标文件: `Curves graph_ang.crv`、`Curves graph.crv`
- 路径: `Path.Combine(_curveStoragePath, 文件名)`
- `File.Exists` 检查 → 存在则 `File.Delete`
- 文件不存在 → 静默跳过
- 异常 → `log.Warn` + `ShowWarningPopUp` 提示操作员手动解锁 → 向 PLC 写 `FinishType.ERROR` → `continue` 跳过本轮

### 5.3 按钮循环

- 循环次数从 8 变为 6
- 移除索引 5 的 `SimulateAltY` 回退
- 移除索引 6 的 `failed = true` 逻辑
- **删除 `failed` 变量**，`SetValueByResult` 直接写 `FinishType.OK`

### 5.4 删除 SimulateAltY 方法

仅被 `ButtonStartFetch_Click` 调用，一并删除。

---

## 六、涉及文件清单

| 文件 | 变更类型 |
|------|---------|
| `STaTool/utils/FetchConfig.cs` | **新增** |
| `STaTool/utils/Config.cs` | 增删属性 |
| `STaTool/MainForm.Designer.cs` | UI 控件增删改（用户微调位置） |
| `STaTool/MainForm.cs` | 逻辑增删改 |
| `STaTool/utils/ImageButtonClickerToolForm.cs` | 流程重构 |

---

## 七、风险 & 边界

- **旧 config.json 兼容**: 删除 `YesBtnImg`/`OkBtnImg` 属性后，旧配置文件反序列化不受影响
- **文件删除失败**: 文件被其他进程锁定时弹窗通知操作员、向 PLC 报 ERROR、跳过本轮，不阻塞后续轮次
- **SimulateAltS**: 保留不动（保存按钮的键盘回退仍然需要）
- **曲线路径校验**: 在打开抓取子窗口之前校验，避免无效窗口弹出
