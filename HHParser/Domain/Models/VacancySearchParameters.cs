using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHParser.Domain.Attributes;

namespace HHParser.Domain.Models
{
    public class VacancySearchParameters
    {
        [QueryParameter("text")]
        public string? Text { get; set; }

        [QueryParameter("per_page")]
        public int PerPage { get; set; } = 10;
    }
}
