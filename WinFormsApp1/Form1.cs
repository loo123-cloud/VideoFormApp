using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Configuration;
using Microsoft.Web.WebView2.WinForms;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public static class Win32Func
        {
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string className, string winName);

            [DllImport("user32.dll")]
            public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlage, uint timeout, IntPtr result);

            //���Ҵ��ڵ�ί�� �����߼�
            public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern bool EnumWindows(EnumWindowsProc proc, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string winName);

            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

            [DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwnd, IntPtr parentHwnd);
            [DllImport("user32.dll")]
            public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);
            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        }
        private IntPtr programHandle;
        public void SendMsgToProgman()
        {
            // ���洰�ھ�������ⲿ���壬���ں��潫�����Լ��Ĵ�����Ϊ�Ӵ��ڷ���
            programHandle = Win32Func.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;
            // �� Program Manager ���ڷ�����Ϣ 0x52c ��һ����Ϣ����ʱ����Ϊ2��
            Win32Func.SendMessageTimeout(programHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);

            // ������������
            Win32Func.EnumWindows((hwnd, lParam) =>
            {
                // �ҵ���һ�� WorkerW ���ڣ��˴��������Ӵ��� SHELLDLL_DefView�����������Ӵ���
                if (Win32Func.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    // �ҵ���ǰ��һ�� WorkerW ���ڵģ���һ�����ڣ����ڶ��� WorkerW ���ڡ�
                    IntPtr tempHwnd = Win32Func.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                    // ���صڶ��� WorkerW ����
                    Win32Func.ShowWindow(tempHwnd, 0);
                    /*programHandle = tempHwnd;*/
                    /*programHandle = Win32Func.FindWindowEx(hwnd, IntPtr.Zero, null, null);*/
                }
                return true;
            }, IntPtr.Zero);
        }
        IntPtr haPid = Process.GetCurrentProcess().Handle;
        uint uiPid = (uint)Process.GetCurrentProcess().Id;
        /*        public IntPtr MainWindowHandle { get ; }*/
        private IntPtr hwnd = IntPtr.Zero;
        StringBuilder MyCN = new StringBuilder(256);

        public Form1()
        {
            InitializeComponent();
            SendMsgToProgman();
            System.Timers.Timer timerSwipe = new System.Timers.Timer();
            timerSwipe.Interval = 1000;
            timerSwipe.AutoReset = true;//�ظ�
            timerSwipe.Elapsed += TimerSwipe_Elapsed;
            timerSwipe.Start();


            /*this.Text = "ovo";*/
            // ���õ�ǰ����Ϊ Program Manager���Ӵ���
            /*DialogResult dr = MessageBox.Show(haPid.ToString(), hwnd.ToString());*/
        }

        public struct RECT
        {
            public int Left;                             //��������
            public int Top;                             //��������
            public int Right;                           //��������
            public int Bottom;                        //��������
        }
        IntPtr hwndTmp = IntPtr.Zero;
        int b = (int)(Screen.PrimaryScreen.WorkingArea.Height * Screen.PrimaryScreen.WorkingArea.Width * 0.96);
        private void TimerSwipe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //����д��ʱ�����¼�...
            if (hwndTmp != IntPtr.Zero)
            {
                RECT rect = new RECT();
                Win32Func.GetWindowRect(hwndTmp, ref rect);
                int c = (rect.Right - rect.Left) * (rect.Bottom - rect.Top);//�������
                if (c < b)
                {
                    hwndTmp = IntPtr.Zero;
                    //����ui�߳�
                    this.Invoke(() =>
                    {
                        webView21.CoreWebView2.ExecuteScriptAsync(MinWin);
                    });
                }
            }
            else
            {
                IntPtr hwnd = Win32Func.GetForegroundWindow();
                if (hwnd != IntPtr.Zero)
                {
                    StringBuilder cn = new StringBuilder(256);
                    Win32Func.GetClassName(hwnd, cn, 256);
                    string hvn = cn.ToString();
                    if (hvn != "_cls_desk_" && hvn != "WorkerW" && hvn != MyCN.ToString() && hvn != "Windows.UI.Core.CoreWindow")
                    {
                        RECT rect = new RECT();
                        Win32Func.GetWindowRect(hwnd, ref rect);
                        int a = (rect.Right - rect.Left) * (rect.Bottom - rect.Top);//�������
                        int x = rect.Right - rect.Left;
                        int y = rect.Bottom - rect.Top;
                        if (a > b)
                        {
                            hwndTmp = hwnd;
                            /*webView21.CoreWebView2.ExecuteScriptAsync(MaxWin);*/
                            /*DialogResult dr = MessageBox.Show(x.ToString(),y.ToString());*/
                            //����ui�߳�
                            this.Invoke(() =>
                            {
                                webView21.CoreWebView2.ExecuteScriptAsync(MaxWin);
                            });
                        }

                    }
                }
            }
        }
        public static string url = ConfigurationManager.ConnectionStrings["htmlurl"].ConnectionString;
        public static string MinWin = ConfigurationManager.ConnectionStrings["MinWin"].ConnectionString;
        public static string MaxWin = ConfigurationManager.ConnectionStrings["MaxWin"].ConnectionString;
        public static string Refresh = ConfigurationManager.ConnectionStrings["Refresh"].ConnectionString;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White; 
            this.TransparencyKey = Color.White;
            hwnd = Win32Func.FindWindow(null, this.Text);
            Win32Func.SetParent(hwnd, programHandle);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            /*DialogResult dr = MessageBox.Show(a.ToString());*/
            webView21.Source = new Uri(url);
            //�ڴ��ڹ��غ��ٻ�ȡ���
            IntPtr hwndMy = Win32Func.GetForegroundWindow();
            Win32Func.GetClassName(hwndMy, MyCN, 256);


        }

        private void �˳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ˢ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            url = ConfigurationManager.ConnectionStrings["htmlurl"].ConnectionString;
            MinWin = ConfigurationManager.ConnectionStrings["MinWin"].ConnectionString;
            MaxWin = ConfigurationManager.ConnectionStrings["MaxWin"].ConnectionString;
            webView21.CoreWebView2.ExecuteScriptAsync(Refresh);
            hwndTmp = IntPtr.Zero;
        }


    }
}