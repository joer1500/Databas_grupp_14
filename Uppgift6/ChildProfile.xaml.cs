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

            GetGuardians();
            GetChildName();
            GetSchedule();
            GetNeeds();
            GetPickup();
        }

        //Sätter valt barn till variabeln id
        int id = int.Parse(StaffChildren.SetValueForList);


        #region Läser in vid öppning

        public void GetGuardians(){
            //Läser in Guardians för valt barn
            List<Guardian> guardian = new List<Guardian>();
            guardian = db.GetGuardianFromSchoolchildID(id);
            var vh = string.Join(Environment.NewLine, guardian);
            label_vardnadshavare.Content = vh;
        }

        public void GetChildName() {
            //Läser in barnets namn här 
            Schoolchild schoolchild = new Schoolchild();
            schoolchild = db.GetSchoolchildByID(id);
            string sc = schoolchild.firstname + " " + schoolchild.lastname;
            label_childname.Content = sc;
        }

        public void GetSchedule() {
            //Läs in barnets scheman
            List<Schedule> schedule = new List<Schedule>();
            schedule = db.GetChildScheduleDatesFromChildID(id);
            listViewSC.ItemsSource = schedule;
        }

        public void GetNeeds (){
            //Läs in barnets behov
            List<Needs> needs = new List<Needs>();
            needs = db.GetNeedsFromSchoolchildID(id);
            var n = string.Join(Environment.NewLine, needs);
            label_needs.Content = n;
        }

        private void GetPickup() {
            //Läs in barnets godkända hämtare
            List<Pickup> pickup = new List<Pickup>();
            pickup = db.GetAllAllowedPickupBySchoolchildID(id);
            var p = string.Join(Environment.NewLine, pickup);
            label_pickup.Content = p;
        }

        #endregion


        private void btnCloseChildProfile_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren m = new StaffChildren();
            m.Show();
            this.Close();
        }
    }
}
