namespace BeautySalonApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
    }
}