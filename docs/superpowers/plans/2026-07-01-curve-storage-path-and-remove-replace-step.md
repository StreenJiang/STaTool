# 曲线数据存储路径 & 移除"是否替换"/"确认"步骤 — 实现计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 新增曲线数据存储路径配置，抓取每轮前删除旧 crv 文件；从循环中移除"是否替换"和"确认"两个步骤。

**Architecture:** 5 个任务依次执行：新增 FetchConfig → 改 Config → 改 UI → 改抓取流程 → 改 MainForm 接线。每个任务独立可构建验证。

**Tech Stack:** .NET 6, Windows Forms, Newtonsoft.Json

## Global Constraints

- 曲线路径默认空字符串，在打开抓取子窗口前校验必填
- 文件不存在时静默跳过
- 文件删除失败（锁定等）→ 弹窗警告 → 向 PLC 写 ERROR → 跳过本轮
- SimulateAltS 保留不动
- 旧 config.json 兼容（Newtonsoft.Json 忽略未知属性）
- Designer 文件手动编辑，控件位置随后由用户在设计器中微调

---

### Task 1: 新增 FetchConfig 类 (`STaTool/utils/FetchConfig.cs`)

**Files:**
- Create: `STaTool/utils/FetchConfig.cs`

**Interfaces:**
- Consumes: 无
- Produces: `FetchConfig` 类（`PlcIp`, `PlcPort`, `CurveStoragePath`），供 Task 4/5 使用

- [ ] **Step 1: 创建文件**

```csharp
// STaTool/utils/FetchConfig.cs
namespace STaTool.utils {
    public class FetchConfig {
        public string PlcIp { get; set; }
        public int PlcPort { get; set; }
        public string CurveStoragePath { get; set; }
    }
}
```

- [ ] **Step 2: 构建验证**

```bash
dotnet build
```

Expected: BUILD SUCCESS（新类尚未被引用，不影响编译）

- [ ] **Step 3: 提交**

```bash
git add STaTool/utils/FetchConfig.cs
git commit -m "feat: add FetchConfig class for PLC and curve path config"
```

---

### Task 2: Config 属性增删 (`STaTool/utils/Config.cs`)

**Files:**
- Modify: `STaTool/utils/Config.cs`

**Interfaces:**
- Consumes: 无
- Produces: `Config.CurveStoragePath` (string, 默认 `""`)，供 Task 4/5 使用

- [ ] **Step 1: 新增 CurveStoragePath，删除 YesBtnImg 和 OkBtnImg**

```csharp
// STaTool/utils/Config.cs — 在 StoragePath 之后新增一行：
public string CurveStoragePath { get; set; } = "";

// 删除以下两行：
public Queue<string> YesBtnImg { get; set; } = new Queue<string>();
public Queue<string> OkBtnImg { get; set; } = new Queue<string>();
```

- [ ] **Step 2: 构建验证**

```bash
dotnet build
```

Expected: 构建**失败**（MainForm.cs 仍在引用 `YesBtnImg`/`OkBtnImg`），这是正常的，后续任务修复。

- [ ] **Step 3: 提交**

```bash
git add STaTool/utils/Config.cs
git commit -m "feat: add CurveStoragePath, remove YesBtnImg and OkBtnImg from Config"
```

---

### Task 3: MainForm.Designer.cs UI 变更 (`STaTool/MainForm.Designer.cs`)

**Files:**
- Modify: `STaTool/MainForm.Designer.cs`

**Interfaces:**
- Consumes: 无
- Produces: 新控件 `textBox_curve_storage_path`、`button_browse_curve`、`label_curve_storage_path`，供 Task 5 引用

> **注意：** 控件位置为代码给出的初始值，用户将在设计器中手动微调。

- [ ] **Step 1: 修改存储配置区域 — 改 label、加高度、新增曲线路径行**

在 `InitializeComponent()` 中定位 `groupBox_storage_settings` 区域：

