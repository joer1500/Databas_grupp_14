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
    /// Interaction logic for UpdateSchoolchild.xaml
    /// </summary>
    public partial class UpdateSchoolchild : Window
    {
        public UpdateSchoolchild()
        {
            InitializeComponent();
            UpdateSchoolchildInfo();
        }

        public void UpdateSchoolchildInfo()
        {
            Schoolchild schoolchild = new Schoolchild();
            
            DbOperations db = new DbOperations();
            schoolchild = db.GetSchoolchildByID(ManageSchoolchild.selectedSchooolchildID);

            txtID.Text = schoolchild.id.ToString();
            txtFirstname.Text = schoolchild.firstname;
            txtLastname.Text = schoolchild.lastname;
            txtSection.Text = schoolchild.sectionID.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();

            int id = int.Parse(txtID.Text);
            string firstname = txtFirstname.Text;
            string lastname = txtLastname.Text;

            if (CheckInput(txtSection.Text) == false) //För att kolla om "avdelning" innehåller annat än siffror
            {
                labelSectionError.Content = "*Endast siffror mellan 1-4 i avdelning";
                return;
            }
            else
            {
                int sectionID = int.Parse(txtSection.Text);

                if (sectionID > 4 || sectionID <= 0)
                {
                    labelSectionError.Content = "*Endast siffror mellan 1-4 i avdelning";
                }
                else
                {
                    try
                    {
                        db.UpdateSchoolchild(id, firstname, lastname, sectionID);

                        ManageSchoolchild win = new ManageSchoolchild();
                        win.Show();
                        this.Close();
                    }
                    catch (PostgresException ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }                
                
            }
            

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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ManageSchoolchild win = new ManageSchoolchild();
            win.Show();
            this.Close();
        }
    }
}
