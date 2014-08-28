namespace RSIGoal
{
    partial class frmMain
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
			this.txtGoal = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtGoal
			// 
			this.txtGoal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
			this.txtGoal.Location = new System.Drawing.Point(86, 12);
			this.txtGoal.Multiline = true;
			this.txtGoal.Name = "txtGoal";
			this.txtGoal.Size = new System.Drawing.Size(254, 51);
			this.txtGoal.TabIndex = 0;
			this.txtGoal.Text = "$52,000,000";
			this.txtGoal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(359, 118);
			this.Controls.Add(this.txtGoal);
			this.Name = "frmMain";
			this.Text = "Form1";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TextBox txtGoal;
    }
}

