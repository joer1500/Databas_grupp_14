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
            string firstname = txtFirstname.Text;
            string lastname = txtLastname.Text;
            string relation = txtRelation.Text;
            int schoolchildID = Guardians.selectedSchoolchildID;

            DbOperations db = new DbOperations();                      
            db.AddNewPickup(schoolchildID, firstname, lastname, relation);
            UpdateListviewPickups();
        }

        private void btnDeletePickup_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            Pickup selectedPickup = (Pickup)listViewPickups.SelectedItem;
            if (selectedPickup == null)
            {
                return;
            }
            else
            {
                int pickupID = selectedPickup.PickupID;

                if (selectedPickup == null)
                {
                    MessageBox.Show($"Vänligen välj en person i listan");
                }
                else if (MessageBox.Show($"Vill du verkligen ta bort: {selectedPickup.Firstname} {selectedPickup.Lastname} som godkänd hämtare?\rObservera att denna åtgärd inte kan ångras.", "Varning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.DeletePickup(pickupID);
                }
            }
         
            UpdateListviewPickups();
        }

        private void listViewPickups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pickup selectedPickup = (Pickup)listViewPickups.SelectedItem;                                 
        }
    }
}
