using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Product
{
    public class ProductParams
    {
        // filter
        public string? Search { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        // sort
        public string SortColumn { get; set; } = "Name";
        public string SortDirection { get; set; } = "asc";
        //page
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
