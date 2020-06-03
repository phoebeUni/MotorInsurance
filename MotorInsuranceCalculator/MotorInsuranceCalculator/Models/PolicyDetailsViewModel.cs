using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotorInsuranceCalculator.Models
{
    public class PolicyDetailsViewModel
    {
        [Required(ErrorMessage ="This field is required")]
        [DisplayName("Policy Start Date")]
        public DateTime? PolicyStartDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Driver Name")]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Driver Occupation")]
        public int DriverOccupationID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Driver Date Of Birth")]
        public DateTime? DriverDateOfBirth { get; set; }

        public List<SelectListItem> Occupations { get; set; }
    }
}