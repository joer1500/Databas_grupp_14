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
using Npgsql;

namespace Uppgift6
{
    /// <summary>
    /// Interaction logic for NewGuardian.xaml
    /// </summary>
    public partial class NewGuardian : Window
    {
        public NewGuardian()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string phone = textBoxPhoneNumber.Text;
            string address = textBoxAddress.Text;


            if (fname == "" || lname == "")
            {
                MessageBox.Show("Vänligen fyll i ett fullständigt namn", "Varning");
                return;
            }
            else
            {
                try
                {
                    db.AddNewGuardian(fname, lname, phone, address);
                    MessageBox.Show($"{fname} {lname} är nu tillagd i registret för vårdnadshavare", "Lyckad inmatning");
                    EmptyTextboxes();
                    UpdateGuardiansList();
                    this.Close();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }         
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateGuardiansList()
        {
            GuardianManage gm = new GuardianManage();
            gm.UpdateListView();
        }

        private void EmptyTextboxes()
        {
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxPhoneNumber.Text = "";
            textBoxAddress.Text = "";
        }
    }
}
