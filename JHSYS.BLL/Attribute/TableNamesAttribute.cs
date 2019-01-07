using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHSYS.BLL
{
    public class TableNamesAttribute : Attribute
    {
        public string TableName
        {
            get;
            private set;
        }
        public TableNamesAttribute(string tableName)
        {
            this.TableName = tableName;

        }
    }
}
