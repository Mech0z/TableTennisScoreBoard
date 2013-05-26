using System.ComponentModel.DataAnnotations;

namespace TableTennis.ViewModels
{
    public class CreateDoubleViewModel : CreateMatchViewModelBase
    {
        [Required]
        public string Player3Username { get; set; }

        [Required]
        public string Player4Username { get; set; }
    }
}