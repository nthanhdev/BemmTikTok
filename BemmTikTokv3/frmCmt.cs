using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class frmCmt : Form
    {
        public frmCmt()
        {
            InitializeComponent();
        }

        private void frmCmt_Load(object sender, EventArgs e)
        {

            richcmt.Text = File.ReadAllText(Application.StartupPath + @"\Data\cmt.txt");

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Application.StartupPath + @"\Data\cmt.txt", richcmt.Text);
        }
    }
}
