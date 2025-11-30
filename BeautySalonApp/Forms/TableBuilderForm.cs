using BeautySalonApp.Database;
using BeautySalonApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BeautySalonApp.Forms
{
    public partial class TableBuilderForm : Form
    {
        private DatabaseHelper db = new DatabaseHelper();
        private List<TableColumn> columns = new List<TableColumn>();
        private DataTable existingTables;

        // Элементы управления
        private DataGridView dgvExistingTables;
        private ListBox lstColumns;
        private ListBox lstStructure;
        private ComboBox cmbDataType;
        private ComboBox cmbRefTable;
        private ComboBox cmbRefColumn;
        private TextBox txtTableName;
        private TextBox txtColumnName;
        private CheckBox chkPrimaryKey;
        private CheckBox chkForeignKey;
        private Button btnAddColumn;
        private Button btnRemoveColumn;
        private Button btnCreateTable;
        private Button btnViewStructure;

        public TableBuilderForm()
        {
            InitializeControls();
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(878, 594);
            this.Name = "TableBuilderForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Конструктор таблиц";
            this.Load += new System.EventHandler(this.TableBuilderForm_Load_2);
            this.ResumeLayout(false);
            LoadExistingTables();
            SetupDataTypes();
        }

        private void InitializeComponent()
            
        {
            this.SuspendLayout();
            // 
            // TableBuilderForm
            // 
            this.ClientSize = new System.Drawing.Size(493, 325);
            this.Name = "TableBuilderForm";
            this.ResumeLayout(false);

        }

        private void InitializeControls()
        {
            // DataGridView для существующих таблиц
            dgvExistingTables = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(400, 150),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvExistingTables);

            // ListBox для структуры таблицы
            lstStructure = new ListBox
            {
                Location = new Point(20, 190),
                Size = new Size(400, 150)
            };
            this.Controls.Add(lstStructure);

            // Кнопка просмотра структуры
            btnViewStructure = new Button
            {
                Text = "Просмотреть структуру",
                Location = new Point(20, 350),
                Size = new Size(150, 30),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White
            };
            btnViewStructure.Click += (s, e) => ViewTableStructure();
            this.Controls.Add(btnViewStructure);

            // Кнопка удаления таблицы
            Button btnDeleteTable = new Button
            {
                Text = "Удалить таблицу",
                Location = new Point(200, 350),
                Size = new Size(150, 30),
                BackColor = Color.Crimson,
                ForeColor = Color.White
            };
            btnDeleteTable.Click += BtnDeleteTable_Click;
            this.Controls.Add(btnDeleteTable);


            // Поля для создания таблицы
            Label lblTableName = new Label
            {
                Text = "Имя таблицы:",
                Location = new Point(450, 20),
                Size = new Size(100, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblTableName);

            txtTableName = new TextBox
            {
                Location = new Point(550, 20),
                Size = new Size(200, 25),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(txtTableName);

            // Поля для добавления колонок
            Label lblColumnName = new Label
            {
                Text = "Имя колонки:",
                Location = new Point(450, 60),
                Size = new Size(100, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblColumnName);

            txtColumnName = new TextBox
            {
                Location = new Point(550, 60),
                Size = new Size(200, 25),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(txtColumnName);

            Label lblDataType = new Label
            {
                Text = "Тип данных:",
                Location = new Point(450, 100),
                Size = new Size(100, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblDataType);

            cmbDataType = new ComboBox
            {
                Location = new Point(550, 100),
                Size = new Size(200, 25),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(cmbDataType);

            chkPrimaryKey = new CheckBox
            {
                Text = "Primary Key",
                Location = new Point(450, 140),
                Size = new Size(100, 20),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(chkPrimaryKey);

            chkForeignKey = new CheckBox
            {
                Text = "Foreign Key",
                Location = new Point(550, 140),
                Size = new Size(100, 20),
                Font = new Font("Arial", 9)
            };
            chkForeignKey.CheckedChanged += ChkForeignKey_CheckedChanged;
            this.Controls.Add(chkForeignKey);

            Label lblRefTable = new Label
            {
                Text = "Ссылочная таблица:",
                Location = new Point(450, 180),
                Size = new Size(120, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblRefTable);

            cmbRefTable = new ComboBox
            {
                Location = new Point(580, 180),
                Size = new Size(170, 25),
                Font = new Font("Arial", 9),
                Enabled = false
            };
            cmbRefTable.SelectedIndexChanged += CmbRefTable_SelectedIndexChanged;
            this.Controls.Add(cmbRefTable);

            Label lblRefColumn = new Label
            {
                Text = "Ссылочная колонка:",
                Location = new Point(450, 220),
                Size = new Size(120, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblRefColumn);

            cmbRefColumn = new ComboBox
            {
                Location = new Point(580, 220),
                Size = new Size(170, 25),
                Font = new Font("Arial", 9),
                Enabled = false
            };
            this.Controls.Add(cmbRefColumn);

            // Кнопки для управления колонками
            btnAddColumn = new Button
            {
                Text = "Добавить колонку",
                Location = new Point(450, 260),
                Size = new Size(120, 35),
                BackColor = Color.Green,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnAddColumn.Click += BtnAddColumn_Click;
            this.Controls.Add(btnAddColumn);

            btnRemoveColumn = new Button
            {
                Text = "Удалить колонку",
                Location = new Point(580, 260),
                Size = new Size(120, 35),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnRemoveColumn.Click += BtnRemoveColumn_Click;
            this.Controls.Add(btnRemoveColumn);

            // ListBox для добавленных колонок
            lstColumns = new ListBox
            {
                Location = new Point(450, 310),
                Size = new Size(300, 150),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(lstColumns);

            // Кнопка создания таблицы
            btnCreateTable = new Button
            {
                Text = "Создать таблицу",
                Location = new Point(450, 480),
                Size = new Size(150, 40),
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnCreateTable.Click += BtnCreateTable_Click;
            this.Controls.Add(btnCreateTable);
        }

        private void ChkForeignKey_CheckedChanged(object sender, EventArgs e)
        {
            cmbRefTable.Enabled = chkForeignKey.Checked;
            cmbRefColumn.Enabled = chkForeignKey.Checked;

            if (!chkForeignKey.Checked)
            {
                cmbRefTable.SelectedIndex = -1;
                cmbRefColumn.SelectedIndex = -1;
            }
        }

        private void CmbRefTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRefTable.SelectedItem != null)
            {
                LoadReferenceColumns(cmbRefTable.SelectedItem.ToString());
            }
        }

        private void BtnAddColumn_Click(object sender, EventArgs e)
        {
            AddColumn();
        }

        private void BtnRemoveColumn_Click(object sender, EventArgs e)
        {
            RemoveSelectedColumn();
        }

        private void BtnCreateTable_Click(object sender, EventArgs e)
        {
            CreateTable();
        }

        private void SetupDataTypes()
        {
            // Типы данных для MS Access
            cmbDataType.Items.AddRange(new string[]
            {
                "INTEGER",
                "TEXT(255)",
                "MEMO",
                "CURRENCY",
                "DATETIME",
                "DATE",
                "BOOLEAN",
                "SINGLE",
                "DOUBLE",
                "LONG"
            });
            cmbDataType.SelectedIndex = 0;
        }

        private void LoadExistingTables()
        {
            try
            {
                // Получаем список таблиц из MS Access
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");

                    existingTables = new DataTable();
                    existingTables.Columns.Add("TableName");

                    foreach (DataRow row in schema.Rows)
                    {
                        string tableType = row["TABLE_TYPE"].ToString();
                        if (tableType == "TABLE")
                        {
                            string tableName = row["TABLE_NAME"].ToString();
                            if (!tableName.StartsWith("MSys")) // Исключаем системные таблицы
                            {
                                existingTables.Rows.Add(tableName);
                            }
                        }
                    }

                    dgvExistingTables.DataSource = existingTables;

                    // Обновляем комбобокс для Foreign Key
                    cmbRefTable.Items.Clear();
                    foreach (DataRow row in existingTables.Rows)
                    {
                        cmbRefTable.Items.Add(row["TableName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблиц: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReferenceColumns(string tableName)
        {
            cmbRefColumn.Items.Clear();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Columns", new string[] { null, null, tableName, null });

                    foreach (DataRow row in schema.Rows)
                    {
                        cmbRefColumn.Items.Add(row["COLUMN_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки колонок таблицы {tableName}: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddColumn()
        {
            if (string.IsNullOrWhiteSpace(txtColumnName.Text))
            {
                MessageBox.Show("Введите имя колонки!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbDataType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип данных!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TableColumn column = new TableColumn
            {
                Name = txtColumnName.Text.Trim(),
                DataType = cmbDataType.SelectedItem.ToString(),
                IsPrimaryKey = chkPrimaryKey.Checked,
                IsForeignKey = chkForeignKey.Checked
            };

            if (chkForeignKey.Checked)
            {
                if (cmbRefTable.SelectedItem == null)
                {
                    MessageBox.Show("Выберите таблицу для связи!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmbRefColumn.SelectedItem == null)
                {
                    MessageBox.Show("Выберите колонку для связи!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                column.ReferencedTable = cmbRefTable.SelectedItem.ToString();
                column.ReferencedColumn = cmbRefColumn.SelectedItem.ToString();
            }

            columns.Add(column);
            UpdateColumnsList();

            // Очищаем поля
            txtColumnName.Clear();
            chkPrimaryKey.Checked = false;
            chkForeignKey.Checked = false;
            cmbRefTable.SelectedIndex = -1;
            cmbRefColumn.SelectedIndex = -1;
            cmbRefTable.Enabled = false;
            cmbRefColumn.Enabled = false;
        }

        private void RemoveSelectedColumn()
        {
            if (lstColumns.SelectedIndex >= 0 && lstColumns.SelectedIndex < columns.Count)
            {
                columns.RemoveAt(lstColumns.SelectedIndex);
                UpdateColumnsList();
            }
            else
            {
                MessageBox.Show("Выберите колонку для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateColumnsList()
        {
            lstColumns.Items.Clear();
            foreach (var column in columns)
            {
                string pk = column.IsPrimaryKey ? " 🔑" : "";
                string fk = column.IsForeignKey ? $" 🔗→ {column.ReferencedTable}.{column.ReferencedColumn}" : "";
                lstColumns.Items.Add($"{column.Name} ({column.DataType}){pk}{fk}");
            }
        }

        private void CreateTable()
        {
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show("Введите имя таблицы!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (columns.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну колонку!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string newTableName = txtTableName.Text.Trim();

                if (TableExists(newTableName))
                {
                    MessageBox.Show($"Таблица с именем '{newTableName}' уже существует!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 1. Создаём новую таблицу (как раньше)
                string createQuery = GenerateCreateTableQuery(newTableName);
                bool tableCreated = CreateTableDirect(createQuery);

                if (!tableCreated && !TableExists(newTableName))
                {
                    MessageBox.Show("Ошибка при создании таблицы!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Для всех колонок с IsForeignKey создаём FK в выбранной таблице
                bool fkSuccess = true;
                string fkErrors = "";

                foreach (var col in columns.Where(c => c.IsForeignKey))
                {
                    try
                    {
                        // col.Name      – имя новой колонки (например, LocationID)
                        // col.ReferencedTable   – таблица, в которой будет эта колонка (например, Services)
                        // col.ReferencedColumn  – столбец в НОВОЙ таблице, на который ссылаемся (обычно ID)

                        CreateForeignKeyInExistingTable(
                            ownerTable: col.ReferencedTable,
                            fkColumnName: col.Name,
                            referencedTable: newTableName,
                            referencedColumn: col.ReferencedColumn);
                    }
                    catch (Exception ex2)
                    {
                        fkSuccess = false;
                        fkErrors += $"Ошибка связи {col.ReferencedTable}.{col.Name} → {newTableName}.{col.ReferencedColumn}: {ex2.Message}\n";
                    }
                }

                LoadExistingTables();

                string message = $"Таблица '{newTableName}' успешно создана!";
                if (!fkSuccess)
                {
                    message += $"\n\nНо возникли проблемы со связями:\n{fkErrors}";
                    MessageBox.Show(message, "Таблица создана с предупреждениями",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(message, "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                columns.Clear();
                lstColumns.Items.Clear();
                txtTableName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания таблицы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateForeignKeyInExistingTable(
        string ownerTable,        // в какой таблице создать колонку
        string fkColumnName,      // имя новой колонки (например, LocationID)
        string referencedTable,   // на какую таблицу ссылаемся
        string referencedColumn)  // на какой столбец там
        {
            using (var connection = db.GetConnection())
            {
                connection.Open();

                // 1. Проверяем, есть ли уже такая колонка
                bool hasColumn = false;
                DataTable cols = connection.GetSchema("Columns", new string[] { null, null, ownerTable, null });
                foreach (DataRow r in cols.Rows)
                {
                    if (string.Equals(r["COLUMN_NAME"].ToString(), fkColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        hasColumn = true;
                        break;
                    }
                }

                // 2. Если нет – создаём (LONG под ID)
                if (!hasColumn)
                {
                    using (var cmd = new OleDbCommand(
                        $"ALTER TABLE [{ownerTable}] ADD COLUMN [{fkColumnName}] LONG",
                        connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                //3.Добавляем внешний ключ
                string constraintName = $"FK_{ownerTable}_{fkColumnName}";

                using (var cmd = new OleDbCommand(
                    $"ALTER TABLE [{ownerTable}] " +
                    $"ADD CONSTRAINT [{constraintName}] " +
                    $"FOREIGN KEY ([{fkColumnName}]) " +
                    $"REFERENCES [{referencedTable}]([{referencedColumn}])",
                    connection))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmdRel = new OleDbCommand(
    "INSERT INTO AppRelations (ParentTable, ParentColumn, ChildTable, ChildColumn, ConstraintName) " +
    "VALUES (@pTab, @pCol, @cTab, @cCol, @name)", connection))
                {
                    cmdRel.Parameters.AddWithValue("@pTab", referencedTable);   // Locations
                    cmdRel.Parameters.AddWithValue("@pCol", referencedColumn);  // ID
                    cmdRel.Parameters.AddWithValue("@cTab", ownerTable);        // Services
                    cmdRel.Parameters.AddWithValue("@cCol", fkColumnName);      // LocationID
                    cmdRel.Parameters.AddWithValue("@name", constraintName);    // FK_Services_LocationID
                    cmdRel.ExecuteNonQuery();
                }
            }
        }

        private bool CreateTableDirect(string createQuery)
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var command = new OleDbCommand(createQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но не прерываем выполнение
                System.Diagnostics.Debug.WriteLine($"Create table error: {ex.Message}");
                return false;
            }
        }
        private bool TableExists(string tableName)
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");
                    foreach (DataRow row in schema.Rows)
                    {
                        if (row["TABLE_NAME"].ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                // Игнорируем ошибки при проверке
            }
            return false;
        }

        private string GenerateCreateTableQuery(string tableName)
        {
            List<string> columnDefinitions = new List<string>();
            var primaryKeyColumns = columns.Where(c => c.IsPrimaryKey).ToList();

            foreach (var column in columns)
            {
                string definition = $"[{column.Name}] {column.DataType}";

                // Для MS Access лучше не добавлять PRIMARY KEY в определении колонки
                // Вместо этого создадим отдельное ограничение
                columnDefinitions.Add(definition);
            }

            // Добавляем PRIMARY KEY constraint отдельно, если есть первичные ключи
            if (primaryKeyColumns.Count > 0)
            {
                string pkColumns = string.Join(", ", primaryKeyColumns.Select(c => $"[{c.Name}]"));
                columnDefinitions.Add($"PRIMARY KEY ({pkColumns})");
            }

            return $"CREATE TABLE [{tableName}] ({string.Join(", ", columnDefinitions)})";
        }


        private bool AddForeignKeyConstraints(string tableName, List<TableColumn> foreignKeyColumns, out string errorMessages)
        {
            errorMessages = "";
            bool overallSuccess = true;

            foreach (var column in foreignKeyColumns)
            {
                try
                {
                    // Создаем уникальное имя для ограничения
                    string constraintName = $"FK_{tableName}_{column.Name}_{DateTime.Now:HHmmss}";

                    string fkQuery = $@"ALTER TABLE [{tableName}] 
                              ADD CONSTRAINT [{constraintName}] 
                              FOREIGN KEY ([{column.Name}]) 
                              REFERENCES [{column.ReferencedTable}]([{column.ReferencedColumn}])";

                    using (var connection = db.GetConnection())
                    {
                        connection.Open();
                        using (var command = new OleDbCommand(fkQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    overallSuccess = false;
                    errorMessages += $"Ошибка создания связи для {column.Name}: {ex.Message}\n";
                    // Логируем ошибку для отладки
                    System.Diagnostics.Debug.WriteLine($"FK error for {column.Name}: {ex.Message}");
                }
            }

            return overallSuccess;
        }
        private bool ForeignKeyExists(string tableName, string columnName)
        {
            try
            {
                // Эта проверка сложна для MS Access, поэтому просто возвращаем false
                // В реальном приложении можно реализовать проверку через schema
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void ViewTableStructure()
        {
            if (dgvExistingTables.CurrentRow == null)
            {
                MessageBox.Show("Выберите таблицу для просмотра!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = dgvExistingTables.CurrentRow.Cells[0].Value.ToString();

            try
            {
                // Получаем структуру таблицы для MS Access
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Columns", new string[] { null, null, tableName, null });

                    lstStructure.Items.Clear();
                    lstStructure.Items.Add($"Структура таблицы: {tableName}");
                    lstStructure.Items.Add("");

                    foreach (DataRow row in schema.Rows)
                    {
                        string columnName = row["COLUMN_NAME"].ToString();
                        string dataType = row["DATA_TYPE"].ToString();
                        string isNullable = row["IS_NULLABLE"].ToString();

                        lstStructure.Items.Add($"{columnName} ({dataType}) {(isNullable == "YES" ? "NULL" : "NOT NULL")}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения структуры: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            if (dgvExistingTables.CurrentRow == null)
            {
                MessageBox.Show("Выберите таблицу для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = dgvExistingTables.CurrentRow.Cells[0].Value.ToString();

            var result = MessageBox.Show(
                $"Точно удалить таблицу '{tableName}' и все связи, в которых она участвует?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();

                    // 1. Находим все связи из нашей служебной таблицы AppRelations
                    var relations = new List<(string ChildTable, string ConstraintName)>();

                    using (var cmd = new OleDbCommand(
                        "SELECT ChildTable, ConstraintName " +
                        "FROM AppRelations " +
                        "WHERE ParentTable = @t OR ChildTable = @t", connection))
                    {
                        cmd.Parameters.AddWithValue("@t", tableName);

                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                string childTable = r["ChildTable"].ToString();
                                string constraintName = r["ConstraintName"].ToString();
                                relations.Add((childTable, constraintName));
                            }
                        }
                    }

                    // 2. Снимаем все найденные внешние ключи
                    foreach (var rel in relations)
                    {
                        string sqlDropFk =
                            $"ALTER TABLE [{rel.ChildTable}] DROP CONSTRAINT [{rel.ConstraintName}]";

                        try
                        {
                            using (var cmdDropFk = new OleDbCommand(sqlDropFk, connection))
                            {
                                cmdDropFk.ExecuteNonQuery();
                            }
                        }
                        catch
                        {
                            // если не получилось снять constraint – продолжаем остальные
                        }
                    }

                    // 3. Удаляем записи о связях для этой таблицы из AppRelations
                    using (var cmdDel = new OleDbCommand(
                        "DELETE FROM AppRelations WHERE ParentTable = @t OR ChildTable = @t",
                        connection))
                    {
                        cmdDel.Parameters.AddWithValue("@t", tableName);
                        cmdDel.ExecuteNonQuery();
                    }

                    // 4. Удаляем саму таблицу
                    using (var cmdDrop = new OleDbCommand(
                        $"DROP TABLE [{tableName}]", connection))
                    {
                        cmdDrop.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Таблица '{tableName}' и все связанные с ней ограничения удалены.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadExistingTables();
                lstStructure.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось удалить таблицу: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void TableBuilderForm_Load(object sender, EventArgs e)
        {
            // Дополнительная инициализация при необходимости
        }

        private void TableBuilderForm_Load_1(object sender, EventArgs e)
        {
            // Удалите этот метод, так как он дублирует TableBuilderForm_Load
        }

        private void TableBuilderForm_Load_2(object sender, EventArgs e)
        {

        }
    }
}