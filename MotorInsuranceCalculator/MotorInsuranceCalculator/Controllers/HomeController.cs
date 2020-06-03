using MotorInsuranceCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotorInsuranceCalculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new PolicyDetailsViewModel();        
            model.Occupations = GetOccupations();
            return View(model);
        }

        [HttpPost]
        public ActionResult CalculatePremium(PolicyDetailsViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Occupations = GetOccupations();
                return View("Index", model);
            }

            var driverAge = GetDriverAgeAtPolicyStartDate((DateTime)model.DriverDateOfBirth, (DateTime)model.PolicyStartDate);
            var policyDeclined = CheckIfPolicyDeclined((DateTime)model.PolicyStartDate, driverAge);
            if (!string.IsNullOrEmpty(policyDeclined))
            {
                var policyDeclinedModel = new PolicyDeclinedViewModel
                {
                    PolicyDeclinedReason = policyDeclined
                };

                return View("PolicyDeclined", policyDeclinedModel);
            }

            var premiumCalculatedViewModel = new PremiumCalculatedViewModel();
            premiumCalculatedViewModel.Premium = CalculatePremium(model.DriverOccupationID, driverAge);

            return View(premiumCalculatedViewModel);
        }

        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<SelectListItem> GetOccupations()
        {
            var occupationsToReturn = new List<SelectListItem>();
            var accountantItem = new SelectListItem
            {
                Value = DriverOccupationEnum.Accountant.ToString("D"),
                Text = "Accountant"
            };
            occupationsToReturn.Add(accountantItem);

            var chauffeurItem = new SelectListItem
            {
                Value = DriverOccupationEnum.Chauffeur.ToString("D"),
                Text = "Chauffeur"
            };
            occupationsToReturn.Add(chauffeurItem);

            return occupationsToReturn;
        }

        private string CheckIfPolicyDeclined(DateTime policyStartDate, int driverAge)
        {
            if(policyStartDate < DateTime.Now)
            {
                return "Start Date of Policy cannot be in the past";
            }else if(driverAge < 17)
            {
                return "Driver is too young";
            }else if(driverAge > 75)
            {
                return "Driver is too old";
            }else
            {
                return string.Empty;
            }
        }

        private int GetDriverAgeAtPolicyStartDate(DateTime driverDateOfBirth, DateTime policyStartDate)
        {
            int age = policyStartDate.Year - driverDateOfBirth.Year;
            if (policyStartDate < driverDateOfBirth.AddYears(age)) age--;

            return age;
        }

        private int CalculatePremium(int driverOccupationID, int driverAge)
        {
            int premium = 500;

            if((DriverOccupationEnum)driverOccupationID == DriverOccupationEnum.Chauffeur)
            {
                premium += (int)(premium * 0.1);
            }else if((DriverOccupationEnum)driverOccupationID == DriverOccupationEnum.Accountant) 
            {
                premium -= (int)(premium * (0.1));
            }

            if(17 <= driverAge && driverAge <= 25)
            {
                premium += (int)(premium * 0.2);
            }else if(26 <= driverAge && driverAge <= 75)
            {
                premium -= (int)(premium * 0.1);
            }

            return premium;
        }
        
    }
    public enum DriverOccupationEnum
    {
        Accountant = 1,
        Chauffeur = 2,
    }

}