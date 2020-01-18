using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace SSISUtilityLib_CSharp
{
    public static class DataUtilities
    {
        public static bool isValidUSPostalCode(string PostalCode)
        {
            return Regex.IsMatch(PostalCode, "^[0-9]{5}(-[0-9]{4})?$");
        }
    }


}
