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
        public static int selectedStaffID;
        string orderby = "";

        public void UpdateListView()
        {
            DbOperations db = new DbOperations();

            if (radioButtonID.IsChecked == true)
            {
                //orderby = "staff_id";
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
            else if(radioButtonFname.IsChecked == true)
            {
                //orderby = "firstname";
                try
                {
                    listViewStaffs.ItemsSource = null;
                    listViewStaffs.ItemsSource = db.GetAllStaffOrderByFirstname();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }       
            else if (radioButtonLname.IsChecked == true)
            {
                //orderby = "lastname";
                try
                {
                    listViewStaffs.ItemsSource = null;
                    listViewStaffs.ItemsSource = db.GetAllStaffOrderByLastname();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }


       

        private void listViewStaffs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStaff = (Staff)listViewStaffs.SelectedItem;
            
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

        private void btnUpdateStaff_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            selectedStaff = (Staff)listViewStaffs.SelectedItem;

            if (selectedStaff == null)
            {
                return;
            }
            else
            {
                try
                {
                    selectedStaffID = selectedStaff.staffID;
                    UpdateStaff win = new UpdateStaff();
                    win.Show();
                    this.Close();
                }
                catch (PostgresException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }                     
        }

        private void buttonDeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            selectedStaff = (Staff)listViewStaffs.SelectedItem;

            if (selectedStaff == null)
            {
                return;
            }
            else
            {
                int id = selectedStaff.staffID;
                
                try
                {
                    if (MessageBox.Show($"Vill du verkligen ta bort: {selectedStaff.firstname} {selectedStaff.lastname} från personalregistret?", "Varning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        db.DeleteStaff(selectedStaff.staffID);
                    }
                    else
                    {
                        return;
                    }
                    UpdateListView();

                }
                catch (PostgresException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
           

           

        }

        private void listViewStaffs_MouseDoubleClick(object sender, MouseButtonEventArgs e) //Dubbelklicka på en anställd för att få upp information
        {
            selectedStaff = (Staff)listViewStaffs.SelectedItem;

            MessageBox.Show($"Förnamn: {selectedStaff.firstname}\rEfternamn: {selectedStaff.lastname}\rRoll: {selectedStaff.lastname}\r Avdelning: {selectedStaff.section}", "Information om personal");          
        }

        private void radioButtonLname_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
        private void radioButtonID_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
        private void radioButtonFname_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
    }
}
