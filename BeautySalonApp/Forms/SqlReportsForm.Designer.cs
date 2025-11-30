using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BeautySalonApp.Forms
{
    partial class SqlReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblPredefined;
        private ComboBox cmbPredefinedQueries;
        private Label lblSql;
        private RichTextBox txtSqlQuery;
        private Button btnExecute;
        private Button btnGenerateReport;
        private DataGridView dataGridViewResults;
        private Chart chartReport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPredefined = new Label();
            this.cmbPredefinedQueries = new ComboBox();
            this.lblSql = new Label();
            this.txtSqlQuery = new RichTextBox();
            this.btnExecute = new Button();
            this.btnGenerateReport = new Button();
            this.dataGridViewResults = new DataGridView();
            this.chartReport = new Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartReport)).BeginInit();
            this.SuspendLayout();

            // lblPredefined
            this.lblPredefined.AutoSize = true;
            this.lblPredefined.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.lblPredefined.Location = new Point(20, 20);
            this.lblPredefined.Name = "lblPredefined";
            this.lblPredefined.Size = new Size(170, 21);
            this.lblPredefined.TabIndex = 0;
            this.lblPredefined.Text = "Предопределенные отчеты:";

            // cmbPredefinedQueries
            this.cmbPredefinedQueries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPredefinedQueries.Font = new Font("Arial", 9F);
            this.cmbPredefinedQueries.FormattingEnabled = true;
            this.cmbPredefinedQueries.Location = new Point(200, 17);
            this.cmbPredefinedQueries.Name = "cmbPredefinedQueries";
            this.cmbPredefinedQueries.Size = new Size(400, 29);
            this.cmbPredefinedQueries.TabIndex = 1;
            this.cmbPredefinedQueries.SelectedIndexChanged += new System.EventHandler(this.CmbPredefinedQueries_SelectedIndexChanged);

            // lblSql
            this.lblSql.AutoSize = true;
            this.lblSql.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.lblSql.Location = new Point(20, 60);
            this.lblSql.Name = "lblSql";
            this.lblSql.Size = new Size(82, 21);
            this.lblSql.TabIndex = 2;
            this.lblSql.Text = "SQL запрос:";

            // txtSqlQuery
            this.txtSqlQuery.Font = new Font("Consolas", 10F);
            this.txtSqlQuery.Location = new Point(20, 85);
            this.txtSqlQuery.Name = "txtSqlQuery";
            this.txtSqlQuery.Size = new Size(840, 150);
            this.txtSqlQuery.TabIndex = 3;
            this.txtSqlQuery.Text = "";
            this.txtSqlQuery.TextChanged += new System.EventHandler(this.TxtSqlQuery_TextChanged);

            // btnExecute
            this.btnExecute.BackColor = Color.DodgerBlue;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatStyle = FlatStyle.Flat;
            this.btnExecute.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.btnExecute.ForeColor = Color.White;
            this.btnExecute.Location = new Point(20, 250);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new Size(150, 35);
            this.btnExecute.TabIndex = 4;
            this.btnExecute.Text = "Выполнить запрос";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);

            // btnGenerateReport
            this.btnGenerateReport.BackColor = Color.Green;
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.FlatStyle = FlatStyle.Flat;
            this.btnGenerateReport.Font = new Font("Arial", 9F, FontStyle.Bold);
            this.btnGenerateReport.ForeColor = Color.White;
            this.btnGenerateReport.Location = new Point(180, 250);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new Size(150, 35);
            this.btnGenerateReport.TabIndex = 5;
            this.btnGenerateReport.Text = "Экспорт в CSV";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            this.btnGenerateReport.Click += new System.EventHandler(this.BtnGenerateReport_Click);

            // dataGridViewResults
            this.dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewResults.BackgroundColor = Color.White;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Location = new Point(20, 300);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.RowHeadersWidth = 62;
            this.dataGridViewResults.RowTemplate.Height = 28;
            this.dataGridViewResults.Size = new Size(840, 350);
            this.dataGridViewResults.TabIndex = 6;

            // chartReport
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            chartArea1.Name = "ChartArea1";
            legend1.Name = "Legend1";
            this.chartReport.ChartAreas.Add(chartArea1);
            this.chartReport.Legends.Add(legend1);
            this.chartReport.Location = new Point(880, 20);
            this.chartReport.Name = "chartReport";
            this.chartReport.Size = new Size(600, 630);
            this.chartReport.TabIndex = 7;
            this.chartReport.Text = "Графический отчет";

            // SqlReportsForm
            this.BackColor = Color.White;
            this.ClientSize = new Size(1500, 670);
            this.Controls.Add(this.chartReport);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtSqlQuery);
            this.Controls.Add(this.lblSql);
            this.Controls.Add(this.cmbPredefinedQueries);
            this.Controls.Add(this.lblPredefined);
            this.Name = "SqlReportsForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SQL Отчеты и Аналитика - Салон Красоты";
            this.Load += new System.EventHandler(this.SqlReportsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
