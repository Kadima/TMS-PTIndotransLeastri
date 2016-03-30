using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel
{
    class Modfunction
    {
        public static string CheckNull(object ojbValue)
        {
            if (ojbValue == null)
            { return ""; }
            return ojbValue.ToString();
        }
        public static string SQLSafeValue(object ojbValue)
        {
            if (ojbValue == null)
            {
                return "NULL";
            }
            else
                return "'" + SQLSafe((String)ojbValue) + "'";
        }

        public static string SQLSafe(string strValue)
        {
            if (strValue.Length > 0)
                return strValue.Replace("'", "''");
            else
                return strValue;
        }
    }
}
