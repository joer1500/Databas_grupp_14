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
            textBlockSectionInfo.Visibility = Visibility.Collapsed;
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
            textBoxSection.Text = staff.sectionid.ToString();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();

            int staffid = int.Parse(textBoxID.Text);
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string prof = textBoxProfession.Text;

            if (CheckInput(textBoxSection.Text) == false) //För att kolla om "avdelning" innehåller annat än siffror
            {
                labelSectionError.Content = "*Endast siffror 1-4 i avdelning";
                return;
            }
            else
            {
                int sectionid = int.Parse(textBoxSection.Text);

                try
                {
                    if (fname == "" || lname == "" || prof == "")
                    {
                        MessageBox.Show("Vänligen ange både förnamn, efternamn och roll.");
                        return;
                    }
                    else if (sectionid == 0 || sectionid > 4)
                    {
                        MessageBox.Show($"Felaktigt avdelnings-id. Ange en avdelning med sifforna 1-4.");
                        return;
                    }
                    else
                    {
                        db.UpdateStaff(staffid, fname, lname, prof, sectionid);
                        Staffwindow win = new Staffwindow();
                        EmptyTextBoxes();
                        MessageBox.Show($"Personalregistret uppdaterat", "Lyckad inmatning");

                        win.Show();
                        this.Close();
                    }

                }
                catch (PostgresException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
                        
        }

        private void EmptyTextBoxes()
        {
            textBoxID.Text = "";
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxProfession.Text = "";
            textBoxSection.Text = "";

        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }

        private void Icon_Question_mark_svg_png_MouseEnter(object sender, MouseEventArgs e)
        {
            textBlockSectionInfo.Visibility = Visibility.Visible;
        }

        private void Icon_Question_mark_svg_png_MouseLeave(object sender, MouseEventArgs e)
        {
            textBlockSectionInfo.Visibility = Visibility.Collapsed;
        }

        private void labelSection_MouseEnter(object sender, MouseEventArgs e)
        {
            textBlockSectionInfo.Visibility = Visibility.Visible;
        }

        private void labelSection_MouseLeave(object sender, MouseEventArgs e)
        {
            textBlockSectionInfo.Visibility = Visibility.Collapsed;
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
    }
}
