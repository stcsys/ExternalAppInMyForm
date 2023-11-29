using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormsApp22
{
    public partial class Form1 : Form
    {
        // DLL �C���|�[�g�p�̒萔�ƃ��\�b�h
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        // �O���A�v���P�[�V�����̃v���Z�X
        private Process externalProcess;

        // DLL �C���|�[�g�p�̒萔�ƃ��\�b�h
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        // �t�H�[����������Ƃ��ɊO���A�v���P�[�V�������I������
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
            // �O���A�v���P�[�V�����̃p�X���w��
            string externalAppPath = "notepad.exe";

            // �O���A�v���P�[�V�������J�n
            externalProcess = Process.Start(externalAppPath);

            // �O���A�v���P�[�V�����̃E�B���h�E�n���h�����擾���A�����̃t�H�[���ɖ��ߍ���
            if (externalProcess != null)
            {
                IntPtr externalAppHandle = externalProcess.MainWindowHandle;
                SetParent(externalAppHandle, this.Handle);

                // �O���A�v���P�[�V�����̃E�B���h�E�������̃t�H�[�����ɕ\��
                SetWindowPos(externalAppHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
            }
        }
    }
}