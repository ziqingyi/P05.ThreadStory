using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace P05.ThreadFramework.Serialize
{
    public class JsonHelper
    {
        
        public static string ObjectToJsonStringJS<T>(T obj)
        {
            JavaScriptSerializer jss= new JavaScriptSerializer();
            string jsonString = jss.Serialize(obj);
            return jsonString;
        }
        public static string ObjectToJsonStringJsonConvert<T>(T obj)
        {
            string JsonString = JsonConvert.SerializeObject(obj);
            return JsonString;
        }

        public static T JsonStringToObjectJS<T>(string content)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            T t = jss.Deserialize<T>(content);
            return t;
        }

        public static T JsonStringToObjectJsonConvert<T>(string content)
        {
            T t =JsonConvert.DeserializeObject<T>(content);
            return t;
        }



        //read Json string from file
        public static T JsonFileToObject<T>(string filename)
        {
            string fullname = Path.Combine(StaticConstant.JsonPath,filename);
            if (File.Exists(fullname))
            {
                string json = File.ReadAllText(fullname, Encoding.UTF8);
                return JsonStringToObjectJS<T>(json);
            }
            else
            {
                throw new Exception($"json file {filename} not exist !");
            }

        }




    }
}
