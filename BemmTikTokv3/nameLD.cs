using Auto_LDPlayer;
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
    public partial class nameLD : Form
    {
        public nameLD()
        {
            InitializeComponent();
        }
        public string name = "" ;
        private void txtpathimg_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(txtpathimg.Text + @"\ldconsole.exe"))
            {
                try
                {
                    LDPlayer.pathLD = txtpathimg.Text + @"\ldconsole.exe";
                    LDPlayer ldplayer = new LDPlayer();
                    var listName = ldplayer.GetDevices(); ;
                    cboLdName.DataSource = listName;
                }
                catch (Exception)
                {

                }
            }
            else
            {
                cboLdName.DataSource = null;
            }
     
        }
       
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            Setting st = new Setting(Application.StartupPath);
            st.setSetting("pathLD", "path", txtpathimg.Text);
            name = cboLdName.Text;
            Close();
        }

        private void nameLD_Load(object sender, EventArgs e)
        {
            Setting st = new Setting(Application.StartupPath);
           txtpathimg.Text = st.getSetting("pathLD", "path");
        }
    }
}
