using BeautySalonApp.Database;
using BeautySalonApp.Models;
using BeautySalonApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;



namespace BeautySalonApp.Forms
{
    public partial class AdminForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();
        private User currentUser;
        private string lastGeneratedReportPath;
        private List<QuickNote> quickNotes = new List<QuickNote>();
        private string notesFilePath = "quick_notes.json";
        private TabPage tabDatabase;


        public AdminForm(User user)
        {
            currentUser = user;
            InitializeComponent();
            InitializeServicesTab(tabServices);
            InitializeClientsTab(tabClients);
            InitializeAppointmentsTab(tabAppointments);
            InitializeReportsTab(tabReports);
            InitializeNotesTab(tabNotes);
            InitializeDatabaseTab(tabDatabase); // ✅ Добавляем эту строку

            LoadQuickNotes();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (dataGridViewServices != null)
                {
                    string servicesQuery = @"
                        SELECT 
                            ID,
                            ServiceName,
                            Price,
                            Duration,
                            Category,
                            ROUND(Price / Duration, 2) AS PricePerMinute
                        FROM Services";

                    dataGridViewServices.DataSource = db.ExecuteQuery(servicesQuery);
                }

                if (dataGridViewClients != null)
                    dataGridViewClients.DataSource = db.ExecuteQuery("SELECT * FROM Clients");

                LoadAppointments();
                LoadClientsComboBox();
                LoadServicesComboBox();

                if (cmbServiceCategory != null)
                    LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAppointments()
        {
            if (dataGridViewAppointments == null) return;

            string query = @"
                SELECT Appointments.ID, 
                       Appointments.ClientID,
                       Appointments.ServiceID,
                       Clients.FirstName + ' ' + Clients.LastName as ClientName, 
                       Services.ServiceName, 
                       Services.Price, 
                       ROUND(Services.Price / Services.Duration, 2) AS PricePerMinute,
                       Appointments.AppointmentDate, 
                       Appointments.Status
                FROM (Appointments
                INNER JOIN Clients ON Appointments.ClientID = Clients.ID)
                INNER JOIN Services ON Appointments.ServiceID = Services.ID
                ORDER BY Appointments.AppointmentDate DESC";

            dataGridViewAppointments.DataSource = db.ExecuteQuery(query);
        }

        private void LoadClientsComboBox()
        {
            if (cmbAppointmentClient == null) return;

            cmbAppointmentClient.Items.Clear();
            DataTable clients = db.ExecuteQuery("SELECT ID, FirstName + ' ' + LastName as FullName FROM Clients ORDER BY LastName, FirstName");

            foreach (DataRow row in clients.Rows)
            {
                cmbAppointmentClient.Items.Add(new ComboboxItem
                {
                    Text = row["FullName"].ToString(),
                    Value = row["ID"]
                });
            }
        }

        private void LoadServicesComboBox()
        {
            if (cmbAppointmentService == null) return;

            cmbAppointmentService.Items.Clear();
            string query = @"
                SELECT 
                    ID, 
                    ServiceName, 
                    Price,
                    Duration,
                    ROUND(Price / Duration, 2) AS PricePerMinute
                FROM Services 
                ORDER BY ServiceName";

            DataTable services = db.ExecuteQuery(query);

            foreach (DataRow row in services.Rows)
            {
                cmbAppointmentService.Items.Add(new ComboboxItem
                {
                    Text = $"{row["ServiceName"]} - {row["Price"]} руб. ({row["PricePerMinute"]} руб./мин.)",
                    Value = row["ID"]
                });
            }
        }

        private void LoadCategories()
        {
            if (cmbServiceCategory == null) return;

            cmbServiceCategory.Items.Clear();
            cmbServiceCategory.Items.AddRange(new string[] { "Стрижки", "Окрашивание", "Маникюр", "Косметология", "Уходовые процедуры" });
        }

        private class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() { return Text; }
        }

