using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleSearch.Models
{
    public class SearchModel
    {
        [Required]
        public string SearchKeyword { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string SearchUrl { get; set; }
    }
}
