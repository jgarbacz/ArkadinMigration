using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Stack = Antlr.Runtime.Collections.StackList;

namespace MvmScript
{
    /// <summary>
    /// This wraps up what antlr would throw and allows me to add additional information like fileName.
    /// </summary>
    public class MvmScriptMissingClosingException : MvmScriptParserException
    {
        public string baseMessage;
        public string startText;
        public string endText;
        public MvmScriptMissingClosingException(string baseMessage,int startingLine, int startingPosition,string startText,string endText)
        {
            this.baseMessage = baseMessage;
            this.LineNo = startingLine;
            this.LinePosition = startingPosition;
            this.startText = startText;
            this.endText = endText;
            this.Text = this.startText;
        }
        public override string Message
        {
            get
            {
                string msg=this.baseMessage;
                if (FromLocation != null) 
                    msg += " in " + FromLocation.Location;
                return msg;
            }
        }
    }
}
