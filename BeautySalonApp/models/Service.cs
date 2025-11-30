namespace BeautySalonApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}