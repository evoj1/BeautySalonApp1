using System.Drawing;
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
            // DataGridView для записей
            this.dataGridViewAppointments = new DataGridView();
            this.dataGridViewAppointments.Location = new Point(30, 30);
            this.dataGridViewAppointments.Size = new Size(850, 300);
            this.dataGridViewAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAppointments.SelectionChanged += DataGridViewAppointments_SelectionChanged;
            this.dataGridViewAppointments.ReadOnly = true;
            tab.Controls.Add(this.dataGridViewAppointments);

            // Группа для управления записями
            GroupBox managementGroup = new GroupBox();
            managementGroup.Text = "Управление записями";
            managementGroup.Location = new Point(30, 350);
            managementGroup.Size = new Size(850, 200);
            managementGroup.Font = new Font("Arial", 10, FontStyle.Bold);
            tab.Controls.Add(managementGroup);

            // Поля для редактирования записи
            Label lblClient = new Label()
            {
                Text = "Клиент:",
                Location = new Point(30, 40),
                Size = new Size(80, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbAppointmentClient = new ComboBox()
            {
                Location = new Point(120, 40),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbAppointmentClient.Text = "Введите ID или имя клиента...";

            // Кнопка поиска клиента
            Button btnSearchClient = new Button()
            {
                Text = "Найти",
                Location = new Point(350, 40),
                Size = new Size(60, 25),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            btnSearchClient.Click += BtnSearchClientForAppointment_Click;

            Label lblService = new Label()
            {
                Text = "Услуга:",
                Location = new Point(30, 75),
                Size = new Size(80, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbAppointmentService = new ComboBox()
            {
                Location = new Point(120, 75),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbAppointmentService.Text = "Введите ID или название услуги...";

            // Кнопка поиска услуги
            Button btnSearchService = new Button()
            {
                Text = "Найти",
                Location = new Point(350, 75),
                Size = new Size(60, 25),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            btnSearchService.Click += BtnSearchServiceForAppointment_Click;

            Label lblDate = new Label()
            {
                Text = "Дата и время:",
                Location = new Point(30, 110),
                Size = new Size(100, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.dateTimePickerAppointment = new DateTimePicker()
            {
                Location = new Point(140, 110),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm",
                ShowUpDown = true,
                Font = new Font("Arial", 9, FontStyle.Regular)
            };

            Label lblStatus = new Label()
            {
                Text = "Статус:",
                Location = new Point(30, 145),
                Size = new Size(80, 25),
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbAppointmentStatus = new ComboBox()
            {
                Location = new Point(120, 145),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 9, FontStyle.Regular)
            };
            this.cmbAppointmentStatus.Items.AddRange(new string[] { "Запланирован", "Выполнен", "Отменен" });

            // Кнопки управления записями
            this.btnAddAppointment = new Button()
            {
                Text = "Добавить запись",
                Location = new Point(500, 40),
                Size = new Size(150, 35),
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnAddAppointment.Click += BtnAddAppointment_Click;

            this.btnUpdateAppointment = new Button()
            {
                Text = "Обновить запись",
                Location = new Point(500, 85),
                Size = new Size(150, 35),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnUpdateAppointment.Click += BtnUpdateAppointment_Click;

            this.btnDeleteAppointment = new Button()
            {
                Text = "Удалить запись",
                Location = new Point(500, 130),
                Size = new Size(150, 35),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnDeleteAppointment.Click += BtnDeleteAppointment_Click;

            // Кнопка "Выполнено"
            this.btnCompleteAppointment = new Button()
            {
                Text = "Отметить выполненным",
                Location = new Point(670, 85),
                Size = new Size(150, 35),
                BackColor = Color.Green,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.btnCompleteAppointment.Click += BtnCompleteAppointment_Click;

            // Добавление элементов в группу
            managementGroup.Controls.AddRange(new Control[] {
                lblClient, this.cmbAppointmentClient, btnSearchClient,
                lblService, this.cmbAppointmentService, btnSearchService,
                lblDate, this.dateTimePickerAppointment,
                lblStatus, this.cmbAppointmentStatus,
                this.btnAddAppointment, this.btnUpdateAppointment, this.btnDeleteAppointment,
                this.btnCompleteAppointment
            });
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