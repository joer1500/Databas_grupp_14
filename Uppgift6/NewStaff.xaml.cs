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
    /// Interaction logic for NewStaff.xaml
    /// </summary>
    public partial class NewStaff : Window
    {
        public NewStaff()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string profession = textBoxProfession.Text;
            int section = int.Parse(textBoxSection.Text);

            try
            {
                db.AddNewStaff(fname, lname, profession, section);
                MessageBox.Show($"{fname} {lname} är nu tillagd i personalregistret");
                EmptyTextboxes();
                this.Close();
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private void EmptyTextboxes()
        {
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxProfession.Text = "";
            textBoxSection.Text = "";
        }

        private void textBoxSection_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxSection.Text = "";
        }

       
    }
}
