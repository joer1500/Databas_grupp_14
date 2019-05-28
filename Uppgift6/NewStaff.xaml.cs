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
    /// Interaction logic for NewStaff.xaml
    /// </summary>
    public partial class NewStaff : Window
    {
        public NewStaff()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string profession = textBoxProfession.Text;

            if (textBoxSection.Text == "")
            {
                MessageBox.Show("Vänligen ange ett avdelnings-id");
                return;
                
            }

            int section = int.Parse(textBoxSection.Text);

            if (fname == null || lname == null || profession == null)
            {
                MessageBox.Show("Vänligen ange ett förnamn, efternamn, roll samt en avdelning");
                return;
            }

            try
            {
                db.AddNewStaff(fname, lname, profession, section);
                MessageBox.Show($"{fname} {lname} är nu tillagd i personalregistret", "Titel");
                EmptyTextboxes();

                Staffwindow win = new Staffwindow();
                win.Show();
                this.Close();
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private void EmptyTextboxes()
        {
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxProfession.Text = "";
            textBoxSection.Text = "";
        }

        private void textBoxSection_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxSection.Text = "";
        }

        private void Icon_Question_mark_svg_png_MouseEnter(object sender, MouseEventArgs e) //okklart om denna funkar
        {
            textBlockSectionInfo.IsEnabled = true;
        }

        private void Icon_Question_mark_svg_png_MouseLeave(object sender, MouseEventArgs e) //oklart om denna funkar
        {
            textBlockSectionInfo.IsEnabled = false;
        }
    }
}
