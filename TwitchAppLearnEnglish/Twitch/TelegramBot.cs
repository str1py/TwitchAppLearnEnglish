using System;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net;
using MihaZupan;

namespace TwitchAppLearnEnglish.Twitch
{
    class TelegramBot
    {
        private TelegramBotClient Bot;
  
        
        ShowNewWord snw = new ShowNewWord();
        UsersClass us = new UsersClass();

        string callbackNum;
        string howsend;
        int howdendid;
        string token ="" ;

        public async void Main()
        {
             var proxy = new HttpToSocks5Proxy("", 1080, "", "");

            Bot = new TelegramBotClient(token, proxy);
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            try
            {
                var me = Bot.GetMeAsync().Result;
                await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    MainWindow.Instance.ListBoxAnswers.Items.Add(me.Username);
                }));
                Bot.StartReceiving();
            }
            catch { } 
        }
        public void StopReciving()
        {
            try
            {
                Bot.StopReceiving();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private async void BotClickPlayerButtons(string message)
        {
            if (callbackNum == "!1" ) {
                await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate () {Cheker(message); }));
            }
            else if (callbackNum == "!2") {
                await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate () { Cheker(message); }));
            }
            else if (callbackNum == "!3") {
                await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate () { Cheker(message); }));
            }
            else if(callbackNum == "!4") {
                await MainWindow.Instance.ListBoxAnswers.Dispatcher.BeginInvoke(new Action(delegate () { Cheker(message); }));
            }
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            await MainWindow.Instance.Dispatcher.BeginInvoke(new Action(delegate ()
            {               
             var message = messageEventArgs.Message;
             var answer = message.Text; //Cообщение от пользователя 
             howsend = message.From.Username.ToLower();
             howdendid = message.From.Id;
             BotClickPlayerButtons(answer);
             if (answer == "/show")
             {
                 ShowPlayerButtons(this, null);
             }
            }));
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            callbackNum = callbackQueryEventArgs.CallbackQuery.Data;
            await Task.Delay(500);
            howsend = callbackQueryEventArgs.CallbackQuery.From.Username.ToLower();
            howdendid = callbackQueryEventArgs.CallbackQuery.From.Id;
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, $" {callbackNum}");
            BotClickPlayerButtons(callbackNum);
        }

        public async void ShowPlayerButtons(object sender, MessageEventArgs messageargs)
        {
            await Bot.SendChatActionAsync(howdendid, ChatAction.Typing);
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                   {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("!1"),
                            InlineKeyboardButton.WithCallbackData("!2"),
                        },
                        new [] // second row
                        {
                            InlineKeyboardButton.WithCallbackData("!3"),
                            InlineKeyboardButton.WithCallbackData("!4"),
                        }
                    });


            await Bot.SendTextMessageAsync(howdendid, "Дайте ваш ответ",
            replyMarkup: inlineKeyboard);
        }


        public void Cheker (string message)
        {
            if (us.CheckForExistPersonByName(howsend) == false)
                us.AddUser(howsend);
            if (us.SearchWhoAnswer(howsend) == true)
            {
                MainWindow.Instance.ListBoxAnswers.Items.Add(howsend + " ответил(а) через Telegram");
                us.AddWhoAnswer(howsend);
                if (snw.IsAnswerRight(message) == true)
                {
                    us.AddPoint(howsend);
                }
            }
        }

    }
}
