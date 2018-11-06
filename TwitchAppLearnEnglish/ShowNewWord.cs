using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TwitchAppLearnEnglish
{
    class ShowNewWord
    {
        ///////////////////////LOCALVARS/////////////////////////////////
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        protected static List<string> Words = new List<string>();
        protected static List<string> Translate = new List<string>();

        public static int selectedIndex { get; set; }
        public static int prevSelectedIndex = 0;
        public static int rightAnswer;
        public static int time;
        Twitch.UsersClass us = new Twitch.UsersClass();
        Twitch.UserPlaces up = new Twitch.UserPlaces();

        Dictionary<string, string> topics = new Dictionary<string, string>();
        ///////////////////////LOCALVARS/////////////////////////////////


        public void StartToShow()
        { 
            AddWordsFromTxt();
            TimerStart();
        }

        private void TopicAdd()
        {
            topics.Add("Basic nouns", @"E:\ENGLISH\Basic nouns.txt");
            topics.Add("Basic adjectives", @"E:\ENGLISH\Basic adjectives.txt");
        }

        private void AddWordsFromTxt()
        {
            TopicAdd();
            ///PATH TO WORDS ! IT MAY BE CHANGED!
            var path = topics.ElementAt(1).Value;
            MainWindow.Instance.Title.Content = topics.ElementAt(1).Key +"("+ Words.Count +")";
            using (StreamReader fs = new StreamReader(path))
            {
                while (!fs.EndOfStream)
                {
                    string line = fs.ReadLine();
                    string[] words = line.Split('-');
                    string first = words[0].Trim();
                    string second = words[1].Trim();
                    Words.Add(first);
                    Translate.Add(second);
                }
            }
        }

  
        /////////////Timer//////////////////
        private void TimerStart()
        {
            dispatcherTimer.Tick += new EventHandler(timerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Start();
        }
        private void timerStop()
        {
            dispatcherTimer.Stop();
            dispatcherTimer.IsEnabled = false;
        }
        public async void timerTick(object sender, EventArgs e)
        {

            if (time == 0)
            {
                selectedIndex = SelectWord();

                HideAnswers();// спрятать варианты ответов
                await MainWindow.Instance.RuWord.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    MainWindow.Instance.RuWord.Content = Translate[prevSelectedIndex]; // показать правильный
                    System.Threading.Thread.Sleep(1000);
                }));           
                System.Threading.Thread.Sleep(2000);

                MainWindow.Instance.EngWord.Content = Words[selectedIndex];//показать новое слово
                MainWindow.Instance.RuWord.Content = "";
                SetAswers();// Задать новые варинты ответов
                ShowAnswers();// Показать новые варианты ответов

                time = 15;// Задать время
                MainWindow.Instance.TimerLabel.Text = time.ToString(); //Показать время 
                System.Threading.Thread.Sleep(3000);
                     
                prevSelectedIndex = selectedIndex;

                us.ClearWhoAnswer();
                MainWindow.Instance.ListBoxAnswers.Items.Clear();
                up.GetUsers();
            }
            else if (time != 0)
            {
                time -= 1;
                MainWindow.Instance.TimerLabel.Text = time.ToString();
            }

        }
        /////////////Timer//////////////////

        public bool IsAnswerRight(string useranswer)
        {
            string answer = "!" + rightAnswer;
            if (answer == useranswer)           
                return true;
            else
                return false;
              
        }

        /////////////Выбор и сет вариантов ответов//////////////////
        private int SelectWord()
        {
            Random random = new Random();
            return random.Next(0, Words.Count);
        }
        private int SelectVarOne()
        {
            System.Threading.Thread.Sleep(20);
            Random random = new Random((int)DateTime.Now.Ticks);
            int variant;
            variant = random.Next(0, Words.Count);
            return variant;
        }
        private int SelectVarTwo()
        {
            System.Threading.Thread.Sleep(20);
            Random random = new Random((int)DateTime.Now.Ticks);
            int variant;
            variant = random.Next(0, Words.Count);
            return variant;
        }
        private int SelectVarThree()
        {
            System.Threading.Thread.Sleep(20);
            Random random = new Random((int)DateTime.Now.Ticks);
            int variant;
            variant = random.Next(0, Words.Count);
            return variant;
        }
        private int SelectVarFour()
        {
            System.Threading.Thread.Sleep(20);
            Random random = new Random((int)DateTime.Now.Ticks);
            int variant;
            variant = random.Next(0, Words.Count);
            if (variant == rightAnswer)
            {
                System.Threading.Thread.Sleep(20);
                variant = random.Next(0, Words.Count);
                return variant;
            }
            else
            {
                return variant;
            }
        }

        public void SetAswers()
        {

            Random rn = new Random();
            rightAnswer = rn.Next(0, 4);
            MainWindow.Instance.Title.Content = topics.ElementAt(1).Key + "(" + Words.Count + ")";
            rightAnswer = rightAnswer + 1;

            switch (rightAnswer)
            {
                case 1:
                    MainWindow.Instance.AnswerVarOne.Content = "!1 " + Translate[prevSelectedIndex]; MainWindow.Instance.AnswerVarTwo.Content = "!2 " + Translate[SelectVarOne()];
                    MainWindow.Instance.AnswerVarThree.Content = "!3 " + Translate[SelectVarTwo()]; MainWindow.Instance.AnswerVarFour.Content = "!4 " + Translate[SelectVarThree()];
                    ShowAnswers();
                    break;
                case 2:
                    MainWindow.Instance.AnswerVarOne.Content = "!1 " + Translate[SelectVarOne()]; MainWindow.Instance.AnswerVarTwo.Content = "!2 " + Translate[prevSelectedIndex];
                    MainWindow.Instance.AnswerVarThree.Content = "!3 " + Translate[SelectVarTwo()]; MainWindow.Instance.AnswerVarFour.Content = "!4 " + Translate[SelectVarThree()];
                    ShowAnswers();
                    break;
                case 3:
                    MainWindow.Instance.AnswerVarOne.Content = "!1 " + Translate[SelectVarOne()]; MainWindow.Instance.AnswerVarTwo.Content = "!2 " + Translate[SelectVarTwo()];
                    MainWindow.Instance.AnswerVarThree.Content = "!3 " + Translate[prevSelectedIndex]; MainWindow.Instance.AnswerVarFour.Content = "!4 " + Translate[SelectVarThree()];
                    ShowAnswers();
                    break;
                case 4:
                    MainWindow.Instance.AnswerVarOne.Content = "!1 " + Translate[SelectVarOne()]; MainWindow.Instance.AnswerVarTwo.Content = "!2 " + Translate[SelectVarTwo()];
                    MainWindow.Instance.AnswerVarThree.Content = "!3 " + Translate[SelectVarThree()]; MainWindow.Instance.AnswerVarFour.Content = "!4 " + Translate[prevSelectedIndex];

                    break;
            }
      
        }
        /////////////Выбор и сет вариантов ответов//////////////////

        public void HideAnswers()
        {
            MainWindow.Instance.AnswerVarOne.Visibility = Visibility.Hidden; MainWindow.Instance.AnswerVarTwo.Visibility = Visibility.Hidden;
            MainWindow.Instance.AnswerVarThree.Visibility = Visibility.Hidden; MainWindow.Instance.AnswerVarFour.Visibility = Visibility.Hidden;
        }
        public void ShowAnswers()
        {
            MainWindow.Instance.AnswerVarOne.Visibility = Visibility.Visible;  MainWindow.Instance.AnswerVarTwo.Visibility = Visibility.Visible;
            MainWindow.Instance.AnswerVarThree.Visibility = Visibility.Visible;   MainWindow.Instance.AnswerVarFour.Visibility = Visibility.Visible;

        }


    }
}
