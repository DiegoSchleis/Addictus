using Addictus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Addictus
{
    /// <summary>
    /// Interaktionslogik für MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new ViewModel.MainWindowViewModel(this);
        }

        public User CurrentUser { get; set; }
        public Dictionary<string, double> Dict { get; set; }
        public List<string> Blacklist { get; set; }
        public Recording Recording { get; set; }
        public ManageDatabase ManageDatabase { get; set; }
        
        private void MainWindow_LoadFiles(Object sender, RoutedEventArgs e)
        {
            ManageDatabase = new ManageDatabase(CurrentUser);
            ManageDatabase.CreateFiles();
            ManageDatabase.LoadTargets(Target_Stackpanel);
            ManageDatabase.OneTimeLoadRecords(this);
            ManageDatabase.LoadRecords(this);
            ManageDatabase.OneTimeLoadBlacklist();
            ManageDatabase.AutoUpdate();
            ((ViewModel.MainWindowViewModel)DataContext).Transform();
                        
            Recording = new Recording(CurrentUser, this);
            Recording.CreateThread();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Recording.StopRecording();
            ManageDatabase.SaveRecords();
            ManageDatabase.SaveBlacklist();
        }
    }
}
