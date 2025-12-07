using System.Drawing;
using System.Windows.Forms;

namespace BeautySalonApp.Forms
{
    partial class UserForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private DataGridView dataGridViewServices;
        private TextBox txtFilterCategory;
        private TextBox txtFilterMaxPrice;
        private Button btnFilter;
        private TextBox txtClientPhone;
        private TextBox txtClientFirstName;
        private TextBox txtClientLastName;
        private DateTimePicker dateTimePicker;
        private Button btnCreateOrder;
        private DataGridView dataGridViewClientAppointments;
        private Label lblSelectedService;
        private TextBox txtSearchPhone;
        private Button btnSearchClient;
        private TextBox txtFilterMinPrice;
        private TabPage tabServices;
        private TabPage tabClientSearch;

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
            this.tabClientSearch = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabServices);
            this.tabControl.Controls.Add(this.tabClientSearch);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 550);
            this.tabControl.TabIndex = 0;
            // 
            // tabServices
            // 
            this.tabServices.Location = new System.Drawing.Point(4, 29);
            this.tabServices.Name = "tabServices";
            this.tabServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabServices.Size = new System.Drawing.Size(792, 517);
            this.tabServices.TabIndex = 0;
            this.tabServices.Text = "Услуги и запись";
            this.tabServices.UseVisualStyleBackColor = true;
            this.tabServices.Click += new System.EventHandler(this.tabServices_Click_1);
            // 
            // tabClientSearch
            // 
            //this.tabClientSearch.Location = new System.Drawing.Point(4, 29);
            //this.tabClientSearch.Name = "tabClientSearch";
            //this.tabClientSearch.Padding = new System.Windows.Forms.Padding(3);
            //this.tabClientSearch.Size = new System.Drawing.Size(792, 517);
            //this.tabClientSearch.TabIndex = 1;
            //this.tabClientSearch.Text = "Поиск клиента";
            //this.tabClientSearch.UseVisualStyleBackColor = true;
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.tabControl);
            this.Name = "UserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Клиентская панель - Салон Красоты";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void InitializeServicesTab(TabPage tab)
        {
            // Фильтры
            GroupBox filterGroup = new GroupBox();
            filterGroup.Text = "Фильтр услуг";
            filterGroup.Location = new Point(20, 20);
            filterGroup.Size = new Size(750, 80);

            Label lblCategory = new Label() { Text = "Катег.:", Location = new Point(20, 25), Size = new Size(70, 20) };

            // ✅ ИСПРАВЛЕНО: создаем txtFilterCategory только один раз
            txtFilterCategory = new TextBox()
            {
                Location = new Point(90, 25),
                Size = new Size(100, 20),
                MaxLength = 50
            };
            // ✅ Добавляем обработчик события KeyPress
            txtFilterCategory.KeyPress += TxtFilterCategory_KeyPress;

            Label lblMinPrice = new Label() { Text = "Мин. цена:", Location = new Point(200, 25), Size = new Size(70, 20) };
            txtFilterMinPrice = new TextBox()
            {
                Location = new Point(270, 25),
                Size = new Size(60, 20),
                Text = "0"
            };
            txtFilterMinPrice.KeyPress += TxtPrice_KeyPress;

            Label lblMaxPrice = new Label() { Text = "Макс. цена:", Location = new Point(340, 25), Size = new Size(110, 20) };
            txtFilterMaxPrice = new TextBox()
            {
                Location = new Point(460, 25),
                Size = new Size(60, 20)
            };
            txtFilterMaxPrice.KeyPress += TxtPrice_KeyPress;

            btnFilter = new Button()
            {
                Text = "Применить фильтр",
                Location = new Point(530, 25),
                Size = new Size(200, 40),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White
            };
            btnFilter.Click += BtnFilterServices_Click;

            filterGroup.Controls.AddRange(new Control[] {
                lblCategory, txtFilterCategory,  // ✅ Используем один экземпляр
                lblMinPrice, txtFilterMinPrice,
                lblMaxPrice, txtFilterMaxPrice,
                btnFilter
            });

            // DataGridView услуг
            dataGridViewServices = new DataGridView();
            dataGridViewServices.Location = new Point(20, 110);
            dataGridViewServices.Size = new Size(750, 150);
            dataGridViewServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewServices.SelectionChanged += DataGridViewServices_SelectionChanged;
            dataGridViewServices.ReadOnly = true; // ✅ Делаем доступным только для чтения

            // Метка выбранной услуги
            lblSelectedService = new Label();
            lblSelectedService.Text = "Выберите услугу из списка";
            lblSelectedService.Location = new Point(20, 270);
            lblSelectedService.Size = new Size(600, 20);
            lblSelectedService.Font = new Font("Arial", 9, FontStyle.Bold);
            lblSelectedService.ForeColor = Color.DarkSlateBlue;

            // Группа для создания заказа
            GroupBox orderGroup = new GroupBox();
            orderGroup.Text = "Запись на услугу";
            orderGroup.Location = new Point(20, 300);
            orderGroup.Size = new Size(750, 150);

            Label lblClientFirstName = new Label() { Text = "Имя:", Location = new Point(20, 25), Size = new Size(40, 20) };
            txtClientFirstName = new TextBox() { Location = new Point(60, 25), Size = new Size(100, 20) };

            Label lblClientLastName = new Label() { Text = "Фамилия:", Location = new Point(170, 25), Size = new Size(60, 20) };
            txtClientLastName = new TextBox() { Location = new Point(230, 25), Size = new Size(100, 20) };

            Label lblClientPhone = new Label() { Text = "Телефон:", Location = new Point(340, 25), Size = new Size(60, 20) };
            txtClientPhone = new TextBox() { Location = new Point(400, 25), Size = new Size(150, 20) };

            Label lblDate = new Label() { Text = "Дата и время:", Location = new Point(20, 55), Size = new Size(80, 20) };
            dateTimePicker = new DateTimePicker()
            {
                Location = new Point(100, 55),
                Size = new Size(150, 20),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm",
                ShowUpDown = true
            };

            btnCreateOrder = new Button()
            {
                Text = "Записаться",
                Location = new Point(270, 55),
                Size = new Size(100, 30),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnCreateOrder.Click += BtnCreateOrder_Click;

            orderGroup.Controls.AddRange(new Control[] {
                lblClientFirstName, txtClientFirstName, lblClientLastName, txtClientLastName,
                lblClientPhone, txtClientPhone, lblDate, dateTimePicker, btnCreateOrder
            });

            tab.Controls.Add(filterGroup);
            tab.Controls.Add(dataGridViewServices);
            tab.Controls.Add(lblSelectedService);
            tab.Controls.Add(orderGroup);
        }

        private void InitializeClientSearchTab(TabPage tab)
        {
            // Поиск клиента
            GroupBox searchGroup = new GroupBox();
            searchGroup.Text = "Поиск клиента по телефону";
            searchGroup.Location = new Point(20, 20);
            searchGroup.Size = new Size(750, 80);

            Label lblSearchPhone = new Label() { Text = "Телефон:", Location = new Point(20, 30), Size = new Size(60, 20) };
            txtSearchPhone = new TextBox() { Location = new Point(80, 30), Size = new Size(150, 20) };

            btnSearchClient = new Button()
            {
                Text = "Найти",
                Location = new Point(240, 30),
                Size = new Size(80, 25),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White
            };
            btnSearchClient.Click += BtnSearchClient_Click;

            searchGroup.Controls.AddRange(new Control[] { lblSearchPhone, txtSearchPhone, btnSearchClient });

            // История записей клиента
            GroupBox historyGroup = new GroupBox();
            historyGroup.Text = "История записей клиента";
            historyGroup.Location = new Point(20, 120);
            historyGroup.Size = new Size(750, 350);

            dataGridViewClientAppointments = new DataGridView();
            dataGridViewClientAppointments.Location = new Point(20, 25);
            dataGridViewClientAppointments.Size = new Size(710, 310);
            dataGridViewClientAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewClientAppointments.ReadOnly = true; // ✅ Делаем доступным только для чтения

            historyGroup.Controls.Add(dataGridViewClientAppointments);

            tab.Controls.Add(searchGroup);
            tab.Controls.Add(historyGroup);
        }
    }
}