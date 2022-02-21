using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BemmTikTokv3
{
    public partial class fDownTikTok : UserControl
    {
        public fDownTikTok()
        {
            InitializeComponent();
        }
        private List<Object> ListVideo = new List<Object>();
        private RQVideo RVIDEO = new RQVideo();
        private int numpage = 0;
        private int numi = 1;
        private string timkiemnow = "";
        private bool isByuser = true;

      
        private bool DownLoadVideo(string url, string folderPath)
        {
         

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(new System.Uri(url), folderPath);
             
            }
            return true;
        }
    



        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lblstt.Invoke(new Action(() =>
            {
                lblstt.Text = "Đã tải xong: " + num + " / " + count;
                prg1.Value += 1;
                num++;
            }));
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressDownVideo.Value = e.ProgressPercentage;

        }

     
        void designGridview()
        {
            dataGridViewVideo.Invoke(new Action(() =>
                {

                

                    dataGridViewVideo.Columns[0].HeaderText = "";
                    dataGridViewVideo.Columns[1].HeaderText = "Name File";
                    dataGridViewVideo.Columns[2].HeaderText = "Video ID";
                    dataGridViewVideo.Columns[3].HeaderText = "User ID";
                    dataGridViewVideo.Columns[7].HeaderText = "Trạng Thái";
                    dataGridViewVideo.Columns[1].ReadOnly = false;
                    dataGridViewVideo.Columns[2].ReadOnly = true;
                    dataGridViewVideo.Columns[3].ReadOnly = true;
                    dataGridViewVideo.Columns[4].ReadOnly = true;
                    dataGridViewVideo.Columns[5].ReadOnly = true;
                    dataGridViewVideo.Columns[6].ReadOnly = true;
                    dataGridViewVideo.Columns[7].ReadOnly = true;
                    dataGridViewVideo.Columns[0].DividerWidth = 2;
                    dataGridViewVideo.Columns[1].DividerWidth = 2;
                    dataGridViewVideo.Columns[2].DividerWidth = 2;
                    dataGridViewVideo.Columns[3].DividerWidth = 2;
                    dataGridViewVideo.Columns[4].DividerWidth = 2;
                    dataGridViewVideo.Columns[5].DividerWidth = 2;
                    dataGridViewVideo.Columns[6].DividerWidth = 2;

                    dataGridViewVideo.Columns[7].DividerWidth = 2;

                }));
            

        }

        private void btngetthongtinn_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.check)
            {
                MessageBox.Show("Bạn kích hoạt phần mềm đi nha :(", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            prg1.Visible = false;
            btnProPage.Enabled = false;
            btnnextPage.Enabled = false;
            ListVideo.Clear();
            numpage = 0;
            numi = 1;
            RVIDEO.max_cursor = "";
            string url = "";
            string path = "";
            
            if (txtpath.Text != "" && txttimkiem.Text != "")
            {
                if (checkVideoID.Checked || checkLink.Checked)
                {
                    try
                    {
                        if (checkVideoID.Checked)
                        {
                            url = txttimkiem.Text;
                            if (txtfileName.Text == "[random]")
                            {
                                Random r = new Random();
                                path = txtpath.Text + @"\tik_" + txttimkiem.Text.Substring(12, 7) + ".mp4";
                            }
                            else path = txtpath.Text + @"\" + txtfileName.Text + ".mp4";
                        }
                        else if (checkLink.Checked)
                        {
                            url = txttimkiem.Text;
                            int num = url.IndexOf("video/");
                            if (num == -1)
                            {
                                MessageBox.Show("Không nhận dạng được ID video!", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                                url = url.Substring(num + 6, 19);

                            if (txtfileName.Text == "[random]")
                            {
                                Random r = new Random();
                                path = txtpath.Text + @"\tik_" + r.Next(1000000, 9999999).ToString() + ".mp4";
                            }
                            else path = txtpath.Text + @"\" + txtfileName.Text + ".mp4";
                        }
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Không nhận dạng được ID video!", "BemmTeam", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    btngetthongtinn.Enabled = false;

                    Thread dwn = new Thread(() =>
                    {
                        string urldown = RVIDEO.getUrlDown(url);
                        if (!DownLoadVideo(urldown, path))
                        {
                            progressDownVideo.Value = 100;
                            progressDownVideo.ProgressColor = Color.Red;
                            progressDownVideo.ProgressColor2 = Color.Red;
                        }
                        else
                        {
                            progressDownVideo.Value = 0;
                            progressDownVideo.ProgressColor = Color.LawnGreen;
                            progressDownVideo.ProgressColor2 = Color.LawnGreen;
                        }
                        btngetthongtinn.Invoke(new Action(() => btngetthongtinn.Enabled = true));


                    });
                    dwn.IsBackground = true;
                    dwn.Start();
                }
                else
                {

                    Thread t = new Thread(() =>
                    {
                        List<DBVideo> videos = new List<DBVideo>();
                        setText(btngetthongtinn, "ĐANG LẤY THÔNG TIN");
                        setEnabled(btngetthongtinn, false);
                        if (checkUserID.Checked)
                        {
                            isByuser = true;
                            numi = 1;
                            videos = RVIDEO.getByi4(true, txttimkiem.Text);

                            dataGridViewVideo.Invoke(new Action(() => dataGridViewVideo.DataSource = videos));
                            if (videos is null)
                            {
                                setText(btngetnew, "Không còn video!");

                                setEnabled(btngetnew, false);
                            }


                            else
                            {
                                setText(btngetnew, "Lấy thêm danh sách");

                                setEnabled(btngetnew, true);

                            }

                        }
                        else
                        {
                            isByuser = false;
                            numi = 2;
                            videos = RVIDEO.getByi4(false, "https://urlebird.com/hash/" + txttimkiem.Text + "/");
                            dataGridViewVideo.Invoke(new Action(() => dataGridViewVideo.DataSource = videos));

                            if (videos is null)
                            {
                                setText(btngetnew, "Không còn video!");

                                setEnabled(btngetnew, false);
                            }
                               

                            else
                                setEnabled(btngetnew, true);

                        }
                        designGridview();
                        setText(btngetthongtinn, "LẤY THÔNG TIN");
                        setEnabled(btngetthongtinn, true);

                        setText(lblpagecount, "1");
                        setText(lblpagenow, "1");
                        timkiemnow = txttimkiem.Text;
                        var data = dataGridViewVideo.DataSource;
                        ListVideo.Add(data);
                        setEnabled(btnDownAll, true);
                    });
                    t.IsBackground = true;
                    t.Start();


                }


            }
        }

        void setText(Control control , string text)
        {
            control.Invoke(new Action(() => control.Text = text));
        }
        void setEnabled(Control control,bool check)
        {
            control.Invoke(new Action(() => control.Enabled = check));

        }
        private void btnopenpath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //   string[] files = Directory.GetFiles(fbd.SelectedPath);
                    //   System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                    txtpath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btngetnew_Click(object sender, EventArgs e)
        {
            
            Thread thread = new Thread(() =>
            {

                setText(btngetnew, "Đang lấy thông tin");
                setEnabled(btngetnew, false);

                List<DBVideo> videos = new List<DBVideo>();
                int un = numi++;
                if (isByuser)
                    videos = RVIDEO.getByi4(true, timkiemnow,un );
                else videos = RVIDEO.getByi4(false, "https://urlebird.com/hash/" + timkiemnow + "/page/" + un + "/",un);

           
                if (videos is null || videos.Count == 0)
                    {
                        setText(btngetnew, "Không còn video!");
                        setEnabled(btngetnew, false);
                    }
                    else
                    {

                    setText(lblpagecount, (int.Parse(lblpagecount.Text) + 1).ToString());
                    setText(lblpagenow, lblpagecount.Text);

                    numpage = int.Parse(lblpagecount.Text) - 1;

                    dataGridViewVideo.Invoke(new Action(() => dataGridViewVideo.DataSource = videos));

                    var data = dataGridViewVideo.DataSource;
                    ListVideo.Add(data);
                    setEnabled(btnnextPage, true);
                    setEnabled(btnProPage, true);
                    setText(btngetnew, "Lấy thêm danh sách");

                    designGridview();

                    setEnabled(btngetnew, true);
                    }
               

            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnProPage_Click(object sender, EventArgs e)
        {
            btnnextPage.Enabled = true;
            if (numpage == 0)
            {
                numpage = ListVideo.Count;
                lblpagenow.Text = lblpagecount.Text;
            }
            numpage--;
            dataGridViewVideo.DataSource = ListVideo[numpage];
            lblpagenow.Text = (numpage + 1).ToString();

            designGridview();
        }

        private void btnnextPage_Click(object sender, EventArgs e)
        {
            btnProPage.Enabled = true;
            if (numpage == ListVideo.Count - 1)
            {
                numpage = -1;
                lblpagenow.Text = 1.ToString();

            }

            numpage++;
            dataGridViewVideo.DataSource = ListVideo[numpage];
            lblpagenow.Text = (numpage + 1).ToString();

            designGridview();
        }

        private void checkNe(object sender, EventArgs e)
        {
            if (checkLink.Checked || checkVideoID.Checked)
            {
                btngetthongtinn.Text = "TẢI VIDEO";
            }
            else btngetthongtinn.Text = "LẤY THÔNG TIN";
        }

        private void fDownTikTok_Load(object sender, EventArgs e)
        {
            txtpath.Text = Application.StartupPath + @"\Video";
        }

        private void dataGridViewVideo_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ignore if a column or row header is clicked
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    this.dataGridViewVideo.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridViewVideo.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip1.Show(dataGridViewVideo, relativeMousePosition);
                }
            }
        }

        private void menucopyVideoID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridViewVideo.Rows[dataGridViewVideo.CurrentCell.RowIndex].Cells[2].Value.ToString()) ;
        }

        private void menuCopyuser_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridViewVideo.Rows[dataGridViewVideo.CurrentCell.RowIndex].Cells[3].Value.ToString());

        }

        private void menuViewVideo_Click(object sender, EventArgs e)
        {
            string id = dataGridViewVideo.Rows[dataGridViewVideo.CurrentCell.RowIndex].Cells[2].Value.ToString();
            Process.Start("https://m.tiktok.com/v/" + id);
        }
        bool downLoadVideo(int row , string id, string path)
        {
            bool result = true;
            dataGridViewVideo.Rows[row].Cells[8].Value = "Đang tải...";
         

                string urldown = RVIDEO.getUrlDown(id);
                if (!DownLoadVideo(urldown, path))
                {
                    progressDownVideo.Value = 100;
                    progressDownVideo.ProgressColor = Color.Red;
                    progressDownVideo.ProgressColor2 = Color.Red;
                    dataGridViewVideo.Invoke(new Action(() => {
                        dataGridViewVideo.Rows[row].Cells[8].Value = "Lỗi!";
                        dataGridViewVideo.Rows[row].DefaultCellStyle.BackColor = Color.DarkRed;
                    }));
                    result = false;
                }
                else
                {
                    progressDownVideo.Value = 0;
                    progressDownVideo.ProgressColor = Color.LawnGreen;
                    progressDownVideo.ProgressColor2 = Color.LawnGreen;
                    dataGridViewVideo.Invoke(new Action(() => {
                        dataGridViewVideo.Rows[row].Cells[8].Value = "Done";
                        dataGridViewVideo.Rows[row].DefaultCellStyle.BackColor = Color.Cyan;
                    }));

                }
                btngetthongtinn.Invoke(new Action(() => btngetthongtinn.Enabled = true));
                return result;

        }

        private void menuDownLoadVideo_Click(object sender, EventArgs e)
        {

            int row = dataGridViewVideo.CurrentCell.RowIndex;
            string id = dataGridViewVideo.Rows[row].Cells[2].Value.ToString();
            string path = txtpath.Text + @"\" + dataGridViewVideo.Rows[row].Cells[1].Value.ToString() + ".mp4";
            Thread r = new Thread(() => downLoadVideo(row, id, path));
            r.IsBackground = true;
            r.Start();
        }

        private void viewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Process.Start("https://tiktok.com/@" + dataGridViewVideo.Rows[dataGridViewVideo.CurrentCell.RowIndex].Cells[3].Value.ToString());
        }

        List<DataGridViewRow> getCountselected()
        {
            List<DataGridViewRow> result = new List<DataGridViewRow>();
            foreach (DataGridViewRow item in dataGridViewVideo.Rows)
            {
                if ((bool)item.Cells[0].Value)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        private void btnDownAll_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rows = getCountselected();
            lblstt.Text = "Đang lấy tổng số...";
            int count = rows.Count;
            int num = 1;
            prg1.Visible = true;
            prg1.Maximum = count;

            if (rows.Count != 0 && rows.Count <= 10)
            {

                foreach (DataGridViewRow item in rows)
                {

                    if ((bool)item.Cells[0].Value)
                    {
                        Thread down = new Thread(() =>
                        {
                            Thread.Sleep(2000);
                            int row = item.Index;
                            string id = item.Cells[2].Value.ToString();
                            string path = txtpath.Text + @"\tik_" + id.Substring(12, 7) + ".mp4";
                            downLoadVideo(row, id, path);
                            this.num = num;
                            this.count = count;
                            
                            
                        });
                        down.IsBackground = true;
                        down.Start();
                    }

                }
                lblstt.Invoke(new Action(() => lblstt.Text = "Đã tải xong: " + (num - 1) + " / " + count));

            }
            else if (rows.Count > 10)
            {
                lblstt.Text = "Chỉ được dưới 10 video 1 lần";
                prg1.Visible = false;

            }
            else
            {
                lblstt.Text = "Bạn chưa chọn !";
                prg1.Visible = false;
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            prg1.Value += 1;

        }
        int num, count;
      
        private void btnimport_Click(object sender, EventArgs e)
        {
            frmtiktokTQ frm = new frmtiktokTQ();
            frm.ShowDialog();
        }
    }
}
