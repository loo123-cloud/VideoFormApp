using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Configuration;


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

            //查找窗口的委托 查找逻辑
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

        private IntPtr tempHwnd;

        public void SendMsgToProgman()
        {
            // 桌面窗口句柄，在外部定义，用于后面将我们自己的窗口作为子窗口放入
            IntPtr programHandle = Win32Func.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;
            // 向 Program Manager 窗口发送消息 0x52c 的一个消息，超时设置为2秒
            Win32Func.SendMessageTimeout(programHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);

            // 遍历顶级窗口
            Win32Func.EnumWindows((hwnd, lParam) =>
            {
                // 找到第一个 WorkerW 窗口，此窗口中有子窗口 SHELLDLL_DefView，所以先找子窗口
                if (Win32Func.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    // 找到当前第一个 WorkerW 窗口的，后一个窗口，及第二个 WorkerW 窗口。
                    tempHwnd = Win32Func.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                    // 隐藏第二个 WorkerW 窗口
                    /*Win32Func.ShowWindow(tempHwnd, 0);*/
                }
                return true;
            }, IntPtr.Zero);
        }
        IntPtr haPid = Process.GetCurrentProcess().Handle;
        uint uiPid = (uint)Process.GetCurrentProcess().Id;
        private IntPtr hwnd = IntPtr.Zero;
        StringBuilder MyCN = new StringBuilder(256);
        String MyCNString = "";
        public Form1()
        {
            InitializeComponent();
            SendMsgToProgman();
            System.Timers.Timer timerSwipe = new System.Timers.Timer();
            timerSwipe.Interval = Timei;
            timerSwipe.AutoReset = true;//重复
            timerSwipe.Elapsed += TimerSwipe_Elapsed;
            timerSwipe.Start();
        }

        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        IntPtr hwndTmp = IntPtr.Zero;
        int winH = (int)Screen.PrimaryScreen.WorkingArea.Height;
        int winW = (int)Screen.PrimaryScreen.WorkingArea.Width;
        int b;
        //判断窗口是否全屏
        private void TimerSwipe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //这里写定时处理事件...
            if (hwndTmp != IntPtr.Zero)
            {
                RECT rect = new RECT();
                Win32Func.GetWindowRect(hwndTmp, ref rect);
                int x = rect.Right, y = rect.Bottom, z = rect.Left;
                if (winW < x) { x = winW; }
                if (winH < y) { y = winH; }
                if (z < 0) { z = 0; }
                int a = (x - z) * (y - rect.Top);//窗口面积
                if (a < b)
                {
                    hwndTmp = IntPtr.Zero;
                    //返回ui线程
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
                    //排除窗口
                    if (hvn != "_cls_desk_" && hvn != "WorkerW" && hvn != MyCNString && hvn != "Windows.UI.Core.CoreWindow")
                    {
                        RECT rect = new RECT();
                        Win32Func.GetWindowRect(hwnd, ref rect);
                        int x = rect.Right, y = rect.Bottom, z = rect.Left;
                        if (winW < x) { x = winW; }
                        if (winH < y) { y = winH; }
                        if (z < 0) { z = 0; }
                        int a = (x - z) * (y - rect.Top);//窗口面积
                        if (a > b)
                        {
                            hwndTmp = hwnd;
                            //返回ui线程
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
        public static int Timei = int.Parse(ConfigurationManager.AppSettings["TimeInterval"]);
        private void Form1_Load(object sender, EventArgs e)
        {
            b = (int)(winH * winW * 0.94);
            //设置form1透明
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
            // 设置当前窗口为 WorkerW的子窗口
            hwnd = Win32Func.FindWindow(null, this.Text);
            Win32Func.SetParent(hwnd, tempHwnd);
            this.FormBorderStyle = FormBorderStyle.None;
            /*this.WindowState = FormWindowState.Maximized;*/
            this.Left = 0;
            this.Top = 0;
            Width = winW; Height = winH;
            /*DialogResult dr = MessageBox.Show(a.ToString());*/
            webView21.Source = new Uri(url);
            //在窗口挂载后再获取句柄
            IntPtr hwndMy = Win32Func.GetForegroundWindow();
            Win32Func.GetClassName(hwndMy, MyCN, 256);
            MyCNString = MyCN.ToString();


        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //刷新
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            url = ConfigurationManager.ConnectionStrings["htmlurl"].ConnectionString;
            MinWin = ConfigurationManager.ConnectionStrings["MinWin"].ConnectionString;
            MaxWin = ConfigurationManager.ConnectionStrings["MaxWin"].ConnectionString;
            webView21.CoreWebView2.ExecuteScriptAsync(Refresh);
            hwndTmp = IntPtr.Zero;
        }
    }
}