using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TranslateGarminTxt
{
    public class Program
    {

         static void Main(string[] args)
        {
            var taiwanList = getXml("Taiwan.xml");
            var Garmin66iDefaultList = getXml("Dutch.xml");
            var notExistList = new List<str>();
            foreach (var str in Garmin66iDefaultList.str) {
                var isExist = taiwanList.str.Any(x=>x.tag == str.tag);
                if (!isExist) {
                    notExistList.Add(str);
                }
            }

            XmlDocument doc = new XmlDocument();
            XmlElement element1 = doc.CreateElement(string.Empty, "gtt", string.Empty);
            doc.AppendChild(element1);

            //(1) the xml declaration is recommended, but not mandatory
            //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //XmlElement root = doc.DocumentElement;
            //doc.InsertBefore(xmlDeclaration, root);


            foreach (var str in notExistList) {

                XmlElement element2 = doc.CreateElement(string.Empty, "str", string.Empty);
                element1.AppendChild(element2);

                XmlElement element3 = doc.CreateElement(string.Empty, "tag", string.Empty);
                XmlText text1 = doc.CreateTextNode(str.tag);
                element3.AppendChild(text1);
                element2.AppendChild(element3);

                XmlElement element4 = doc.CreateElement(string.Empty, "txt", string.Empty);
                XmlText text2 = doc.CreateTextNode(str.txt);
                element4.AppendChild(text2);
                element2.AppendChild(element4);
            }


            doc.Save("output.xml");
        }

        public static gtt getXml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(gtt));
            gtt result = null;
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
               result = (gtt)serializer.Deserialize(fileStream);
            }
            return result;
        }
    }
    [XmlRoot("gtt")]
    public class gtt
    {
        [XmlElement("str")]
        public List<str> str { get; set; }
    }

    public class str
    {
        [XmlElement("tag")]
        public string tag { get; set; }
        [XmlElement("txt")]
        public string txt { get; set; }
    }

}

