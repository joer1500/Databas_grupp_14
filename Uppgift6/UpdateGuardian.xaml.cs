﻿using System;
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
            UpdateSchoolchildsList();
        }

        Schoolchild selectedSchoolchild;

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
            DbOperations db = new DbOperations();
            //int guardID = GuardianManage.selectedGuardianID;
            guardian = db.GetGuardianById(GuardianManage.selectedGuardianID);

            return guardian.id;
        }

        private void UpdateSchoolchildsList()
        {
            listViewChilds.ItemsSource = null;
            DbOperations db = new DbOperations();       
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
        }


        private void ShowAllChildsList()
        {
            listViewAllChilds.Visibility = Visibility.Visible;
            labelTitleAllChilds.Visibility = Visibility.Visible;
            buttonConnectChild.Visibility = Visibility.Visible;

            DbOperations db = new DbOperations();

            listViewAllChilds.ItemsSource = null;
            listViewAllChilds.ItemsSource = db.GetSchoolchildrenOrderByLastname();
        }

        private void buttonConnectChild_Click(object sender, RoutedEventArgs e)
        {
            selectedSchoolchild = (Schoolchild)listViewAllChilds.SelectedItem;
            //int sid = selectedSchoolchild.id;
            //int gid = int.Parse(textBoxID.Text);

            DbOperations db = new DbOperations();

            db.ConnectGuardianSchoolchild(selectedSchoolchild.id, int.Parse(textBoxID.Text));
            UpdateSchoolchildsList();
        }
    }
}
