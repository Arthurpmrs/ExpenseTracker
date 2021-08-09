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
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : Page
    {
        Profile myProfile { get; }

        public History(Profile profile)
        {
            InitializeComponent();
            this.myProfile = profile;
            StartHistoryTable();
        }

        public void StartHistoryTable()
        {
            Account myAccount1 = myProfile.AddAccount("Poupança Caixa", "CEF");
            AccountDBWrapper wrapper = new AccountDBWrapper(myProfile.DB);
            List<Tuple<Transaction, Channel>> entrys = wrapper.GetAccountTransactions(myAccount1.AccountID);

            foreach (var entry in entrys)
            {
               TransactionsDataGrid.Items.Add(entry.Item1);
            }

        }
    }
}
