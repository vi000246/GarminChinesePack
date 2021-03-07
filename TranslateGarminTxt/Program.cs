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
            var taiwanList = getXml("Taiwan.gtt");
            var Garmin66iDefaultList = getXml("Dutch.gtt");
            var notExistList = new List<Str>();
            //Dutch是從66i取出的語言檔
            //Taiwan.gtt是從舊款導航取出的語言檔
            //比對Taiwan.gtt的缺漏後加到notExistList
            foreach (var str in Garmin66iDefaultList.Str) {
                var isExist = taiwanList.Str.Any(x=>x.Tag == str.Tag);
                if (!isExist) {
                    notExistList.Add(str);
                }
            }
            //將不在Taiwan.gtt的翻譯，儲存到新的xml檔裡
            createGttFile(notExistList);


        }

        public static void createGttFile(List<Str> notExistList) {
            XmlDocument doc = new XmlDocument();
            XmlElement element1 = doc.CreateElement(string.Empty, "gtt", string.Empty);
            doc.AppendChild(element1);


            foreach (var str in notExistList)
            {

                XmlElement element2 = doc.CreateElement(string.Empty, "str", string.Empty);
                element1.AppendChild(element2);

                XmlElement element3 = doc.CreateElement(string.Empty, "tag", string.Empty);
                XmlText text1 = doc.CreateTextNode(str.Tag);
                element3.AppendChild(text1);
                element2.AppendChild(element3);

                XmlElement element4 = doc.CreateElement(string.Empty, "txt", string.Empty);
                XmlText text2 = doc.CreateTextNode(str.Txt);
                element4.AppendChild(text2);
                element2.AppendChild(element4);
            }


            doc.Save("result_ParamNotInTaiwanGtt.xml");
        }

        public static Gtt getXml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Gtt));
            Gtt result = null;
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
               result = (Gtt)serializer.Deserialize(fileStream);
            }
            return result;
        }
    }
    [XmlRoot(ElementName = "hdr", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
    public class Hdr
    {
        [XmlElement(ElementName = "lang", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Lang { get; set; }
        [XmlElement(ElementName = "desc", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Desc { get; set; }
        [XmlElement(ElementName = "type", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Type { get; set; }
        [XmlElement(ElementName = "sort", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Sort { get; set; }
        [XmlElement(ElementName = "cpage", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Cpage { get; set; }
        [XmlElement(ElementName = "pnum", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Pnum { get; set; }
        [XmlElement(ElementName = "ver", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Ver { get; set; }
        [XmlElement(ElementName = "upperkbrd", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Upperkbrd { get; set; }
        [XmlElement(ElementName = "lowerkbrd", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Lowerkbrd { get; set; }
    }

    [XmlRoot(ElementName = "str", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
    public class Str
    {
        [XmlElement(ElementName = "tag", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Tag { get; set; }
        [XmlElement(ElementName = "txt", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public string Txt { get; set; }
    }

    [XmlRoot(ElementName = "gtt", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
    public class Gtt
    {
        [XmlElement(ElementName = "hdr", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public Hdr Hdr { get; set; }
        [XmlElement(ElementName = "str", Namespace = "http://www.garmin.com/xmlschemas/GarminTextTranslation/v1")]
        public List<Str> Str { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
    }

}

