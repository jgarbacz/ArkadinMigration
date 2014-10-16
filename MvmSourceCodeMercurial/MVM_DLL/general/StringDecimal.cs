using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace MVM
{
    public class StringDecimal: IComparable<StringDecimal>
    {
        static readonly string stringDecimalPattern=@"^\s*0*(\d*)(\.?)(\d*?)0*\s*$";
        static readonly Regex stringDecimalRegex = new Regex(stringDecimalPattern);
        
        // integer part or 0
        readonly string integer;
        // fractional part or ""
        readonly string fraction;
        
        // speed up the hashing
        private int h = 0;
        public override int GetHashCode()
        {
            if (h != 0) return h;
            h = 1;
            h = 31 * h + (integer == null ? 0 : integer.GetHashCode());
            h = 31 * h + (fraction == null ? 0 : fraction.GetHashCode());
            return h; 
        }
        public override bool Equals(object obj)
        {
            StringDecimal that = obj as StringDecimal;
            if (that == null) return false;
            return this.Equals(that);
        }


        public bool Equals(StringDecimal that)
        {
            if (this == that) return true;
            if (this.GetHashCode() != that.GetHashCode()) return false;
            int i = this.CompareTo(that);
            return i == 0;
        }
        
        
        public StringDecimal(int integer)
        {            
            this.integer = integer.ToString();
            this.fraction = "";
        }

        public StringDecimal(string stringDecimal)
        {
            string[] parts = StringDecimal.SplitStringDecimal(stringDecimal);
            this.integer = parts[0];
            this.fraction = parts[1];
        }

        public static string[] SplitStringDecimal(string stringDecimal){
            Match m=stringDecimalRegex.Match(stringDecimal);
            if(!m.Success) throw new Exception("["+stringDecimal+"] is not a valid StringDecimal. It failed to match pattern ["+stringDecimalPattern+"]");
            string i=m.Groups[1].Value;
            string d = m.Groups[2].Value;
            string f = m.Groups[3].Value;
            if (i.Length == 0) i = "0";
            return new string[]{i,f};
        }

        public override string ToString()
        {
            string d=this.fraction.Length>0?".":"";
            return this.integer + d + this.fraction;
        }

        public int CompareTo(StringDecimal that)
        {
            if (this == that) return 0;
            if (this.integer.Length > that.integer.Length) return 1;
            if (that.integer.Length > this.integer.Length) return -1;
            int temp = this.integer.CompareTo(that.integer);
            if (temp != 0) return temp;
            if(this.fraction.Length==that.fraction.Length)return this.fraction.CompareTo(that.fraction);
            if (this.fraction.Length > that.fraction.Length){
                temp=this.fraction.Substring(0,that.fraction.Length).CompareTo(that.fraction);
                if(temp!=0) return temp;
                return 1;
            }else{
                temp=that.fraction.Substring(0,this.fraction.Length).CompareTo(this.fraction);
                if(temp!=0) return temp;
                return -1;
            }
        }
        public bool IsGreaterThan(StringDecimal that)
        {
            int i = this.CompareTo(that);
            return i > 0;
        }
        public bool IsLessThan(StringDecimal that)
        {
            int i = this.CompareTo(that);
            return i < 0;
        }
        public bool IsEqualTo(StringDecimal that)
        {
            if (this.GetHashCode() != that.GetHashCode()) return false;
            int i = this.CompareTo(that);
            return i == 0;
        }
       
        public bool IsGreaterThanOrEqualTo(StringDecimal that)
        {
            int i = this.CompareTo(that);
            return i >= 0;
        }
        public bool IsLessThanOrEqualTo(StringDecimal that)
        {
            int i = this.CompareTo(that);
            return i <= 0;
        }
        //public static void Main(string[] args)
        //{
        //    Tests();
        //}

        public static void Tests()
        {
            StringDecimal x = new StringDecimal("0.22");
            StringDecimal y = new StringDecimal("10");
            Console.WriteLine(x.CompareTo(y));
            Console.WriteLine("x="+x);
            Console.ReadKey();
        }
    }
}
