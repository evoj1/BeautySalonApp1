using BeautySalonApp.Database;
using BeautySalonApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace BeautySalonApp.Forms
{
    public partial class UserForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();
        private User currentUser;

        public UserForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            InitializeServicesTab(tabServices);
            //InitializeClientSearchTab(tabClientSearch);
            this.Load += UserForm_Load;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в LoadServices: " + ex.Message);
            }
        }

        private void LoadServices()
        {
            try
            {
                if (db == null)
                {
                    db = new DatabaseHelper();
                }

                if (dataGridViewServices == null)
                {
                    MessageBox.Show("DataGridView не инициализирован");
                    return;
                }

                string query = @"
                    SELECT 
                        ID,
                        ServiceName,
                        Price,
                        Duration,
                        Category,
                        ROUND(Price / Duration, 2) AS PricePerMinute
                    FROM Services";

                var result = db.ExecuteQuery(query);

                if (result != null)
                {
                    dataGridViewServices.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить услуги");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке услуг: " + ex.Message);
            }
        }

        private void BtnFilterServices_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
                    SELECT 
                        ID,
                        ServiceName,
                        Price,
                        Duration,
                        Category,
                        ROUND(Price / Duration, 2) AS PricePerMinute
                    FROM Services WHERE 1=1";

                var parameters = new List<OleDbParameter>();

                if (!string.IsNullOrEmpty(txtFilterCategory.Text))
                {
                    query += " AND Category LIKE ?";
                    parameters.Add(new OleDbParameter("@Category", "%" + txtFilterCategory.Text + "%"));
                }

                if (!string.IsNullOrEmpty(txtFilterMinPrice.Text) && decimal.TryParse(txtFilterMinPrice.Text, out decimal minPrice))
                {
                    query += " AND Price >= ?";
                    parameters.Add(new OleDbParameter("@MinPrice", minPrice));
                }

                if (!string.IsNullOrEmpty(txtFilterMaxPrice.Text) && decimal.TryParse(txtFilterMaxPrice.Text, out decimal maxPrice))
                {
                    query += " AND Price <= ?";
                    parameters.Add(new OleDbParameter("@MaxPrice", maxPrice));
                }

                var result = db.ExecuteQuery(query, parameters.ToArray());
                if (result != null)
                {
                    dataGridViewServices.DataSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при фильтрации: " + ex.Message);
            }
        }
        private void TxtFilterCategory_KeyPress(object sender, KeyPressEventArgs e)
{
    // Разрешаем управляющие символы (Backspace, Delete и т.д.)
    if (char.IsControl(e.KeyChar))
    {
        return;
    }

    // Разрешаем пробел
    if (e.KeyChar == ' ')
    {
        return;
    }

    // Проверяем, что символ - русская буква
    if (!IsRussianLetter(e.KeyChar))
    {
        e.Handled = true;
        System.Media.SystemSounds.Beep.Play(); // Звуковое предупреждение
    }
}

