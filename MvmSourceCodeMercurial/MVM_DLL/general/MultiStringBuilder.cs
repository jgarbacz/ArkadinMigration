using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{

    public class MultiStringBuilder
    {
        public StringBuilder[] stringBuilders;
        public MultiStringBuilder(params StringBuilder[] stringBuilders)
        {
            this.stringBuilders = stringBuilders;
        }
        public void Append(string s)
        {
            foreach(var sb in this.stringBuilders)
                if(sb!=null)sb.Append(s);
        }
        public void AppendLine(string s)
        {
            foreach (var sb in this.stringBuilders)
                if (sb != null) sb.AppendLine(s);
        }
    }
}
