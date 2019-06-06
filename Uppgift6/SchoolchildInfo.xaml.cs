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
    /// Interaction logic for SchoolchildInfo.xaml
    /// </summary>
    public partial class SchoolchildInfo : Window
    {
        public SchoolchildInfo()
        {
            InitializeComponent();
            ShowSchoolchildName();
            ShowSchoolchildSchedule();
            ShowGuardians();
            ShowSchoolchildNeeds();
            ShowAllowedPickups();
        }

        DbOperations db = new DbOperations();
        
        int id = ManageSchoolchild.selectedSchooolchildID;

        public void ShowSchoolchildName()
        {
            Schoolchild schoolchild = new Schoolchild();
            schoolchild = db.GetSchoolchildByID(id);
            lblSCName.Content = $"{schoolchild.firstname} {schoolchild.lastname}";
                        
        }

        public void ShowGuardians()
        {
            List<Guardian> guardian = new List<Guardian>();
            guardian = db.GetGuardianFromSchoolchildID(id);
            var vh = string.Join(Environment.NewLine, guardian);
            lblSeeGuardians.Content = vh;
        }

        public void ShowAllowedPickups()
        {
            List<Pickup> pickups = new List<Pickup>();
            pickups = db.GetAllAllowedPickupBySchoolchildID(id);
            var p = string.Join(Environment.NewLine, pickups);
            lblSeePickups.Content = p;
        }

        public void ShowSchoolchildNeeds()
        {
            List<Needs> needs = new List<Needs>();
            needs = db.GetNeedsFromSchoolchildID(id);
            var n = string.Join(Environment.NewLine, needs);
            lblSeeNeeds.Content = n;
        }

        public void ShowSchoolchildSchedule()
        {
            List<Schedule> schedules = new List<Schedule>();
            schedules = db.GetChildScheduleDatesFromChildID(id);
            listViewSchoolchild.ItemsSource = null;
            listViewSchoolchild.ItemsSource = schedules;
        }

        public void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ManageSchoolchild win = new ManageSchoolchild();
            win.Show();
            this.Close();
        }
    }
}
