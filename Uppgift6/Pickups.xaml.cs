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

namespace Uppgift6
{
    /// <summary>
    /// Interaction logic for Pickups.xaml
    /// </summary>
    public partial class Pickups : Window
    {
        public Pickups()
        {
            InitializeComponent();
            UpdateAllowedPickupsTitle();
            UpdateListviewPickups();
        }

        private void UpdateListviewPickups()

        {
            DbOperations db = new DbOperations();
           
            
            List<Pickup> pickups = new List<Pickup>();
            pickups = db.GetAllAllowedPickupBySchoolchildID(Guardians.selectedSchoolchildID);

            listViewPickups.ItemsSource = null;
            listViewPickups.ItemsSource = pickups;
        }

        private void UpdateAllowedPickupsTitle ()
        {
            DbOperations db = new DbOperations();
            Schoolchild schoolchild = db.GetSchoolchildByID(Guardians.selectedSchoolchildID);

            lblAllowedPickups.Content = $"Personer som får hämta {schoolchild.firstname} {schoolchild.lastname}:";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Guardians win = new Guardians();
            win.Show();
            this.Close();
        }

        private void btnAddPickup_Click(object sender, RoutedEventArgs e)
        {
            
            UpdateListviewPickups();
        }
    }
}
