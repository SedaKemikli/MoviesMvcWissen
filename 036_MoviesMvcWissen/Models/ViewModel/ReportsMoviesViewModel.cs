using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _036_MoviesMvcWissen.Models.ViewModel
{
    public class ReportsMoviesViewModel
    {
        public List<MovieReportsModel> MovieReports { get; set; }
        public int RecordCount { get; set; }
        public int RecordsPerPageCount { get; set; }
        public int PageNumber { get; set; } = 1 ;
        public SelectList PageNumbers { get; set; }
    }
}