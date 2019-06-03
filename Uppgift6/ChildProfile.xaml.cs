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
    /// Interaction logic for ChildProfile.xaml
    /// </summary>
    public partial class ChildProfile : Window
    {
        public ChildProfile()
        {
            InitializeComponent();
        }

        private void btnCloseChildProfile_Click_1(object sender, RoutedEventArgs e)
        {
            StaffChildren m = new StaffChildren();
            m.Show();
            this.Close();
        }

        // Schedule sd;

        /*sd = (Schedule)listViewSC.SelectedItem;
            List<Guardian> guardian = new List<Guardian>();
            guardian = db.GetGuardianFromSchoolchildID(sd.schoolchild_id);
            var message = string.Join(Environment.NewLine, guardian);

            if (guardian == null)
            {
                return;
            }
            else
            {
                MessageBox.Show(message);
            }*/
    }
}
