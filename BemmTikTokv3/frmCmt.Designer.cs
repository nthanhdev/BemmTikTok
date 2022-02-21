
namespace BemmTikTokv3
{
    partial class frmCmt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCmt));
            this.richcmt = new System.Windows.Forms.RichTextBox();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // richcmt
            // 
            this.richcmt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richcmt.Location = new System.Drawing.Point(8, 4);
            this.richcmt.Name = "richcmt";
            this.richcmt.Size = new System.Drawing.Size(436, 224);
            this.richcmt.TabIndex = 0;
            this.richcmt.Text = "";
            // 
            // guna2Button2
            // 
            this.guna2Button2.CheckedState.Parent = this.guna2Button2;
            this.guna2Button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2Button2.CustomImages.Parent = this.guna2Button2;
            this.guna2Button2.FillColor = System.Drawing.Color.Turquoise;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.guna2Button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.guna2Button2.HoverState.Parent = this.guna2Button2;
            this.guna2Button2.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2Button2.ImageSize = new System.Drawing.Size(30, 30);
            this.guna2Button2.Location = new System.Drawing.Point(-3, 234);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.ShadowDecoration.Parent = this.guna2Button2;
            this.guna2Button2.Size = new System.Drawing.Size(454, 33);
            this.guna2Button2.TabIndex = 21;
            this.guna2Button2.Text = "SAVE";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // frmCmt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 269);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.richcmt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCmt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý cmt (1 cmt là 1 dòng)";
            this.Load += new System.EventHandler(this.frmCmt_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richcmt;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}