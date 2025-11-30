using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace BeautySalonApp.Database
{
    public class DatabaseHelper
    {
        private string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BeautySalon.mdb;";

        public OleDbConnection GetConnection()
        {
            return new OleDbConnection(connectionString);
        }

        public bool ExecuteNonQuery(string query, OleDbParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OleDbCommand(query, connection))
                    {
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
                    return false;
                }
            }
        }

        public DataTable ExecuteQuery(string query, OleDbParameter[] parameters = null)
        {
            var dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new OleDbCommand(query, connection))
                    {
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        using (var adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
                }
            }
            return dataTable;
        }
    }
}