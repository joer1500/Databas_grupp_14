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
        Staff staff = new Staff();   
        List<Staff> staffList = new List<Staff>();


        //string orderby = "";
        //private void GetSchedules()
        //{
        //    schedule = db.GetSchoolchildrenSchedule();
        //}

        private void UpdateListView()
        {
            DbOperations db = new DbOperations();
            staffList = db.GetAllStaff();

            if (radioButtonID.IsChecked == true)
            {
                
                try
                {
                    listViewStaffs.ItemsSource = null;
                    //staffList = OrderListById();
                    //listViewStaffs.ItemsSource = db.GetAllStaff();
                    //staffList = OrderListById(staffList);
                    listViewStaffs.ItemsSource = OrderListById(staffList);
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(radioButtonFname.IsChecked == true)
            {               
                try
                {
                    listViewStaffs.ItemsSource = null;                  
                    listViewStaffs.ItemsSource = OrderListByFirstname(staffList);
                    //listViewStaffs.ItemsSource = db.GetAllStaffOrderByFirstname();
                }
                catch (PostgresException ex)    
                {
                    MessageBox.Show(ex.Message);
                }
            }       
            else if (radioButtonLname.IsChecked == true)
            {
               
                try
                {
                    listViewStaffs.ItemsSource = null;
                    listViewStaffs.ItemsSource = OrderListByLastname(staffList);
                    //listViewStaffs.ItemsSource = db.GetAllStaffOrderByLastname();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (radioButtonProfession.IsChecked == true)
            {
                try
                {
                    listViewStaffs.ItemsSource = null;
                    listViewStaffs.ItemsSource = OrderListByProffession(staffList);
                    //listViewStaffs.ItemsSource = db.GetAllStaffOrderByProfession();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if (radioButtonSection.IsChecked == true)
            {
                //orderby = "section.section_id";
                try
                {
                    listViewStaffs.ItemsSource = null;
                    listViewStaffs.ItemsSource = OrderListBySection(staffList);
                    //listViewStaffs.ItemsSource = db.GetAllStaffOrderBySection();
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
            StaffChildren m = new StaffChildren();
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
                MessageBox.Show("Vänligen markera en anställd i listan.");
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
                    if (MessageBox.Show($"Vill du verkligen ta bort: {selectedStaff.firstname} {selectedStaff.lastname} från anställningsregistret?\rObservera att denna åtgärd inte kan ångras.", "Varning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
            if (selectedStaff == null)
            {
                return;
            }
            else
            {
                MessageBox.Show($"Förnamn: {selectedStaff.firstname}\rEfternamn: {selectedStaff.lastname}\rRoll: {selectedStaff.profession}\rAvdelning: {selectedStaff.sectionname}\rAvdelnings-id: {selectedStaff.sectionid}", "Information om personal");
            }                     
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
        private void radioButtonProfession_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
        private void radioButtonSection_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
        private void arrow_back_png_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StaffChildren m = new StaffChildren();
            m.Show();
            this.Close();
        }

        private List<Staff> OrderListById(List<Staff> staff)
        {
            return staffList = staffList.OrderBy(s => s.staffID).ToList();            
        }
        private List<Staff> OrderListByFirstname(List<Staff> staff)
        {
            return staffList = staffList.OrderBy(s => s.firstname).ToList();
        }
        private List<Staff> OrderListByLastname(List<Staff> staff)
        {
            return staffList = staffList.OrderBy(s => s.lastname).ToList();
        }
        private List<Staff> OrderListByProffession(List<Staff> staff)
        {
            return staffList = staffList.OrderBy(s => s.profession).ToList();
        }
        private List<Staff> OrderListBySection(List<Staff> staff)
        {
            return staffList = staffList.OrderBy(s => s.sectionid).ToList();
        }
    }
}
