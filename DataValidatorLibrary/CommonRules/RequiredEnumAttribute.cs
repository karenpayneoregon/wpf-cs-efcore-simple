using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataValidatorLibrary.CommonRules
{
    /// <summary>
    /// Validates a property of type Enum is assigned.
    /// For this to work number the elements from 1 to x,
    /// do not assign 0 to an element.
    /// </summary>
    public class RequiredEnumAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value); ;
        }
    }
}
