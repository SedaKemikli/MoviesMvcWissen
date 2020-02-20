using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace _036_MoviesMvcWissen.Models
{
    public class MovieReportsModel
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [DisplayName("Movie Name")]
        public string MovieName { get; set; }
        [DisplayName("Movie ProductionYear")]
        public string MovieProductionYear { get; set; }
        public double? _MovieBoxOfficeReturn { get; set; }
        [DisplayName("Movie BoxOfficeReturn")]
        public string MovieBoxOfficeReturn { get; set; }
        [DisplayName("Director FullName")]
        public string DirectorFullName { get; set; }
        public bool _DirectorRetired { get; set; }
        [DisplayName("Director Retired")]
        public string DirectorRetired { get; set; }
        [DisplayName("Review Content")]
        public string ReviewContent { get; set; }
        [DisplayName("Review Rating")]
        public int? ReviewRating { get; set; }
        [DisplayName("Review Reviewer")]
        public string ReviewReviewer { get; set; }

    }
}