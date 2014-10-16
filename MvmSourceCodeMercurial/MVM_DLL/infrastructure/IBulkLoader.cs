using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public interface IBulkLoader
    {
        List<string> GetOrderedFieldNames();
        int InsertRow(string[] row);
        int Flush();
        int Abort();
        string TableName { get; }
    }
}
