using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BeautySalonApp.Forms
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private DataGridView dataGridViewServices;
        private DataGridView dataGridViewClients;
        private DataGridView dataGridViewAppointments;
        private Button btnAddService;
        private Button btnDeleteService;
        private Button btnUpdateService;
        private TextBox txtServiceName;
        private TextBox txtServicePrice;
        private TextBox txtServiceDuration;
        private ComboBox cmbServiceCategory;
        private Button btnGenerateReport;
        private Button btnCompleteAppointment;
        private Button btnSqlReports;
        private Button btnSendReportEmail;
        private TextBox txtEmailAddress;

        // Новые поля для управления записями
        private ComboBox cmbEditClient;
        private ComboBox cmbEditService;
        private DateTimePicker dateTimePickerEdit;
        private ComboBox cmbCreateStatus;
        private Button btnUpdateAppointment;
        private Button btnDeleteAppointment;
        private Button btnAddAppointment;
        private ComboBox cmbAppointmentClient;
        private ComboBox cmbAppointmentService;
        private ComboBox cmbAppointmentStatus;
        private DateTimePicker dateTimePickerAppointment;

        // Поля для быстрых заметок
        private TabPage tabNotes;
        private ListBox lstNotes;
        private TextBox txtNoteTitle;
        private TextBox txtNoteContent;
        private ComboBox cmbNoteColor;

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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabServices = new System.Windows.Forms.TabPage();
            this.tabClients = new System.Windows.Forms.TabPage();
            this.tabAppointments = new System.Windows.Forms.TabPage();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.tabNotes = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabServices);
            this.tabControl.Controls.Add(this.tabClients);
            this.tabControl.Controls.Add(this.tabAppointments);
            this.tabControl.Controls.Add(this.tabReports);
            this.tabControl.Controls.Add(this.tabNotes);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 700);
            this.tabControl.TabIndex = 0;
            // 
            // tabServices
            // 
            this.tabServices.Location = new System.Drawing.Point(4, 29);
            this.tabServices.Name = "tabServices";
            this.tabServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabServices.Size = new System.Drawing.Size(992, 667);
            this.tabServices.TabIndex = 0;
            this.tabServices.Text = "Управление услугами";
            this.tabServices.UseVisualStyleBackColor = true;
            this.tabServices.Click += new System.EventHandler(this.TabServices_Click);
            // 
            // tabClients
            // 
            this.tabClients.Location = new System.Drawing.Point(4, 29);
            this.tabClients.Name = "tabClients";
            this.tabClients.Padding = new System.Windows.Forms.Padding(3);
            this.tabClients.Size = new System.Drawing.Size(992, 667);
            this.tabClients.TabIndex = 1;
            this.tabClients.Text = "Клиенты";
            this.tabClients.UseVisualStyleBackColor = true;
            // 
            // tabAppointments
            // 
            this.tabAppointments.Location = new System.Drawing.Point(4, 29);
            this.tabAppointments.Name = "tabAppointments";
            this.tabAppointments.Padding = new System.Windows.Forms.Padding(3);
            this.tabAppointments.Size = new System.Drawing.Size(992, 667);
            this.tabAppointments.TabIndex = 2;
            this.tabAppointments.Text = "Записи";
            this.tabAppointments.UseVisualStyleBackColor = true;
            // 
            // tabReports
            // 
            this.tabReports.Location = new System.Drawing.Point(4, 29);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(992, 667);
            this.tabReports.TabIndex = 3;
            this.tabReports.Text = "Отчеты";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // tabNotes
            // 
            this.tabNotes.Location = new System.Drawing.Point(4, 29);
            this.tabNotes.Name = "tabNotes";
            this.tabNotes.Size = new System.Drawing.Size(992, 667);
            this.tabNotes.TabIndex = 4;
            this.tabNotes.Text = "Заметки";
            this.tabNotes.UseVisualStyleBackColor = true;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.tabControl);
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Панель администратора - Салон Красоты";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void InitializeServicesTab(TabPage tab)
        {
            // DataGridView для услуг
            this.dataGridViewServices = new DataGridView();
            this.dataGridViewServices.Location = new Point(30, 30);
            this.dataGridViewServices.Size = new Size(850, 300);
            this.dataGridViewServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewServices.SelectionChanged += DataGridViewServices_SelectionChanged;
            this.dataGridViewServices.ReadOnly = true;
            tab.Controls.Add(this.dataGridViewServices);

            // Группа для управления услугами
            GroupBox managementGroup = new GroupBox();
            managementGroup.Text = "Управление услугами";
            managementGroup.Location = new Point(30, 350);
            managementGroup.Size = new Size(850, 200);
            managementGroup.Font = new Font("Arial", 10, FontStyle.Bold);
            tab.Controls.Add(managementGroup);

            // Поля для ввода
            Label lblName = new Label()
            {
                Text = "Название:",
                Location = new Point(30, 40),
                Size = new Size(100, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.txtServiceName = new TextBox()
            {
                Location = new Point(140, 40),
                Size = new Size(220, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };

            Label lblPrice = new Label()
            {
                Text = "Цена:",
                Location = new Point(30, 75),
                Size = new Size(100, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.txtServicePrice = new TextBox()
            {
                Location = new Point(140, 75),
                Size = new Size(220, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };

            Label lblDuration = new Label()
            {
                Text = "Длительность (мин):",
                Location = new Point(30, 110),
                Size = new Size(140, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.txtServiceDuration = new TextBox()
            {
                Location = new Point(180, 110),
                Size = new Size(180, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };

            Label lblCategory = new Label()
            {
                Text = "Категория:",
                Location = new Point(30, 145),
                Size = new Size(100, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbServiceCategory = new ComboBox()
            {
                Location = new Point(140, 145),
                Size = new Size(220, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };

            // Кнопки
            this.btnAddService = new Button()
            {
                Text = "Добавить услугу",
                Location = new Point(400, 40),
                Size = new Size(150, 35),
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnAddService.Click += BtnAddService_Click;

            this.btnUpdateService = new Button()
            {
                Text = "Обновить",
                Location = new Point(400, 85),
                Size = new Size(150, 35),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnUpdateService.Click += BtnUpdateService_Click;

            this.btnDeleteService = new Button()
            {
                Text = "Удалить",
                Location = new Point(400, 130),
                Size = new Size(150, 35),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnDeleteService.Click += BtnDeleteService_Click;

            // Добавление элементов в группу
            managementGroup.Controls.AddRange(new Control[] {
                lblName, this.txtServiceName,
                lblPrice, this.txtServicePrice,
                lblDuration, this.txtServiceDuration,
                lblCategory, this.cmbServiceCategory,
                this.btnAddService, this.btnUpdateService, this.btnDeleteService
            });
        }

        private void InitializeClientsTab(TabPage tab)
        {
            this.dataGridViewClients = new DataGridView();
            this.dataGridViewClients.Location = new Point(30, 30);
            this.dataGridViewClients.Size = new Size(850, 520);
            this.dataGridViewClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClients.ReadOnly = true;
            tab.Controls.Add(this.dataGridViewClients);
        }
        private void InitializeDatabaseTab(TabPage tab)
        {
            // Заголовок
            Label lblTitle = new Label
            {
                Text = "🗃️ Управление базой данных",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Location = new Point(30, 20),
                Size = new Size(400, 30)
            };
            tab.Controls.Add(lblTitle);

            // Кнопка конструктора таблиц
            Button btnTableBuilder = new Button
            {
                Text = "🧩 Конструктор таблиц",
                Location = new Point(30, 70),
                Size = new Size(200, 50),
                BackColor = Color.Purple,
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            btnTableBuilder.Click += (s, e) =>
            {
                TableBuilderForm builderForm = new TableBuilderForm();
                builderForm.ShowDialog();
            };
            tab.Controls.Add(btnTableBuilder);

            // Информация
            Label lblInfo = new Label
            {
                Text = "Создавайте новые таблицы и устанавливайте связи между ними.\n" +
                       "Поддерживаются Primary Keys и Foreign Keys.",
                Location = new Point(30, 140),
                Size = new Size(500, 40),
                Font = new Font("Arial", 9),
                ForeColor = Color.Gray
            };
            tab.Controls.Add(lblInfo);
        }

        private void InitializeAppointmentsTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(245, 247, 250);

            // ============ ЗАГОЛОВОК ============
            Label lblTitle = new Label()
            {
                Text = "📅 УПРАВЛЕНИЕ ЗАПИСЯМИ КЛИЕНТОВ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(30, 15),
                Size = new Size(450, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            tab.Controls.Add(lblTitle);

            // ============ ПАНЕЛЬ СТАТИСТИКИ ============
            Panel statsPanel = new Panel()
            {
                Location = new Point(500, 15),
                Size = new Size(380, 30),
                BackColor = Color.FromArgb(241, 245, 249),
                BorderStyle = BorderStyle.None
            };

            Label lblStats = new Label()
            {
                Text = "📊 Всего записей: 0 | Запланировано: 0 | Выполнено: 0",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(10, 5),
                Size = new Size(360, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            statsPanel.Controls.Add(lblStats);
            tab.Controls.Add(statsPanel);

            // ============ ТАБЛИЦА ЗАПИСЕЙ ============
            Label lblTableTitle = new Label()
            {
                Text = "📋 Список всех записей:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(30, 55),
                Size = new Size(200, 20)
            };
            tab.Controls.Add(lblTableTitle);

            this.dataGridViewAppointments = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(920, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                GridColor = Color.FromArgb(226, 232, 240),
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) }
            };

            this.dataGridViewAppointments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);
            this.dataGridViewAppointments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dataGridViewAppointments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.dataGridViewAppointments.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewAppointments.ColumnHeadersHeight = 35;
            this.dataGridViewAppointments.EnableHeadersVisualStyles = false;

            this.dataGridViewAppointments.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            this.dataGridViewAppointments.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            this.dataGridViewAppointments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            this.dataGridViewAppointments.DefaultCellStyle.SelectionForeColor = Color.White;

            tab.Controls.Add(this.dataGridViewAppointments);

            // ============ ЛЕВАЯ ПАНЕЛЬ: ДОБАВЛЕНИЕ ЗАПИСИ ============
            Panel createPanel = new Panel()
            {
                Location = new Point(30, 315),
                Size = new Size(450, 290),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            createPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, createPanel.Width - 1, createPanel.Height - 1);
                }
            };

            Label lblCreateTitle = new Label()
            {
                Text = "➕ Добавление новой записи",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 185, 129),
                Location = new Point(10, 10),
                Size = new Size(430, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            createPanel.Controls.Add(lblCreateTitle);

            // Клиент с автопоиском
            Label lblClient = new Label()
            {
                Text = "👤 Клиент",
                Location = new Point(15, 50),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            createPanel.Controls.Add(lblClient);

            this.cmbAppointmentClient = new ComboBox()
            {
                Location = new Point(15, 73),
                Size = new Size(420, 30),
                DropDownStyle = ComboBoxStyle.DropDownList, // только выбор из списка
                Font = new Font("Segoe UI", 9)
            };
            createPanel.Controls.Add(this.cmbAppointmentClient);

            // Услуга с автопоиском
            Label lblService = new Label()
            {
                Text = "💅 Услуга",
                Location = new Point(15, 115),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            createPanel.Controls.Add(lblService);

            this.cmbAppointmentService = new ComboBox()
            {
                Location = new Point(15, 138),
                Size = new Size(420, 30),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 9)
            };
            cmbAppointmentService.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbAppointmentService.AutoCompleteSource = AutoCompleteSource.ListItems;
            createPanel.Controls.Add(this.cmbAppointmentService);

            // Дата и время
            Label lblDate = new Label()
            {
                Text = "📆 Дата и время",
                Location = new Point(15, 180),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            createPanel.Controls.Add(lblDate);

            this.dateTimePickerAppointment = new DateTimePicker()
            {
                Location = new Point(140, 178),
                Size = new Size(180, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 9)
            };
            createPanel.Controls.Add(this.dateTimePickerAppointment);

            // Статус (только для добавления)
            Label lblCreateStatus = new Label()
            {
                Text = "📊 Статус",
                Location = new Point(330, 180),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            createPanel.Controls.Add(lblCreateStatus);

            this.cmbCreateStatus = new ComboBox()
            {
                Location = new Point(330, 203),
                Size = new Size(105, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };
            this.cmbCreateStatus.Items.AddRange(new string[] { "Запланирован", "Выполнен", "Отменен" });
            this.cmbCreateStatus.SelectedIndex = 0;
            createPanel.Controls.Add(this.cmbCreateStatus);

            // Кнопка добавления
            this.btnAddAppointment = new Button()
            {
                Text = "✓ Добавить запись",
                Location = new Point(15, 235),
                Size = new Size(420, 40),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            this.btnAddAppointment.FlatAppearance.BorderSize = 0;
            this.btnAddAppointment.Click += (s, e) =>
            {
                try
                {
                    // Проверяем выбор клиента
                    if (cmbAppointmentClient.SelectedItem == null || string.IsNullOrWhiteSpace(cmbAppointmentClient.Text))
                    {
                        MessageBox.Show("Выберите или введите клиента!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Проверяем выбор услуги
                    if (cmbAppointmentService.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите услугу!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int clientId;
                    int serviceId = Convert.ToInt32(((ComboboxItem)cmbAppointmentService.SelectedItem).Value);
                    string status = cmbCreateStatus.SelectedItem.ToString();

                    // Если выбран существующий клиент
                    if (cmbAppointmentClient.SelectedItem is ComboboxItem clientItem)
                    {
                        clientId = Convert.ToInt32(clientItem.Value);
                    }
                    else
                    {
                        // Создаем нового клиента (только имя и фамилия)
                        string fullName = cmbAppointmentClient.Text.Trim();
                        string[] nameParts = fullName.Split(' ');

                        if (nameParts.Length < 2)
                        {
                            MessageBox.Show("Введите Имя и Фамилию клиента через пробел!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string firstName = nameParts[0];
                        string lastName = nameParts[1];

                        // Создаем нового клиента с телефоном "Не указан"
                        string insertClientQuery = "INSERT INTO Clients (FirstName, LastName, Phone) VALUES (?, ?, ?)";
                        var clientParams = new OleDbParameter[]
                        {
                    new OleDbParameter("@FirstName", firstName),
                    new OleDbParameter("@LastName", lastName),
                    new OleDbParameter("@Phone", "Не указан")
                        };

                        if (!db.ExecuteNonQuery(insertClientQuery, clientParams))
                        {
                            MessageBox.Show("Ошибка создания клиента!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Получаем ID нового клиента
                        DataTable newClientTable = db.ExecuteQuery("SELECT @@IDENTITY");
                        clientId = Convert.ToInt32(newClientTable.Rows[0][0]);
                    }

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
                        LoadClientsComboBox();
                        cmbAppointmentClient.SelectedIndex = -1;
                        cmbAppointmentService.SelectedIndex = -1;
                        cmbCreateStatus.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            createPanel.Controls.Add(this.btnAddAppointment);

            tab.Controls.Add(createPanel);

            // ============ ПРАВАЯ ПАНЕЛЬ: РЕДАКТИРОВАНИЕ ЗАПИСИ ============
            Panel editPanel = new Panel()
            {
                Location = new Point(500, 315),
                Size = new Size(450, 290),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            editPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, editPanel.Width - 1, editPanel.Height - 1);
                }
            };

            Label lblEditTitle = new Label()
            {
                Text = "✏️ Редактирование записи",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 130, 246),
                Location = new Point(10, 10),
                Size = new Size(430, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            editPanel.Controls.Add(lblEditTitle);

            Label lblEditInfo = new Label()
            {
                Name = "lblEditInfo",
                Text = "ℹ️ Выберите запись из таблицы выше",
                Location = new Point(15, 50),
                Size = new Size(420, 225),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.None
            };
            editPanel.Controls.Add(lblEditInfo);

            // Панель с полями редактирования
            Panel editFieldsPanel = new Panel()
            {
                Name = "editFieldsPanel",
                Location = new Point(15, 50),
                Size = new Size(420, 225),
                BackColor = Color.White,
                Visible = false
            };

            // Клиент для редактирования
            Label lblEditClient = new Label()
            {
                Text = "👤 Клиент",
                Location = new Point(0, 0),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            editFieldsPanel.Controls.Add(lblEditClient);

            this.cmbEditClient = new ComboBox()
            {
                Name = "cmbEditClient",
                Location = new Point(0, 23),
                Size = new Size(420, 30),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 9)
            };
            cmbEditClient.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbEditClient.AutoCompleteSource = AutoCompleteSource.ListItems;
            editFieldsPanel.Controls.Add(this.cmbEditClient);

            // Услуга для редактирования
            Label lblEditService = new Label()
            {
                Text = "💅 Услуга",
                Location = new Point(0, 65),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            editFieldsPanel.Controls.Add(lblEditService);

            this.cmbEditService = new ComboBox()
            {
                Name = "cmbEditService",
                Location = new Point(0, 88),
                Size = new Size(420, 30),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 9),
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            editFieldsPanel.Controls.Add(this.cmbEditService);

            // Дата для редактирования
            Label lblEditDate = new Label()
            {
                Text = "📆 Дата и время",
                Location = new Point(0, 130),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            editFieldsPanel.Controls.Add(lblEditDate);

            this.dateTimePickerEdit = new DateTimePicker()
            {
                Name = "dateTimePickerEdit",
                Location = new Point(125, 128),
                Size = new Size(180, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 9)
            };
            editFieldsPanel.Controls.Add(this.dateTimePickerEdit);

            // Статус для редактирования
            Label lblEditStatus = new Label()
            {
                Text = "📊 Статус",
                Location = new Point(315, 130),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            editFieldsPanel.Controls.Add(lblEditStatus);

            this.cmbAppointmentStatus = new ComboBox()
            {
                Location = new Point(315, 153),
                Size = new Size(105, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };
            this.cmbAppointmentStatus.Items.AddRange(new string[] { "Запланирован", "Выполнен", "Отменен" });
            editFieldsPanel.Controls.Add(this.cmbAppointmentStatus);

            // Кнопки действий
            this.btnUpdateAppointment = new Button()
            {
                Text = "✓ Сохранить",
                Location = new Point(0, 186),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            this.btnUpdateAppointment.FlatAppearance.BorderSize = 0;
            this.btnUpdateAppointment.Click += (s, e) =>
            {
                if (dataGridViewAppointments.CurrentRow != null)
                {
                    if (cmbEditClient.SelectedItem == null || cmbEditService.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите клиента и услугу!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int id = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ID"].Value);
                    int clientId = Convert.ToInt32(((ComboboxItem)cmbEditClient.SelectedItem).Value);
                    int serviceId = Convert.ToInt32(((ComboboxItem)cmbEditService.SelectedItem).Value);
                    DateTime appointmentDate = dateTimePickerEdit.Value;
                    string status = cmbAppointmentStatus.SelectedItem?.ToString();

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
                        MessageBox.Show("Запись обновлена!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAppointments();
                    }
                }
            };
            editFieldsPanel.Controls.Add(this.btnUpdateAppointment);

            this.btnDeleteAppointment = new Button()
            {
                Text = "🗑️ Удалить",
                Location = new Point(145, 186),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            this.btnDeleteAppointment.FlatAppearance.BorderSize = 0;
            this.btnDeleteAppointment.Click += BtnDeleteAppointment_Click;
            editFieldsPanel.Controls.Add(this.btnDeleteAppointment);

            this.btnCompleteAppointment = new Button()
            {
                Text = "✓ Выполнена",
                Location = new Point(250, 186),
                Size = new Size(85, 35),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            this.btnCompleteAppointment.FlatAppearance.BorderSize = 0;
            this.btnCompleteAppointment.Click += BtnCompleteAppointment_Click;
            editFieldsPanel.Controls.Add(this.btnCompleteAppointment);

            Button btnCancelAppointment = new Button()
            {
                Text = "✖ Отменена",
                Location = new Point(345, 186),
                Size = new Size(75, 35),
                BackColor = Color.FromArgb(251, 146, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelAppointment.FlatAppearance.BorderSize = 0;
            btnCancelAppointment.Click += (s, e) =>
            {
                if (dataGridViewAppointments.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ID"].Value);
                    string query = "UPDATE Appointments SET Status = 'Отменен' WHERE ID = ?";
                    var parameters = new OleDbParameter[] { new OleDbParameter("@Id", id) };

                    if (db.ExecuteNonQuery(query, parameters))
                    {
                        MessageBox.Show("Запись отменена!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAppointments();
                    }
                }
            };
            editFieldsPanel.Controls.Add(btnCancelAppointment);

            editPanel.Controls.Add(editFieldsPanel);

            // Обработчик выбора записи из таблицы
            dataGridViewAppointments.SelectionChanged += (s, e) =>
            {
                if (dataGridViewAppointments.CurrentRow != null)
                {
                    lblEditInfo.Visible = false;
                    editFieldsPanel.Visible = true;

                    // Заполняем данные
                    int clientId = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ClientID"].Value);
                    int serviceId = Convert.ToInt32(dataGridViewAppointments.CurrentRow.Cells["ServiceID"].Value);

                    // Выбираем клиента
                    foreach (ComboboxItem item in cmbEditClient.Items)
                    {
                        if (Convert.ToInt32(item.Value) == clientId)
                        {
                            cmbEditClient.SelectedItem = item;
                            break;
                        }
                    }

                    // Выбираем услугу
                    foreach (ComboboxItem item in cmbEditService.Items)
                    {
                        if (Convert.ToInt32(item.Value) == serviceId)
                        {
                            cmbEditService.SelectedItem = item;
                            break;
                        }
                    }

                    DateTime appointmentDate = Convert.ToDateTime(dataGridViewAppointments.CurrentRow.Cells["AppointmentDate"].Value);
                    dateTimePickerEdit.Value = appointmentDate;

                    string status = dataGridViewAppointments.CurrentRow.Cells["Status"].Value.ToString();
                    cmbAppointmentStatus.SelectedItem = status;
                }
                else
                {
                    lblEditInfo.Visible = true;
                    editFieldsPanel.Visible = false;
                }
            };

            tab.Controls.Add(editPanel);

            // Обновление статистики
            dataGridViewAppointments.DataSourceChanged += (s, e) =>
            {
                if (dataGridViewAppointments.DataSource is DataTable dt)
                {
                    int total = dt.Rows.Count;
                    int planned = dt.AsEnumerable().Count(row => row["Status"].ToString() == "Запланирован");
                    int completed = dt.AsEnumerable().Count(row => row["Status"].ToString() == "Выполнен");
                    lblStats.Text = $"📊 Всего записей: {total} | Запланировано: {planned} | Выполнено: {completed}";
                }
            };

            // Заполнение ComboBox при загрузке
            this.Load += (s, e) =>
            {
                // Заполняем клиентов для редактирования
                DataTable clients = db.ExecuteQuery("SELECT ID, FirstName + ' ' + LastName as FullName FROM Clients ORDER BY LastName, FirstName");
                foreach (DataRow row in clients.Rows)
                {
                    cmbEditClient.Items.Add(new ComboboxItem
                    {
                        Text = row["FullName"].ToString(),
                        Value = row["ID"]
                    });
                }

                // Заполняем услуги для редактирования
                DataTable services = db.ExecuteQuery("SELECT ID, ServiceName, Price FROM Services ORDER BY ServiceName");
                foreach (DataRow row in services.Rows)
                {
                    cmbEditService.Items.Add(new ComboboxItem
                    {
                        Text = $"{row["ServiceName"]} - {row["Price"]} руб.",
                        Value = row["ID"]
                    });
                }
            };
        }

        private void InitializeReportsTab(TabPage tab)
        {
            // Группа для отчетов
            GroupBox reportsGroup = new GroupBox();
            reportsGroup.Text = "Генерация отчетов";
            reportsGroup.Location = new Point(30, 30);
            reportsGroup.Size = new Size(850, 200);
            reportsGroup.Font = new Font("Arial", 10, FontStyle.Bold);
            tab.Controls.Add(reportsGroup);

            // Кнопка создания отчета
            this.btnGenerateReport = new Button()
            {
                Text = "Создать отчет",
                Location = new Point(30, 40),
                Size = new Size(180, 45),
                BackColor = Color.Teal,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnGenerateReport.Click += BtnGenerateReport_Click;

            // Кнопка SQL отчетов
            this.btnSqlReports = new Button()
            {
                Text = "SQL Отчеты",
                Location = new Point(230, 40),
                Size = new Size(180, 45),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnSqlReports.Click += BtnSqlReports_Click;

            // Поле для ввода email
            Label lblEmail = new Label()
            {
                Text = "Email для отправки:",
                Location = new Point(30, 110),
                Size = new Size(160, 25),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            this.txtEmailAddress = new TextBox()
            {
                Location = new Point(200, 110),
                Size = new Size(250, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };

            // Кнопка отправки отчета на email
            this.btnSendReportEmail = new Button()
            {
                Text = "Отправить отчет на email",
                Location = new Point(470, 110),
                Size = new Size(200, 35),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnSendReportEmail.Click += BtnSendReportEmail_Click;

            // Добавление элементов в группу
            reportsGroup.Controls.AddRange(new Control[] {
                this.btnGenerateReport,
                this.btnSqlReports,
                lblEmail,
                this.txtEmailAddress,
                this.btnSendReportEmail
            });
        }

        private void InitializeNotesTab(TabPage tab)
        {
            // Заголовок
            Label lblTitle = new Label
            {
                Text = "📝 Быстрые заметки",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Location = new Point(30, 20),
                Size = new Size(300, 30)
            };
            tab.Controls.Add(lblTitle);

            // Поле заголовка
            Label lblNoteTitle = new Label
            {
                Text = "Заголовок:",
                Location = new Point(30, 70),
                Size = new Size(80, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            tab.Controls.Add(lblNoteTitle);

            this.txtNoteTitle = new TextBox
            {
                Location = new Point(110, 68),
                Size = new Size(300, 25),
                Font = new Font("Arial", 10),
            };
            tab.Controls.Add(this.txtNoteTitle);

            // Поле содержания
            Label lblNoteContent = new Label
            {
                Text = "Содержание:",
                Location = new Point(30, 110),
                Size = new Size(80, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            tab.Controls.Add(lblNoteContent);

            this.txtNoteContent = new TextBox
            {
                Location = new Point(30, 135),
                Size = new Size(500, 120),
                Font = new Font("Arial", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
            };
            tab.Controls.Add(this.txtNoteContent);

            // Цветовые метки
            Label lblColor = new Label
            {
                Text = "Цвет метки:",
                Location = new Point(30, 270),
                Size = new Size(80, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            tab.Controls.Add(lblColor);

            this.cmbNoteColor = new ComboBox
            {
                Location = new Point(110, 268),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.cmbNoteColor.Items.AddRange(new string[] {
                "Белая", "Желтая", "Голубая", "Зеленая", "Розовая", "Оранжевая"
            });
            this.cmbNoteColor.SelectedIndex = 0;
            tab.Controls.Add(this.cmbNoteColor);

            // Кнопки управления
            Button btnSaveNote = new Button
            {
                Text = "💾 Сохранить заметку",
                Location = new Point(30, 310),
                Size = new Size(150, 35),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnSaveNote.Click += BtnSaveNote_Click;
            tab.Controls.Add(btnSaveNote);

            Button btnClearNote = new Button
            {
                Text = "🗑️ Очистить",
                Location = new Point(190, 310),
                Size = new Size(100, 35),
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnClearNote.Click += BtnClearNote_Click;
            tab.Controls.Add(btnClearNote);

            // Список существующих заметок
            Label lblExistingNotes = new Label
            {
                Text = "Мои заметки:",
                Location = new Point(30, 370),
                Size = new Size(150, 20),
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            tab.Controls.Add(lblExistingNotes);
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.tabControl.Controls.Add(this.tabDatabase);
            this.tabDatabase.Text = "База данных";
            this.lstNotes = new ListBox
            {
                Location = new Point(30, 395),
                Size = new Size(500, 150),
                Font = new Font("Arial", 10),
                DisplayMember = "Title"
            };
            tab.Controls.Add(this.lstNotes);

            // Кнопки управления списком заметок
            Button btnLoadNote = new Button
            {
                Text = "📖 Открыть",
                Location = new Point(30, 555),
                Size = new Size(100, 30),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White
            };
            btnLoadNote.Click += BtnLoadNote_Click;
            tab.Controls.Add(btnLoadNote);

            Button btnDeleteNote = new Button
            {
                Text = "❌ Удалить",
                Location = new Point(140, 555),
                Size = new Size(100, 30),
                BackColor = Color.Crimson,
                ForeColor = Color.White
            };
            btnDeleteNote.Click += BtnDeleteNote_Click;
            tab.Controls.Add(btnDeleteNote);
        }

        private TabPage tabServices;
        private TabPage tabClients;
        private TabPage tabAppointments;
        private TabPage tabReports;
    }
}