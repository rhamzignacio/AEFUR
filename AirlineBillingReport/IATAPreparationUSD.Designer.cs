namespace AirlineBillingReport
{
    partial class IATAPreparationUSD
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.debitMemoIATAUSDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.refundsIATAUSDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ticketedIATAUSDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.unpostedIATAUSDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMaximized = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.AirlineBillingReportDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.creditMemoIATAUSDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.debitMemoIATAUSDBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refundsIATAUSDBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketedIATAUSDBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unpostedIATAUSDBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirlineBillingReportDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.creditMemoIATAUSDBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // debitMemoIATAUSDBindingSource
            // 
            this.debitMemoIATAUSDBindingSource.DataMember = "DebitMemoIATAUSD";
            this.debitMemoIATAUSDBindingSource.DataSource = typeof(AirlineBillingReportRepository.AirlineBillingReportDataSet);
            // 
            // refundsIATAUSDBindingSource
            // 
            this.refundsIATAUSDBindingSource.DataMember = "RefundsIATAUSD";
            this.refundsIATAUSDBindingSource.DataSource = typeof(AirlineBillingReportRepository.AirlineBillingReportDataSet);
            // 
            // ticketedIATAUSDBindingSource
            // 
            this.ticketedIATAUSDBindingSource.DataMember = "TicketedIATAUSD";
            this.ticketedIATAUSDBindingSource.DataSource = typeof(AirlineBillingReportRepository.AirlineBillingReportDataSet);
            // 
            // unpostedIATAUSDBindingSource
            // 
            this.unpostedIATAUSDBindingSource.DataMember = "UnpostedIATAUSD";
            this.unpostedIATAUSDBindingSource.DataSource = typeof(AirlineBillingReportRepository.AirlineBillingReportDataSet);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnMaximized);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(883, 23);
            this.panel1.TabIndex = 6;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AirlineBillingReport.Properties.Resources.Air_icon;
            this.pictureBox1.Location = new System.Drawing.Point(1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            // 
            // btnMaximized
            // 
            this.btnMaximized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximized.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximized.BackgroundImage = global::AirlineBillingReport.Properties.Resources.rsz_tick_blank;
            this.btnMaximized.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMaximized.FlatAppearance.BorderSize = 0;
            this.btnMaximized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximized.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximized.ForeColor = System.Drawing.Color.Black;
            this.btnMaximized.Location = new System.Drawing.Point(813, 1);
            this.btnMaximized.Name = "btnMaximized";
            this.btnMaximized.Size = new System.Drawing.Size(35, 23);
            this.btnMaximized.TabIndex = 4;
            this.btnMaximized.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMaximized.UseVisualStyleBackColor = false;
            this.btnMaximized.Click += new System.EventHandler(this.btnMaximized_Click);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(25, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(199, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "AEFUR - IATA Preparation USD";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label7_MouseDown);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(778, 1);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(35, 23);
            this.btnMinimize.TabIndex = 3;
            this.btnMinimize.Text = "_";
            this.btnMinimize.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(848, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "X";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DebitMemoUSD";
            reportDataSource1.Value = this.debitMemoIATAUSDBindingSource;
            reportDataSource2.Name = "RefundsUSD";
            reportDataSource2.Value = this.refundsIATAUSDBindingSource;
            reportDataSource3.Name = "TicketedUSD";
            reportDataSource3.Value = this.ticketedIATAUSDBindingSource;
            reportDataSource4.Name = "UnpostedUSD";
            reportDataSource4.Value = this.unpostedIATAUSDBindingSource;
            reportDataSource5.Name = "CreditMemoUSD";
            reportDataSource5.Value = this.creditMemoIATAUSDBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource5);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "AirlineBillingReport.Report.IATAUSD.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 23);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(883, 524);
            this.reportViewer1.TabIndex = 7;
            // 
            // AirlineBillingReportDataSetBindingSource
            // 
            this.AirlineBillingReportDataSetBindingSource.DataMember = "DebitMemoIATAUSD";
            this.AirlineBillingReportDataSetBindingSource.DataSource = typeof(AirlineBillingReportRepository.AirlineBillingReportDataSet);
            // 
            // creditMemoIATAUSDBindingSource
            // 
            this.creditMemoIATAUSDBindingSource.DataMember = "CreditMemoIATAUSD";
            this.creditMemoIATAUSDBindingSource.DataSource = typeof(AirlineBillingReportRepository.AirlineBillingReportDataSet);
            // 
            // IATAPreparationUSD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 547);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IATAPreparationUSD";
            this.Text = "IATAPreparationUSD";
            this.Load += new System.EventHandler(this.IATAPreparationUSD_Load);
            this.Resize += new System.EventHandler(this.IATAPreparationUSD_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.debitMemoIATAUSDBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refundsIATAUSDBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketedIATAUSDBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unpostedIATAUSDBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirlineBillingReportDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.creditMemoIATAUSDBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMaximized;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource AirlineBillingReportDataSetBindingSource;
        private System.Windows.Forms.BindingSource debitMemoIATAUSDBindingSource;
        private System.Windows.Forms.BindingSource refundsIATAUSDBindingSource;
        private System.Windows.Forms.BindingSource ticketedIATAUSDBindingSource;
        private System.Windows.Forms.BindingSource unpostedIATAUSDBindingSource;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.BindingSource creditMemoIATAUSDBindingSource;
    }
}