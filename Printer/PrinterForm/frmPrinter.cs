using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fleck;
using Microsoft.Win32;
using CefSharp;
using CefSharp.WinForms;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using PdfiumViewer;

namespace PrinterForm
{
    public partial class frmPrinter : Form
    {
        private WebSocketServer server;
        private ChromiumWebBrowser browser;

        public frmPrinter()
        {
            InitializeComponent();
            InitializeChromium();
            this.Text = "快递面单打印组件";
        }

        private void InitializeChromium()
        {
            var settings = new CefSettings();
            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser("about:blank")
            {
                Dock = DockStyle.Fill,
            };
            this.plBrowser.Controls.Add(browser);
        }

        private void StartWebSocket()
        {
            server = new WebSocketServer("ws://0.0.0.0:5597");
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message =>
                {
                    PrintHTML(message);
                };
            });
        }

        private void PrintHTML(string html)
        {
            // 取消之前的事件订阅
            browser.FrameLoadEnd -= OnFrameLoadEnd;

            // 获取用户输入的纸张尺寸并进行校验
            if (!ValidateDimensions(txtWidth.Text.Trim(), txtHeight.Text.Trim(), out double width, out double height))
            {
                // 如果校验失败，显示错误信息
                ShowError("请输入有效的纸张尺寸（宽度和高度必须为正数）。");
                return;
            }

            // 检查传入的html是否已经包含HTML标签
            if (!html.TrimStart().StartsWith("<!DOCTYPE html>", StringComparison.OrdinalIgnoreCase))
            {
                // 如果不包含HTML标签，添加HTML头部信息，确保使用UTF-8编码，并添加打印样式
                html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        @page {{
                            size: {width}mm {height}mm;
                            margin: 0;
                        }}
                        html,body {{
                            margin: 0;
                            padding: 0;
                        }}
                    </style>
                </head>
                <body>{html}</body>
                </html>";
            }

            browser.LoadHtml(html);
            // 等待页面加载完成并调用打印
            browser.FrameLoadEnd += OnFrameLoadEnd;
        }

        private async void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                // 确保在主框架加载完成后调用打印方法
                var pdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
                var printToPdfSettings = new CefSharp.PdfPrintSettings
                {
                    //  true 表示打印背景图形，设置为 false 表示不打印背景图形。
                    PrintBackground = false,
                    // 表示打印页眉和页脚，设置为 false 表示不打印页眉和页脚。
                    DisplayHeaderFooter = false,
                    // 设置为 true 表示横向模式，设置为 false 表示纵向模式。
                    Landscape = false,
                    // 设置为 true 表示优先使用 CSS 定义的页面大小。默认为 false，在这种情况下内容将缩放以适应纸张大小。
                    PreferCssPageSize = true,
                    // 打印前缩放 PDF 的百分比（例如 0.5 表示 50%）。如果该值小于或等于零，将使用默认值 1.0。
                    // Scale = 1.8,
                    // 页边距类型。
                    MarginType = CefPdfPrintMarginType.Custom,
                    MarginTop = 0,
                    MarginBottom = 0,
                    MarginLeft = 0,
                    MarginRight = 0,
                };

                var success = await browser.PrintToPdfAsync(pdfPath, printToPdfSettings);

                if (success)
                {
                    PrintPdf(pdfPath);
                }
                else
                {
                    ShowError("面单生成PDF失败");
                }
            }
        }

        private void ShowError(string errorMessage)
        {
            string errorHtml = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        margin: 0;
                    }}
                    .error-message {{
                        color: red;
                        font-size: 20px;
                        text-indent: 2em;
                    }}
                </style>
            </head>
            <body>
                <div class='error-message'>{errorMessage}</div>
            </body>
            </html>";
            browser.LoadHtml(errorHtml);
        }

        private bool ValidateDimensions(string widthText, string heightText, out double width, out double height)
        {
            bool isValidWidth = double.TryParse(widthText, out width) && width > 0;
            bool isValidHeight = double.TryParse(heightText, out height) && height > 0;

            return isValidWidth && isValidHeight;
        }

        private void PrintPdf(string pdfPath)
        {
            // 获取用户输入的纸张尺寸，并转换为百分之一英寸
            int paperWidth = (int)(double.Parse(txtWidth.Text.Trim()) / 25.4 * 100);
            int paperHeight = (int)(double.Parse(txtHeight.Text.Trim()) / 25.4 * 100);

            using (var document = PdfiumViewer.PdfDocument.Load(pdfPath))
            {
                using (var printDocument = document.CreatePrintDocument())
                {
                    // 设置自定义纸张尺寸
                    printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", paperWidth, paperHeight);
                    printDocument.DocumentName = Path.GetFileName(pdfPath); ;
                    printDocument.PrintController = new StandardPrintController();

                    // 打印结束时删除文件
                    printDocument.EndPrint += (sender, e) =>
                    {
                        // 使用Task.Run将操作放到后台，避免阻塞UI线程
                        Task.Run(async () =>
                        {
                            try
                            {
                                // 等待3秒钟。这个时间通常足够打印机驱动程序完成对文件的读取。
                                // 您可以根据实际测试情况调整这个延迟时间。
                                await Task.Delay(3000);

                                // 延迟后，执行删除操作
                                File.Delete(pdfPath);
                            }
                            catch (Exception)
                            {
                                // 如果延迟后删除仍然失败（虽然可能性很小），则忽略异常。
                                // 因为打印任务已经提交，不应因此打扰用户。
                            }
                        });
                    };

                    try
                    {
                        printDocument.Print();
                    }
                    catch (Exception ex)
                    {
                        ShowError($"打印失败: {ex.Message}");
                    }
                }
                document.Dispose();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfig();
            StartWebSocket();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
            server.Dispose();
            server = null;
            Cef.Shutdown();
        }


        private void LoadConfig()
        {
            // 从AppSettings加载配置信息
            string txtWidth = ConfigurationManager.AppSettings["txtWidth"];
            string txtHeight = ConfigurationManager.AppSettings["txtHeight"];

            // 设置txtLeft和txtTop的文本内容
            this.txtWidth.Text = txtWidth;
            this.txtHeight.Text = txtHeight;
        }


        private void SaveConfig()
        {
            // 使用AppSettings保存配置信息
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["txtWidth"].Value = txtWidth.Text.Trim() == "" ? "75" : txtWidth.Text.Trim();
            config.AppSettings.Settings["txtHeight"].Value = txtHeight.Text.Trim() == "" ? "120" : txtHeight.Text.Trim();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
