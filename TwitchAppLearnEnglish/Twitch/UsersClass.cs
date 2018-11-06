using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace TwitchAppLearnEnglish.Twitch
{
    class UsersClass
    {
        public bool CheckForExistPersonByName(string name)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/Users.xml";
            XDocument xdoc = XDocument.Load(path);
            using (FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                System.Xml.XmlDocument CXML = new System.Xml.XmlDocument();
                CXML.Load(fStream);
                var elems = xdoc.Element("users").Elements("user").Select(item => item.Element("name").Value);

                foreach (string e in elems)
                {
                    try
                    {
                        if (name == e)
                            return true;
                    }
                    catch
                    {
                        return false;
                    }
                }


            }
            return false;
        }

        public void AddUser(string username)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/Users.xml";
            using (var fStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(fStream);
                try
                {
                    XmlElement newitem;
                    XmlElement newOUTERitem = xmlDoc.CreateElement("user");

                    newitem = xmlDoc.CreateElement("name");
                    newOUTERitem.AppendChild(newitem);
                    newitem.InnerText = username;

                    newitem = xmlDoc.CreateElement("date");
                    newOUTERitem.AppendChild(newitem);
                    newitem.InnerText = DateTime.Now.ToShortDateString();

                    newitem = xmlDoc.CreateElement("right");
                    newOUTERitem.AppendChild(newitem);
                    newitem.InnerText = "0";

                    newitem = xmlDoc.CreateElement("wrong");
                    newOUTERitem.AppendChild(newitem);
                    newitem.InnerText = "0";

                    // Close outer node  
                    xmlDoc.DocumentElement.InsertAfter(newOUTERitem, xmlDoc.DocumentElement.LastChild);

                    //Save the XML file   
                    FileStream WRITER = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);

                    xmlDoc.Save(WRITER);

                    //Close the writer filestream   
                    WRITER.Close();

                }
                catch { }
            }
        }

        public void AddWhoAnswer(string username)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/whoansweryet.txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(username);
            }

        }

        public bool SearchWhoAnswer(string username)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/whoansweryet.txt";
            string line;
            using (FileStream f = new FileStream(path, FileMode.Open))
            using (StreamReader sr = new StreamReader(f, Encoding.Default))
                while ((line = sr.ReadLine()) != null)
                    if (line.Contains(username))
                        return false;
            return true;
        }

        public void ClearWhoAnswer()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/whoansweryet.txt";
            StreamWriter writeFile;
            writeFile = new StreamWriter(new FileStream(path, FileMode.Truncate));
            writeFile.WriteLine("");
            writeFile.Close();
        }

        public void AddPoint(string username)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/Users.xml";
            XDocument xdoc = XDocument.Load(path);
            string points = xdoc.Element("users").Elements("user").Where(x => x.Element("name").Value==username).Select(item => item.Element("right").Value).Single();
            int userpoints = Int32.Parse(points);
            userpoints = userpoints + 1;
            points = userpoints.ToString();

            var el = (xdoc.Element("users").Elements("user").Where(x => x.Element("name").Value == username).Select(item => item.Element("right")).Single());
            el.Value = points;

            xdoc.Save(path);
        }
    }
}

