using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TwitchAppLearnEnglish.Twitch
{
    class UserPlaces
    {
        protected static List<string> users = new List<string>();
        protected static List<string> points = new List<string>();
        protected static Dictionary<string,int> usersAndPlaces = new Dictionary<string,int>();
        public void GetUsers()
        {
            users.Clear();
            points.Clear();
            usersAndPlaces.Clear();
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/Users.xml";
            XDocument xdoc = XDocument.Load(path);
            var elems = xdoc.Element("users").Elements("user").Select(item => item.Element("name").Value);

            foreach (string el in elems)
            {
                users.Add(el);
            }

            GetPlaces();
        }

        void GetPlaces()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/Users.xml";
            XDocument xdoc = XDocument.Load(path);
            var elems = xdoc.Element("users").Elements("user").Select(item => item.Element("right").Value);
            foreach (string a in elems)
            {
                points.Add(a);
            }

            for (int i = 0; i < users.Count; i++)
            {
                usersAndPlaces.Add(users[i], Int32.Parse(points[i]));
            }

            List<KeyValuePair<string, int>> sorted = (from kv in usersAndPlaces orderby kv.Value select kv).ToList();
            MainWindow.Instance.NameList.Items.Clear();
            MainWindow.Instance.ScoreList.Items.Clear();
            for (int i = sorted.Count-1; i >= 0; i--)
            {
                MainWindow.Instance.NameList.Items.Add(sorted[i].Key);
                MainWindow.Instance.ScoreList.Items.Add(sorted[i].Value);
            }
        }


    }
}
