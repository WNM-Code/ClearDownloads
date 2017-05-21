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
using CLRDownloads;

namespace WPFCLRDownloads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, int> days = new Dictionary<string, int>
        {
            {"Monday"   , 0 },
            {"Tuesday"  , 1 },
            {"Wednesday", 2 },
            {"Thursday" , 3 },
            {"Friday"   , 4 },
            {"Saturday" , 5 },
            {"Sunday"   , 6 }
        };
        Remover r;
        public MainWindow()
        {
            InitializeComponent();
            r = new Remover();
            r.run();

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
                r.setRunning(true);
                ((Button)sender).Content = "Stop";
            }
            
        }

        private void Handle(CheckBox checkbox)
        {
            int day = days[checkbox.Name];
            if ((bool)checkbox.IsChecked)
            {
                CLRDownloads.Remover.setDayEnabled(day, 1);
            }
            else
            {
                CLRDownloads.Remover.setDayEnabled(day, 0);
            }
            
        }
    }
}
