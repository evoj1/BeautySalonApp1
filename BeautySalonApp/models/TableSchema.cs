using System.Collections.Generic;

namespace BeautySalonApp.Models
{
    public class TableSchema
    {
        public string TableName { get; set; }
        public List<TableColumn> Columns { get; set; } = new List<TableColumn>();
    }
}