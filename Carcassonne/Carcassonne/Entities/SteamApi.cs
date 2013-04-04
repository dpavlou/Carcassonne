using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;

namespace Carcassonne
{
    public class SteamApi
    {
        public SteamApi(string usrname)
        {
            var path = ("http://steamcommunity.com/id/" + usrname + "/?xml=1");
            string xmlStr;
            Console.WriteLine(path);
            using (var wc = new WebClient())
            {
                xmlStr = wc.DownloadString(path);
            }

            var xDoc = new XmlDocument();
            xDoc.LoadXml(xmlStr);

            foreach (XmlNode xNode in xDoc.SelectNodes("profile"))
            {
                AvatarPath = xNode.SelectSingleNode("avatarMedium").InnerText;
                Name = xNode.SelectSingleNode("steamID").InnerText;
            }
        }

        public string AvatarPath
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
