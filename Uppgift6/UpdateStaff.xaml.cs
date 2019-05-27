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
    /// Interaction logic for UpdateStaff.xaml
    /// </summary>
    public partial class UpdateStaff : Window
    {
        public UpdateStaff()
        {
            InitializeComponent();
            UpdateStaffInfo();
        }


        public void UpdateStaffInfo()
        {
            DbOperations db = new DbOperations();
            Staff staff = new Staff();

            //int staffID = Staffwindow.selectedStaffID;
            staff = db.GetStaffByID(Staffwindow.selectedStaffID);

            textBoxID.Text = staff.staffID.ToString();
            textBoxFirstname.Text = staff.firstname;
            textBoxLastname.Text = staff.lastname;
            textBoxProfession.Text = staff.profession;
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            int id = Convert.ToInt32(textBoxID.Text);
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string prof = textBoxProfession.Text;

            try
            {
                db.UpdateStaff(id, fname, lname, prof);
                Staffwindow win = new Staffwindow();
                EmptyTextBoxes();
                MessageBox.Show($"Personalregistret uppdaterat");

                //win.UpdateListView();
                
                win.Show();
                this.Close();

            }
            catch (PostgresException ex)
            {

                MessageBox.Show(ex.Message);
            }    

            
        }

        private void EmptyTextBoxes()
        {
            textBoxID.Text = "";
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxProfession.Text = "";

        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }
    }
}
