using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;
namespace MVM
{
    public class MvmXlWorksheet
    {
        public IWorksheet worksheet;

        public int currentRow=1;
        public int currentCol=1;

        public MvmXlWorksheet(IWorksheet worksheet)
        {
            this.worksheet = worksheet;
        }

        public int NextRow(){
            currentCol = 1;
            return ++currentRow;
        }
        public void AppendField(string value){
            worksheet[currentRow,currentCol++].Value=value;
        }
    }
}
