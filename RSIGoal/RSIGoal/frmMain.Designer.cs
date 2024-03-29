﻿/**
 * Copyright (c) David-John Miller AKA Anoyomouse 2014
 *
 * See LICENCE in the project directory for licence information
 **/
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
            this.cmdGrabData = new System.Windows.Forms.Button();
            this.bgwLoadData = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // cmdGrabData
            // 
            this.cmdGrabData.Location = new System.Drawing.Point(277, 123);
            this.cmdGrabData.Name = "cmdGrabData";
            this.cmdGrabData.Size = new System.Drawing.Size(70, 27);
            this.cmdGrabData.TabIndex = 0;
            this.cmdGrabData.Text = "Grab Data";
            this.cmdGrabData.UseVisualStyleBackColor = true;
            this.cmdGrabData.Click += new System.EventHandler(this.cmdGrabData_Click);
            // 
            // bgwLoadData
            // 
            this.bgwLoadData.WorkerReportsProgress = true;
            this.bgwLoadData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwLoadData_DoWork);
            this.bgwLoadData.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwLoadData_ProgressChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 171);
            this.Controls.Add(this.cmdGrabData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Goal";
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button cmdGrabData;
        private System.ComponentModel.BackgroundWorker bgwLoadData;

	}
}

