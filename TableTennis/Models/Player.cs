using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TableTennis.Models
{
    [Bind(Exclude = "Id")]
    public class Player
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}