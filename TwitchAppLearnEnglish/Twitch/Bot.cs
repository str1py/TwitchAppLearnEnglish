using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;

namespace TwitchAppLearnEnglish.Twitch
{
    class Bot
    {
        TwitchClient client;
        ShowNewWord snw = new ShowNewWord();
        UsersClass us = new UsersClass();

        string twitchusername = "";
        string twitchOAuth = "";

        public Bot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(twitchusername, twitchOAuth);

            client = new TwitchClient();
            client.Initialize(credentials, twitchusername);

            client.OnMessageReceived += onMessageReceived;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }

        private async void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                MainWindow.Instance.ListBoxAnswers.Items.Add($"Connected to {e.AutoJoinChannel}");
            }));
        }

        private async void  onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("!"))
            {
                //string message = e.ChatMessage.Message.Substring(e.ChatMessage.Message.IndexOf(' '));
                string message = e.ChatMessage.Message;
                if (us.CheckForExistPersonByName(e.ChatMessage.Username) == false)
                    us.AddUser(e.ChatMessage.Username);

                await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    if (us.SearchWhoAnswer(e.ChatMessage.Username) == true)
                    {
                        MainWindow.Instance.ListBoxAnswers.Items.Add(e.ChatMessage.Username + " ответил(а) через Twitch");
                        us.AddWhoAnswer(e.ChatMessage.Username);
                        if (snw.IsAnswerRight(message) == true)
                        {
                            us.AddPoint(e.ChatMessage.Username);
                        }
                    }
                }));

              
            }
        }


      
    }
}
