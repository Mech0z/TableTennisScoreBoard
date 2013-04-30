using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TableTennis.ViewModels
{
    public class CreateMatchViewModel
    {
        public IEnumerable<SelectListItem> PlayerList { get; set; }
        public IEnumerable<SelectListItem> Winner { get; set; }
        
        [Required]
        public Guid Player1ID { get; set; }

        [Required]
        public Guid Player2ID { get; set; }

        [Required]
        public int WinnerID { get; set; }
    }
}