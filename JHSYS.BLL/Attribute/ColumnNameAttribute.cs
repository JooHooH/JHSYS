using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHSYS.BLL
{
    public class ColumnNameAttribute : Attribute
    {
        public string ColumnName
        {
            get;
            private set;
        }
        public ColumnNameAttribute(string columnName)
        {
            this.ColumnName = columnName;

        }
    }
}
