using BeautySalonApp.Database;
using BeautySalonApp.Models;
using BeautySalonApp.Security;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace BeautySalonApp.Forms
{

        public partial class AuthForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();

        public AuthForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {

        }

        private void AuthForm_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (var registerForm = new Form())
            {
                registerForm.Text = "Регистрация нового пользователя";
                registerForm.Size = new Size(470, 330);
                registerForm.StartPosition = FormStartPosition.CenterParent;
                registerForm.BackColor = Color.White;
                registerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                registerForm.MaximizeBox = false;
                registerForm.Padding = new Padding(20);

                // Панель для содержимого
                Panel contentPanel = new Panel();
                contentPanel.Dock = DockStyle.Fill;
                contentPanel.BackColor = Color.Lavender;
                contentPanel.BorderStyle = BorderStyle.FixedSingle;
                registerForm.Controls.Add(contentPanel);

                // Заголовок
                Label lblTitle = new Label();
                lblTitle.Text = "Регистрация";
                lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
                lblTitle.ForeColor = Color.DarkSlateBlue;
                lblTitle.TextAlign = ContentAlignment.MiddleCenter;
                lblTitle.Size = new Size(300, 40);
                lblTitle.Location = new Point(30, 20);
                contentPanel.Controls.Add(lblTitle);

                // Поле логина
                Label lblUser = new Label();
                lblUser.Text = "Логин:";
                lblUser.Font = new Font("Arial", 10, FontStyle.Bold);
                lblUser.Size = new Size(80, 25);
                lblUser.Location = new Point(50, 80);
                contentPanel.Controls.Add(lblUser);

                TextBox txtNewUser = new TextBox();
                txtNewUser.Size = new Size(200, 25);
                txtNewUser.Location = new Point(130, 80);
                txtNewUser.Font = new Font("Arial", 10);
                txtNewUser.BorderStyle = BorderStyle.FixedSingle;
                txtNewUser.BackColor = Color.White;
                contentPanel.Controls.Add(txtNewUser);

                // Поле пароля
                Label lblPass = new Label();
                lblPass.Text = "Пароль:";
                lblPass.Font = new Font("Arial", 10, FontStyle.Bold);
                lblPass.Size = new Size(80, 25);
                lblPass.Location = new Point(50, 120);
                contentPanel.Controls.Add(lblPass);

                TextBox txtNewPass = new TextBox();
                txtNewPass.Size = new Size(200, 25);
                txtNewPass.Location = new Point(130, 120);
                txtNewPass.Font = new Font("Arial", 10);
                txtNewPass.BorderStyle = BorderStyle.FixedSingle;
                txtNewPass.BackColor = Color.White;
                txtNewPass.PasswordChar = '*';
                contentPanel.Controls.Add(txtNewPass);

                // Поле email
                Label lblEmail = new Label();
                lblEmail.Text = "Email:";
                lblEmail.Font = new Font("Arial", 10, FontStyle.Bold);
                lblEmail.Size = new Size(80, 25);
                lblEmail.Location = new Point(50, 160);
                contentPanel.Controls.Add(lblEmail);

                TextBox txtEmail = new TextBox();
                txtEmail.Size = new Size(200, 25);
                txtEmail.Location = new Point(130, 160);
                txtEmail.Font = new Font("Arial", 10);
                txtEmail.BorderStyle = BorderStyle.FixedSingle;
                txtEmail.BackColor = Color.White;
                contentPanel.Controls.Add(txtEmail);

                // Кнопка регистрации
                Button btnReg = new Button();
                btnReg.Text = "Зарегистрировать";
                btnReg.Size = new Size(150, 35);
                btnReg.Location = new Point(110, 200);
                btnReg.BackColor = Color.MediumSeaGreen;
                btnReg.ForeColor = Color.White;
                btnReg.Font = new Font("Arial", 10, FontStyle.Bold);
                btnReg.FlatStyle = FlatStyle.Flat;
                btnReg.FlatAppearance.BorderSize = 0;
                contentPanel.Controls.Add(btnReg);

                // Обработчик кнопки регистрации
                btnReg.Click += (s, ev) =>
                {
                    string username = txtNewUser.Text.Trim();
                    string password = txtNewPass.Text;
                    string email = txtEmail.Text.Trim();

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Заполните логин и пароль!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Проверяем, нет ли уже такого пользователя
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = ?";
                    var checkParams = new OleDbParameter[] { new OleDbParameter("@Username", username) };
                    DataTable checkResult = db.ExecuteQuery(checkQuery, checkParams);

                    if (Convert.ToInt32(checkResult.Rows[0][0]) > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string passwordHash = PasswordHasher.HashPassword(password);

                    string query = "INSERT INTO Users (Username, UserPass, Role, Email) VALUES (?, ?, ?, ?)";
                    var parameters = new OleDbParameter[]
                    {
new OleDbParameter { Value = username },
new OleDbParameter { Value = passwordHash }, // сохраняем ХЭШ
new OleDbParameter { Value = "user" },
new OleDbParameter { Value = string.IsNullOrEmpty(email) ? DBNull.Value : (object)email }
                    };

                    if (db.ExecuteNonQuery(query, parameters))
                    {
                        MessageBox.Show("Пользователь успешно зарегистрирован!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        registerForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при регистрации пользователя!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // Кнопка отмены
                Button btnCancel = new Button();
                btnCancel.Text = "Отмена";
                btnCancel.Size = new Size(80, 25);
                btnCancel.Location = new Point(270, 200);
                btnCancel.BackColor = Color.LightCoral;
                btnCancel.ForeColor = Color.White;
                btnCancel.Font = new Font("Arial", 9);
                btnCancel.FlatStyle = FlatStyle.Flat;
                btnCancel.FlatAppearance.BorderSize = 0;
                btnCancel.Click += (s, ev) => registerForm.Close();
                contentPanel.Controls.Add(btnCancel);

                registerForm.ShowDialog();
            }
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ОТЛАДКА: Проверяем кэш ДО проверки
            bool fromCache = PasswordCache.IsPasswordCached(username);
            string cachedPassword = PasswordCache.GetCachedPassword(username);

            string debugInfo = $"ДО проверки: Кэш={fromCache}, Пароль в кэше={cachedPassword != null}";

            // Проверяем кэш перед обращением к базе
            if (cachedPassword != null && cachedPassword == password)
            {
                // Пароль найден в кэше, выполняем вход
                debugInfo += $"\n✓ Вход через КЭШ (пароль совпал)";
                MessageBox.Show($"{debugInfo}\nДобро пожаловать, {username}!", "Успешный вход (КЭШ)",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                PerformLogin(username);
                return;
            }
            else if (cachedPassword != null)
            {
                debugInfo += $"\n✗ Пароль в кэше НЕ совпал: '{cachedPassword}' vs '{password}'";
            }

            // Если нет в кэше, проверяем в базе данных
            string query = "SELECT * FROM Users WHERE Username = ?";
            var parameters = new OleDbParameter[]
            {
new OleDbParameter("@Username", username)
            };

            DataTable result = db.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                string storedHash = row["UserPass"].ToString();

                // Проверяем хэш
                if (!PasswordHasher.VerifyPassword(password, storedHash))
                {
                    MessageBox.Show($"{debugInfo}\nНеверное имя пользователя или пароль!", "Ошибка входа",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                User currentUser = new User
                {
                    Id = Convert.ToInt32(row["ID"]),
                    Username = row["Username"].ToString(),
                    Role = row["Role"].ToString(),
                    Email = row["Email"].ToString()
                };

                // Кэшируем пароль только если отмечен чекбокс
                if (chkRememberMe.Checked)
                {
                    PasswordCache.CachePassword(username, password);
                    debugInfo += $"\n✓ Пароль закэширован для: {username}";
                }

                debugInfo += $"\n✓ Вход через БАЗУ ДАННЫХ (хэш)";
                MessageBox.Show($"{debugInfo}\nДобро пожаловать, {currentUser.Username}!", "Успешный вход",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (currentUser.Role == "admin")
                {
                    AdminForm adminForm = new AdminForm(currentUser);
                    adminForm.Show();
                }
                else
                {
                    UserForm userForm = new UserForm(currentUser);
                    userForm.Show();
                }
                this.Hide();

            }
            else
            {
                MessageBox.Show($"{debugInfo}\nНеверное имя пользователя или пароль!", "Ошибка входа",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformLogin(string username)
        {
            // Получаем данные пользователя из базы (без проверки пароля)
            string query = "SELECT * FROM Users WHERE Username = ?";
            var parameters = new OleDbParameter[]
            {
        new OleDbParameter("@Username", username)
            };

            DataTable result = db.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                User currentUser = new User
                {
                    Id = Convert.ToInt32(row["ID"]),
                    Username = row["Username"].ToString(),
                    Role = row["Role"].ToString(),
                    Email = row["Email"].ToString()
                };

                MessageBox.Show($"Добро пожаловать, {currentUser.Username}!", "Успешный вход",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (currentUser.Role == "admin")
                {
                    AdminForm adminForm = new AdminForm(currentUser);
                    adminForm.Show();
                }
                else
                {
                    UserForm userForm = new UserForm(currentUser);
                    userForm.Show();
                }
                this.Hide();
            }
        }

        private void BtnDebugCache_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            bool isCached = PasswordCache.IsPasswordCached(username);
            string cachedPassword = PasswordCache.GetCachedPassword(username);

            string message = $"Пользователь: {username}\n" +
                            $"В кэше: {(isCached ? "ДА" : "НЕТ")}\n" +
                            $"Пароль в кэше: {(cachedPassword != null ? "***" : "нет")}\n" +
                            $"Всего в кэше: {PasswordCache.GetCachedUsersCount()} пользователей";

            if (isCached)
            {
                message += $"\n\nОтладочная информация:\n" +
                          $"Фактический пароль в кэше: '{cachedPassword}'";
            }

            MessageBox.Show(message, "Отладочная информация о кэше",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    internal static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return string.Equals(hash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}