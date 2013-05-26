using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TableTennis.ViewModels
{
    public class CreateMatchViewModelBase
    {
        public CreateMatchViewModelBase()
        {
            Score1Set1 = 0;
            Score1Set2 = 0;
            Score1Set3 = 0;
            Score2Set1 = 0;
            Score2Set2 = 0;
            Score2Set3 = 0;
        }

        public IEnumerable<SelectListItem> PlayerList { get; set; }
        public IEnumerable<SelectListItem> Winner { get; set; }
        public IEnumerable<SelectListItem> GameTypes { get; set; }
        
        [Required]
        public string Player1Username { get; set; }

        [Required]
        public string Player2Username { get; set; }
        
        [Required]
        [DisplayName("Game mode")]
        public string GameType { get; set; }

        [Required]
        [RegularExpression("[0-9][0-9]?")]
        public int Score1Set1 { get; set; }

        [RegularExpression("[0-9][0-9]?")]
        [Required]
        public int Score1Set2 { get; set; }

        [RegularExpression("[0-9][0-9]?")]
        [Required]
        public int Score1Set3 { get; set; }

        [RegularExpression("[0-9][0-9]?")]
        [Required]
        public int Score2Set1 { get; set; }

        [RegularExpression("[0-9][0-9]?")]
        [Required]
        public int Score2Set2 { get; set; }

        [RegularExpression("[0-9][0-9]?")]
        [Required]
        public int Score2Set3 { get; set; }
    }
}