```csharp
// 高度从 94 改为 130
groupBox_storage_settings.Size = new Size(512, 130);

// label 文本修改
label_storage_path.Text = "拧紧数据存储路径";

// 新增 label
label_curve_storage_path = new Label();
label_curve_storage_path.AutoSize = true;
label_curve_storage_path.Location = new Point(18, 74);
label_curve_storage_path.Name = "label_curve_storage_path";
label_curve_storage_path.Size = new Size(116, 17);
label_curve_storage_path.Text = "曲线数据存储路径";

// 新增 textBox
textBox_curve_storage_path = new TextBox();
textBox_curve_storage_path.Location = new Point(18, 93);
textBox_curve_storage_path.Name = "textBox_curve_storage_path";
textBox_curve_storage_path.Size = new Size(397, 23);

// 新增 浏览按钮
button_browse_curve = new Button();
button_browse_curve.Location = new Point(423, 92);
button_browse_curve.Name = "button_browse_curve";
button_browse_curve.Size = new Size(71, 24);
button_browse_curve.Text = "浏览";
button_browse_curve.UseVisualStyleBackColor = true;

// 加入 groupBox
groupBox_storage_settings.Controls.Add(label_curve_storage_path);
groupBox_storage_settings.Controls.Add(textBox_curve_storage_path);
groupBox_storage_settings.Controls.Add(button_browse_curve);
```

- [ ] **Step 2: 删除"是否替换"和"确认"控件**

在 `InitializeComponent()` 中删除以下控件的**构造、属性设置、Controls.Add** 代码块：

- `label_yes_btn_img`
- `comboBox_yes_btn_img`
- `button_yes_btn_img`
- `label_ok_btn_img`
- `comboBox_ok_btn_img`
- `button_ok_btn_img`

同时从 `groupBox_curve_settings.Controls.Add(...)` 链中删除这 6 个控件。

- [ ] **Step 3: 重新排列剩余控件（初始位置）**

关闭按钮移到保存按钮的右侧（原"是否替换"的位置）：

```csharp
// label_close_btn_img y 从 181 移到 135
label_close_btn_img.Location = new Point(276, 135);
// comboBox_close_btn_img y 从 200 移到 154
comboBox_close_btn_img.Location = new Point(276, 154);
// button_close_btn_img y 从 198 移到 152
button_close_btn_img.Location = new Point(435, 152);
```

PLC 配置区域上移：
```csharp
groupBox_plc_settings.Location = new Point(18, 190);
```

groupBox_curve_settings 高度缩小：
```csharp
groupBox_curve_settings.Size = new Size(512, 290);
```

- [ ] **Step 4: 更新字段声明**

在 `#endregion` 前：

```csharp
// 删除：
private Label label_yes_btn_img;
private ComboBox comboBox_yes_btn_img;
private Button button_yes_btn_img;
private Label label_ok_btn_img;
private ComboBox comboBox_ok_btn_img;
private Button button_ok_btn_img;

// 新增：
private Label label_curve_storage_path;
private TextBox textBox_curve_storage_path;
private Button button_browse_curve;
```

- [ ] **Step 5: 构建验证（预期失败，Task 5 修复）**

```bash
dotnet build
```

预期错误：MainForm.cs 引用已删除控件。

- [ ] **Step 6: 提交**

```bash
git add STaTool/MainForm.Designer.cs
git commit -m "feat: add curve storage path UI, remove yes/ok button image UI"
```

---

### Task 4: ImageButtonClickerToolForm 抓取流程重构 (`STaTool/utils/ImageButtonClickerToolForm.cs`)

**Files:**
- Modify: `STaTool/utils/ImageButtonClickerToolForm.cs`

**Interfaces:**
- Consumes: `FetchConfig` (Task 1), `Config.CurveStoragePath` (Task 2)
- Produces: 构造函数新签名 `ImageButtonClickerToolForm(MainForm, List<string>, FetchConfig, Action)`，供 Task 5 调用

- [ ] **Step 1: 新增字段，修改构造函数签名**

字段区域：
```csharp
private string _curveStoragePath;
```

构造函数改为接收 FetchConfig：
```csharp
public ImageButtonClickerToolForm(MainForm mainForm,
                                  List<string> imageButtons,
                                  FetchConfig fetchConfig,
                                  Action resetControls) {
    log = LogManager.GetLogger(GetType());
    _mainForm = mainForm;

    _proc = HookCallback;
    _hookID = SetHook(_proc);

    InitializeComponent();

    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    Location = new Point(
        workingArea.Right - Width,
        workingArea.Bottom - Height
    );
    Config config = FileUtil.LoadConfig();
    if (config.RepeatTimes != 0) {
        textBox_repeat_times.Text = config.RepeatTimes.ToString();
    }
    if (config.ClickInterval != 0) {
        textBox_click_interval.Text = config.ClickInterval.ToString();
    }

    _imageButtons = imageButtons;
    _plcIp = fetchConfig.PlcIp;
    _plcPort = fetchConfig.PlcPort;
    _curveStoragePath = fetchConfig.CurveStoragePath;
    _resetControls = resetControls;

    button_start_fetch.Click += ButtonStartFetch_Click;
    button_stop_fetch.Click += ButtonStopFetch_Click;
}
```

