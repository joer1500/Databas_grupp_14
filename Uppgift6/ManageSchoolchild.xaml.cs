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
    /// Interaction logic for ManageSchoolchild.xaml
    /// </summary>
    public partial class ManageSchoolchild : Window
    {
        public ManageSchoolchild()
        {
            InitializeComponent();
            UpdateListview();
        }


        private void UpdateListview()
        {
            DbOperations db = new DbOperations();

            if (rbtnSortByID.IsChecked == true)
            {
                listViewSchoolchildren.ItemsSource = null;
                listViewSchoolchildren.ItemsSource = db.GetSchoolchildrenOrderByID();
            }

            else if (rbtnSortByFirstname.IsChecked == true)
            {
                listViewSchoolchildren.ItemsSource = null;
                listViewSchoolchildren.ItemsSource = db.GetSchoolchildrenOrderByFirstname();
            }

            else if (rbtnSortByLastname.IsChecked == true)
            {
                listViewSchoolchildren.ItemsSource = null;
                listViewSchoolchildren.ItemsSource = db.GetSchoolchildrenOrderByLastname();
            }

            else if (rbtnSortBySection.IsChecked == true)
            {
                listViewSchoolchildren.ItemsSource = null;
                listViewSchoolchildren.ItemsSource = db.GetSchoolchildrenOrderBySection();
            }
        }

        private void rbtnSortByID_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListview();
        }

        private void rbtnSortByFirstname_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListview();
        }

        private void rbtnSortByLastname_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListview();
        }

        private void rbtnSortBySection_Checked(object sender, RoutedEventArgs e)
        {
            UpdateListview();
        }

        private void btnAddChild_Click(object sender, RoutedEventArgs e)
        {
            NewSchoolchild win = new NewSchoolchild();
            win.Show();
            this.Close();
        }

        private void btnCloseManageSchoolchild_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren win = new StaffChildren();
            win.Show();
            this.Close();
        }
    }
}