        #region Управление услугами
        private void BtnAddService_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServiceName.Text) || string.IsNullOrEmpty(txtServicePrice.Text) ||
                string.IsNullOrEmpty(txtServiceDuration.Text) || cmbServiceCategory.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            string query = "INSERT INTO Services (ServiceName, Price, Duration, Category) VALUES (?, ?, ?, ?)";
            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@Name", txtServiceName.Text),
                new OleDbParameter("@Price", decimal.Parse(txtServicePrice.Text)),
                new OleDbParameter("@Duration", int.Parse(txtServiceDuration.Text)),
                new OleDbParameter("@Category", cmbServiceCategory.SelectedItem.ToString())
            };

            if (db.ExecuteNonQuery(query, parameters))
            {
                MessageBox.Show("Услуга добавлена!");
                LoadData();
                ClearServiceFields();
            }
        }

        private void BtnUpdateService_Click(object sender, EventArgs e)
        {
            if (dataGridViewServices.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridViewServices.CurrentRow.Cells["ID"].Value);
                string query = "UPDATE Services SET ServiceName = ?, Price = ?, Duration = ?, Category = ? WHERE ID = ?";
                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Name", txtServiceName.Text),
                    new OleDbParameter("@Price", decimal.Parse(txtServicePrice.Text)),
                    new OleDbParameter("@Duration", int.Parse(txtServiceDuration.Text)),
                    new OleDbParameter("@Category", cmbServiceCategory.SelectedItem.ToString()),
                    new OleDbParameter("@Id", id)
                };

                if (db.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Услуга обновлена!");
                    LoadData();
                    ClearServiceFields();
                }
            }
        }

        private void BtnDeleteService_Click(object sender, EventArgs e)
        {
            if (dataGridViewServices.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridViewServices.CurrentRow.Cells["ID"].Value);
                string query = "DELETE FROM Services WHERE ID = ?";
                var parameters = new OleDbParameter[] { new OleDbParameter("@Id", id) };

                if (db.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Услуга удалена!");
                    LoadData();
                    ClearServiceFields();
                }
            }
        }

        private void ClearServiceFields()
        {
            txtServiceName.Clear();
            txtServicePrice.Clear();
            txtServiceDuration.Clear();
            cmbServiceCategory.SelectedIndex = -1;
        }

        private void DataGridViewServices_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewServices.CurrentRow != null)
            {
                txtServiceName.Text = dataGridViewServices.CurrentRow.Cells["ServiceName"].Value.ToString();
                txtServicePrice.Text = dataGridViewServices.CurrentRow.Cells["Price"].Value.ToString();
                txtServiceDuration.Text = dataGridViewServices.CurrentRow.Cells["Duration"].Value.ToString();

                string category = dataGridViewServices.CurrentRow.Cells["Category"].Value.ToString();
                cmbServiceCategory.SelectedItem = category;
            }
        }
        #endregion

        #region Управление записями
        private void BtnAddAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                // Извлекаем ID клиента из текста (формат: "ID - Имя Фамилия")
                string clientText = cmbAppointmentClient.Text;
                if (!int.TryParse(clientText.Split('-')[0].Trim(), out int clientId))
                {
                    MessageBox.Show("Некорректный формат клиента! Используйте поиск для выбора клиента.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Извлекаем ID услуги из текста (формат: "ID - Название услуги")
                string serviceText = cmbAppointmentService.Text;
                if (!int.TryParse(serviceText.Split('-')[0].Trim(), out int serviceId))
                {
                    MessageBox.Show("Некорректный формат услуги! Используйте поиск для выбора услуги.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем статус
                if (cmbAppointmentStatus.SelectedItem == null)
                {
                    MessageBox.Show("Выберите статус записи!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string status = cmbAppointmentStatus.SelectedItem.ToString();

                // Создаем запись
                string insertQuery = @"INSERT INTO Appointments (ClientID, ServiceID, AppointmentDate, Status) 
                              VALUES (?, ?, ?, ?)";
                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@ClientID", clientId),
                    new OleDbParameter("@ServiceID", serviceId),
                    new OleDbParameter("@AppointmentDate", dateTimePickerAppointment.Value),
                    new OleDbParameter("@Status", status)
                };

                if (db.ExecuteNonQuery(insertQuery, parameters))
                {
                    MessageBox.Show("Запись успешно добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments();
                    ClearAppointmentFields();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении записи!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdateAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.CurrentRow != null &&
                cmbAppointmentClient.SelectedItem != null &&
                cmbAppointmentService.SelectedItem != null)
            {
                int id = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ID"].Value);
                int clientId = Convert.ToInt32(((ComboboxItem)cmbAppointmentClient.SelectedItem).Value);
                int serviceId = Convert.ToInt32(((ComboboxItem)cmbAppointmentService.SelectedItem).Value);
                DateTime appointmentDate = dateTimePickerAppointment.Value;
                string status = cmbAppointmentStatus.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Выберите статус!");
                    return;
                }

                string query = "UPDATE Appointments SET ClientID = ?, ServiceID = ?, AppointmentDate = ?, Status = ? WHERE ID = ?";
                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@ClientID", clientId),
                    new OleDbParameter("@ServiceID", serviceId),
                    new OleDbParameter("@AppointmentDate", appointmentDate),
                    new OleDbParameter("@Status", status),
                    new OleDbParameter("@Id", id)
                };

                if (db.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Запись обновлена!");
                    LoadAppointments();
                    ClearAppointmentFields();
                }
            }
        }

        private void BtnDeleteAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ID"].Value);
                string query = "DELETE FROM Appointments WHERE ID = ?";
                var parameters = new OleDbParameter[] { new OleDbParameter("@Id", id) };

                if (db.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Запись удалена!");
                    LoadAppointments();
                    ClearAppointmentFields();
                }
            }
        }

        private void ClearAppointmentFields()
        {
            cmbAppointmentClient.Text = "Введите ID или имя клиента...";
            cmbAppointmentService.Text = "Введите ID или название услуги...";
            cmbAppointmentStatus.SelectedIndex = -1;
            dateTimePickerAppointment.Value = DateTime.Now;
        }

        private void DataGridViewAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.CurrentRow != null)
            {
                int clientId = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ClientID"].Value);
                int serviceId = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ServiceID"].Value);

                foreach (ComboboxItem item in cmbAppointmentClient.Items)
                {
                    if (Convert.ToInt32(item.Value) == clientId)
                    {
                        cmbAppointmentClient.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboboxItem item in cmbAppointmentService.Items)
                {
                    if (Convert.ToInt32(item.Value) == serviceId)
                    {
                        cmbAppointmentService.SelectedItem = item;
                        break;
                    }
                }

                DateTime appointmentDate = Convert.ToDateTime(dataGridViewAppointments.CurrentRow.Cells["AppointmentDate"].Value);
                dateTimePickerAppointment.Value = appointmentDate;

                string status = dataGridViewAppointments.CurrentRow.Cells["Status"].Value.ToString();
                cmbAppointmentStatus.SelectedItem = status;
            }
        }

        private void BtnCompleteAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ID"].Value);
                string query = "UPDATE Appointments SET Status = 'Выполнен' WHERE ID = ?";
                var parameters = new OleDbParameter[] { new OleDbParameter("@Id", id) };

                if (db.ExecuteNonQuery(query, parameters))
                {
                    MessageBox.Show("Статус записи обновлен!");
                    LoadAppointments();
                }
            }
        }
        #endregion

        #region Поиск клиентов и услуг для записей
        private void BtnSearchClientForAppointment_Click(object sender, EventArgs e)
        {
            string searchText = cmbAppointmentClient.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == "Введите ID или имя клиента...")
            {
                MessageBox.Show("Введите ID или имя клиента для поиска!", "Поиск клиента",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query;
                OleDbParameter[] parameters;

                if (int.TryParse(searchText, out int clientId))
                {
                    query = "SELECT ID, FirstName, LastName, Phone FROM Clients WHERE ID = ?";
                    parameters = new OleDbParameter[] { new OleDbParameter("@ID", clientId) };
                }
                else
                {
                    query = "SELECT ID, FirstName, LastName, Phone FROM Clients WHERE FirstName LIKE ? OR LastName LIKE ?";
                    parameters = new OleDbParameter[] {
                        new OleDbParameter("@FirstName", "%" + searchText + "%"),
                        new OleDbParameter("@LastName", "%" + searchText + "%")
                    };
                }

                DataTable result = db.ExecuteQuery(query, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    if (result.Rows.Count == 1)
                    {
                        DataRow row = result.Rows[0];
                        string clientInfo = $"{row["ID"]} - {row["FirstName"]} {row["LastName"]} ({row["Phone"]})";
                        cmbAppointmentClient.Text = clientInfo;
                        MessageBox.Show($"Найден клиент: {clientInfo}", "Результат поиска",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ShowClientSelectionDialog(result);
                    }
                }
                else
                {
                    MessageBox.Show("Клиент не найден!", "Результат поиска",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска клиента: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearchServiceForAppointment_Click(object sender, EventArgs e)
        {
            string searchText = cmbAppointmentService.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == "Введите ID или название услуги...")
            {
                MessageBox.Show("Введите ID или название услуги для поиска!", "Поиск услуги",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query;
                OleDbParameter[] parameters;

                if (int.TryParse(searchText, out int serviceId))
                {
                    query = "SELECT ID, ServiceName, Price, Duration FROM Services WHERE ID = ?";
                    parameters = new OleDbParameter[] { new OleDbParameter("@ID", serviceId) };
                }
                else
                {
                    query = "SELECT ID, ServiceName, Price, Duration FROM Services WHERE ServiceName LIKE ?";
                    parameters = new OleDbParameter[] {
                        new OleDbParameter("@ServiceName", "%" + searchText + "%")
                    };
                }

                DataTable result = db.ExecuteQuery(query, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    if (result.Rows.Count == 1)
                    {
                        DataRow row = result.Rows[0];
                        string serviceInfo = $"{row["ID"]} - {row["ServiceName"]} ({row["Price"]} руб., {row["Duration"]} мин.)";
                        cmbAppointmentService.Text = serviceInfo;
                        MessageBox.Show($"Найдена услуга: {serviceInfo}", "Результат поиска",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ShowServiceSelectionDialog(result);
                    }
                }
                else
                {
                    MessageBox.Show("Услуга не найдена!", "Результат поиска",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска услуги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowClientSelectionDialog(DataTable clients)
        {
            using (var form = new Form())
            {
                form.Text = "Выберите клиента";
                form.Size = new Size(500, 300);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;

                var label = new Label()
                {
                    Text = "Найдено несколько клиентов. Выберите нужного:",
                    Location = new Point(10, 10),
                    Size = new Size(450, 20),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };

                var listBox = new ListBox()
                {
                    Location = new Point(10, 40),
                    Size = new Size(450, 150),
                    Font = new Font("Arial", 9)
                };

                foreach (DataRow row in clients.Rows)
                {
                    string clientInfo = $"{row["ID"]} - {row["FirstName"]} {row["LastName"]} ({row["Phone"]})";
                    listBox.Items.Add(clientInfo);
                }

                var btnSelect = new Button()
                {
                    Text = "Выбрать",
                    Location = new Point(150, 200),
                    Size = new Size(80, 30),
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White
                };

                var btnCancel = new Button()
                {
                    Text = "Отмена",
                    Location = new Point(250, 200),
                    Size = new Size(80, 30)
                };

                btnSelect.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null)
                    {
                        cmbAppointmentClient.Text = listBox.SelectedItem.ToString();
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("Выберите клиента из списка!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                btnCancel.Click += (s, e) => form.Close();

                form.Controls.AddRange(new Control[] { label, listBox, btnSelect, btnCancel });
                form.ShowDialog();
            }
        }

        private void ShowServiceSelectionDialog(DataTable services)
        {
            using (var form = new Form())
            {
                form.Text = "Выберите услугу";
                form.Size = new Size(500, 300);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;

                var label = new Label()
                {
                    Text = "Найдено несколько услуг. Выберите нужную:",
                    Location = new Point(10, 10),
                    Size = new Size(450, 20),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };

                var listBox = new ListBox()
                {
                    Location = new Point(10, 40),
                    Size = new Size(450, 150),
                    Font = new Font("Arial", 9)
                };

                foreach (DataRow row in services.Rows)
                {
                    string serviceInfo = $"{row["ID"]} - {row["ServiceName"]} ({row["Price"]} руб., {row["Duration"]} мин.)";
                    listBox.Items.Add(serviceInfo);
                }

                var btnSelect = new Button()
                {
                    Text = "Выбрать",
                    Location = new Point(150, 200),
                    Size = new Size(80, 30),
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White
                };

                var btnCancel = new Button()
                {
                    Text = "Отмена",
                    Location = new Point(250, 200),
                    Size = new Size(80, 30)
                };

                btnSelect.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null)
                    {
                        cmbAppointmentService.Text = listBox.SelectedItem.ToString();
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("Выберите услугу из списка!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                btnCancel.Click += (s, e) => form.Close();

                form.Controls.AddRange(new Control[] { label, listBox, btnSelect, btnCancel });
                form.ShowDialog();
            }
        }
        #endregion

        #region Отчеты
        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Текстовый файл (*.txt)|*.txt";
            saveDialog.FileName = $"Отчет_салон_{DateTime.Now:ddMMyyyy}.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                    {
                        writer.WriteLine($"ОТЧЕТ САЛОНА КРАСОТЫ 'ПРЕСТИЖ'");
                        writer.WriteLine($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}");
                        writer.WriteLine("=============================================");

                        writer.WriteLine("\nСТАТИСТИКА ПО УСЛУГАМ:");
                        string servicesQuery = @"
                            SELECT 
                                ServiceName, 
                                Price, 
                                Duration,
                                ROUND(Price / Duration, 2) AS PricePerMinute,
                                Category 
                            FROM Services 
                            ORDER BY Category, ServiceName";

                        DataTable services = db.ExecuteQuery(servicesQuery);

                        string currentCategory = "";
                        foreach (DataRow row in services.Rows)
                        {
                            if (currentCategory != row["Category"].ToString())
                            {
                                currentCategory = row["Category"].ToString();
                                writer.WriteLine($"\n--- {currentCategory} ---");
                            }
                            writer.WriteLine($"  {row["ServiceName"]} - {row["Price"]} руб. ({row["PricePerMinute"]} руб./мин.)");
                        }

                        writer.WriteLine("\nПРЕДСТОЯЩИЕ ЗАПИСИ (завтра):");
                        string appointmentsQuery = @"
                            SELECT Clients.FirstName, Clients.LastName, Services.ServiceName, Appointments.AppointmentDate 
                            FROM (Appointments
                              INNER JOIN Clients ON Appointments.ClientID = Clients.ID)
                              INNER JOIN Services ON Appointments.ServiceID = Services.ID
                            WHERE 
                              Appointments.AppointmentDate >= ? AND Appointments.AppointmentDate < ? 
                              AND Appointments.Status = 'Запланирован'
                            ORDER BY Appointments.AppointmentDate";

                        var parameters = new OleDbParameter[]
                        {
                            new OleDbParameter("@Start", DateTime.Today.AddDays(1)),
                            new OleDbParameter("@End", DateTime.Today.AddDays(2))
                        };

                        DataTable appointments = db.ExecuteQuery(appointmentsQuery, parameters);
                        if (appointments.Rows.Count > 0)
                        {
                            foreach (DataRow row in appointments.Rows)
                            {
                                writer.WriteLine($"  {row["FirstName"]} {row["LastName"]} - {row["ServiceName"]} " +
                                               $"в {Convert.ToDateTime(row["AppointmentDate"]):HH:mm}");
                            }
                        }
                        else
                        {
                            writer.WriteLine("  Записей нет");
                        }

                        writer.WriteLine("\n=============================================");
                        writer.WriteLine("Конец отчета");
                    }

                    lastGeneratedReportPath = saveDialog.FileName;
                    MessageBox.Show($"Отчет сохранен в файл: {saveDialog.FileName}", "Отчет создан",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSendReportEmail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastGeneratedReportPath))
            {
                MessageBox.Show("Сначала создайте отчет!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtEmailAddress.Text) || !txtEmailAddress.Text.Contains("@"))
            {
                MessageBox.Show("Введите корректный email адрес!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (EmailService.SendReport(txtEmailAddress.Text, lastGeneratedReportPath))
                {
                    MessageBox.Show($"Отчет успешно отправлен на email: {txtEmailAddress.Text}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке отчета: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSqlReports_Click(object sender, EventArgs e)
        {
            SqlReportsForm reportsForm = new SqlReportsForm();
            reportsForm.ShowDialog();
        }
        #endregion

        #region Быстрые заметки
        private void LoadQuickNotes()
        {
            try
            {
                if (File.Exists(notesFilePath))
                {
                    string json = File.ReadAllText(notesFilePath);
                    quickNotes = JsonConvert.DeserializeObject<List<QuickNote>>(json) ?? new List<QuickNote>();
                    LoadNotesToListBox();
                }
            }
            catch (Exception)
            {
                quickNotes = new List<QuickNote>();
            }
        }

        private void SaveQuickNotes()
        {
            try
            {
                string json = JsonConvert.SerializeObject(quickNotes, Formatting.Indented);
                File.WriteAllText(notesFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения заметок: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNotesToListBox()
        {
            if (lstNotes == null) return;

            lstNotes.Items.Clear();
            foreach (var note in quickNotes.OrderByDescending(n => n.ModifiedDate ?? n.CreatedDate))
            {
                lstNotes.Items.Add(note);
            }
        }

        private void BtnSaveNote_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoteTitle.Text))
            {
                MessageBox.Show("Введите заголовок заметки!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            QuickNote newNote = new QuickNote
            {
                Id = quickNotes.Count > 0 ? quickNotes.Max(n => n.Id) + 1 : 1,
                Title = txtNoteTitle.Text.Trim(),
                Content = txtNoteContent.Text.Trim(),
                Color = cmbNoteColor.SelectedItem.ToString(),
                ModifiedDate = DateTime.Now
            };

            quickNotes.Add(newNote);
            SaveQuickNotes();
            LoadNotesToListBox();

            MessageBox.Show("Заметка сохранена!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearNoteFields();
        }

        private void BtnClearNote_Click(object sender, EventArgs e)
        {
            ClearNoteFields();
        }

        private void BtnLoadNote_Click(object sender, EventArgs e)
        {
            if (lstNotes.SelectedItem is QuickNote selectedNote)
            {
                txtNoteTitle.Text = selectedNote.Title;
                txtNoteContent.Text = selectedNote.Content;

                for (int i = 0; i < cmbNoteColor.Items.Count; i++)
                {
                    if (cmbNoteColor.Items[i].ToString() == selectedNote.Color)
                    {
                        cmbNoteColor.SelectedIndex = i;
                        break;
                    }
                }

                MessageBox.Show($"Заметка загружена!\nСоздана: {selectedNote.CreatedDate:dd.MM.yyyy HH:mm}",
                    "Заметка загружена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выберите заметку из списка!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDeleteNote_Click(object sender, EventArgs e)
        {
            if (lstNotes.SelectedItem is QuickNote selectedNote)
            {
                var result = MessageBox.Show($"Удалить заметку \"{selectedNote.Title}\"?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    quickNotes.Remove(selectedNote);
                    SaveQuickNotes();
                    LoadNotesToListBox();
                    ClearNoteFields();

                    MessageBox.Show("Заметка удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите заметку для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearNoteFields()
        {
            txtNoteTitle.Clear();
            txtNoteContent.Clear();
            cmbNoteColor.SelectedIndex = 0;
        }
        #endregion

        private void TabServices_Click(object sender, EventArgs e)
        {
        }
    }
}