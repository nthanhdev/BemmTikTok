using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace BemmTikTokv3
{
    public partial class tabAccount : Form
    {
        public tabAccount()
        {
            InitializeComponent();
        }
        string name = "";
        public string nameTab()
        {
            return name;
        }

        private void tabAccount_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Application.StartupPath + @"\Data\Account");
            for (int i = 0; i < files.Count(); i++)
            {
                files[i] = Path.GetFileName(files[i]).Replace(".txt","");
            }
            comTab.DataSource = files;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            name = comTab.Text;
            Close();
        }
    }
}
