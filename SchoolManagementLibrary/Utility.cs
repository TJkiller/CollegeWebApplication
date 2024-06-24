using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementLibrary
{
    public class Utility
    {
        public static string GetFormattedDate(DateTime date)
        {
            return date.ToString("dd-MM-yyyy");
        }
    }
}
