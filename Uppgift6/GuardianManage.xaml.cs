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
    }
}
