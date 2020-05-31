using System;
using System.ComponentModel.DataAnnotations;

namespace DataValidatorLibrary.CommonRules
{
    public class MyDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validate date is greater than today. Of course this can change to
        /// match your date rule.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            var date = Convert.ToDateTime(value);
            return date >= DateTime.Now;
        }
    }
}
