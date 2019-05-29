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
    /// Interaction logic for NewSchoolchild.xaml
    /// </summary>
    public partial class NewSchoolchild : Window
    {
        public NewSchoolchild()
        {
            InitializeComponent();
            UpdateListview();
        }


        Guardian selectedGuardian;

        public void UpdateListview()
        {
            DbOperations db = new DbOperations();

            listViewGuardians.ItemsSource = null;
            listViewGuardians.ItemsSource = db.GetAllGuardians();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren win = new StaffChildren();
            win.Show();
            this.Close();
        }


        private void btnAddChild_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            string firstname = txtBoxFirstname.Text;
            string lastname = txtBoxLastname.Text;
            int section = int.Parse(txtBoxSection.Text);

            db.AddSchoolchild(firstname, lastname, section);
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
    }
}
