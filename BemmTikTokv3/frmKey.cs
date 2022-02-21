using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;

namespace BemmTikTokv3
{
    public partial class frmKey : Form
    {
        protected string key = "6190cb2541928b001768fe49";
        public frmKey()
        {
            InitializeComponent();
            txtuid.Text = getuid();
            txtuid.ReadOnly = true;
            txtuid.TextAlign = HorizontalAlignment.Center;
            txtkey.TextAlign = HorizontalAlignment.Center;
            txtkey.PlaceholderText = "Enter Key";
            txtid.Text = getid();

        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
         
          Memo_ckb.Checked =  Properties.Settings.Default.save;
            if (Memo_ckb.Checked)
            {
                txtkey.Text = Properties.Settings.Default.key;
                if (txtkey.Text != "")
                {
                    loadKey();
                }
                else
                {
                    pictureBox3.Visible = false;
                    guna2Transition1.AnimationType = Guna.UI2.AnimatorNS.AnimationType.Particles;
                    guna2Transition1.Show(pictureBox2);
                }

            }
            else
            {
                pictureBox3.Visible = false;
                guna2Transition1.AnimationType = Guna.UI2.AnimatorNS.AnimationType.Particles;
                guna2Transition1.Show(pictureBox2);
            }
               

        }
        string getuid()
        {
            //string key = "Win32_BIOS";
            //ManagementObjectSearcher selectvalue = new ManagementObjectSearcher("select * from " + key);
            //string serialId = "";
            //foreach (ManagementObject getserial in selectvalue.Get())
            //{
            //    serialId += getserial["SerialNumber"].ToString();
            //}
            return "FREE";
        }
        private IRestResponse rest(string li)
        {
            try
            {
                var res = new RestClient("https://" + key + ".mockapi.io/key/user/" + li);
                var method = new RestRequest(method: Method.GET);
                method.AddHeader("content-type", "application/json");
                IRestResponse result = res.Execute(method);
                return result;
            }
            catch 
            {
                return null;
            }
        }
        private void loadKey()
        {
            Thread r = new Thread(() =>
            {

                var result = rest(txtkey.Text);
                if (result.IsSuccessful && result != null)
                {
                    info = JsonConvert.DeserializeObject<user>(result.Content);
                    if (info.uid != txtuid.Text)
                    {
                        MessageBox.Show("Key không đúng máy đã mua", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        invoBtn(txtkey, "");
                        Properties.Settings.Default.key = "";
                        Properties.Settings.Default.Save();

                    }
                    else
                    {
                        invoBtn(lbltime, info.time, true);
                        invoBtn(Login_btn, "HỦY KÍCH HOẠT", true);
                        Login_btn.Invoke(new Action(() =>
                        {
                            this.Login_btn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(62)))), ((int)(((byte)(103)))));
                            this.Login_btn.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(52)))));

                        }));
                    }
                }
                else
                {
                    MessageBox.Show("Key không đúng hoặc hết hạn", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                pictureBox2.Invoke(new Action(() => {
                    guna2Transition1.AnimationType = Guna.UI2.AnimatorNS.AnimationType.Mosaic;
             
                    guna2Transition1.Hide(pictureBox3);
               
                    pictureBox2.Visible = true;
                 
                }));
            });
            r.IsBackground = true;
            r.Start();
        }
        void invoBtn(Control control, string text, bool Enable = true)
        {
            control.Invoke(new Action(() => { control.Text = text; control.Enabled = Enable;  control.Visible = true; }));
        }
        private static user info;
        private void Login_btn_Click(object sender, EventArgs e)
        {
            if (Login_btn.Text == "XÁC NHẬN")
            {
                if (txtkey.Text != "")
                {
                    Thread r = new Thread(() =>
                    {

                        invoBtn(Login_btn, "Đang kiểm tra key", false);
                        try
                        {

                            var result = rest(txtkey.Text);
                            if (result.IsSuccessful && result != null)
                            {
                                info = JsonConvert.DeserializeObject<user>(result.Content);
                                if (info.uid != txtuid.Text)
                                {
                                    MessageBox.Show("Key không đúng máy đã mua", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    invoBtn(Login_btn, "XÁC NHẬN");

                                }
                                else
                                {
                                    MessageBox.Show("Kích hoạt key thành công cảm ơn bạn đã tin tưởng! ", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Properties.Settings.Default.key = txtkey.Text;
                                    Properties.Settings.Default.save = Memo_ckb.Checked;
                                    Properties.Settings.Default.check = true;
                                    check = true;
                                    Properties.Settings.Default.Save();
                                    invoBtn(lbltime, info.time, true);
                                    invoBtn(Login_btn, "HỦY KÍCH HOẠT", true);
                                    Login_btn.Invoke(new Action(() =>
                                    {
                                        this.Login_btn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(62)))), ((int)(((byte)(103)))));
                                        this.Login_btn.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(52)))));

                                    }));
                                   
                                }
                            }
                            else
                            {
                                MessageBox.Show("Key không đúng hoặc hết hạn", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                invoBtn(Login_btn, "XÁC NHẬN");

                            }

                        }
                        catch (Exception es)
                        {

                            MessageBox.Show(es.Message, "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            invoBtn(Login_btn, "XÁC NHẬN");


                        }


                    });
                    r.IsBackground = true;
                    r.Start();
                }
              
            }
            else if (Login_btn.Text == "HỦY KÍCH HOẠT")
            {
                if (MessageBox.Show("Bạn có chắc hủy kích hoạt key này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                {
                    Properties.Settings.Default.key = "";
                    Properties.Settings.Default.Save();
                    lbltime.Visible = false;
                    txtkey.Text = "";
                    this.Login_btn.FillColor = System.Drawing.Color.LightSeaGreen;
                    this.Login_btn.FillColor2 = System.Drawing.Color.LightSeaGreen;
                    Login_btn.Text = "XÁC NHẬN";
                }
            }
        }
        private bool check = false;
       public bool isSuccess
        {
            get
            {
                return check;
            }
                
        }

        private void lblbuykey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            guna2Transition1.AnimationType = Guna.UI2.AnimatorNS.AnimationType.Leaf;
            guna2Transition1.Interval = 10;
            guna2Transition1.Show(panelbuy);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            guna2Transition1.AnimationType = Guna.UI2.AnimatorNS.AnimationType.Leaf;
            guna2Transition1.Hide(panelbuy);
        }
      private string getid()
        {
            Random r = new Random();
            int id = r.Next(10000, 99999);
            return "BT" + id.ToString();
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                string text = string.Format("Bạn đã xác nhận thanh toán thành công với mã máy: {0} \nmã giao dịch: {1}", txtuid.Text, txtid.Text);
                text = HttpUtility.UrlEncode(text);
                string url = "https://m.me/bemmteam/?ref=" + text;
                Process.Start(url);
            }
            else MessageBox.Show("Vui lòng nhập mã giao dịch", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void lbltime_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void frmLoading_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.save = Memo_ckb.Checked;
            Properties.Settings.Default.key = txtkey.Text;

            Properties.Settings.Default.Save();

        }
    }
}