- [ ] **Step 2: 主循环中插入文件删除逻辑（含失败保护）**

在 `if (_plcFlagDetected)` 块内、`int count = 0;` 之前：

```csharp
if (_plcFlagDetected) {
    // 新增：删除曲线文件
    bool deleteFailed = false;
    if (!string.IsNullOrEmpty(_curveStoragePath)) {
        string[] curveFiles = { "Curves graph_ang.crv", "Curves graph.crv" };
        foreach (var file in curveFiles) {
            string filePath = Path.Combine(_curveStoragePath, file);
            try {
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                    WidgetUtils.AppendMsg($"已删除曲线文件: {file}");
                }
            } catch (Exception ex) {
                deleteFailed = true;
                log.Warn($"删除曲线文件失败: {filePath}", ex);
                WidgetUtils.ShowWarningPopUp(
                    $"曲线文件被占用，无法删除！\n\n" +
                    $"文件: {file}\n" +
                    $"路径: {_curveStoragePath}\n\n" +
                    $"请手动关闭占用该文件的程序后重试。");
                break;
            }
        }
    }

    if (deleteFailed) {
        // 向 PLC 报错，跳过本轮
        SetValueByResult(fx5UModbusClient, config.PlcFlagPos, true);
        WidgetUtils.AppendMsg("曲线文件删除失败，已跳过本轮抓取。");
    } else {
        int count = 0;
        // ... 原有按钮循环逻辑 ...
```

- [ ] **Step 3: 简化按钮循环，删除 failed 变量**

```csharp
// 删除 failed 变量声明
// 原来: bool failed = false;  → 删除
// 原来: failed = false;       → 删除

int count = 0;
while (count < repeatTimes && !token.IsCancellationRequested) {
    for (int i = 0; i < _imageButtons.Count; i++) {
        token.ThrowIfCancellationRequested();

        string btnImg = FileUtil.GetImagePath(_imageButtons[i]);
        if (i == 1) {
            await clicker.ClickButtonByImage_Special(btnImg);
        } else {
            bool found = await clicker.ClickButtonByImage(btnImg);
            if (!found) {
                if (i == 4) {
                    // 保存按钮 — Alt+S 回退
                    SimulateAltS(clicker.InputSimulator);
                }
            }
        }

        await Task.Delay(clickInterval, token);
    }

    count++;
    WidgetUtils.AppendMsg($"第 {count} 次曲线抓取完毕，结果：抓取成功");
    break;
}

// 重置 PLC 标志位 — 直接写 OK
SetValueByResult(fx5UModbusClient, config.PlcFlagPos, false);
```

> 注意：`SetValueByResult(..., false)` 即 `FinishType.OK`。`SetValueByResult` 签名不变，`failed=false` → OK。

- [ ] **Step 4: 删除 SimulateAltY 方法**

```csharp
// 删除整个方法（约在第 225-235 行）：
private void SimulateAltY(InputSimulator inputSimulator) {
    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.MENU);
    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
    inputSimulator.Keyboard.KeyUp(VirtualKeyCode.MENU);
}
```

- [ ] **Step 5: 构建验证（预期失败，Task 5 修复）**

```bash
dotnet build
```

预期错误：MainForm.cs 调用构造函数参数不匹配。

- [ ] **Step 6: 提交**

```bash
git add STaTool/utils/ImageButtonClickerToolForm.cs
git commit -m "feat: add curve file deletion with lock protection, remove yes/ok steps and SimulateAltY"
```

---

### Task 5: MainForm.cs 代码逻辑变更 (`STaTool/MainForm.cs`)

**Files:**
- Modify: `STaTool/MainForm.cs`

