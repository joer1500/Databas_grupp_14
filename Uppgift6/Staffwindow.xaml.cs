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
    /// Interaction logic for Staffwindow.xaml
    /// </summary>
    public partial class Staffwindow : Window
    {
        public Staffwindow()
        {
            InitializeComponent();
            UpdateListView();
        }

        Staff selectedStaff;

        private void UpdateListView()
        {
            DbOperations db = new DbOperations();

            try
            {
                listViewStaffs.ItemsSource = null;
                listViewStaffs.ItemsSource = db.GetAllStaff();
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void UpdateTextboxes()
        {
            textBoxID.Text = selectedStaff.staffID.ToString();
            textBoxFirstName.Text = selectedStaff.firstname;
            textBoxLastName.Text = selectedStaff.lastname;
            textBoxProfession.Text = selectedStaff.profession;
        }

        private void listViewStaffs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStaff = (Staff)listViewStaffs.SelectedItem;
            UpdateTextboxes();
        }

        private void btnCloseStaff_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
            NewStaff win = new NewStaff();
            win.Show();
            this.Close();
        }
    }
}
