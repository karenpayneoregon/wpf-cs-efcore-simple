using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataValidatorLibrary.CommonRules
{
    /// <summary>
    /// Provides custom rule for phone number
    /// </summary>
    public class CheckPhoneValidationAttribute : ValidationAttribute 
    {
        public override bool IsValid(object value)
        {
            /*
             * VS2017 or higher
             */
            bool IsDigitsOnly(string str)
            {
                return str.All(c => c >= '0' && c <= '9');
            }

            if (value == null)
            {
                return false;
            }

            var convertedValue = value.ToString();

            return !string.IsNullOrWhiteSpace(convertedValue) && 
                   IsDigitsOnly(convertedValue) && 
                   convertedValue.Length <= 10;
        }
    }
}