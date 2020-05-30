using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataValidatorLibrary.LanguageExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Given a string with upper and lower cased letters separate them before each upper cased characters
        /// </summary>
        /// <param name="sender">String to work against</param>
        /// <returns>String with spaces between upper-case letters</returns>
        public static string SplitCamelCase(this string sender)
        {
            return Regex.Replace(Regex.Replace(sender, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }
    }
}
