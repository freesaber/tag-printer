# Tag Printer — 快递鸟电子面单 / HTML 热敏打印组件

[![Windows](https://img.shields.io/badge/Windows-10%20%2F%2011-0078D4?logo=windows)](https://github.com/freesaber/tag-printer/releases)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-512BD4)](https://dotnet.microsoft.com/download/dotnet-framework/net472)
[![Release](https://img.shields.io/github/v/release/freesaber/tag-printer?display_name=tag)](https://github.com/freesaber/tag-printer/releases/latest)
[![License](https://img.shields.io/github/license/freesaber/tag-printer)](LICENSE)

一个运行在 Windows 本机的开源电子面单打印客户端。业务系统从**快递鸟**等电子面单服务取得 HTML 面单后，通过 WebSocket 将 HTML 发送到本程序，即可使用默认打印机静默打印；普通 HTML 标签、小票和仓库拣货单也能打印。

> 本项目不是快递鸟官方产品，不负责申请电子面单、下单或获取快递单号；它解决的是“已经拿到 HTML 面单，如何从网页调用本地热敏打印机打印”的最后一步。

<p align="center">
  <img src="assets/tag-printer.png" width="180" alt="Tag Printer 图标">
</p>

## 下载与安装

### 普通用户

1. 进入 [Releases](https://github.com/freesaber/tag-printer/releases/latest)，下载最新版 `TagPrinter-Setup-x64.exe`。
2. 如果电脑尚未安装运行库，请先安装：
   - [.NET Framework 4.7.2 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net472)
   - [Microsoft Visual C++ 2015–2022 Redistributable x64](https://learn.microsoft.com/cpp/windows/latest-supported-vc-redist)
3. 双击安装包完成安装。

目前仅支持 **Windows 10/11 64 位**。安装包未购买商业代码签名证书，Windows 首次运行时可能显示 SmartScreen 提示；请确认下载来源是本仓库的 Release，再选择“更多信息 → 仍要运行”。

### 5 分钟跑通打印

1. 安装好热敏打印机驱动，并将要使用的打印机设为 Windows **默认打印机**。
2. 启动 `Tag Printer`。
3. 按实际面单尺寸填写宽和高，单位为毫米，例如 `100 × 150` 或 `75 × 120`。设置会在退出时保存。
4. 下载并用浏览器打开仓库中的 [`Helper/打印调用示例程序.html`](Helper/%E6%89%93%E5%8D%B0%E8%B0%83%E7%94%A8%E7%A4%BA%E4%BE%8B%E7%A8%8B%E5%BA%8F.html)。
5. 页面加载后会自动连接本机打印组件，点击“发送数据”。

正常链路如下：

```text
业务系统 / 测试网页
        │  HTML 字符串
        ▼
ws://127.0.0.1:5597
        │
        ▼
Tag Printer → 生成 PDF → Windows 默认打印机
```

## 对接快递鸟电子面单

典型用法是：服务端调用快递鸟电子面单接口，取得返回结果里的 HTML 面单内容；浏览器端再把这段 HTML 原样发送给本机打印组件。

```javascript
// printTemplate 是业务后端返回给前端的 HTML 面单字符串。
// 不要把包含它的整个 JSON 对象直接发送给打印组件。
function printWaybill(printTemplate) {
  const socket = new WebSocket('ws://127.0.0.1:5597');

  socket.addEventListener('open', () => {
    socket.send(printTemplate);
    socket.close();
  });

  socket.addEventListener('error', () => {
    alert('无法连接本地打印组件，请确认 Tag Printer 已启动');
  });
}
```

快递鸟返回字段和调用方式可能随接口版本变化，请以你正在使用的快递鸟接口文档为准。本项目只要求最终传入的是可渲染的 HTML 字符串，因此同样适用于其他电子面单平台、自建仓系统、ERP、WMS 和电商后台。

## 通用 WebSocket API

| 项目 | 值 |
| --- | --- |
| 地址 | `ws://127.0.0.1:5597` |
| 消息方向 | 网页 → 打印组件 |
| 消息格式 | UTF-8 HTML 字符串 |
| 打印机 | Windows 默认打印机 |
| 纸张尺寸 | 在客户端界面中设置，单位 mm |

可以发送 HTML 片段：

```javascript
socket.send(`
  <div style="width:100mm;height:150mm;font-size:16px">
    <h1>测试面单</h1>
    <p>订单号：TEST-2026-001</p>
  </div>
`);
```

也可以发送完整 HTML 文档。为了得到稳定的打印尺寸，建议在完整文档中加入：

```css
@page {
  size: 100mm 150mm;
  margin: 0;
}

html,
body {
  margin: 0;
  padding: 0;
}
```

## 常见问题

### 提示打印机未连接，或 WebSocket 连接失败

- 确认 `Tag Printer` 正在运行。
- 刷新业务网页后重试。
- 确认其他程序没有占用 `5597` 端口。
- 网页如果运行在 HTTPS 下，浏览器可能限制连接不安全的 `ws://` 地址；请先在目标浏览器中验证部署环境。

### 点击打印后没有出纸

- 确认热敏打印机已开机、驱动正常，并已设为 Windows 默认打印机。
- 先用 Windows 测试页排除打印机或驱动问题。
- 确认客户端内填写的宽高都是大于 0 的数字。

### 打印缩放、偏移或分页不正确

- 客户端尺寸必须与标签纸实际尺寸一致。
- HTML 使用毫米（`mm`）作为尺寸单位，并设置 `@page` 的 `margin: 0`。
- 去掉浏览器默认的 `body` 外边距。
- 检查打印机驱动中是否又启用了“适应页面”或额外边距。

### 如何开机自动启动

重新运行安装包并勾选“开机自动启动”，或按 `Win + R` 输入 `shell:startup`，把 Tag Printer 的快捷方式放入打开的目录。

## 功能与边界

- 支持快递鸟等平台返回的 HTML 电子面单，以及任意可信 HTML 内容。
- 使用 CefSharp/Chromium 渲染 HTML，再通过 PDF 交给默认打印机。
- 支持自定义标签宽高，并自动保存设置。
- 本地 WebSocket 接口无需浏览器插件。
- 当前版本不提供打印机选择、打印队列回执、鉴权和远程打印。
- 请只打印可信来源的 HTML，不要把此服务暴露到公网。

## 从源码构建

环境要求：Visual Studio 2022、`.NET desktop development` 工作负载、.NET Framework 4.7.2 Developer Pack、NuGet，以及 Inno Setup 6（仅打安装包时需要）。

```powershell
nuget restore Printer/Printer.sln -PackagesDirectory Printer/packages
msbuild Printer/Printer.sln /m /p:Configuration=Release /p:Platform=x64
iscc Helper/PrintForm.iss
```

构建输出位于 `Printer/PrinterForm/bin/x64/Release`，安装包输出位于 `dist/TagPrinter-Setup-x64.exe`。

## 发布安装包

仓库已提供 GitHub Actions 工作流：

- 手动运行 `Build and Release`，可在该次 Actions 的 Artifacts 中下载测试安装包和免安装压缩包。
- 推送形如 `v1.1.0` 的 Git 标签，会自动构建并创建 GitHub Release，同时上传 `TagPrinter-Setup-x64.exe` 和 `TagPrinter-portable-x64.zip`。

```powershell
git tag v1.1.0
git push origin v1.1.0
```

## 项目结构

```text
Printer/                         Visual Studio 解决方案与 WinForms 客户端
Helper/打印调用示例程序.html      可直接运行的 WebSocket 打印示例
Helper/PrintForm.iss             Inno Setup 安装脚本
.github/workflows/release.yml    自动构建与发布安装包
```

## 技术栈

- C# / WinForms / .NET Framework 4.7.2
- CefSharp
- Fleck WebSocket Server
- PdfiumViewer
- Inno Setup

## License

[MIT License](LICENSE)

---

搜索关键词：快递鸟打印、快递鸟电子面单、电子面单打印、快递单打印、HTML 面单打印、热敏打印机、WebSocket 本地打印、快递打印组件、WMS 打印、ERP 打印、KDNiao、shipping label printer、waybill printing、HTML printing。
