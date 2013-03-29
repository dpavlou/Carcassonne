using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne
{
    public class Avatar
    {

        public Avatar(string usrname, GraphicsDevice graphicsDevice)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[2048];
            SteamApi steamapi = new SteamApi(usrname);

            if (steamapi.Name != null)
                UsrName = steamapi.Name;
            else
                UsrName = usrname;
            try
            {
                HttpWebRequest avatarRequest = (HttpWebRequest)WebRequest.Create(steamapi.AvatarPath);
                HttpWebResponse avatarResponse = (HttpWebResponse)avatarRequest.GetResponse();
                Stream stream = avatarResponse.GetResponseStream();

                int bytesRead = 1;
                while (bytesRead != 0)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, bytesRead);
                }

                memoryStream.Position = 0;

                Texture = Texture2D.FromStream(graphicsDevice, memoryStream);
            }
            catch
            { ; }
        }

        public Texture2D Texture
        {
            get; 
            set;
        }

        public string UsrName
        {
            get;
            set;
        }
    }
}
