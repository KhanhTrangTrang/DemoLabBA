using System.ComponentModel.DataAnnotations;

namespace LabDemoWebASPMVC.Models
{
    /// <summary>
    ///   Model dùng để quản lí thông tin nhân viên trong ứng dụng
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// khanhnn 5/24/2021 created
    /// </Modified>
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Hãy nhập tên")]
        [RegularExpression(@"^\w((?!(<\S)|(\S>)).)*$", ErrorMessage = "Tên không đúng format")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hãy nhập email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail không đúng theo format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hãy nhập số tel")]
        [RegularExpression(@"^(?=.{0,14}$)([0-9]+)[-. ]([0-9]+)[-. ]([0-9]+)", ErrorMessage = "Tel không đúng như format")]
        public string Tel { get; set; }
    }
}
