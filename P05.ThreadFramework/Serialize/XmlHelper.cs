using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace P05.ThreadFramework.Serialize
{
    public static class XmlHelper
    {
       //xmlserializer: from obj to string, put in file
       public static string ObjToXml<T>(T t) where T : new()
       {
            XmlSerializer xmlSerializer = new XmlSerializer(t.GetType());
            Stream stream = new MemoryStream();
            xmlSerializer.Serialize(stream,t);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string OutputText = reader.ReadToEnd();

            //1 output the result into a file
            if (!Directory.Exists(StaticConstant.SerializeDataPath))
            {
                Directory.CreateDirectory(StaticConstant.SerializeDataPath);
            }
            string outputFile = Path.Combine(StaticConstant.SerializeDataPath + "ClassToXml.xml");
            using (FileStream file = File.Create(outputFile))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(outputFile);
                file.Write(bytes,0,bytes.Length);
                file.Flush();
            }

            //2 return xml string used in program
            return OutputText;
       }

       //xmlserializer: from obj to string,
       //if using BinaryFormatter and SoapFormatter,
       //t and derived class must have [Serializable] attribute
       //this method serialize to file and string both for your test
       public static string ObjToXmlUsingBinaryFormatter<T>(T t) where T : new()
       {
           //1 output to a file
           string outputFile = Path.Combine(StaticConstant.SerializeDataPath + "objToXml.xml");
           using (FileStream file = new FileStream(outputFile,FileMode.Create, FileAccess.ReadWrite))
           {
               IFormatter formatter = new BinaryFormatter();
               formatter.Serialize(file,t);
           }

           // 2 return a string 
           {
                IFormatter formatter2 = new BinaryFormatter();
                Stream stream = new MemoryStream();
                formatter2.Serialize(stream, t);
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string OutputText = reader.ReadToEnd();
                return OutputText;
           }
       }

       //**************************Deserialize**************************************************
       // string being deserialized to Object
       public static T ToObject<T>(string content) where T : new()
       {
           using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
           {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                return (T) xmlFormat.Deserialize(stream);
           }
       }
       //file being deserialized to object
       public static T FileToOneObject<T>(string fileName) where T : new()
       {
           //string CurrentXMLPath = Constant.SerializeDataPath;
           //fileName = Path.Combine(CurrentXMLPath, @"users.xml");
           using (Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
           {
               XmlRootAttribute xRoot = new XmlRootAttribute();
               xRoot.ElementName = "User";
               xRoot.IsNullable = true;
               XmlSerializer xmlFormat = new XmlSerializer(typeof(T), xRoot);
               return (T) xmlFormat.Deserialize(fStream);
           }
       }

       //file being deserialized to an array of object
       public static T[] FileToObjects<T>(string fileName) where T : new()
       {
           //string CurrentXMLPath = Constant.SerializeDataPath;
           //fileName = Path.Combine(CurrentXMLPath, @"users.xml");
           using (Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
           {
               XmlRootAttribute xRoot = new XmlRootAttribute();
               xRoot.ElementName = "Users";
               xRoot.IsNullable = true;
               XmlSerializer XmlFormat = new XmlSerializer(typeof(T[]), xRoot);
               return (T[]) XmlFormat.Deserialize(fStream);
           }
       }




    }
}
