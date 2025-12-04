namespace BeautySalonApp.Models
{
    public class TableColumn
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string ReferencedTable { get; set; }
        public string ReferencedColumn { get; set; }
        public string PkTargetTable { get; set; }
    }
}