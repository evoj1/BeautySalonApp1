using System;

namespace BeautySalonApp.Models
{
    public class QuickNote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Color { get; set; }

        public QuickNote()
        {
            CreatedDate = DateTime.Now;
            Color = "Белая";
        }

        public override string ToString()
        {
            return $"{Title} ({CreatedDate:dd.MM.yyyy})";
        }
    }
}