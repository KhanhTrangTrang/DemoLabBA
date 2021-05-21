using System.ComponentModel.DataAnnotations;

namespace LabDemoWebASPMVC.Models
{
    public class Employees
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,100, ErrorMessage ="Age is invalid")]
        public int Age { get; set; }
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        //[RegularExpression(@"([0-9]+)[-. ]([0-9]+)[-. ]([0-9]+)", ErrorMessage = "Not a valid phone number")]
    }
}
