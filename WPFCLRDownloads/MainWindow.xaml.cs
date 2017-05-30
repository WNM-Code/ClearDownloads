using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLRDownloads;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using Hardcodet.Wpf.TaskbarNotification;
using WPFCLRDownloads.Properties;

namespace WPFCLRDownloads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Remover r;
        TaskbarIcon icon;
        bool minimized;
        public MainWindow()
        {
            InitializeComponent();
            icon = Icon;
            icon.Icon = Properties.Resources.eraser_PNF_icon;
            icon.ToolTipText = "Clear Downloads";
            icon.Visibility = Visibility.Hidden;
            minimized = false;
            r = new Remover();
            r.run(LogsPane, RunButton);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }

        private void RunClick(object sender, RoutedEventArgs e)
        {
            if (r.getRunning())
            {
                r.setRunning(false);
                ((Button)sender).Content = "Run";
            }
            else
            {
                bool repeats = false;
                foreach (var v in r.getDayEnabled())
                {
                    if (v.Value == 1)
                    {
                        repeats = true;
                        break;
                    }
                }
                if (repeats)
                {
                    r.setRunning(true);
                }
                else
                {
                    r.runNow(LogsPane, RunButton);
                }
                ((Button)sender).Content = "Stop";
            }
            
        }
        private void LocationClick(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            try
            {
                ((Button)sender).Content = dialog.FileName;
                r.setLocation(dialog.FileName);
            }
            catch
            {
                ((Button)sender).Content = "Set Clear Location";
                r.setLocation("null");
            }
        }


        private void TextFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextNoFocus(object sender, RoutedEventArgs e)
        {
            string temp = MakeFive(((TextBox)sender).Text);
            r.setRepeatTime(((TextBox)sender).Text);
            ((TextBox)sender).Text = fixTime(temp);

        }

        private void grid1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Focus();
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                RunButton.Content = "Run";
                r.setRunning(false);
            }
            catch (NullReferenceException)
            {

            }
           
            string text = ((TextBox)sender).Text;
            int len = text.Length;
            if (len > 0)
            {
                text = CleanColon(text);
                text = CleanLetter(text);
                len = text.Length;
                
                if(len >= 2)
                {
                    if(len != 2)
                    {
                        text = text.Insert(2, ":");
                        len = text.Length;
                    }
                    
                    if (len > 5)
                    {
                        text = text.Substring(0, 5);
                        len = 5;
                    }
                }
                ((TextBox)sender).Text = text;
                len = text.Length;
                ((TextBox)sender).SelectionStart = len;
                ((TextBox)sender).SelectionLength = 0;
            }
        }

            private string CleanColon(string s)
        {
            while (s.Contains(":"))
            {
                s = s.Replace(":", "");
            }
            return s;
        }

        private string CleanLetter(string s)
        {
            return Regex.Replace(s, "[A-Za-z ~!@#$%^&*()_+=-`{}<>,.:\";\']", "");
        }

        private string MakeFive(string s)
        {
            while (s.Length < 5)
            {
                s = s + "0";
            }
            return s;
        }

        private void Handle(CheckBox checkbox)
        {
            if ((bool)checkbox.IsChecked)
            {
                CLRDownloads.Remover.setDayEnabled(checkbox.Name, 1);
            }
            else
            {
                CLRDownloads.Remover.setDayEnabled(checkbox.Name, 0);
            }
            
        }

        private void CorR_Click(object sender, RoutedEventArgs e)
        {
            if (r.getCorR())
            {
                r.setCorR(false);
                ((Button)sender).Content = "Archiving";
                if (r.getRemArchive())
                {
                    RemArchive.Content = "Archive Archived Folders";

                }
            }
            else
            {
                r.setCorR(true);
                ((Button)sender).Content = "Recycling";
                if (r.getRemArchive())
                {
                    RemArchive.Content = "Recycle Archived Folders";

                }
            }
        }

        private string fixTime(string s)
        {
            int hour = Int32.Parse(s.Substring(0, 2));
            int min = Int32.Parse(s.Substring(3, 2));
            if(hour > 23)
            {
                hour = 23;
            }
            if(min> 59)
            {
                min = 59;
            }
            return hour + ":" + min;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RemArchive_Click(object sender, RoutedEventArgs e)
        {
            if (r.getRemArchive())
            {
                r.setRemArchive(false);
                ((Button)sender).Content = "Avoid Archived Folders";
            }
            else
            {
                r.setRemArchive(true);
                if (r.getCorR())
                {
                    ((Button)sender).Content = "Recycle Archived Folders";
                }
                else
                {
                    ((Button)sender).Content = "Archive Archived Folders";
                }
                
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                this.Hide();
                icon.Visibility = Visibility.Visible;
            }
            base.OnStateChanged(e);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            icon.Visibility = Visibility.Hidden;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
