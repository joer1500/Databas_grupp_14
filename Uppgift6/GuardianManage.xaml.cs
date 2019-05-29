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
    /// Interaction logic for GuardianManage.xaml
    /// </summary>
    public partial class GuardianManage : Window
    {
        public GuardianManage()
        {
            InitializeComponent();
            UpdateListView();
        }

        Guardian selectedGuardian;
        public static int selectedGuardianID;

        public void UpdateListView()
        {
            DbOperations db = new DbOperations();

            listViewGuardians.ItemsSource = null;
            listViewGuardians.ItemsSource = db.GetAllGuardians();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren s = new StaffChildren();
            s.Show();
            this.Close();
        }

        private void listViewGuardians_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGuardian = (Guardian)listViewGuardians.SelectedItem;
        }

        private void listViewGuardians_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedGuardian = (Guardian)listViewGuardians.SelectedItem;
            if (selectedGuardian == null)
            {
                return;
            }
            else
            {
                MessageBox.Show($"Förnamn: {selectedGuardian.firstname}\rEfternamn: {selectedGuardian.lastname}\rTelefonnummer: {selectedGuardian.phonenumber}\rAdress: {selectedGuardian.address}\r", "Information om vårdnadshavare");
            }
        }

        private void buttonAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            NewGuardian n = new NewGuardian();
            n.Show();
        }

        private void buttonDeleteGuardian_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            selectedGuardian = (Guardian)listViewGuardians.SelectedItem;

            if (selectedGuardian == null)
            {
                return;
            }
            else
            {
                //int guardianID = selectedGuardian.id;

                try
                {
                    if (MessageBox.Show($"Vill du verkligen ta bort: {selectedGuardian.firstname} {selectedGuardian.lastname} från registret?\rObservera att denna åtgärd inte kan ångras.", "Varning!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        db.DeleteGuardian(selectedGuardian.id);
                    }
                    else
                    {
                        return;
                    }
                    UpdateListView();

                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonUpdateGuardian_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            selectedGuardian = (Guardian)listViewGuardians.SelectedItem;

            if (selectedGuardian == null)
            {
                MessageBox.Show("Vänligen markera en vårdnadshavare i listan.");
                return;
            }
            else
            {
                try
                {
                    selectedGuardianID = selectedGuardian.id;
                    UpdateGuardian win = new UpdateGuardian();
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
}
