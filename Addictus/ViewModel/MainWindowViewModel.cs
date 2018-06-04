using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Addictus.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindowView caller;
        private string target_title = "Title";
        private string target_content = "Content";
        private string target_add_btn = "Add";
        private string target_clear_btn = "Clear list";
        private string _start_totalTime_content = "Total time:";
        private string _start_addBlackListItemContent = "Add item to blacklist:";
        private ObservableCollection<KeyValuePair<string, double>> _chartRecords;
        public ObservableCollection<KeyValuePair<string, double>> ChartRecords
        {
            get
            {
                return _chartRecords;
            }
            set
            {
                _chartRecords = value;
                OnPropertyChanged(nameof(ChartRecords));
            }
        }

        public MainWindowViewModel(MainWindowView window)
        {
            caller = window;
        }

        public void Transform()
        {
            ChartRecords = new ObservableCollection<KeyValuePair<string, double>>();
            foreach (var e in caller.Dict)
            {
                ChartRecords.Add(KeyValuePair(e.Key, e.Value));
            }
        }

        private KeyValuePair<string, double> KeyValuePair(string s, double d)
        {
            return new KeyValuePair<string, double>(s, d);
        }

        private string _target_title_text;
        public string Target_title_text
        {
            get
            {
                return _target_title_text;
            }
            set
            {
                _target_title_text = value;
                OnPropertyChanged(Target_title_text);
            }
        }

        private string _target_content_text;
        public string Target_content_text
        {
            get
            {
                return _target_content_text;
            }
            set
            {
                _target_content_text = value;
                OnPropertyChanged(Target_content_text);
            }
        }

        private string _start_BlacklistItem_text;
        public string Start_BlacklistItem_text
        {
            get
            {
                return _start_BlacklistItem_text;
            }
            set
            {
                _start_BlacklistItem_text = value;
                OnPropertyChanged(Start_BlacklistItem_text);
            }
        }

        private RelayCommand addBlacklistItem;
        public ICommand AddBlacklistItem
        {
            get
            {
                if (addBlacklistItem == null)
                {
                    addBlacklistItem = new RelayCommand(
                        param => this.AddBlackListItemMethod(param),
                        param => true);
                }
                return addBlacklistItem;
            }
        }
        public void AddBlackListItemMethod(Object o)
        {
            if (o != null)
            {
                string text = ((TextBox)o).Text;
                if (!text.Equals("") && text != string.Empty)
                {
                    caller.Recording.AddToBlacklist(text);
                    ((TextBox)o).Clear();
                    ((TextBox)o).Focus();
                }
            }
        }

        private RelayCommand targetsAddTargetCommand;
        public ICommand TargetsAddTargetCommand
        {
            get
            {
                if(targetsAddTargetCommand == null)
                {
                    targetsAddTargetCommand = new RelayCommand(
                        param => this.TargetsAddTargetCommandMethod(),
                        param => true);
                }
                return targetsAddTargetCommand;
            }
        }
        public void TargetsAddTargetCommandMethod()
        {
            ManageDatabase manageDatabase = new ManageDatabase(caller.CurrentUser);
            manageDatabase.AddTarget(Target_title_text, Target_content_text);
            caller.Target_title.Clear();
            caller.Target_content.Clear();
            manageDatabase.LoadTargets(caller.Target_Stackpanel);
        }

        private RelayCommand targetsClearCommand;
        public ICommand TargetsClearCommand
        {
            get
            {
                if (targetsClearCommand == null)
                {
                    targetsClearCommand = new RelayCommand(
                        param => this.TargetsClearCommandMethod(),
                        param => true);
                }
                return targetsClearCommand;
            }
        }
        public void TargetsClearCommandMethod()
        {
            ManageDatabase manageDatabase = new ManageDatabase(caller.CurrentUser);
            manageDatabase.ClearTargets();
            manageDatabase.LoadTargets(caller.Target_Stackpanel);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Target_TitleContent
        {
            get
            {
                return target_title;
            }
        }

        public string Target_ContentContent
        {
            get
            {
                return target_content;
            }
        }

        public string Target_AddButtonContent
        {
            get
            {
                return target_add_btn;
            }
        }

        public string Target_ClearButtonContent
        {
            get
            {
                return target_clear_btn;
            }
        }

        public string Start_TotalTimeContent
        {
            get
            {
                return _start_totalTime_content;
            }
        }

        public string Start_AddBlackListItemContent
        {
            get
            {
                return _start_addBlackListItemContent;
            }
        }
    }
}
