namespace AirlineBillingReport
{
    partial class BilledReport
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.airlineBillingReportDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AirlineBillingReportDataSet = new AirlineBillingReportRepository.AirlineBillingReportDataSet();
            this.BilledSummaryReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BilledSummaryReportTableAdapter = new AirlineBillingReportRepository.AirlineBillingReportDataSetTableAdapters.BilledSummaryReportTableAdapter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMaximized = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.airlineBillingReportDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirlineBillingReportDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BilledSummaryReportBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // airlineBillingReportDataSetBindingSource
            // 
            this.airlineBillingReportDataSetBindingSource.DataSource = this.AirlineBillingReportDataSet;
            this.airlineBillingReportDataSetBindingSource.Position = 0;
            // 
            // AirlineBillingReportDataSet
            // 
            this.AirlineBillingReportDataSet.DataSetName = "AirlineBillingReportDataSet";
            this.AirlineBillingReportDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // BilledSummaryReportBindingSource
            // 
            this.BilledSummaryReportBindingSource.DataMember = "BilledSummaryReport";
            this.BilledSummaryReportBindingSource.DataSource = this.AirlineBillingReportDataSet;
            // 
            // BilledSummaryReportTableAdapter
            // 
            this.BilledSummaryReportTableAdapter.ClearBeforeFill = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaGreen;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnMaximized);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 23);
            this.panel1.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AirlineBillingReport.Properties.Resources.Air_icon;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            // 
            // btnMaximized
            // 
            this.btnMaximized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximized.BackColor = System.Drawing.Color.SeaGreen;
            this.btnMaximized.BackgroundImage = global::AirlineBillingReport.Properties.Resources.rsz_tick_blank;
            this.btnMaximized.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMaximized.FlatAppearance.BorderSize = 0;
            this.btnMaximized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximized.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximized.ForeColor = System.Drawing.Color.White;
            this.btnMaximized.Location = new System.Drawing.Point(843, 1);
            this.btnMaximized.Name = "btnMaximized";
            this.btnMaximized.Size = new System.Drawing.Size(35, 23);
            this.btnMaximized.TabIndex = 5;
            this.btnMaximized.UseVisualStyleBackColor = false;
            this.btnMaximized.Click += new System.EventHandler(this.btnMaximized_Click_1);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(23, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(263, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "AEFUR - Export Unbilled Monitoring";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label7_MouseDown_1);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.SeaGreen;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(808, 1);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(35, 23);
            this.btnMinimize.TabIndex = 3;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.SeaGreen;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(878, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource3.Name = "BilledSummary";
            reportDataSource3.Value = this.airlineBillingReportDataSetBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "AirlineBillingReport.Report.BilledSummaryReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 23);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(913, 490);
            this.reportViewer1.TabIndex = 0;
            // 
            // BilledReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 513);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BilledReport";
            this.Text = "Billed Summary Report";
            this.Load += new System.EventHandler(this.BilledReport_Load);
            this.Resize += new System.EventHandler(this.BilledReport_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.airlineBillingReportDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirlineBillingReportDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BilledSummaryReportBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource BilledSummaryReportBindingSource;
        private AirlineBillingReportRepository.AirlineBillingReportDataSet AirlineBillingReportDataSet;
        private AirlineBillingReportRepository.AirlineBillingReportDataSetTableAdapters.BilledSummaryReportTableAdapter BilledSummaryReportTableAdapter;
        private System.Windows.Forms.BindingSource airlineBillingReportDataSetBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMaximized;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}