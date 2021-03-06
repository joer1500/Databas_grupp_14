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
    /// Interaction logic for NewSchoolchild.xaml
    /// </summary>
    public partial class NewSchoolchild : Window
    {
        public NewSchoolchild()
        {
            InitializeComponent();
            UpdateGuardianListview();
            UpdateSchoolchildListview();
            txtBlckSectionInfo.Visibility = Visibility.Collapsed;
        }


        Guardian selectedGuardian;

        public void UpdateGuardianListview()
        {
            DbOperations db = new DbOperations();

            listViewGuardians.ItemsSource = null;
            listViewGuardians.ItemsSource = db.GetAllGuardians();
        }

        private void UpdateSchoolchildListview()
        {
            DbOperations db = new DbOperations();
            listViewSchoolchild.ItemsSource = null;
            listViewSchoolchild.ItemsSource = db.GetSchoolchildrenOrderByID();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ManageSchoolchild win = new ManageSchoolchild();
            win.Show();
            this.Close();
        }


        private void btnAddChild_Click(object sender, RoutedEventArgs e)
        {
            
            DbOperations db = new DbOperations();
            string firstname = txtBoxFirstname.Text;
            string lastname = txtBoxLastname.Text;

            if (CheckInput(txtBoxSection.Text) == false) //För att kolla om "avdelning" innehåller annat än siffror
            {
                labelSectionError.Content = "*Endast siffror mellan 1-4 i avdelning";
                return;
            }
            else
            {
                int section = int.Parse(txtBoxSection.Text);
                try
                {
                    db.AddSchoolchild(firstname, lastname, section);
                    UpdateSchoolchildListview();
                    MessageBox.Show($"{firstname} {lastname} är nu tillagd i registret", "Lyckad inmatning");
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnNewGuardian_Click(object sender, RoutedEventArgs e)
        {
            NewSCGuardian win = new NewSCGuardian();
            win.Show();
            this.Close();
        }

        private void listViewGuardians_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGuardian = (Guardian)listViewGuardians.SelectedItem;
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            Schoolchild selectedSchoolchild = (Schoolchild)listViewSchoolchild.SelectedItem;
            Guardian selectedGuardian = (Guardian)listViewGuardians.SelectedItem;

            if (selectedSchoolchild == null)
            {
                MessageBox.Show($"Vänligen markera ett barn i listan");
            }
            else if (selectedGuardian == null)
            {
                MessageBox.Show($"Vänligen markera en vårdnadshavare i listan");
            }
            else if (MessageBox.Show($"Vill du koppla barnet {selectedSchoolchild.firstname} {selectedSchoolchild.lastname} till vårdnadshavaren {selectedGuardian.firstname} {selectedGuardian.lastname}? ", "Bekräfta koppling", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.ConnectGuardianSchoolchild(selectedSchoolchild.id, selectedGuardian.id);
                ManageSchoolchild win = new ManageSchoolchild();
                win.Show();
                this.Close();
            }
        }

        private void listViewSchoolchild_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Schoolchild selectedSchoolchild = (Schoolchild)listViewSchoolchild.SelectedItem;
        }

        private bool CheckInput(string s)
        {
            foreach (char c in s)
            {
                if (Char.IsLetter(c))
                    return false;
            }
            return true;
        }

        private void txtBoxSection_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBoxSection.Text = "";
        }

        private void Icon_Question_mark_svg_png_MouseEnter(object sender, MouseEventArgs e)
        {
            txtBlckSectionInfo.Visibility = Visibility.Visible;
        }

        private void Icon_Question_mark_svg_png_MouseLeave(object sender, MouseEventArgs e)
        {
            txtBlckSectionInfo.Visibility = Visibility.Collapsed;
        }

        private void txtBlckSectionInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            txtBlckSectionInfo.Visibility = Visibility.Visible;
        }

        private void txtBlckSectionInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            txtBlckSectionInfo.Visibility = Visibility.Collapsed;
        }
    }
}
