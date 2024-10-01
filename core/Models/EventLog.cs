using System.ComponentModel.DataAnnotations;

namespace core.Models
{
    public class EventLog
    {
        [Key]
        [Required(ErrorMessage = "Please enter event log guid")]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Please enter the event log date")]
        public DateTime EventDate { get; set; } = DateTime.Now;


        [Display(Name = "event log description")]
        public string? Description { get; set; }
    }
}
