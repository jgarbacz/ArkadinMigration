using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;

namespace MVM
{

    public class StringComparerValid:IComparer<string>
    {
        public static int CompareTo(string x, string y){
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            char[] xa = x.ToCharArray();
            char[] ya = y.ToCharArray();
            int minLen = xa.Length < ya.Length ? xa.Length : ya.Length;
            for (int i = 0; i < minLen; i++)
            {
                char xc = xa[i];
                char yc = ya[i];
                if (xc < yc) return -1;
                if (xc > yc) return 1;
            }
            if (xa.Length < ya.Length) return -1;
            if (xa.Length > ya.Length) return 1;
            return 0;
        }
        public int Compare(string x, string y)
        {
            return StringComparerValid.CompareTo(x,y);
        }
    }

    // string extension methods


    // Rob Guideline for writing extension methods:
    // 
    // when trying to support null versions of methods use this naming convention:
    //
    // *OrNull, which returns null if input is null
    // *OrDefault, which takes an extra argument with same type as return type and returns that if input is null
    // *OrNullString, which return null string if input is null 
    // *_Safe, does 'the right thing' when passed a null. We should always have a _Safe and an not _Safe version
    // else, it should throw a null object exception.
    //
    // Equality is treated differently in MyObject because it is so commonly used and I want it to read nicely.
    // 

    public static class MyString
    {

	                
        /// <summary>
        /// Gives you the same string broken up on quoted boundaries
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IEnumerable<string> ChunkForQuotes(this string s){
            bool q=false;
            bool qq=false;
            StringBuilder sb = new StringBuilder();
            foreach (var c in s.ToCharArray())
            {
                if (q)
                {
                    sb.Append(c);
                    if (c == '\'')
                    {
                        yield return sb.ToString();
                        sb.Length=0;
                        q = false;
                    }
                }
                else if (qq)
                {
                    sb.Append(c);
                    if (c == '"')
                    {
                        yield return sb.ToString();
                        sb.Length = 0;
                        qq = false;
                    }
                }
                else if (c == '\'')
                {
                    yield return sb.ToString();
                    sb.Length = 0;
                    q = true;
                    sb.Append(c);
                }
                else if (c == '"')
                {
                    yield return sb.ToString();
                    sb.Length = 0;
                    qq = true;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            yield return sb.ToString();
        }
        

        static readonly string LineSeparator = new StringBuilder("").AppendLine().ToString();

        public static void Test()
        {
            foreach (string s in new string[] { "/a/b/c", "x", "" })
            {
                Console.WriteLine("head(" + s + ")=" + s.PathHead());
                Console.WriteLine("tail(" + s + ")=" + s.PathTail());
            }
            
           
        }

        public static int CompareToValid(this string thisString, string thatString)
        {
            return StringComparerValid.CompareTo(thisString, thatString);
        }

       // all numeric comparisons return false if not a number. They do not error.
        #region numeric comparisons

        public static bool IsNumericNe(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return !(thisDecimal.Equals(thatDecimal));
                }
            }
            return false;
        }
        public static bool IsNumericEq(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return thisDecimal.Equals(thatDecimal);
                }
            }
            return false;
        }
        public static string StripFractionalTrailingZeros(this string thisString)
        {
            if (thisString == null) return thisString;
            int decimalIdx = thisString.LastIndexOf('.');
            if (decimalIdx < 0) return thisString;
            int len = thisString.Length;
            for (int i = thisString.Length - 1; ;i-- )
            {
                if (thisString[i].Equals('0'))
                {
                    len--;
                    continue;
                }
                if (thisString[i].Equals('.'))
                {
                    len--;
                }
                break;
            }
            return thisString.Substring(0, len);
        }

        /// <summary>
        /// Trims characters from the left side of a string
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Trimmed string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("ltrim")]
        public static string LeftTrim(this string thisString, string chars)
        {
            if (thisString == null) return thisString;
            return thisString.TrimStart(chars.ToCharArray());
        }

        public static bool IsNumericGt(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return thisDecimal > thatDecimal;
                }
            }
            return false;
        }
        public static bool IsNumericGte(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return thisDecimal >= thatDecimal;
                }
            }
            return false;
        }
        public static bool IsNumericLt(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return thisDecimal < thatDecimal;
                }
            }
            return false;
        }
        public static bool IsNumericLte(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return thisDecimal <= thatDecimal;
                }
            }
            return false;
        }