private bool IsRussianLetter(char c)
{
    // Русский алфавит в Unicode
    return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') || c == 'Ё' || c == 'ё';
}

        private int FindOrCreateClient(string phone)
        {
            try
            {
                string findQuery = "SELECT ID FROM Clients WHERE Phone = ?";
                var findParams = new OleDbParameter[] { new OleDbParameter("@Phone", phone) };
                DataTable clientResult = db.ExecuteQuery(findQuery, findParams);

                if (clientResult != null && clientResult.Rows.Count > 0)
                {
                    return Convert.ToInt32(clientResult.Rows[0]["ID"]);
                }
                else
                {
                    string createQuery = "INSERT INTO Clients (FirstName, LastName, Phone) VALUES (?, ?, ?)";
                    var createParams = new OleDbParameter[]
                    {
                        new OleDbParameter("@FirstName", txtClientFirstName.Text),
                        new OleDbParameter("@LastName", txtClientLastName.Text),
                        new OleDbParameter("@Phone", phone)
                    };

                    if (db.ExecuteNonQuery(createQuery, createParams))
                    {
                        var newFindParams = new OleDbParameter[] { new OleDbParameter("@Phone", phone) };
                        clientResult = db.ExecuteQuery(findQuery, newFindParams);
                        if (clientResult != null && clientResult.Rows.Count > 0)
                            return Convert.ToInt32(clientResult.Rows[0]["ID"]);
                        else
                            return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске/создании клиента: " + ex.Message);
            }
            return -1;
        }

        private void BtnCreateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewServices.CurrentRow != null)
                {
                    int serviceId = Convert.ToInt32(dataGridViewServices.CurrentRow.Cells["ID"].Value);
                    string clientPhone = txtClientPhone.Text.Trim();

                    if (string.IsNullOrEmpty(clientPhone))
                    {
                        MessageBox.Show("Введите телефон клиента!");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtClientFirstName.Text) || string.IsNullOrEmpty(txtClientLastName.Text))
                    {
                        MessageBox.Show("Введите имя и фамилию клиента!");
                        return;
                    }

                    int clientId = FindOrCreateClient(clientPhone);

                    if (clientId == -1)
                    {
                        MessageBox.Show("Ошибка при создании клиента!");
                        return;
                    }

                    string insertQuery = @"INSERT INTO Appointments (ClientID, ServiceID, AppointmentDate, Status) 
                                          VALUES (?, ?, ?, 'Запланирован')";
                    var parameters = new OleDbParameter[]
                    {
                        new OleDbParameter("@ClientID", clientId),
                        new OleDbParameter("@ServiceID", serviceId),
                        new OleDbParameter("@AppointmentDate", dateTimePicker.Value)
                    };

                    if (db.ExecuteNonQuery(insertQuery, parameters))
                    {
                        string serviceName = dataGridViewServices.CurrentRow.Cells["ServiceName"].Value.ToString();
                        string servicePrice = dataGridViewServices.CurrentRow.Cells["Price"].Value.ToString();
                        string pricePerMinute = dataGridViewServices.CurrentRow.Cells["PricePerMinute"].Value.ToString();

                        MessageBox.Show($"Клиент успешно записан!\n\n" +
                                      $"Услуга: {serviceName}\n" +
                                      $"Стоимость: {servicePrice} руб.\n" +
                                      $"Цена за минуту: {pricePerMinute} руб.\n" +
                                      $"Дата и время: {dateTimePicker.Value:dd.MM.yyyy HH:mm}",
                                      "Запись создана",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearOrderFields();
                        LoadClientAppointments(clientId);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите услугу из списка!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании заказа: " + ex.Message);
            }
        }

        private void LoadClientAppointments(int clientId)
        {
            try
            {
                string query = @"
                    SELECT 
                        s.ServiceName, 
                        a.AppointmentDate, 
                        a.Status, 
                        s.Price,
                        ROUND(s.Price / s.Duration, 2) AS PricePerMinute
                    FROM Appointments a
                    INNER JOIN Services s ON a.ServiceID = s.ID
                    WHERE a.ClientID = ?
                    ORDER BY a.AppointmentDate DESC";

                var parameters = new OleDbParameter[] { new OleDbParameter("@ClientID", clientId) };
                var result = db.ExecuteQuery(query, parameters);
                if (result != null)
                {
                    dataGridViewClientAppointments.DataSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке записей клиента: " + ex.Message);
            }
        }

        private void ClearOrderFields()
        {
            txtClientFirstName.Clear();
            txtClientLastName.Clear();
            txtClientPhone.Clear();
        }

        private void DataGridViewServices_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewServices.CurrentRow != null &&
                    dataGridViewServices.CurrentRow.Cells["ServiceName"].Value != null)
                {
                    string serviceName = dataGridViewServices.CurrentRow.Cells["ServiceName"].Value.ToString();
                    string price = dataGridViewServices.CurrentRow.Cells["Price"].Value.ToString();
                    string duration = dataGridViewServices.CurrentRow.Cells["Duration"].Value.ToString();
                    string pricePerMinute = dataGridViewServices.CurrentRow.Cells["PricePerMinute"].Value.ToString();

                    lblSelectedService.Text = $"Выбрано: {serviceName} - {price} руб. ({duration} мин.) - {pricePerMinute} руб./мин.";
                }
            }
            catch (Exception ex)
            {
                // Игнорируем ошибки при изменении выделения
            }
        }

        private void BtnSearchClient_Click(object sender, EventArgs e)
        {
            try
            {
                string phone = txtSearchPhone.Text.Trim();
                if (!string.IsNullOrEmpty(phone))
                {
                    string query = "SELECT * FROM Clients WHERE Phone = ?";
                    var parameters = new OleDbParameter[] { new OleDbParameter("@Phone", phone) };
                    DataTable client = db.ExecuteQuery(query, parameters);

                    if (client != null && client.Rows.Count > 0)
                    {
                        DataRow row = client.Rows[0];
                        txtClientFirstName.Text = row["FirstName"].ToString();
                        txtClientLastName.Text = row["LastName"].ToString();
                        txtClientPhone.Text = row["Phone"].ToString();

                        LoadClientAppointments(Convert.ToInt32(row["ID"]));
                    }
                    else
                    {
                        MessageBox.Show("Клиент с таким телефоном не найден. Заполните данные для новой записи.");
                        txtClientFirstName.Clear();
                        txtClientLastName.Clear();
                        txtClientPhone.Text = phone;
                        dataGridViewClientAppointments.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске клиента: " + ex.Message);
            }
        }

        private void TabServices_Click(object sender, EventArgs e)
        {
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tabServices_Click_1(object sender, EventArgs e)
        {

        }
    }
}