namespace PrinterForm
{
    partial class frmPrinter
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWidth = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.plBrowser = new System.Windows.Forms.Panel();
            this.gbWh = new System.Windows.Forms.GroupBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.gbWh.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(26, 29);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(37, 15);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "宽：";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(69, 24);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(60, 25);
            this.txtWidth.TabIndex = 2;
            this.txtWidth.Text = "100";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(208, 24);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(60, 25);
            this.txtHeight.TabIndex = 4;
            this.txtHeight.Text = "150";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(165, 29);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(37, 15);
            this.lblHeight.TabIndex = 3;
            this.lblHeight.Text = "高：";
            // 
            // plBrowser
            // 
            this.plBrowser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plBrowser.Location = new System.Drawing.Point(0, 92);
            this.plBrowser.Name = "plBrowser";
            this.plBrowser.Size = new System.Drawing.Size(542, 611);
            this.plBrowser.TabIndex = 5;
            // 
            // gbWh
            // 
            this.gbWh.Controls.Add(this.txtWidth);
            this.gbWh.Controls.Add(this.lblWidth);
            this.gbWh.Controls.Add(this.txtHeight);
            this.gbWh.Controls.Add(this.lblHeight);
            this.gbWh.Location = new System.Drawing.Point(12, 12);
            this.gbWh.Name = "gbWh";
            this.gbWh.Size = new System.Drawing.Size(306, 66);
            this.gbWh.TabIndex = 6;
            this.gbWh.TabStop = false;
            this.gbWh.Text = "面单尺寸（毫米）";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.ForeColor = System.Drawing.Color.Blue;
            this.lblCopyright.Location = new System.Drawing.Point(353, 41);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(0, 15);
            this.lblCopyright.TabIndex = 7;
            // 
            // frmPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 703);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.gbWh);
            this.Controls.Add(this.plBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrinter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbWh.ResumeLayout(false);
            this.gbWh.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Panel plBrowser;
        private System.Windows.Forms.GroupBox gbWh;
        private System.Windows.Forms.Label lblCopyright;
    }
}

