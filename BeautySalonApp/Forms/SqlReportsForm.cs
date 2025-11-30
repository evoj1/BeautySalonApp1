using System;
using System.Data;
using System.Windows.Forms;
using BeautySalonApp.Database;

namespace BeautySalonApp.Forms
{
    public partial class SqlReportsForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();

        public SqlReportsForm()
        {
            InitializeComponent();
            LoadPredefinedQueries();
        }

        private void LoadPredefinedQueries()
        {
            cmbPredefinedQueries.Items.AddRange(new object[] {
                "Выберите отчет...",
                "1. Все услуги",
                "2. Все клиенты",
                "3. Все записи",
                "4. Услуги по категориям",
                "5. Записи на завтра",
                "6. Выполненные записи",
                "7. Постоянные клиенты"
            });
            cmbPredefinedQueries.SelectedIndex = 0;
        }

        private void CmbPredefinedQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedQuery = cmbPredefinedQueries.SelectedItem.ToString();

            switch (selectedQuery)
            {
                case "1. Все услуги":
                    txtSqlQuery.Text = @"SELECT 
                        ID,
                        ServiceName AS Услуга,
                        Category AS Категория, 
                        Price AS Цена,
                        Duration AS Длительность,
                        ROUND(Price / Duration, 2) AS Цена_за_минуту
                    FROM Services
                    ORDER BY Category, Price DESC;";
                    break;

                case "2. Все клиенты":
                    txtSqlQuery.Text = @"SELECT 
                        ID,
                        FirstName AS Имя,
                        LastName AS Фамилия,
                        Phone AS Телефон,
                        Email AS Email
                    FROM Clients
                    ORDER BY LastName, FirstName;";
                    break;

                case "3. Все записи":
                    txtSqlQuery.Text = @"SELECT 
                        Appointments.ID,
                        Clients.FirstName AS Имя,
                        Clients.LastName AS Фамилия, 
                        Services.ServiceName AS Услуга,
                        Services.Price AS Цена,
                        ROUND(Services.Price / Services.Duration, 2) AS Цена_за_минуту,
                        Appointments.AppointmentDate AS Дата,
                        Appointments.Status AS Статус
                    FROM (Appointments
                    INNER JOIN Clients ON Appointments.ClientID = Clients.ID)
                    INNER JOIN Services ON Appointments.ServiceID = Services.ID
                    ORDER BY Appointments.AppointmentDate DESC;";
                    break;

                case "4. Услуги по категориям":
                    txtSqlQuery.Text = @"SELECT 
                        Category AS Категория,
                        COUNT(*) AS Количество_услуг,
                        SUM(Price) AS Общая_стоимость,
                        AVG(Price) AS Средняя_цена,
                        AVG(ROUND(Price / Duration, 2)) AS Средняя_цена_за_минуту
                    FROM Services
                    GROUP BY Category
                    ORDER BY COUNT(*) DESC;";
                    break;

                case "5. Записи на завтра":
                    txtSqlQuery.Text = @"SELECT 
                        Clients.FirstName AS Имя,
                        Clients.LastName AS Фамилия,
                        Services.ServiceName AS Услуга,
                        Services.Price AS Цена,
                        ROUND(Services.Price / Services.Duration, 2) AS Цена_за_минуту,
                        Appointments.AppointmentDate AS Время
                    FROM (Appointments
                    INNER JOIN Clients ON Appointments.ClientID = Clients.ID) 
                    INNER JOIN Services ON Appointments.ServiceID = Services.ID
                    WHERE Appointments.AppointmentDate >= Date() + 1 
                    AND Appointments.AppointmentDate < Date() + 2
                    AND Appointments.Status = 'Запланирован'
                    ORDER BY Appointments.AppointmentDate;";
                    break;

                case "6. Выполненные записи":
                    txtSqlQuery.Text = @"SELECT 
                        Clients.FirstName AS Имя,
                        Clients.LastName AS Фамилия, 
                        Services.ServiceName AS Услуга,
                        Services.Price AS Цена,
                        ROUND(Services.Price / Services.Duration, 2) AS Цена_за_минуту,
                        Appointments.AppointmentDate AS Дата
                    FROM (Appointments
                    INNER JOIN Clients ON Appointments.ClientID = Clients.ID)
                    INNER JOIN Services ON Appointments.ServiceID = Services.ID
                    WHERE Appointments.Status = 'Выполнен'
                    ORDER BY Appointments.AppointmentDate DESC;";
                    break;

                case "7. Постоянные клиенты":
                    txtSqlQuery.Text = @"SELECT 
                        Clients.FirstName AS Имя,
                        Clients.LastName AS Фамилия,
                        Clients.Phone AS Телефон,
                        COUNT(Appointments.ID) AS Количество_записей,
                        SUM(Services.Price) AS Общая_сумма,
                        AVG(ROUND(Services.Price / Services.Duration, 2)) AS Средняя_цена_за_минуту
                    FROM Clients
                    INNER JOIN Appointments ON Clients.ID = Appointments.ClientID
                    INNER JOIN Services ON Appointments.ServiceID = Services.ID
                    GROUP BY Clients.ID, Clients.FirstName, Clients.LastName, Clients.Phone
                    HAVING COUNT(Appointments.ID) > 1
                    ORDER BY COUNT(Appointments.ID) DESC;";
                    break;

                default:
                    txtSqlQuery.Text = "-- Выберите отчет из списка";
                    break;
            }
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSqlQuery.Text))
            {
                MessageBox.Show("Введите SQL запрос!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable result = db.ExecuteQuery(txtSqlQuery.Text);
                dataGridViewResults.DataSource = result;

                MessageBox.Show($"Запрос выполнен успешно!\nНайдено записей: {result.Rows.Count}",
                    "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            if (dataGridViewResults.DataSource == null)
            {
                MessageBox.Show("Сначала выполните запрос для получения данных!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV файлы (*.csv)|*.csv|Текстовые файлы (*.txt)|*.txt";
            saveDialog.FileName = $"Отчет_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportToCsv((DataTable)dataGridViewResults.DataSource, saveDialog.FileName);
                    MessageBox.Show($"Отчет сохранен в файл: {saveDialog.FileName}",
                        "Отчет создан", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToCsv(DataTable dataTable, string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                var headers = new System.Collections.Generic.List<string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    headers.Add($"\"{column.ColumnName}\"");
                }
                writer.WriteLine(string.Join(",", headers));

                foreach (DataRow row in dataTable.Rows)
                {
                    var fields = new System.Collections.Generic.List<string>();
                    foreach (var field in row.ItemArray)
                    {
                        string fieldValue = field?.ToString() ?? "";
                        fieldValue = fieldValue.Replace("\"", "\"\"");
                        fields.Add($"\"{fieldValue}\"");
                    }
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }

        private void SqlReportsForm_Load(object sender, EventArgs e)
        {
        }

        private void TxtSqlQuery_TextChanged(object sender, EventArgs e)
        {
        }
    }
}