**Interfaces:**
- Consumes: `FetchConfig` (Task 1), `Config.CurveStoragePath` (Task 2), 新 UI 控件 (Task 3), 新构造函数签名 (Task 4)
- Produces: 完整可编译运行的应用

- [ ] **Step 1: 构造函数 — 加载曲线路径、删除 yes/ok 引用、绑定新事件**

```csharp
// 删除这两行：
_loadImages(config.YesBtnImg, comboBox_yes_btn_img, "是否替换按钮图片");
_loadImages(config.OkBtnImg, comboBox_ok_btn_img, "确认按钮图片");

// 在 StoragePath 加载代码后面新增：
if (!string.IsNullOrEmpty(config.CurveStoragePath)) {
    textBox_curve_storage_path.Text = config.CurveStoragePath;
}

// 删除这两行事件绑定：
button_yes_btn_img.Click += (s, e)
    => ButtonCaptureBtnImage_Click(comboBox_yes_btn_img, "是否替换按钮", cfg => cfg.YesBtnImg);
button_ok_btn_img.Click += (s, e)
    => ButtonCaptureBtnImage_Click(comboBox_ok_btn_img, "确认按钮", cfg => cfg.OkBtnImg);

// 新增事件绑定：
button_browse_curve.Click += ButtonBrowseCurve_Click;
```

- [ ] **Step 2: 新增 ButtonBrowseCurve_Click 方法**

```csharp
private void ButtonBrowseCurve_Click(object? sender, EventArgs e) {
    using var folderDialog = new FolderBrowserDialog();
    folderDialog.ShowNewFolderButton = true;
    if (!string.IsNullOrEmpty(textBox_curve_storage_path.Text)
            && !string.IsNullOrWhiteSpace(textBox_curve_storage_path.Text)) {
        folderDialog.SelectedPath = textBox_curve_storage_path.Text;
    }
    folderDialog.Description = "请选择曲线文件存储文件夹";

    if (folderDialog.ShowDialog() == DialogResult.OK) {
        string selectedFolderPath = folderDialog.SelectedPath;

        if (!FileUtil.IsPathValid(selectedFolderPath)) {
            WidgetUtils.SetError(textBox_curve_storage_path, "请输入正确的路径");
            return;
        } else {
            WidgetUtils.SetError(textBox_curve_storage_path, "");
        }

        textBox_curve_storage_path.Text = selectedFolderPath;
    }
}
```

- [ ] **Step 3: ButtonStartFetch_Click — 前置校验 + FetchConfig + 简化按钮列表**

在方法开头、任何校验之前插入曲线路径校验：

```csharp
// 新增：曲线数据存储路径校验（在打开子窗口之前）
string curveStoragePath = textBox_curve_storage_path.Text;
if (string.IsNullOrWhiteSpace(curveStoragePath)) {
    WidgetUtils.ShowWarningPopUp("请先选择曲线数据存储路径！");
    return;
}
if (!FileUtil.IsPathValid(curveStoragePath)) {
    WidgetUtils.SetError(textBox_curve_storage_path, "请输入正确的曲线数据存储路径");
    return;
} else {
    WidgetUtils.SetError(textBox_curve_storage_path, "");
}
```

删除 yes/ok 变量声明，修改空值检查（6 个按钮）：
```csharp
// 删除：
string yesBtnImgName = comboBox_yes_btn_img.Text;
string okBtnImgName = comboBox_ok_btn_img.Text;

// 修改空值检查：
if (string.IsNullOrWhiteSpace(updateBtnImgName) || string.IsNullOrWhiteSpace(crvHeaderImgName)
    || string.IsNullOrWhiteSpace(exportBtnImgName) || string.IsNullOrWhiteSpace(blmBtnImgName)
    || string.IsNullOrWhiteSpace(saveBtnImgName) || string.IsNullOrWhiteSpace(closeBtnImgName)
    ) {
    WidgetUtils.ShowWarningPopUp("请先选择所有按钮的图片！");
    return;
}
```

保存配置时新增曲线路径：
```csharp
config.CurveStoragePath = curveStoragePath;  // 新增
```

按钮列表从 8 减为 6：
```csharp
List<string> buttons = new() {
    updateBtnImgName,
    crvHeaderImgName,
    exportBtnImgName,
    blmBtnImgName,
    saveBtnImgName,
    closeBtnImgName,
};
```

