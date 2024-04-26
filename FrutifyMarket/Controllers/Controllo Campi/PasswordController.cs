using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers.Controllo_Campi
{
    public class Password : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string password = value.ToString();
                if (password.Length < 8)
                {
                    return new ValidationResult("La password deve contenere almeno 8 caratteri");
                }
                else if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    return new ValidationResult("La password deve contenere almeno una lettera minuscola");
                }
                else if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    return new ValidationResult("La password deve contenere almeno una lettera maiuscola");
                }
                else if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    return new ValidationResult("La password deve contenere almeno un numero");
                }
                else if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                {
                    return new ValidationResult("La password deve contenere almeno un carattere speciale");
                }
            }
            return ValidationResult.Success;
        }
    }
}