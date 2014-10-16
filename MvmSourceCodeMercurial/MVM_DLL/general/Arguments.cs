using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace MVM
{
    /// <summary>
    /// Arguments class
    /// </summary>

    public class Arguments
    {
        // Variables
        public StringDictionary Parameters;
        public List<string> ParameterList;

        // Constructor
        public Arguments(string[] Args)
        {
            Parameters = new StringDictionary();
            ParameterList = new List<string>();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-procInst" 
            // /param4=happy -param5 '--=nice=--'

            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[0] = Remover.Replace(Parts[0], "$1");
                                this.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                this.Add(Parameter, "true");
                            }
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                this.Add(Parameter, "true");
                            }
                        }
                        Parameter = Parts[1];

                        // PurgeCluster possible enclosing characters (",')
                        if (!Parameters.ContainsKey(Parameter))
                        {
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            this.Add(Parameter, Parts[2]);
                        }

                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!Parameters.ContainsKey(Parameter))
                {
                    this.Add(Parameter, "true");
                }
            }
        }

        public void Add(string param, string value)
        {
            Parameters.Add(param, value);
            ParameterList.Add(param);
        }

        public void Remove(string param)
        {
            Parameters.Remove(param);
            ParameterList.Remove(param);
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string Param]
        {
            get
            {
                return (Parameters[Param]);
            }
        }

        public string Dump()
        {
            string value = "";
            foreach (DictionaryEntry e in Parameters)
            {
                value += e.Key + ": " + e.Value + "; ";
            }
            return value;
        }
    }
}
