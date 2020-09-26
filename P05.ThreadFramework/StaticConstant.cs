using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P05.ThreadFramework
{
    public class StaticConstant
    {



        #region file location
        public static string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static string JsonPath = ConfigurationManager.AppSettings["JsonPath"];
        #endregion

    }
}
