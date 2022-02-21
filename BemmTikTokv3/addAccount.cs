using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class addAccount : Form
    {
        public addAccount(string nameTab)
        {
            InitializeComponent();
            this.Text = "Thêm tài khoản vào Tab: " + nameTab;
        }
        public string data;
        void macDinh()
        {
            if (txtuserid.Text != "" && txtpass.Text != "" && txtpathbackup.Text != "" && txtpathbackup.Text != "")
            {
                data = "0$#" + txtuserid.Text + "," + txtpass.Text + "," + txtpathbackup.Text + "," + txtcookie.Text + "," + txtnameLD.Text + "," + txtProxy.Text+ "," + txtemail.Text;
                Close();
            }
            else
                MessageBox.Show("Vui lòng điền đầy đủ", "BemmTeam");
        }
        void dinhDang()
        {
            try
            {
                if (richTextBox1.Text != "")
                {
                    data = "1$" + richTextBox1.Text;
                }
                else
                    MessageBox.Show("Vui lòng điền đầy đủ", "BemmTeam");
            }
            catch (Exception)
            {
                    MessageBox.Show("Vui lòng kiểm tra lại dữ liệu", "BemmTeam",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
        }
        void import()
        {
            try
            {
                if (txtpath.Text != "")
                {
                    data = "2$" + txtpath.Text;

                }
                else
                    MessageBox.Show("Vui lòng chọn file", "BemmTeam");
            }
            catch (Exception)
            {

                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        void tabname()
        {
            try
            {
                if (btnTab.Text != "Nhấn vào đây để chọn Tab Account")
                {
                    data = "3$" + btnTab.Text;
                }
                else
                    MessageBox.Show("Vui lòng chọn tab", "BemmTeam");
            }
            catch (Exception)
            {

                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {

            int num = tabControl1.SelectedIndex;
            if (num == 0)
                macDinh();
            else if (num == 1)
                dinhDang();
            else if (num == 2)
                import();
            else
                tabname();
            Close();
        }

        private void btnTab_Click(object sender, EventArgs e)
        {
            using (tabAccount tab = new tabAccount())
            {
                tab.ShowDialog();
                string a = tab.nameTab();
                if ( a != null)
                {
                    btnTab.Text = a;
                }
            } ;
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Text Files (*.TXT;)|*.TXT|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                
                    txtpath.Text = dialog.FileName;

                }
            }
        }
    }
}
