using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataValidatorLibrary.Helpers;


namespace DataValidatorLibrary.LanguageExtensions
{
    public static class ValidatorExtensions
    {
        public static string ErrorMessageList(this EntityValidationResult sender)
        {
            string RemoveSpaces(string item)
            {
                return Regex.Replace(item, @"\s+", " ");
            }

            var sb = new StringBuilder();
            sb.AppendLine("Validation issues");

            foreach (var validationResult in sender.Errors)
            {
                sb.AppendLine(RemoveSpaces(validationResult.ErrorMessage.SplitCamelCase()));
            }

            return sb.ToString();
        }

    }
}
