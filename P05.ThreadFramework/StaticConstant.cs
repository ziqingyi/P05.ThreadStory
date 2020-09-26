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
        private static string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
        //get serialized data path
        public static string SerializeDataPath = ConfigurationManager.AppSettings["SerializeDataPath"];


        #region file location
        public static string LogPath = currentFolder + ConfigurationManager.AppSettings["LogPath"];
        public static string JsonPath = currentFolder + ConfigurationManager.AppSettings["JsonPath"];
        #endregion

    }
}
