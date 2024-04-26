using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers.Controllo_Campi
{
    public class Email : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Email = value?.ToString();

            if (Email == null || Email.Length < 5 || !Email.Contains("@"))
            {
                return new ValidationResult("L'E-Mail non valida");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}