using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Carcassonne
{
    
    public sealed class DatabaseManager
    {

        #region Declarations

        private readonly DeckManager deckManager;

        #endregion

        #region Constructor

        public DatabaseManager(DeckManager deckManager)
        {
            this.deckManager = deckManager;
        }

        #endregion

        #region Save/Load Methods

      /*   public void saveToDatabase()
        {
            string path = "C:\\";
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path + "\\My AddressBook\\addressBookInfo.xml");
            XmlNode xNode = xDoc.SelectSingleNode("Contacts");
            xNode.RemoveAll();

            foreach (Contact contact in contactManager.contacts)
            {
                XmlNode xTop = xDoc.CreateElement("Contact");
                XmlNode xName = xDoc.CreateElement("Name");
                XmlNode xSurname = xDoc.CreateElement("Surname");
                XmlNode xPhoneNumber = xDoc.CreateElement("PhoneNumber");
                xName.InnerText = contact.Name;
                xSurname.InnerText = contact.Surname;
                xPhoneNumber.InnerText = contact.PhoneNumber;
                xTop.AppendChild(xName);
                xTop.AppendChild(xSurname);
                xTop.AppendChild(xPhoneNumber);

                xDoc.DocumentElement.AppendChild(xTop);
            }

            xDoc.Save(path + "\\My AddressBook\\addressBookInfo.xml");
        }
      */

        public void loadFromDatabase()
        {

            if (!Directory.Exists(".\\TileSets"))
                Directory.CreateDirectory(".\\TileSets");
            if (!File.Exists(".\\TileSets\\tilesets.xml"))
            {
                XmlTextWriter xA = new XmlTextWriter(".\\TileSets\\tilesets.xml", Encoding.UTF8);
                xA.WriteStartElement("Gallery");
                xA.WriteEndElement();
                xA.Close();
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(".\\TileSets\\tilesets.xml");

            Console.WriteLine("loaded");

            int counter = 0; 
            int tileCounter= 0;

            foreach (XmlNode xNode1 in xDoc.SelectNodes("Gallery"))
            {

                {
                    foreach (XmlNode xNode2 in xNode1.ChildNodes)
                    {
                        if (counter <= 1)
                        {
                            deckManager.AddNewDeck();
                            tileCounter = 0;
                        }

                        foreach (XmlNode xNode3 in xNode2.ChildNodes)
                        //xDoc.SelectNodes("Gallery/Tileset/Tile"))
                        {
                            Console.WriteLine(xNode3.SelectSingleNode("Name").InnerText);
                            Console.WriteLine(xNode3.SelectSingleNode("Quantity").InnerText);

                            deckManager.AddTextureName((xNode3.SelectSingleNode("Name").InnerText), (int)MathHelper.Min(counter, 1));

                            int quantity = Convert.ToInt32(xNode3.SelectSingleNode("Quantity").InnerText);
                            for (int i = 0; i < quantity; i++)
                            {
                                deckManager.AddQuantities(tileCounter, (int)MathHelper.Min(counter,1));
                            }
                            tileCounter++;

                        }
                       
                        counter ++;
                    }
                }
            }
            
 

        }

        #endregion

    }
}