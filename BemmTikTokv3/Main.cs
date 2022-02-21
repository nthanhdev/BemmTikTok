using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Auto_LDPlayer;
using Proxy_Client_Tinsoft;
using KAutoHelper;
using LDPlayer = Auto_LDPlayer.LDPlayer;

namespace BemmTikTokv3
{
    public partial class Main : Form
    {
     
        public Main(bool check)
        { 
            InitializeComponent();
            if (check)
            {
                lblxacnhan.Text = "Đã kích hoạt";
            }
            else
            {
                lblxacnhan.Text = "Chưa kích hoạt";
                lblxacnhan.ForeColor = Color.DarkRed;
            }
            this.check = check;
        }
        private bool check;
        void msgBox(string text)
        {
            MessageBox.Show(text);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        private void btnclose_Click(object sender, EventArgs e)
        {
            Close();
        }

        
        private void button1_Click_1(object sender, EventArgs e)
        {
            Auto_LDPlayer.LDPlayer ldplayer = new LDPlayer();
            LDPlayer.pathLD = @"D:\LDPlayer\LDPlayer4.0\ldconsole.exe";
            ldplayer.InstallApp_File("name", "BEMM", Application.StartupPath + @"\TikTok.apk");
        }
     
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            load();
            guna2ShadowForm1.SetShadowForm(this);
        }
        void load()
        {
            Thread r = new Thread(() =>
            {
                tabControl1.Invoke(new Action(() =>
                {

                    Freg freg = new Freg();
                    FQuanLyAccount faccount = new FQuanLyAccount();
                    faccount.Dock = DockStyle.Fill;
                    tabPage1.Controls.Add(freg);
                    freg.Dock = DockStyle.Fill;
                    freg.BringToFront();

                    tabPage3.Controls.Add(faccount);
                    flowLayoutPanel1.Invoke(new Action(() =>
                    {
                        FNuoiTikTok ftiktok = new FNuoiTikTok(flowLayoutPanel1);
                        ftiktok.Dock = DockStyle.Fill;
                        tabPage5.Controls.Add(ftiktok);
                        lblv.Text = File.ReadAllText("version.txt");
                    }));
                    this.Show();
                
                }));
            

            });
            r.IsBackground = true;
            r.Start();
      

        }
      

        private void button1_Click(object sender, EventArgs e)
        {
          

            //this.WindowState = FormWindowState.Maximized;
            //tabControl1.Size = this.Size;
            //float widthRatio = Screen.PrimaryScreen.Bounds.Width / 1280;
            //float heightRatio = Screen.PrimaryScreen.Bounds.Height / 800f;
            //SizeF scale = new SizeF(widthRatio, heightRatio);
            //tabControl1.Dock = DockStyle.Fill;
            //foreach (Control control in this.Controls)
            //{
            //    control.Font = new Font("Verdana", control.Font.SizeInPoints * heightRatio * widthRatio);
            //}
            //foreach (TabPage pages in tabControl1.TabPages)
            //{
            //    foreach (UserControl control in pages.Controls)
            //    {
            //        control.Dock = DockStyle.Fill;
            //        foreach (Control item in control.Controls)
            //        {
            //            float width = tabPage1.Size.Width / 1280;
            //            float height = tabPage1.Size.Height / 800f;
            //            SizeF scalee = new SizeF(width, height);
            //            item.Scale(scalee);
            //        }
            //    }
            //}
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
     

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
             Process[] processes = Process.GetProcessesByName("chromedriver");
            foreach (var process in processes)
            {
                process.Kill();
            }
            Process[] processess = Process.GetProcessesByName("adb");
            foreach (var process in processess)
            {
                process.Kill();
            }
            Environment.Exit(0);

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnKichHoat_Click(object sender, EventArgs e)
        {
            frmKey frm = new frmKey();
            frm.ShowDialog();
            if (frm.isSuccess)
            {
                Properties.Settings.Default.check = true;
                Properties.Settings.Default.Save();
                lblxacnhan.Text = "Đã kích hoạt";
                lblxacnhan.ForeColor = Color.Lime;
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Process.Start("BemmTikUpdate.exe");
        }




        // ----------------------------------- CODE TAB DOWNLOADTIKTOK-------------------------------





    }
}



// lấy truy vấn thông tin user  view-source:https://urlebird.com/user/hongtruc264/
//(?<=class="thumb">).*?(?=overlay)

//https://tik.sandiwara.id/

//(?<=></i> ).*?(?=<)


//com.ss.android.ugc.trill.go_2020-05-07