构造 FetchConfig 并传入 ImageButtonClickerToolForm：
```csharp
var fetchConfig = new FetchConfig {
    PlcIp = ip,
    PlcPort = port,
    CurveStoragePath = curveStoragePath,
};

var imageButtonClickerToolForm = new ImageButtonClickerToolForm(this, buttons, fetchConfig, () => {
    // resetControls 逻辑...
});
```

删除 yes/ok 控件的禁用/启用代码，新增曲线路径控件的禁用/启用：
```csharp
// 开始抓取时（在 button_start_fetch.Enabled = false 附近）：
// 删除: button_yes_btn_img.Enabled = false;  button_ok_btn_img.Enabled = false;
// 删除: comboBox_yes_btn_img.Enabled = false; comboBox_ok_btn_img.Enabled = false;
// 新增:
textBox_curve_storage_path.Enabled = false;
button_browse_curve.Enabled = false;

// resetControls 中：
// 删除: button_yes_btn_img.Enabled = true;   button_ok_btn_img.Enabled = true;
// 删除: comboBox_yes_btn_img.Enabled = true;  comboBox_ok_btn_img.Enabled = true;
// 新增:
textBox_curve_storage_path.Enabled = true;
button_browse_curve.Enabled = true;
```

- [ ] **Step 4: ButtonDisConnect_Click — 恢复曲线路径控件**

```csharp
if (!fetchingCurveData) {
    button_browse.Enabled = true;
    textBox_storage_path.Enabled = true;
    button_browse_curve.Enabled = true;        // 新增
    textBox_curve_storage_path.Enabled = true;  // 新增
}
```

- [ ] **Step 5: AutoStartFetch — 删除 yes/ok 检查**

```csharp
// 从 8 个检查改为 6 个：
if (string.IsNullOrWhiteSpace(comboBox_update_btn_img.Text) ||
    string.IsNullOrWhiteSpace(comboBox_crv_header_img.Text) ||
    string.IsNullOrWhiteSpace(comboBox_export_btn_img.Text) ||
    string.IsNullOrWhiteSpace(comboBox_blm_btn_img.Text) ||
    string.IsNullOrWhiteSpace(comboBox_save_btn_img.Text) ||
    string.IsNullOrWhiteSpace(comboBox_close_btn_img.Text)) {
    WidgetUtils.AppendMsg("❌ 自动抓取失败：请先配置所有按钮图片");
    return;
}
```

- [ ] **Step 6: 构建验证**

```bash
dotnet build
```

Expected: **BUILD SUCCESS**（0 errors）

- [ ] **Step 7: 完整功能验证清单**

1. ✅ MainForm 启动无异常
2. ✅ "存储配置"显示"拧紧数据存储路径"和"曲线数据存储路径"两行
3. ✅ "曲线抓取配置"只有 6 个按钮（无"是否替换"和"确认"）
4. ✅ 曲线路径为空时点"开始抓取"→ 弹警告，子窗口不弹出
5. ✅ 曲线路径浏览按钮可正常选择文件夹
6. ✅ 填写所有必填项后可正常开始抓取
7. ✅ 抓取过程中曲线路径控件被禁用
8. ✅ 断开后曲线路径控件恢复可用
9. ✅ 日志中可看到删除/跳过曲线文件的记录
10. ✅ 曲线文件被锁定时弹窗警告，向 PLC 报 ERROR
11. ✅ 旧 config.json 不会导致启动崩溃

- [ ] **Step 8: 提交**

```bash
git add STaTool/MainForm.cs
git commit -m "feat: wire up curve storage path with FetchConfig, move validation before child form"
```

---

## 任务依赖图

```
Task 1 (FetchConfig.cs)
  └── Task 4 (ImageButtonClickerToolForm.cs) — 依赖 Task 1

Task 2 (Config.cs) — 独立
  ├── Task 4 (ImageButtonClickerToolForm.cs) — 依赖 Task 2
  └── Task 5 (MainForm.cs) — 依赖 Task 2

Task 3 (MainForm.Designer.cs) — 独立
  └── Task 5 (MainForm.cs) — 依赖 Task 3

Task 4 (ImageButtonClickerToolForm.cs) — 依赖 Task 1, 2
  └── Task 5 (MainForm.cs) — 依赖 Task 4
```

执行顺序: **1 → 2 → 3 → 4 → 5**
