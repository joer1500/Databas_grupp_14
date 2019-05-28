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
    /// Interaction logic for StaffChildren.xaml
    /// </summary>
    public partial class StaffChildren : Window
    {
        public StaffChildren()
        {
            InitializeComponent();
            UpdateListView();
        }

        public void UpdateListView()
        {
            DbOperations db = new DbOperations();

            if (rbtnSortByLastname.IsChecked == true)   // Sortera på efternamn
            {
                listViewSC.ItemsSource = null;
                listViewSC.ItemsSource = db.GetSchoolchildrenOrderByLastname();
            }

            else if (rbtnSortByFirstname.IsChecked == true)  // Sortera på förnamn
            {
                listViewSC.ItemsSource = null;
                listViewSC.ItemsSource = db.GetSchoolchildrenOrderByFirstname();
            }

            else if (rbtnSortBySection.IsChecked == true)   // Sortera på avdelning
            {
                listViewSC.ItemsSource = null;
                listViewSC.ItemsSource = db.GetSchoolchildrenOrderBySection();
            }

           
        }

        private void btnHandleStaff_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }

        private void rbtnSortByLastname_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }

        private void rbtnSortByFirstname_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }

        private void rbtnSortBySection_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
    }

}
