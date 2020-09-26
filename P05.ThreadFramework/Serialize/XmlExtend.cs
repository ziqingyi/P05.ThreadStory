using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace P05.ThreadFramework.Serialize
{
    public static class XmlExtend
    {
        //T to Xml
        public static string ParseTtoXml<T>(this T model, string fatherNodeName)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement modelRootNode = xmldoc.CreateElement(fatherNodeName);
            xmldoc.AppendChild(modelRootNode);
            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    XmlElement attributeElement = xmldoc.CreateElement(property.Name);
                    if (property.GetValue(model, null) != null)
                    {
                        attributeElement.InnerText = property.GetValue(model, null).ToString();
                    }
                    else
                    {
                        attributeElement.InnerText = "[Null]";
                    }
                    modelRootNode.AppendChild(attributeElement);
                }
            }
            return xmldoc.OuterXml;
        }

        //xml to object, default: fatherNodeName="body"
        public static T ParseXMLstringToModel<T>(this string xml, string fatherNodeName = "body") where T : class, new()
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            T model = new T();
            //just for ChildNodes of one Node
            XmlNodeList attributes = xmldoc.SelectSingleNode(fatherNodeName).ChildNodes;
            foreach (XmlNode node in attributes)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties().Where(property=>node.Name == property.Name))
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                    {
                        property.SetValue(model,
                            property.PropertyType == typeof(Guid)? 
                                new Guid(node.InnerText) : Convert.ChangeType(node.InnerText,property.PropertyType) );
                    }
                    else
                    {
                        property.SetValue(model,null);
                    }
                }
            }
            return model;
        }

        //XML to object list
        public static List<T> XMLstringToObjectList<T>(this string xml, string headtag) where T : new()
        {
            List<T> list = new List<T>();
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);

            XmlNodeList nodelist = doc.SelectNodes(headtag);
            foreach (XmlNode node in nodelist)
            {
                T entity = new T();
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }

                foreach (PropertyInfo propinfo in propinfos)
                {
                    //turn the first letter into small case
                    string name = propinfo.Name.Substring(0, 1).ToUpper()
                                  + propinfo.Name.Substring(1, propinfo.Name.Length - 1);
                    XmlNode cnode = node.SelectSingleNode(name);//name should be same to xml attribute name
                    string v = cnode?.InnerText;//if null return null
                    if (v != null)
                    {
                        propinfo.SetValue(entity, Convert.ChangeType(v,propinfo.PropertyType), null);
                    }
                }
                list.Add(entity);
            }
            return list;
        }
    }
}
