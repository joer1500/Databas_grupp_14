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
using System.Windows.Shapes;

namespace Uppgift6
{
    /// <summary>
    /// Interaction logic for NewSCGuardian.xaml
    /// </summary>
    public partial class NewSCGuardian : Window
    {
        public NewSCGuardian()
        {
            InitializeComponent();
        }

        private void btnAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            string firstname = txtFirstname.Text;
            string lastname = txtLastname.Text;
            string phone = txtPhoneNr.Text;
            string address = txtAddress.Text;

            db.AddNewGuardian(firstname, lastname, phone, address);

            NewSchoolchild win = new NewSchoolchild();
            win.Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            NewSchoolchild win = new NewSchoolchild();
            win.Show();
            this.Close();
        }
    }
}
