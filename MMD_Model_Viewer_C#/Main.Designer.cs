namespace MMD_Model_Viewer
{
    partial class MMD_Model_Viewer
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MMD_Model_Viewer));
            this.Load_T = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Load_T
            // 
            this.Load_T.AutoSize = true;
            this.Load_T.BackColor = System.Drawing.SystemColors.Control;
            this.Load_T.Font = new System.Drawing.Font("MS UI Gothic", 36F);
            this.Load_T.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Load_T.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Load_T.Location = new System.Drawing.Point(578, 531);
            this.Load_T.Name = "Load_T";
            this.Load_T.Size = new System.Drawing.Size(752, 48);
            this.Load_T.TabIndex = 0;
            this.Load_T.Text = "ロード中です。しばらくお待ちください・・・";
            // 
            // MMD_Model_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.Load_T);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MMD_Model_Viewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MMD Model Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MMD_Model_Viewer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MMD_Model_Viewer_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Load_T;
    }
}

