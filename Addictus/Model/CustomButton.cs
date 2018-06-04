using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Addictus.Model
{
    class CustomButton : Button
    {
        public string Title { get; set; }
        public string Task { get; set; }
        public Button CloseBtn { get; set; }
        public User User { get; set; }
        public StackPanel Stackpanel { get; set; }
        public TextBox Textbox { get; set; }

        public CustomButton(string title, string task)
        {
            Title = title;
            Task = task;
            Margin = new Thickness(0, 5, 0, 0);
            Height = 23;
            CloseBtn = new Button
            {
                Content = "X",
                Visibility = Visibility.Collapsed,
            };
            //Margin = new Thickness(1, 1, 1, 0);
            CloseBtn.Click += CloseBtn_Click;
            
            //Width = ((StackPanel)Parent).ActualWidth;
            Content = Title;

            //Create "Label"
            Textbox = new TextBox
            {
                Visibility = Visibility.Collapsed,
                TextWrapping = TextWrapping.Wrap,
                Text = Task,
                IsReadOnly = true,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            ManageDatabase md = new ManageDatabase(User);
            bool test = md.RemoveTarget(Title, Task);
            md.LoadTargets(Stackpanel);
            Console.WriteLine("CloseBtn_Click");
        }

        protected override void OnClick()
        {
            OnToggle();
        }

        public void OnToggle()
        {
            if (Textbox.Visibility == Visibility.Collapsed)
            {
                Textbox.Visibility = Visibility.Visible;
            }
            else
            {
                Textbox.Visibility = Visibility.Collapsed;
            }

            if (CloseBtn.Visibility == Visibility.Collapsed)
            {
                CloseBtn.Visibility = Visibility.Visible;
            }
            else
            {
                CloseBtn.Visibility = Visibility.Collapsed;
            }
        }

        public string ReturnTargetToRemove()
        {
            return Title + ";" + Task;
        }
    }
}
