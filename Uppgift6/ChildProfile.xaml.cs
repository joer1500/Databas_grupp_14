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
        DbOperations db = new DbOperations();

        public ChildProfile()
        {
            InitializeComponent();

            //Läser in Guardians för valt barn
            List<Guardian> guardian = new List<Guardian>();
            guardian = db.GetGuardianFromSchoolchildID(1);
            var vh = string.Join(Environment.NewLine, guardian);
            label_vardnadshavare.Content = vh;

            // Uppdatera barnets namn här label_childname.Content = 

            //Läs in barnets schema
            List<Schedule> schedule = new List<Schedule>();
            schedule = db.GetChildScheduleDatesFromChildID(1);
            listViewSC.ItemsSource = schedule;

        }

        private void btnCloseChildProfile_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren m = new StaffChildren();
            m.Show();
            this.Close();
        }
    }
}
