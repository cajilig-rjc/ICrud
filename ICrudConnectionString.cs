using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICrud.Data.Sql
{
    class ICrudConnectionString
    {
       public string ConnectionString()
        {

            return ConfigurationManager.ConnectionStrings["ICrudConnectionString"].ConnectionString;
        }                                                          
    }
}
