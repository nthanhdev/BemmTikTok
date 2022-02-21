using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class frmgetcode : Form
    {
        public frmgetcode()
        {
            InitializeComponent();
        }
        Mail mail;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || !textBox1.Text.Contains("@"))
            {
                MessageBox.Show("Email không đúng định dạng");
            }

            Thread r = new Thread(() =>
            {
                if (guna2Button2.Text == "KÍCH HOẠT EMAIL")
                {
                    var email = textBox1.Text.Split('@');
                    mail = new Mail(email[1], Properties.Settings.Default.gid);
                    if (mail.getitemMail(email[0]) == textBox1.Text)
                    {

                        guna2Button2.Invoke(new Action(() => guna2Button2.Text = "Lấy Code"));
                    }else
                    {
                        MessageBox.Show("Email không đúng định dạng");
                        guna2Button2.Invoke(new Action(() => guna2Button2.Text = "KÍCH HOẠT EMAIL"));


                    }
                }
                else if (guna2Button2.Text == "Lấy Code")
                {
                    guna2Button2.Invoke(new Action(() => guna2Button2.Text = "Đang lấy code"));

                    string code = mail.WailgetCode(textBox1.Text,true);

                    guna2Button2.Invoke(new Action(() =>
                    {
                        if (code == "")
                        {
                            guna2HtmlLabel1.Text = "Không tìm thấy email nào";
                        }
                        else
                        {
                            code = code.Replace("\n", "");
                            code = code.Replace("\r", "");
                            code = code.Replace("\t", "");
                            code = code.Replace(@"\", "");

                            guna2HtmlLabel1.Text = code;
                        }
                        guna2Button2.Text = "KÍCH HOẠT EMAIL";

                    }));

                }

            });
            r.IsBackground = true;
            r.Start();

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
