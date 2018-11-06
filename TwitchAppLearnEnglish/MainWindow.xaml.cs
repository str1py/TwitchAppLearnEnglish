using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TwitchAppLearnEnglish
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        ShowNewWord snw = new ShowNewWord();
        Twitch.Bot bot = new Twitch.Bot();
       // Twitch.UsersClass uc = new Twitch.UsersClass();
       // Twitch.UserPlaces up = new Twitch.UserPlaces();
        Twitch.TelegramBot bottl = new Twitch.TelegramBot();


        bool isWindowMaximaze = false;
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            snw.StartToShow();
           bottl.Main();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (isWindowMaximaze == false)
            {
                MainWindow.Instance.WindowState = WindowState.Maximized;
                isWindowMaximaze = true;
            }
            else
            {
                MainWindow.Instance.WindowState = WindowState.Normal;
                isWindowMaximaze = false;
            }

        }
    }
}
