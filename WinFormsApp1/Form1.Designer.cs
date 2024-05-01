namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            mainNotifyIcon = new NotifyIcon(components);
            mainNotifyContextMenuStrip = new ContextMenuStrip(components);
            刷新ToolStripMenuItem = new ToolStripMenuItem();
            退出ToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            mainNotifyContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.BackColor = SystemColors.ControlLightLight;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(0, 0);
            webView21.Margin = new Padding(3, 2, 3, 2);
            webView21.Name = "webView21";
            webView21.Size = new Size(799, 450);
            webView21.TabIndex = 0;
            webView21.TabStop = false;
            webView21.ZoomFactor = 1D;
            // 
            // mainNotifyIcon
            // 
            mainNotifyIcon.ContextMenuStrip = mainNotifyContextMenuStrip;
            mainNotifyIcon.Icon = (Icon)resources.GetObject("mainNotifyIcon.Icon");
            mainNotifyIcon.Text = "VideoForm";
            mainNotifyIcon.Visible = true;
            // 
            // mainNotifyContextMenuStrip
            // 
            mainNotifyContextMenuStrip.ImageScalingSize = new Size(20, 20);
            mainNotifyContextMenuStrip.Items.AddRange(new ToolStripItem[] { 刷新ToolStripMenuItem, 退出ToolStripMenuItem });
            mainNotifyContextMenuStrip.Name = "mainNotifyContextMenuStrip";
            mainNotifyContextMenuStrip.Size = new Size(101, 48);
            // 
            // 刷新ToolStripMenuItem
            // 
            刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            刷新ToolStripMenuItem.Size = new Size(100, 22);
            刷新ToolStripMenuItem.Text = "刷新";
            刷新ToolStripMenuItem.Click += 刷新ToolStripMenuItem_Click;
            // 
            // 退出ToolStripMenuItem
            // 
            退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            退出ToolStripMenuItem.Size = new Size(100, 22);
            退出ToolStripMenuItem.Text = "退出";
            退出ToolStripMenuItem.Click += 退出ToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(799, 450);
            ControlBox = false;
            Controls.Add(webView21);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            ShowIcon = false;
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            mainNotifyContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private NotifyIcon mainNotifyIcon;
        private ContextMenuStrip mainNotifyContextMenuStrip;
        private ToolStripMenuItem 刷新ToolStripMenuItem;
        private ToolStripMenuItem 退出ToolStripMenuItem;
    }
}