#endregion

        public static bool IsGt(this string thisString,string thatString)
        {
            return thisString.CompareTo(thatString)>0 ? true : false;
        }
        public static bool IsGtIgnoreCase(this string thisString, string thatString)
        {
            return thisString.ToLower().CompareTo(thatString.ToLower()) > 0 ? true : false;
        }
        public static bool IsGte(this string thisString, string thatString)
        {
            return thisString.CompareTo(thatString) >= 0 ? true : false;
        }
        public static bool IsGteIgnoreCase(this string thisString, string thatString)
        {
            return thisString.ToLower().CompareTo(thatString.ToLower()) >= 0 ? true : false;
        }
        public static bool IsLt(this string thisString, string thatString)
        {
            return thisString.CompareTo(thatString) < 0 ? true : false;
        }
        public static bool IsLtIgnoreCase(this string thisString, string thatString)
        {
            return thisString.ToLower().CompareTo(thatString.ToLower()) < 0 ? true : false;
        }
        public static bool IsLte(this string thisString, string thatString)
        {
            return thisString.CompareTo(thatString) <= 0 ? true : false;
        }
        public static bool IsLteIgnoreCase(this string thisString, string thatString)
        {
            return thisString.ToLower().CompareTo(thatString.ToLower()) <= 0 ? true : false;
        }

        /// <summary>
        /// Returns true if thisString is not null or ''
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string thisString)
        {
            
            return thisString != null && !thisString.Equals("") ? false : true;
        }

        /// <summary>
        /// Returns true if thisString is not null or ''
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static bool NotNullOrEmpty(this string thisString)
        {

            return thisString != null && !thisString.Equals("") ? true : false;

        }

        /// <summary>
        /// Returns thatString if thisString is null or ""
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="thatString"></param>
        /// <returns></returns>
        public static string Nvl(this string thisString, string thatString)
        {
            return thisString != null && !thisString.Equals("") ? thisString : thatString;
        }

        /// <summary>
        /// If thisString==null returns "" else thisString
        /// </summary>
        /// <param name="thisString"></param>
        /// <returns></returns>
        public static string NullSafe(this string thisString)
        {
            return thisString != null ? thisString : "";
        }

        /// <summary>
        /// Given /x/y/z it returns /x/y. Given x it returns ''. Given '' it returns ''
        /// Works for / or \.
        /// Purely a string function, not directory path!
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string PathHead(this string s)
        {
            int ndx1 = s.LastIndexOf('/');
            int ndx2 = s.LastIndexOf('\\');
            if (ndx1 < 0 && ndx2 < 0) return "";
            int ndx = ndx1 > ndx2 ? ndx1 : ndx2;
            return s.Substring(0, ndx);
        }
        /// <summary>
        /// Given '/x/y/z' it returns z. Given x it returns 'x'. Given '' it returns ''
        /// Works for / or \.
        /// Purely a string function, not directory path!
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string PathTail(this string s)
        {
            if (s == null) return null;
            int ndx1 = s.LastIndexOf('/');
            int ndx2 = s.LastIndexOf('\\');
            if (ndx1 < 0 && ndx2 < 0) return s;
            int ndx = ndx1 > ndx2 ? ndx1 : ndx2;
            return s.Substring(ndx + 1);
        }

        /// <summary>
        /// Removes count chars from the end of the string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Chop(this string s, int count)
        {
            if (count > s.Length) throw new Exception("cannot chop() more characters then exists in string");
            return s.Substring(0, s.Length - count);
        }

        public static void WriteToFile(this string s, string path)
        {
            File.WriteAllText(path, s);
        }
        public static void WriteToConsole(this string s)
        {
            Console.WriteLine(s);
        }

        public static int IndexOfOrDefault(this string s, char value, int defaultResult)
        {
            if (s != null) return s.IndexOf(value);
            return defaultResult;
        }

        public static bool EqualsIgnoreCaseSafe(this string s, string that)
        {
            return s.ToLower().Equals(that.ToLower());
        }

        public static bool EqualsIgnoreCase(this string s, string that)
        {
            return s.ToLower().Equals(that.ToLower());
        }

        #region numeric treatment of string

        public static bool EqualsNumeric(this string thisString, string thatString)
        {
            decimal thisDecimal, thatDecimal;
            if (Decimal.TryParse(thisString, out thisDecimal))
            {
                if (Decimal.TryParse(thatString, out thatDecimal))
                {
                    return thisDecimal.Equals(thatDecimal);
                }
            }
            return false;
        }
        public static bool EqualsOrEqualsNumeric(this string thisString, string thatString)
        {
            if (thisString.Equals(thatString))return true;
            return thisString.EqualsNumeric(thatString);
        }

        public static bool EqualsIgnoreCaseOrEqualsNumeric(this string thisString, string thatString)
        {
            if (thisString.EqualsIgnoreCase(thatString)) return true;
            return thisString.EqualsNumeric(thatString);
        }

        //public static void RaceThem()
        //{
        //    string s = "-001231231";

        //    var x = "  -22 ".IsNumeric();

        //    bool v;
        //    int count = 1000000;
        //    //count = 1;
        //    Console.WriteLine("Started string equals-"+count);
        //    Stopwatch st = new Stopwatch(); 
        //    st.Start(); 
        //    for (int i = 0; i < count; i++)
        //    {
        //        v = s.Equals(s);
        //    }
        //    st.Stop(); 
        //    Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());

        //    Console.WriteLine("Started isNumeric-" + count);
        //    st = new Stopwatch();
        //    st.Start();
        //    for (int i = 0; i < count; i++)
        //    {
        //        v = s.IsNumeric();
        //        v = s.IsNumeric();
        //    }
        //    st.Stop();
        //    Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());


        //    Console.WriteLine("Started decimal-" + count);
        //    decimal d1, d2;
        //    bool b1, b2;
        //    st = new Stopwatch();
        //    st.Start();
        //    for (int i = 0; i < count; i++)
        //    {
        //        b1 = Decimal.TryParse(s,out d1);
        //        b2 = Decimal.TryParse(s, out d2);
        //        v = b1.Equals(b2);
        //    }
        //    st.Stop();
        //    Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());

        //    //Console.WriteLine("Started Regex-" + count);
        //    //st = new Stopwatch();
        //    //st.Start();
        //    //for (int i = 0; i < count; i++)
        //    //{
        //    //    v = s.EqualsNumericRegex(s);
        //    //}
        //    //st.Stop();
        //    //Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());

        //    //Console.WriteLine("Started RegexC-" + count);
        //    //st = new Stopwatch();
        //    //st.Start();
        //    //for (int i = 0; i < count; i++)
        //    //{
        //    //    v = s.EqualsNumericRegex(s);
        //    //}
        //    //st.Stop();
        //    //Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());

        //    Console.WriteLine("Started Rob-" + count);
        //    st = new Stopwatch();
        //    st.Start();
        //    for (int i = 0; i < count; i++)
        //    {
        //        v = s.EqualsNumeric(s);
        //    }
        //    st.Stop();
        //    Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString()); 
 
        //}

        //public static string pat=@"^\s*(-?)(\d+)(\.?)(\d+)\s*$";
        //public static Regex regex = new Regex(pat);
        //public static bool EqualsNumericRegex(this string thisString, string thatString)
        //{
        //    return regex.IsMatch(thisString) && regex.IsMatch(thatString);
        //}
        //public static Regex regexC = new Regex(pat, RegexOptions.Compiled);
        //public static bool EqualsNumericRegexC(this string thisString, string thatString)
        //{
        //    return regexC.IsMatch(thisString) && regexC.IsMatch(thatString);
        //}
        ///// <summary>
        ///// Returns true if both strings look like a number and are equal. 
        ///// "100".EqualsNumeric("100.00") and "-.01".EqualsNumeric("-.01000")
        ///// but "abc".EqualsNumeric(100) is false. "".EqualsNumeric("")==false
        ///// </summary>
        ///// <param name="thisString"></param>
        ///// <param name="thatString"></param>
        ///// <returns></returns>
        //public static bool EqualsNumeric(this string thisString, string thatString)
        //{
        //    char[] thisArr = thisString.ToCharArray();
        //    char[] thatArr = thatString.ToCharArray();
        //    int thisIdx = 0;
        //    int thatIdx = 0;
        //    // skip leading space
        //    ConsumeLeadingSpaces(ref thisArr, ref thisIdx);
        //    ConsumeLeadingSpaces(ref thatArr, ref thatIdx);
        //    // consume and save the sign
        //    bool thisIsNegative = ConsumeSign(ref thisArr, ref thisIdx);
        //    bool thatIsNegative = ConsumeSign(ref thatArr, ref thatIdx);
        //    // skip leading zeros
        //    bool thisHasZero = ConsumeZeros(ref thisArr, ref thisIdx);
        //    bool thatHasZero = ConsumeZeros(ref thatArr, ref thatIdx);
        //    // consume the integer part (including '.')
        //    bool thisHasNonZeroInteger = false;
        //    bool thatHasNonZeroInteger = false;
        //    bool integerPartMatch = ConsumeIntegerPart(ref thisArr, ref thisIdx, out thisHasNonZeroInteger, ref thatArr, ref thatIdx, out thatHasNonZeroInteger);
        //    if (!integerPartMatch) return false;
        //    return true;
        //    // see if we are at a decimal
        //    // TBD
        //    // consume the decimal part 
        //    //bool thisHasDecimal = false;
        //    //bool thatHasDecimal = false;
        //    //bool thisHasNonzeroDecimal = false;
        //    //bool thatHasNonzeroDecimal = false;
        //    //bool decimalPartMatch = ConsumeDecimalPart(thisArr, thisIdx, out thisHasDecimal, out thisHasNonzeroDecimal, thatArr, thatIdx, out thatHasDecimal, out thatHasNonzeroDecimal);
        //    //if (decimalPartMatch) return false;
        //    //// both must look like a number (not: ' ', '', '-', '.' )
        //    //if (!(thisHasZero || thisHasNonZeroInteger || thisHasDecimal) && (thatHasZero || thatHasNonZeroInteger || thatHasDecimal)) return false;
        //    //// if everything is zero return true regardless of sign ie  (-0.00 = 00)
        //    //if (!(thisHasNonZeroInteger || thisHasNonzeroDecimal) && !(thatHasNonZeroInteger || thatHasNonzeroDecimal)) return true;
        //    //// if the sign matches return true
        //    //if (thisIsNegative == thatIsNegative) return true;
        //    //// otherwise we do not match
        //   // return false;
        //}
        //private static bool ConsumeIntegerPart(ref char[] arr1, ref int idx1, out bool hasNonZeroInteger1, ref char[] arr2, ref int idx2, out bool hasNonZeroInteger2)
        //{
        //    hasNonZeroInteger1 = false;
        //    hasNonZeroInteger2 = false;
        //    bool atEnd1 = false;
        //    bool atEnd2 = false;
        //    for (; ;)
        //    {
        //        // set atEnd flag to know if we should consider the current char
        //        if (!atEnd1)
        //        {
        //            if (idx1>=arr1.Length) atEnd1 = true;
        //            else if (arr1[idx1] == '.') atEnd1 = true;
        //            else if (arr1[idx1] == ' ') atEnd1 = true;
        //        }
        //        if (!atEnd2)
        //        {
        //            if (idx2 >= arr2.Length) atEnd2 = true;
        //            else if (arr2[idx2] == '.') atEnd2 = true;
        //            else if (arr2[idx2] == ' ') atEnd2 = true;
        //        }
        //        // if both at end no mismatch
        //        if (atEnd1 && atEnd2) return true;
        //        // if both not at end
        //        if (!atEnd1 && !atEnd2)
        //        {
        //            // if either is not a digit mismatch
        //            if (!IsDigit(arr1[idx1]) || !IsDigit(arr2[idx2])) return false;
        //            // if digits are not equal mismatch
        //            if (arr1[idx1] != arr2[idx2]){
        //                return false;
        //            }else{
        //                idx1++;
        //                idx2++;
        //                hasNonZeroInteger1 = true;
        //                hasNonZeroInteger2 = true;
        //                continue;
        //            }
        //        }
        //        // if we get here one has a digit and the other doesn't so mismatch
        //        return false;
        //    }
        //}
        //private static bool IsDigit(char c)
        //{
        //    return ('0'<=c) && (c<='9');
        //}
        //private static bool ConsumeZeros(ref char[] arr, ref int idx)
        //{
        //    bool output = false;
        //    while (idx < arr.Length && arr[idx + 1] == '0')
        //    {
        //        idx++;
        //        output = true;
        //    }
        //    return output;
        //}
        //private static bool ConsumeSign(ref char[] arr, ref int idx)
        //{
        //    bool output = false;
        //    if (arr[idx] == '-')
        //    {
        //        idx++;
        //        output = true;
        //    }
        //    return output;
        //}
        //private static bool ConsumeLeadingSpaces(ref char[] arr, ref int idx)
        //{
        //    bool output = false;
        //    while (idx < arr.Length && arr[idx + 1] == ' ')
        //    {
        //        output = true;
        //        idx++;
        //    }
        //    return output;
        //}
        #endregion

        public static bool matches(this string s, string regexString)
        {
            Regex regex = new Regex(regexString);
            Match m = regex.Match(s);
            if (m.Success)
            {
                return true;
            }
            return false;
        }


        public static List<string> Cluster(this string s, int clusterSize)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < s.Length; i += clusterSize)
            {
                output.Add(s.Substring(i, clusterSize));
            }
            return output;
        }

        /// <summary>
        /// Returns true if the first argument equals one of the passed strings
        /// </summary>
        /// <param name="s"></param>
        /// <param name="strings"></param>
        /// <returns></returns>
        //public static bool In(this string s, params string[] strings)
        //{
        //    foreach (string ss in strings)
        //    {
        //        if (s.Equals(ss)) return true;
        //    }
        //    return false;
        //}

        

        /// <summary>
        /// Repeats the string n times and returns it.
        /// </summary>
        /// <param name="s">string to repeat</param>
        /// <param name="n">number of times to repeat it</param>
        /// <returns>repeated string</returns>
        public static string repeat(this string s, int n)
        {
            if (n <= 0) return "";
            StringBuilder sb = new StringBuilder(s.Length * n);
            for (int i = 0; i < n; i++) sb.Append(s);
            return sb.ToString();
        }

        /// <summary>
        /// Returns true is string starts with ' and ends with '
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsQ(this string s)
        {
            return s.StartsWith("'") && s.EndsWith("'");
        }


        /// <summary>
        /// Returns true is string starts with " and ends with "
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsQQ(this string s)
        {
            return s.StartsWith("\"") && s.EndsWith("\"");
        }

        /// <summary>
        /// Returns true is single or double quoted
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsQuoted(this string s)
        {
            return s.IsQ() || s.IsQQ();
        }
        /// <summary>
        /// returns true if the string looks like a number int or decimal, pos or neg
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string s)
        {
            decimal junk;
            return Decimal.TryParse(s, out junk);
        }

        /// <summary>
        /// Adds single quotes or doubles is contains singles
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>Quoted output string</returns>
        public static string SingleOrDoubleQuoteMe(this string s)
        {
            return !s.Contains("'") ? s.q() : s.qq();
        }
        /// <summary>
        /// Adds double quotes or singles if contains doubles
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>Quoted output string</returns>
        public static string DoubleOrSingleQuoteMe(this string s)
        {
            return !s.Contains("\"") ? s.qq() : s.q();
        }

        /// <summary>
        /// Adds single quotes
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>Quoted output string</returns>
        public static string q(this string s)
        {
            return "'" + s + "'";
        }

        /// <summary>
        /// Adds double quotes
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>Double=quoted output string</returns>
        public static string qq(this string s)
        {
            return "\"" + s + "\"";
        }
        /// <summary>
        /// Replaces escape '\' with '\\' and returns the result
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EscEsc(this string s)
        {
            return s.Replace(@"\", @"\\"); ;
        }

        public static bool IsInt(this string value)
        {
            int temp = 0;
            if (int.TryParse(value, out temp))
            {
                return true;
            }
            return false;
        }

        public static int GetInt(this string value)
        {
            int temp = 0;
            if (int.TryParse(value, out temp))
            {
                return temp;
            }
            throw new Exception("Cannot convert string value=[" + value + "] to an int");
        }

        public static string StripQ(this string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'")) return value.Substring(1, value.Length - 2);
            return value;
        }

        public static string StripQQ(this string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\"")) return value.Substring(1, value.Length - 2);
            return value;
        }

        public static string StripQuotes(this string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'")) return value.Substring(1, value.Length - 2);
            if (value.StartsWith("\"") && value.EndsWith("\"")) return value.Substring(1, value.Length - 2);
            return value;
        }

        public static string StripNewlines(this string value)
        {
            value = value.Replace("\r\n", "");
            value = value.Replace("\n", "");
            return value;
        }

        /// <summary>
        /// Replaces characters
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Output string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("replace")]
        public static string Replace(this string value, string oldValue, string newValue)
        {
            return value.Replace(oldValue, newValue);
        }

        /// <summary>
        /// Replaces backslash escapes with literal characters
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>escaped string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("interpolate_escapes")]
        public static string InterpolateEscapes(this string value)
        {
            value = value.Replace(@"\r", "\r");
            value = value.Replace(@"\n", "\n");
            value = value.Replace(@"\t", "\t");
            return value;
        }

        public static string InterpolateEscapesReverse(this string value)
        {
            value = value.Replace("\r", @"\r");
            value = value.Replace("\n", @"\n");
            value = value.Replace("\t", @"\t");
            return value;
        }

        public static string StripTrailingLineSeparator(this string value)
        {
            if (value.EndsWith(LineSeparator)) return value.Substring(0, value.Length - LineSeparator.Length);
            return value;
        }
        
        public static string AppendLine(this string value)
        {

            StringBuilder sb = new StringBuilder(value);
            sb.AppendLine();
            return sb.ToString();
        }

        public static string AppendLineAnd(this string value, string line)
        {
            value = value.AppendLine();
            value = value.Append(line);
            return value;
        }
        public static string AppendLine(this string value, string line)
        {

            StringBuilder sb = new StringBuilder(value);
            sb.AppendLine(line);
            return sb.ToString();
        }
        public static string Append(this string value, string line)
        {

            StringBuilder sb = new StringBuilder(value);
            sb.Append(line);
            return sb.ToString();
        }

        public static string PadLeftFixed(this string value, int length)
        {
            if (value.Length != length)
            {
                if (value.Length > length)
                    value = value.Substring(0, length);
                else
                    value = value.PadLeft(length);
            }
            return value;
        }

        public static string PadRightFixed(this string value, int length)
        {
            if (value.Length != length)
            {
                if (value.Length > length)
                    value = value.Substring(0, length);
                else
                    value = value.PadRight(length);
            }
            return value;
        }

        /// <summary>
        /// Pads a string on the left
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="length">Length to pad out to</param>
        /// <param name="paddingChar">Character to pad with</param>
        /// <returns>padded string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("lpad")]
        public static string PadLeftFixed(this string value, int length, string paddingChar)
        {
            if (value.Length != length)
            {
                if (value.Length > length)
                    value = value.Substring(0, length);
                else
                    value = value.PadLeft(length, paddingChar.ToCharArray()[0]);
            }
            return value;
        }
        public static string PadLeftFixed(this string value, int length, char paddingChar)
        {
            if (value.Length != length)
            {
                if (value.Length > length)
                    value = value.Substring(0, length);
                else
                    value = value.PadLeft(length,paddingChar);
            }
            return value;
        }

        /// <summary>
        /// Pads a string on the right
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="length">Length to pad out to</param>
        /// <param name="paddingChar">Character to pad with</param>
        /// <returns>padded string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("rpad")]
        public static string PadRightFixed(this string value, int length, string paddingChar)
        {
            if (value.Length != length)
            {
                if (value.Length > length)
                    value = value.Substring(0, length);
                else
                    value = value.PadRight(length, paddingChar.ToCharArray()[0]);
            }
            return value;
        }
        public static string PadRightFixed(this string value, int length, char paddingChar)
        {
            if (value.Length != length)
            {
                if (value.Length > length)
                    value = value.Substring(0, length);
                else
                    value = value.PadRight(length,paddingChar);
            }
            return value;
        }

        public static string NumberLines(this string value)
        {
            return NumberLines(value, 1);
        }

        public static string NumberLines(this string value, int startNumber)
        {
            if (value.Length == 0) return value;
            StringBuilder outputValue = new StringBuilder();
            StringReader reader = new StringReader(value);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                outputValue.AppendLine((startNumber++).ToString() + ":\t" + line);
            }
            return outputValue.ToString();
        }

        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }
        public static long ToLong(this string s)
        {
            return long.Parse(s);
        }

        public static bool ToInt(this string s, out int result)
        {
            return int.TryParse(s, out result);
        }

        /// <summary>
        /// Returns the length of a string
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <returns>length of the string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("length")]
        public static string Length(this string expr)
        {
            return expr.Length.ToString();
        }

        #region perl substr
        public static string Substr(this string expr, int offset)
        {
            if (offset < 0)
            {
                offset = expr.Length + offset;
            }
            return expr.Substring(offset);
        }

        /// <summary>
        /// Extracts a substring
        /// </summary>
        /// <param name="expr">Input string</param>
        /// <param name="offset">Character offset</param>
        /// <param name="length">Length to extract</param>
        /// <returns>extracted string</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("substr")]
        public static string Substr(this string expr, int offset, int length)
        {
            if (expr == null) return "";
            if (offset < 0)
            {
                offset = expr.Length + offset;
            }
            if (length < 0)
            {
                length = expr.Length + length - offset;
            }
            return expr.Substring(offset, length);
        }

        public static string Substr(this string expr, int offset, int length, string replacement)
        {
            if (offset < 0)
            {
                offset = expr.Length + offset;
            }
            if (length < 0)
            {
                length = expr.Length + length - offset;
            }
            string before=expr.Substr(0,offset);
            string after = expr.Substr(offset + length);
            return before + replacement + after;
        }

        #endregion

        #region perl reverse
        public static string ReverseString(this string expr)
        {
            StringBuilder rev = new StringBuilder(expr.Length);
            for (int i = expr.Length - 1; i >= 0; i--)
            {
                rev.Append(expr[i]);
            }
            return rev.ToString();
        }
        #endregion

        /// <summary>
        /// Provides a logical if-then-else like oracle's decode function
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="test1">Value to test against</param>
        /// <param name="output1">Output if test matches</param>
        /// <param name="otherwise">Output if test does not match</param>
        /// <returns>output value</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("decode")]
        public static string decode(this string value, string test1, string output1, string otherwise = "")
        {
            if (value.Equals(test1))
            {
                return output1;
            }
            return otherwise;
        }
    }
}
