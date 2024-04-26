using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers.Controllo_Campi
{
    public class Nome : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Nome = value?.ToString();

            if (Nome == null || Nome.Length < 5 || !Regex.IsMatch(Nome, "^[a-zA-Z0-9]+$"))
            {
                return new ValidationResult("Il Nome deve contenere almeno 5 caratteri e non deve contenere caratteri speciali");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}