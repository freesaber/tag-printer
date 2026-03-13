# 快递面单打印组件

基于 C# WinForms 开发的快递面单打印客户端，通过 WebSocket 接收打印指令，支持自定义纸张尺寸。

## 功能特点

- 🖨️ **快递面单打印** - 支持打印快递面单、小票等
- 🔌 **WebSocket 服务** - 监听 `ws://localhost:5597`，接收 HTML 内容直接打印
- 📐 **自定义纸张** - 可设置打印纸张宽度和高度（单位：mm）
- 🌐 **内嵌浏览器** - 使用 CefSharp 渲染 HTML，确保打印效果一致
- 💾 **配置保存** - 自动保存纸张尺寸设置

## 技术栈

- **.NET Framework** 4.x
- **WinForms** - Windows 桌面应用
- **CefSharp** - Chromium 嵌入式浏览器
- **Fleck** - WebSocket 服务器
- **PdfiumViewer** - PDF 打印

## 项目结构

```
Printer/
├── Printer.sln              # 解决方案文件
├── PrinterForm/             # 主项目
│   ├── Program.cs           # 程序入口
│   ├── frmPrinter.cs        # 主窗体逻辑
│   ├── frmPrinter.Designer.cs
│   ├── App.config           # 应用配置
│   └── Properties/
├── Helper/
│   ├── 打印调用示例程序.html  # WebSocket 调用示例
│   └── PrintForm.iss        # Inno Setup 安装脚本
```

## 使用方法

### 1. 启动程序

运行 `PrinterForm.exe`，程序会自动启动 WebSocket 服务。

### 2. 设置纸张尺寸

在界面上输入纸张的宽度和高度（单位：毫米），例如：
- 快递面单：75mm × 120mm
- 小票：58mm × 100mm

### 3. 发送打印指令

通过 WebSocket 连接 `ws://localhost:5597`，发送 HTML 内容即可触发打印：

```javascript
const socket = new WebSocket('ws://localhost:5597');

socket.onopen = () => {
    socket.send('<div>打印内容</div>');
};
```

## API

### WebSocket 端点

- **地址**: `ws://localhost:5597`
- **协议**: WebSocket
- **消息格式**: HTML 字符串

### 发送消息

直接发送 HTML 内容，程序会自动包装为完整 HTML 文档并打印：

```javascript
// 简单文本
socket.send('<div style="font-size:24px;">Hello World</div>');

// 完整 HTML（会直接使用）
socket.send('<!DOCTYPE html><html>...</html>');
```

## 编译

1. 使用 Visual Studio 打开 `Printer/Printer.sln`
2. 还原 NuGet 包
3. 编译项目

## 打包安装

使用 Inno Setup 打包：

```bash
iscc Helper/PrintForm.iss
```

## 依赖

- [CefSharp.WinForms](https://github.com/cefsharp/CefSharp)
- [Fleck](https://github.com/statianzo/Fleck)
- [PdfiumViewer](https://github.com/pvginkel/PdfiumViewer)

## 许可证

MIT License

## 作者

freesaber
