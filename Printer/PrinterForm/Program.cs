using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterForm
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 添加全局异常处理程序
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Application.Run(new frmPrinter());
        }

        // 处理UI线程异常
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show("发生了一个未处理的UI线程异常，详情已记录到日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 处理非UI线程异常
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException(e.ExceptionObject as Exception);
            MessageBox.Show("发生了一个未处理的非UI线程异常，详情已记录到日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 将异常信息记录到日志文件
        private static void LogException(Exception ex)
        {
            if (ex == null)
                return;

            string logFilePath = "error_log.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("时间: " + DateTime.Now.ToString());
                    writer.WriteLine("异常类型: " + ex.GetType().Name);
                    writer.WriteLine("消息: " + ex.Message);
                    writer.WriteLine("堆栈跟踪: " + ex.StackTrace);
                    writer.WriteLine(new string('-', 80));
                }
            }
            catch
            {
                // 如果写入日志文件失败，可以在此处理，例如显示消息框通知用户
                MessageBox.Show("无法写入日志文件。", "日志错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
