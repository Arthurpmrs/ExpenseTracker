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
using ExpenseTracker;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Profile myProfile { get; }

        bool IsTransactionsShowed { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new HomePage();
            myProfile = new Profile("Arthurpmrs");
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new HomePage();
        }
        private void History_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new History(myProfile);
        }
    }
}
