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
    public class MvmScriptRecognitionException : MvmScriptParserException
    {
        public RecognitionException recognitionException;
        public System.Collections.IList stack;
        public string baseMessage;
        public MvmScriptRecognitionException(RecognitionException e, System.Collections.IList stack, string baseMessage)
        {
            this.recognitionException = e;
            this.stack = stack;
            this.baseMessage = baseMessage;
            this.LineNo = e.Line;
            this.LinePosition = e.CharPositionInLine + 1;
            this.Token = e.Token;
            this.Text = (this.Token!=null?this.Token.Text:null);
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
