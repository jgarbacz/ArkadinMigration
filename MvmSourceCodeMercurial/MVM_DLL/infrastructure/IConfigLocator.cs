using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public interface IConfigLocator
    {
        string GetLocation();
    }

    public class ConfigLocator : IConfigLocator
    {
        string location;
        public ConfigLocator(string location)
        {
            this.location = location;
        }
        public string GetLocation()
        {
            return this.location;
        }
    }
}
