using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers.Controllo_Campi
{
    public class Cognome : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Nominativo = value?.ToString();

            if (Nominativo == null || Nominativo.Length < 2 || !Regex.IsMatch(Nominativo, "^[a-zA-Z0-9]+$"))
            {
                return new ValidationResult("Il Cognome deve contenere almeno 5 caratteri e non deve contenere caratteri speciali");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}