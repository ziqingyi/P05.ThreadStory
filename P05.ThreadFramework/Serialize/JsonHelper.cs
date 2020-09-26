﻿using System;
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
        public static string ObjectToString<T>(T obj)
        {
            JavaScriptSerializer jss= new JavaScriptSerializer();
            return jss.Serialize(obj);
        }

        public static T StringToObject<T>(string content)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(content);
        }

        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static T JsonFileToObject<T>(string filename)
        {
            string fullname = Path.Combine(StaticConstant.JsonPath,filename);
            if (File.Exists(fullname))
            {
                string json = File.ReadAllText(fullname, Encoding.UTF8);
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new Exception($"json file {filename} not exist !");
            }

        }




    }
}
