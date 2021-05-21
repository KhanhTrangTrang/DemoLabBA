using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabDemoWebASPMVC.Models
{
    public class Admin
    {
        [Required(ErrorMessage = "Hãy nhập ID")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Hãy nhập password")]
        public string Password { get; set; }
    }
}
