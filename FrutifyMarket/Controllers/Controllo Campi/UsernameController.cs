using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers.Controllo_Campi
{
    public class Username : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Username = value?.ToString();

            // Check if the username is already taken in the database
            if (IsUsernameTaken(Username))
            {
                return new ValidationResult("Lo username è già stato scelto");
            }

            if (Username == null || Username.Length < 5 || !Regex.IsMatch(Username, "^[a-zA-Z0-9]+$"))
            {
                return new ValidationResult("Lo username deve contenere almeno 5 caratteri e non deve contenere caratteri speciali");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        private bool IsUsernameTaken(string username)
        {
            using (var db = new ModelDBContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }
    }
}