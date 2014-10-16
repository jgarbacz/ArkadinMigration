using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MVM
{
    class StringAndScalarFunctions
    {
        public static readonly string NEWLINE = "".AppendLine();

        #region perl like string functions

        /// <summary>
        /// Extracts a substring out of EXPR starting at the passed offset.  
        /// If OFFSET is negative starts that far back from the end of the string.
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <param name="offset">Zero based offset to start</param>
        /// <returns>A new string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("substr")]
        public static string substr(string expr, int offset)
        {
            return expr.Substr(offset);
        }
        /// <summary>
        /// Extracts a substring out of EXPR starting at the passed offset with a max LENGTH.  
        /// If OFFSET is negative starts that far back from the end of the string. If LENGTH is negative, leaves that many characters off the end of the string.
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <param name="offset">Zero based offset to start</param>
        /// <param name="length">Size of the substring to extract</param>
        /// <returns>A new string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("substr")]
        public static string substr(string expr, int offset, int length)
        {
            return expr.Substr(offset, length);
        }
        /// <summary>
        /// Replaces a substring with REPLACEMENT out of EXPR starting at the passed offset with a max LENGTH with  
        /// If OFFSET is negative starts that far back from the end of the string. If LENGTH is negative, leaves that many characters off the end of the string.
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <param name="offset">Zero based offset to start</param>
        /// <param name="length">Size of the substring to extract</param>
        /// <param name="replacement">Replacement string</param>
        /// <returns>A new string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("substr")]
        public static string substr(string expr, int offset, int length, string replacement)
        {
            return expr.Substr(offset, length, replacement);
        }

        /// <summary>
        /// Returns the string in upper case
        /// </summary>
        /// <param name="expr"></param>
        /// <returns>Upper case version of input</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("uc")]
        public static string uc(string expr)
        {
            return expr.ToUpper();
        }

        /// <summary>
        /// Returns the string in lower case
        /// </summary>
        /// <param name="expr"></param>
        /// <returns>Lower case version of input</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("lc")]
        public static string lc(string expr)
        {
            return expr.ToLower();
        }
        /// <summary>
        /// The index function searches for one string within another, but without the wildcard-like behavior of a full regular-expression pattern match. 
        /// It returns the position of the first occurrence of SUBSTR in STR. If the substring is not found return -1.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="substr">Substring to search for</param>
        /// <returns>Zero based index of the subsr or -1</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("index")]
        public static int index(string str, string substr)
        {
            return str.IndexOf(substr);
        }
        /// <summary>
        /// The index function searches for one string within another, but without the wildcard-like behavior of a full regular-expression pattern match. 
        /// It returns the position of the first occurrence of SUBSTR in STR at or after POSITION. If the substring is not found return -1.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="substr">Substring to search for</param>
        /// <param name="position">Position to start search</param>
        /// <returns>Zero based index of the subsr or -1</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("index")]
        public static int index(string str, string substr, int position)
        {
            return str.IndexOf(substr, position);
        }
        /// <summary>
        /// Returns number of characters in the string
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <returns>Number of characters in the input string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("length")]
        public static int length(string expr)
        {
            return expr.Length;
        }
        /// <summary>
        /// Returns the string in reverse order
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <returns>The string in reverse order</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("reverse")]
        public static string reverse(string expr)
        {
            return expr.ReverseString();
        }

        #endregion

        /// <summary>
        /// This returns a platform-dependent newline string
        /// </summary>
        /// <returns>Platform dependent newline</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("nl")]
        public static string NewLine()
        {
            return NEWLINE;
        }

        /// <summary>
        /// Returns true if the input string starts with the passed value
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="value">Prefix to look for</param>
        /// <returns>True if string starts with value</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("starts_with")]
        public static bool starts_with(string input, string value)
        {
            return input.StartsWith(value);
        }

        /// <summary>
        /// Returns true if the input string starts with the passed value
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="value">Prefix to look for</param>
        /// <param name="ignore_case">True to ignore case</param>
        /// <returns>True if string starts with value</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("starts_with")]
        public static bool starts_with(string input, string value, bool ignore_case)
        {
            if (ignore_case)
                 return input.StartsWith(value, StringComparison.OrdinalIgnoreCase);
            else
                return input.StartsWith(value);
        }

        /// <summary>
        /// Returns true if the input string ends with the passed value, case insensitive
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="value">Suffix to look for</param>
        /// <returns>True if string ends with value</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("ends_with")]
        public static bool ends_with(string input, string value)
        {
            return input.EndsWith(value);
        }
        /// <summary>
        /// Returns true if the input string starts with the passed value
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="value">Suffix to look for</param>
        /// <param name="ignore_case">True to ignore case</param>
        /// <returns>True if string ends with value</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("ends_with")]
        public static bool ends_with(string input, string value, bool ignore_case)
        {
            if (ignore_case)
                return input.EndsWith(value, StringComparison.OrdinalIgnoreCase);
            else
                return input.EndsWith(value);
        }


        /// <summary>
        /// Returns true if the input string matches the regular expression
        /// </summary>
        /// <param name="regex">Regular expression to test</param>
        /// <param name="input">Input string</param>
        /// <returns>True if input matches the regular expression</returns>
        /// <category>Regular Expressions</category>
        [MvmExport("regex_match")]
        public static bool regex_match(string regex, string input)
        {
            return Regex.IsMatch(input, regex);
        }
        /// <summary>
        /// Returns true if the input string matches the regular expression
        /// </summary>
        /// <param name="regex">Regular expression to test</param>
        /// <param name="input">Input string</param>
        /// <param name="ignore_case">True to ignore case</param>
        /// <returns>True if input matches the regular expression</returns>
        /// <category>Regular Expressions</category>
        [MvmExport("regex_match")]
        public static bool regex_match(string regex, string input, bool ignore_case)
        {
            if (ignore_case)
                return Regex.IsMatch(input, regex, RegexOptions.IgnoreCase);
            else
                return Regex.IsMatch(input, regex);
        }

        /// <summary>
        /// Returns the input string encoded with XML escapes
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>encoded string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("xml_encode")]
        public static string xml_encode(string input)
        {
            return System.Security.SecurityElement.Escape(input);
        }
    }
}
