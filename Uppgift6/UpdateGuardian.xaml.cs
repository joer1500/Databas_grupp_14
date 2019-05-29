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
    /// Interaction logic for UpdateGuardian.xaml
    /// </summary>
    public partial class UpdateGuardian : Window
    {
        public UpdateGuardian()
        {
            InitializeComponent();
            UpdateStaffInfo();
        }



        public void UpdateStaffInfo()
        {
            DbOperations db = new DbOperations();
            Guardian guardian = new Guardian();

            //int guardID = GuardianManage.selectedGuardianID;
            guardian = db.GetGuardianById(GuardianManage.selectedGuardianID);

            textBoxID.Text = guardian.id.ToString();
            textBoxFirstname.Text = guardian.firstname;
            textBoxLastname.Text = guardian.lastname;
            textBoxPhoneNumber.Text = guardian.phonenumber;
            textBoxAddress.Text = guardian.address;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            GuardianManage win = new GuardianManage();
            win.Show();
            this.Close();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fungerar ej just nu");
        }
    }
}
