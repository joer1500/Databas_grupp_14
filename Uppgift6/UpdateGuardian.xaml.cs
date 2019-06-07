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
    /// Interaction logic for UpdateGuardian.xaml
    /// </summary>
    public partial class UpdateGuardian : Window
    {
        public UpdateGuardian()
        {
            InitializeComponent();
            UpdateStaffInfo();
            UpdateChildsList();
        }

        Schoolchild selectedSchoolchild;
        Schoolchild selectedExistedSchoolchild;
        DbOperations db = new DbOperations();
        List<Schoolchild> childsList = new List<Schoolchild>();
        int sectionid = 0;

        public void UpdateStaffInfo()
        {
            DbOperations db = new DbOperations();
            Guardian guardian = new Guardian();

            guardian = db.GetGuardianById(GuardianManage.selectedGuardianID);

            textBoxID.Text = guardian.id.ToString();
            textBoxFirstname.Text = guardian.firstname;
            textBoxLastname.Text = guardian.lastname;
            textBoxPhoneNumber.Text = guardian.phonenumber;
            textBoxAddress.Text = guardian.address;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            GuardianManage win = new GuardianManage();
            win.Show();
            this.Close();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            Guardian guardian = new Guardian();
            guardian = db.GetGuardianById(GuardianManage.selectedGuardianID);

            int guard = guardian.id;
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string phone = textBoxPhoneNumber.Text;
            string address = textBoxAddress.Text;

            try
            {
                if (fname == "" || lname == "")
                {
                    MessageBox.Show("Vänligen ange både förnamn och efternamn.");
                    return;
                }               
                else
                {
                    db.UpdateGuardian(guard, fname, lname, phone, address);
                    //EmptyTextBoxes();
                    MessageBox.Show($"Vårdnadshavare uppdaterad", "Lyckad inmatning");
                    //this.Close();
                    buttonExit.Content = "Tillbaka";
                }
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int GetGuardian()
        {
            Guardian guardian = new Guardian();
            
            //int guardID = GuardianManage.selectedGuardianID;
            guardian = db.GetGuardianById(GuardianManage.selectedGuardianID);

            return guardian.id;
        }

        private void UpdateChildsList()
        {
            //DbOperations db = new DbOperations();
            listViewChilds.ItemsSource = null;                
            listViewChilds.ItemsSource = db.GetChildNameFromGuardianID(int.Parse(textBoxID.Text));        
        }

        private void EmptyTextBoxes()
        {
            textBoxID.Text = "";
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxPhoneNumber.Text = "";
            textBoxAddress.Text = "";
        }

        private void buttonNewChild_Click(object sender, RoutedEventArgs e)
        {
            ShowAllChildsList();
            //UpdateAllChildsList();
        }


        private void ShowAllChildsList()
        {
            //comboBoxSections.Visibility = Visibility.Visible;
            listViewAllChilds.Visibility = Visibility.Visible;
            labelTitleAllChilds.Visibility = Visibility.Visible;
            buttonConnectChild.Visibility = Visibility.Visible;
            //rbtn_allChilds.Visibility = Visibility.Visible;
            //rbtn_red.Visibility = Visibility.Visible;
            //rbtn_green.Visibility = Visibility.Visible;
            //rbtn_blue.Visibility = Visibility.Visible;
            //rbtn_yellow.Visibility = Visibility.Visible;

        }

        private void UpdateAllChildsList()
        {
            DbOperations db = new DbOperations();
            childsList = db.GetSchoolchildrenOrderByID();

            try
            {
                if (rbtn_allChilds.IsChecked == true)
                {
                    listViewAllChilds.ItemsSource = null;
                    listViewAllChilds.ItemsSource = childsList;
                }
                else if (rbtn_blue.IsChecked == true)
                {
                    sectionid = 1;
                    listViewAllChilds.ItemsSource = null;
                    listViewAllChilds.ItemsSource = db.GetSchoolchildrenFromSelectedSection(sectionid);
                }
                else if (rbtn_yellow.IsChecked == true)
                {
                    sectionid = 2;
                    listViewAllChilds.ItemsSource = null;
                    listViewAllChilds.ItemsSource = db.GetSchoolchildrenFromSelectedSection(sectionid);
                }
                else if (rbtn_red.IsChecked == true)
                {
                    sectionid = 3;
                    listViewAllChilds.ItemsSource = null;
                    listViewAllChilds.ItemsSource = db.GetSchoolchildrenFromSelectedSection(sectionid);
                }
                else if (rbtn_green.IsChecked == true)
                {
                    sectionid = 4;
                    listViewAllChilds.ItemsSource = null;
                    listViewAllChilds.ItemsSource = db.GetSchoolchildrenFromSelectedSection(sectionid);
                }
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void buttonConnectChild_Click(object sender, RoutedEventArgs e)
        {
            selectedSchoolchild = (Schoolchild)listViewAllChilds.SelectedItem;
            ConnectChild(selectedSchoolchild);
        }

        private void comboBoxSections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ShowAllChildsList();
        }

        #region Radio buttons
        private void rbtn_allChilds_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAllChildsList();
        }

        private void rbtn_blue_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAllChildsList();        
        }

        private void rbtn_red_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAllChildsList();
        }

        private void rbtn_yellow_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAllChildsList();
        }

        private void rbtn_green_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAllChildsList();
        }
        #endregion

        private void buttonDeleteConnection_Click(object sender, RoutedEventArgs e)
        {
            DeleteConnectedChild();        
        }

        private void ConnectChild(Schoolchild s)
        {           
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;

            DbOperations db = new DbOperations();

            try
            {
                if (selectedSchoolchild == null)
                {
                    return;
                }
                else if (MessageBox.Show($"Vill du koppla barnet {s.firstname} {s.lastname} till vårdnadshavaren {fname} {lname}? ", "Bekräfta koppling", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.ConnectGuardianSchoolchild(s.id, int.Parse(textBoxID.Text));
                    UpdateChildsList();
                }
                else
                {
                    return;
                }
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteConnectedChild()
        {
            selectedExistedSchoolchild = (Schoolchild)listViewChilds.SelectedItem;
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;

            DbOperations db = new DbOperations();

            try
            {
                if (selectedExistedSchoolchild == null)
                {
                    return;
                }
                else if (MessageBox.Show($"Vill du ta bort kopplingen mellan barnet {selectedExistedSchoolchild.firstname} {selectedExistedSchoolchild.lastname} och vårdnadshavaren {fname} {lname}? ", "Bekräfta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.DeleteConnectionGuardianSchoolchild(selectedExistedSchoolchild.id, int.Parse(textBoxID.Text));
                    UpdateChildsList();
                }
                else
                {
                    return;
                }
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listViewChilds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedExistedSchoolchild = (Schoolchild)listViewChilds.SelectedItem;
            if (selectedExistedSchoolchild == null)
            {
                buttonDeleteConnection.IsEnabled = false;
            }
            else
            {
                buttonDeleteConnection.IsEnabled = true;
            }
        }

        private void listViewAllChilds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSchoolchild = (Schoolchild)listViewAllChilds.SelectedItem;
            if (selectedSchoolchild == null)
            {
                buttonConnectChild.IsEnabled = false;
            }
            else
            {
                buttonConnectChild.IsEnabled = true;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            GuardianManage win = new GuardianManage();
            win.Show();
            this.Close();
        }
    }
}
