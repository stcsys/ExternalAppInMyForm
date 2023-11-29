using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormsApp22
{
    public partial class Form1 : Form
    {
        // DLL インポート用の定数とメソッド
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        // 外部アプリケーションのプロセス
        private Process externalProcess;

        // DLL インポート用の定数とメソッド
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        // フォームが閉じられるときに外部アプリケーションも終了する
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (externalProcess != null && !externalProcess.HasExited)
            {
                externalProcess.CloseMainWindow();
                externalProcess.WaitForExit();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 外部アプリケーションのパスを指定
            string externalAppPath = "notepad.exe";

            // 外部アプリケーションを開始
            externalProcess = Process.Start(externalAppPath);

            // 外部アプリケーションのウィンドウハンドルを取得し、自分のフォームに埋め込む
            if (externalProcess != null)
            {
                IntPtr externalAppHandle = externalProcess.MainWindowHandle;
                SetParent(externalAppHandle, this.Handle);

                // 外部アプリケーションのウィンドウを自分のフォーム内に表示
                SetWindowPos(externalAppHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
            }
        }
    }
}