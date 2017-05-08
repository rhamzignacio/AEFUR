namespace AirlineBillingReport
{
    partial class FormSelection
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
            this.btnBilled = new System.Windows.Forms.Button();
            this.btnUnbilled = new System.Windows.Forms.Button();
            this.btnNoRecord = new System.Windows.Forms.Button();
            this.lblBilledCount = new System.Windows.Forms.Label();
            this.lblUnbilledCount = new System.Windows.Forms.Label();
            this.lblNoRecordCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPreparation = new System.Windows.Forms.Button();
            this.btnIATAPreparationUSD = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPALAPAnalysis = new System.Windows.Forms.Button();
            this.btnIASAAPAnalysisPHP = new System.Windows.Forms.Button();
            this.btnCebuPacific = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBilled
            // 
            this.btnBilled.BackColor = System.Drawing.Color.SeaGreen;
            this.btnBilled.FlatAppearance.BorderSize = 0;
            this.btnBilled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBilled.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBilled.ForeColor = System.Drawing.Color.White;
            this.btnBilled.Location = new System.Drawing.Point(12, 38);
            this.btnBilled.Name = "btnBilled";
            this.btnBilled.Size = new System.Drawing.Size(219, 23);
            this.btnBilled.TabIndex = 0;
            this.btnBilled.Text = "Billed Summary";
            this.btnBilled.UseVisualStyleBackColor = false;
            this.btnBilled.Click += new System.EventHandler(this.btnBilled_Click);
            // 
            // btnUnbilled
            // 
            this.btnUnbilled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(1)))), ((int)(((byte)(90)))));
            this.btnUnbilled.FlatAppearance.BorderSize = 0;
            this.btnUnbilled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnbilled.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnbilled.ForeColor = System.Drawing.Color.White;
            this.btnUnbilled.Location = new System.Drawing.Point(12, 67);
            this.btnUnbilled.Name = "btnUnbilled";
            this.btnUnbilled.Size = new System.Drawing.Size(219, 23);
            this.btnUnbilled.TabIndex = 1;
            this.btnUnbilled.Text = "Unbilled Summary";
            this.btnUnbilled.UseVisualStyleBackColor = false;
            this.btnUnbilled.Click += new System.EventHandler(this.btnUnbilled_Click);
            // 
            // btnNoRecord
            // 
            this.btnNoRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(48)))), ((int)(((byte)(162)))));
            this.btnNoRecord.FlatAppearance.BorderSize = 0;
            this.btnNoRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNoRecord.ForeColor = System.Drawing.Color.White;
            this.btnNoRecord.Location = new System.Drawing.Point(12, 96);
            this.btnNoRecord.Name = "btnNoRecord";
            this.btnNoRecord.Size = new System.Drawing.Size(219, 23);
            this.btnNoRecord.TabIndex = 2;
            this.btnNoRecord.Text = "No Record Summary";
            this.btnNoRecord.UseVisualStyleBackColor = false;
            this.btnNoRecord.Click += new System.EventHandler(this.btnNoRecord_Click);
            // 
            // lblBilledCount
            // 
            this.lblBilledCount.AutoSize = true;
            this.lblBilledCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBilledCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblBilledCount.Location = new System.Drawing.Point(233, 43);
            this.lblBilledCount.Name = "lblBilledCount";
            this.lblBilledCount.Size = new System.Drawing.Size(49, 13);
            this.lblBilledCount.TabIndex = 3;
            this.lblBilledCount.Text = "000000";
            // 
            // lblUnbilledCount
            // 
            this.lblUnbilledCount.AutoSize = true;
            this.lblUnbilledCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnbilledCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblUnbilledCount.Location = new System.Drawing.Point(233, 72);
            this.lblUnbilledCount.Name = "lblUnbilledCount";
            this.lblUnbilledCount.Size = new System.Drawing.Size(49, 13);
            this.lblUnbilledCount.TabIndex = 4;
            this.lblUnbilledCount.Text = "000000";
            // 
            // lblNoRecordCount
            // 
            this.lblNoRecordCount.AutoSize = true;
            this.lblNoRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoRecordCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNoRecordCount.Location = new System.Drawing.Point(233, 101);
            this.lblNoRecordCount.Name = "lblNoRecordCount";
            this.lblNoRecordCount.Size = new System.Drawing.Size(49, 13);
            this.lblNoRecordCount.TabIndex = 5;
            this.lblNoRecordCount.Text = "000000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(191, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Total:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTotal.Location = new System.Drawing.Point(233, 122);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(49, 13);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "000000";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 23);
            this.panel1.TabIndex = 8;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AirlineBillingReport.Properties.Resources.Air_icon;
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(23, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(180, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "AEFUR - Summary Selection";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label7_MouseDown);
            // 
            // btnMinimize
            // 
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(224, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(35, 23);
            this.btnMinimize.TabIndex = 3;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(262, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPreparation
            // 
            this.btnPreparation.BackColor = System.Drawing.Color.DimGray;
            this.btnPreparation.FlatAppearance.BorderSize = 0;
            this.btnPreparation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreparation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreparation.ForeColor = System.Drawing.Color.Transparent;
            this.btnPreparation.Location = new System.Drawing.Point(12, 185);
            this.btnPreparation.Name = "btnPreparation";
            this.btnPreparation.Size = new System.Drawing.Size(108, 23);
            this.btnPreparation.TabIndex = 9;
            this.btnPreparation.Text = "IATA - PHP";
            this.btnPreparation.UseVisualStyleBackColor = false;
            this.btnPreparation.Click += new System.EventHandler(this.btnPreparation_Click);
            // 
            // btnIATAPreparationUSD
            // 
            this.btnIATAPreparationUSD.BackColor = System.Drawing.Color.DimGray;
            this.btnIATAPreparationUSD.FlatAppearance.BorderSize = 0;
            this.btnIATAPreparationUSD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIATAPreparationUSD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIATAPreparationUSD.ForeColor = System.Drawing.Color.Transparent;
            this.btnIATAPreparationUSD.Location = new System.Drawing.Point(126, 185);
            this.btnIATAPreparationUSD.Name = "btnIATAPreparationUSD";
            this.btnIATAPreparationUSD.Size = new System.Drawing.Size(108, 23);
            this.btnIATAPreparationUSD.TabIndex = 10;
            this.btnIATAPreparationUSD.Text = "IATA - USD";
            this.btnIATAPreparationUSD.UseVisualStyleBackColor = false;
            this.btnIATAPreparationUSD.Click += new System.EventHandler(this.btnIATAPreparationUSD_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(0, 150);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(297, 22);
            this.panel2.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(103, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "AP Analysis";
            // 
            // btnPALAPAnalysis
            // 
            this.btnPALAPAnalysis.BackColor = System.Drawing.Color.DimGray;
            this.btnPALAPAnalysis.FlatAppearance.BorderSize = 0;
            this.btnPALAPAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPALAPAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPALAPAnalysis.ForeColor = System.Drawing.Color.Transparent;
            this.btnPALAPAnalysis.Location = new System.Drawing.Point(12, 214);
            this.btnPALAPAnalysis.Name = "btnPALAPAnalysis";
            this.btnPALAPAnalysis.Size = new System.Drawing.Size(222, 23);
            this.btnPALAPAnalysis.TabIndex = 12;
            this.btnPALAPAnalysis.Text = "Philippine Airlines";
            this.btnPALAPAnalysis.UseVisualStyleBackColor = false;
            this.btnPALAPAnalysis.Click += new System.EventHandler(this.btnPALAPAnalysis_Click);
            // 
            // btnIASAAPAnalysisPHP
            // 
            this.btnIASAAPAnalysisPHP.BackColor = System.Drawing.Color.DimGray;
            this.btnIASAAPAnalysisPHP.FlatAppearance.BorderSize = 0;
            this.btnIASAAPAnalysisPHP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIASAAPAnalysisPHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIASAAPAnalysisPHP.ForeColor = System.Drawing.Color.Transparent;
            this.btnIASAAPAnalysisPHP.Location = new System.Drawing.Point(12, 243);
            this.btnIASAAPAnalysisPHP.Name = "btnIASAAPAnalysisPHP";
            this.btnIASAAPAnalysisPHP.Size = new System.Drawing.Size(222, 23);
            this.btnIASAAPAnalysisPHP.TabIndex = 13;
            this.btnIASAAPAnalysisPHP.Text = "IASA";
            this.btnIASAAPAnalysisPHP.UseVisualStyleBackColor = false;
            this.btnIASAAPAnalysisPHP.Click += new System.EventHandler(this.btnIASAAPAnalysisPHP_Click);
            // 
            // btnCebuPacific
            // 
            this.btnCebuPacific.BackColor = System.Drawing.Color.DimGray;
            this.btnCebuPacific.FlatAppearance.BorderSize = 0;
            this.btnCebuPacific.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCebuPacific.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCebuPacific.ForeColor = System.Drawing.Color.Transparent;
            this.btnCebuPacific.Location = new System.Drawing.Point(12, 272);
            this.btnCebuPacific.Name = "btnCebuPacific";
            this.btnCebuPacific.Size = new System.Drawing.Size(222, 23);
            this.btnCebuPacific.TabIndex = 14;
            this.btnCebuPacific.Text = "Cebu Pacific";
            this.btnCebuPacific.UseVisualStyleBackColor = false;
            this.btnCebuPacific.Click += new System.EventHandler(this.btnCebuPacific_Click);
            // 
            // FormSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 308);
            this.Controls.Add(this.btnCebuPacific);
            this.Controls.Add(this.btnIASAAPAnalysisPHP);
            this.Controls.Add(this.btnPALAPAnalysis);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnIATAPreparationUSD);
            this.Controls.Add(this.btnPreparation);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNoRecordCount);
            this.Controls.Add(this.lblUnbilledCount);
            this.Controls.Add(this.lblBilledCount);
            this.Controls.Add(this.btnNoRecord);
            this.Controls.Add(this.btnUnbilled);
            this.Controls.Add(this.btnBilled);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Select Report Type";
            this.Load += new System.EventHandler(this.FormSelection_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBilled;
        private System.Windows.Forms.Button btnUnbilled;
        private System.Windows.Forms.Button btnNoRecord;
        private System.Windows.Forms.Label lblBilledCount;
        private System.Windows.Forms.Label lblUnbilledCount;
        private System.Windows.Forms.Label lblNoRecordCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPreparation;
        private System.Windows.Forms.Button btnIATAPreparationUSD;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPALAPAnalysis;
        private System.Windows.Forms.Button btnIASAAPAnalysisPHP;
        private System.Windows.Forms.Button btnCebuPacific;
    }
}