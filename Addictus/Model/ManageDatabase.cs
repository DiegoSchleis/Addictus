using Addictus.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using Addictus.ViewModel;

namespace Addictus
{
    public class ManageDatabase
    {
        public User User { get; set; }
        public MainWindowView CallerMainWindow { get; set; }
        public ManageDatabase(User user)
        {
            User = user;
        }

        public bool CreateFiles()
        {
            if (User != null)
            {
                CreateRecords();
                CreateChart();
                CreateTargets();
                CreateBlacklist();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateRecords()
        {
            if (!File.Exists(Path.Combine(User.Username, "records.ser")))
            {
                using (Stream stream = File.Create(Path.Combine(User.Username, "records.ser")))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, new Dictionary<string, double>());
                }
            }
        }

        public void CreateBlacklist()
        {
            if (!File.Exists(Path.Combine(User.Username, "blacklist.ser")))
            {
                using (Stream stream = File.Create(Path.Combine(User.Username, "blacklist.ser")))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, new List<string>());
                }
            }
        }

        public void CreateChart()
        {
            string filename = "chart";
            CreateFileIfNotExists(filename, ".txt");
        }

        public void CreateTargets()
        {
            string filename = "targets";
            CreateFileIfNotExists(filename, ".txt");
        }

        public void LoadTargets(StackPanel stackpanel)
        {
            if (stackpanel != null)
            {
                string usersPath = Path.Combine(User.Username, "targets.txt");
                StreamReader reader = new StreamReader(usersPath);
                StackPanel panel = stackpanel;
                panel.Children.Clear();
                while (!reader.EndOfStream)
                {
                    string local = reader.ReadLine();
                    if (!local.Equals(""))
                    {
                        List<string> checkList = local.Split(';').ToList();
                        CustomButton customButton = new CustomButton(checkList.ElementAt(0), checkList.ElementAt(1));
                        customButton.User = User;
                        customButton.Stackpanel = stackpanel;
                        StackPanel buttonStack = new StackPanel();
                        buttonStack.Children.Add(customButton);

                        Grid g = new Grid();
                        ColumnDefinition cd = new ColumnDefinition();
                        cd.Width = new GridLength(9, GridUnitType.Star);
                        ColumnDefinition cd2 = new ColumnDefinition();
                        cd2.Width = new GridLength(1, GridUnitType.Star);

                        TextBox localTb = customButton.Textbox;

                        Button localBtn = customButton.CloseBtn;

                        g.ColumnDefinitions.Add(cd);
                        g.ColumnDefinitions.Add(cd2);
                        g.Children.Add(localTb);
                        Grid.SetColumn(localTb, 0);
                        g.Children.Add(localBtn);
                        Grid.SetColumn(localBtn, 1);
                        
                        buttonStack.Children.Add(g);
                        
                        panel.Children.Add(buttonStack);
                    }
                }
                reader.Close();
            }
        }

        public void AddTarget(string title, string content)
        {
            if (title != null && content != null && title != string.Empty && content != string.Empty)
            {
                string usersPath = Path.Combine(User.Username, "targets.txt");
                StreamWriter writer = new StreamWriter(usersPath, true);
                writer.Write("{0};{1}{2}", title, content, Environment.NewLine);
                writer.Close();
            }
            else
            {
                MessageBox.Show("Title and content must not be empty");
            }
        }

        public bool RemoveTarget(string title, string content)
        {
            if (title != null && content != null && title != string.Empty && content != string.Empty)
            {
                string usersPath = Path.Combine(User.Username, "targets.txt");
                StreamReader reader = new StreamReader(usersPath);
                List<string> dict = new List<string>();
                while (!reader.EndOfStream)
                {
                    string local = reader.ReadLine();
                    if (!local.Equals(""))
                    {
                        List<string> checkList = local.Split(';').ToList();
                        dict.Add(checkList.ElementAt(0) + ";" + checkList.ElementAt(1) + "\n");
                        if (checkList.ElementAt(0).Equals(title))
                        {
                            if (checkList.ElementAt(1).Equals(content))
                            {
                                dict.Remove(checkList.ElementAt(0) + ";" + checkList.ElementAt(1) + "\n");
                            }
                        }
                    }
                    
                }
                reader.Close();
                
                string value = "";
                int count = 0;
                while(count < dict.Count)
                {
                    value += dict.ElementAt(count);
                    count++;
                }
                Console.WriteLine(value);
                
                
                using (StreamWriter writer = new StreamWriter(usersPath, false))
                {
                    writer.WriteLine(value);
                }
                return true;
            }
            return false;
        }

        public void ClearTargets()
        {
            if (User != null)
            {
                string path = Path.Combine(User.Username, "targets.txt");
                File.WriteAllText(path, "");
            }
        }

        public bool CreateFileIfNotExists(string filename, string extension)
        {
            string filenameWithExtension = filename + extension;
            string path = Path.Combine(User.Username, filenameWithExtension);
            if (!File.Exists(path))
            {
                var myFile = File.Create(path);
                myFile.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OneTimeLoadRecords(MainWindowView mw)
        {
            CallerMainWindow = mw;
            string usersPath = Path.Combine(User.Username, "records.ser");
            Stream stream = File.Open(usersPath, FileMode.Open);
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            CallerMainWindow.Dict = (Dictionary<string, double>)binaryFormatter.Deserialize(stream);
        }

        public void LoadRecords(MainWindowView mw)
        {
            if (mw != null)
            {
                CallerMainWindow = mw;
                ListBox box = mw.Start_Listbox;
                Dictionary<string, double> localDic = mw.Dict;
                foreach (var e in localDic)
                {
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    stack.Children.Add(new Label
                    {
                        Content = e.Key + ": "
                    });
                    stack.Children.Add(new Label
                    {
                        Content = DoubleToTime(e.Value)
                    });
                    box.Items.Add(stack);
                }
            }
        }

        public void OneTimeLoadBlacklist()
        {
            string usersPath = Path.Combine(User.Username, "blacklist.ser");
            Stream stream = File.Open(usersPath, FileMode.Open);
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            CallerMainWindow.Blacklist = (List<string>)binaryFormatter.Deserialize(stream);
        }

        public void SaveRecords()
        {
            using (Stream stream = File.Create(Path.Combine(User.Username, "records.ser")))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, CallerMainWindow.Dict);
            }
        }

        public void SaveBlacklist()
        {
            using (Stream stream = File.Create(Path.Combine(User.Username, "blacklist.ser")))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, CallerMainWindow.Blacklist);
            }
        }

        public void AutoUpdate()
        {
            CallerMainWindow.Start_TotalTimeAmount.Content = CalcTotalTime();
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += OnLoadRecords;
            timer.Interval = new TimeSpan(30000000);
            timer.Start();
        }

        private void OnLoadRecords(object sender, EventArgs e)
        {
            CallerMainWindow.Start_Listbox.Items.Clear();
            LoadRecords(CallerMainWindow);
            ((MainWindowViewModel)CallerMainWindow.DataContext).Transform();
            CallerMainWindow.Start_TotalTimeAmount.Content = CalcTotalTime();
            Console.WriteLine("ListBox, Totalamount & Chart should update");
        }

        public string CalcTotalTime()
        {
            MainWindowViewModel mw = (MainWindowViewModel)CallerMainWindow.DataContext;
            double amount = 0;
            foreach (var e in CallerMainWindow.Dict)
            {
                amount += e.Value;
            }
            return DoubleToTime(amount);
        }

        public string DoubleToTime(double amount)
        {
            TimeSpan t = TimeSpan.FromSeconds(amount);
            string answer = string.Format("{0}h {1}m {2}s", t.Hours, t.Minutes, t.Seconds);
            return answer;
        }
    }
}
