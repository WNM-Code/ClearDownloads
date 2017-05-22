using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLRDownloads;
using System.Text.RegularExpressions;

namespace WPFCLRDownloads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Remover r;
        public MainWindow()
        {
            InitializeComponent();
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

        private void TextFocus(object sender, RoutedEventArgs e)
        {
            //((TextBox)sender).Text = "";
        }

        private void TextNoFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = MakeFive(((TextBox)sender).Text);
            r.setRepeatTime(((TextBox)sender).Text);
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

                Console.WriteLine(text);
                
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
            }
            else
            {
                r.setCorR(true);
                ((Button)sender).Content = "Recycling";
            }
        }
    }
}
