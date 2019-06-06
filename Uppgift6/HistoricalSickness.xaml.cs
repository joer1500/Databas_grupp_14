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
    /// Interaction logic for HistoricalSickness.xaml
    /// </summary>
    public partial class HistoricalSickness : Window
    {
        public HistoricalSickness()
        {
            InitializeComponent();
            GetSchoolchild();
            UpdateSicknessList();
        }
        DbOperations db = new DbOperations();
        List<Attendance> attList = new List<Attendance>();
        Attendance selectedAttendance;
        Schoolchild child = new Schoolchild();
        Random slump = new Random();

        private void GetSchoolchild()
        {
            child = db.GetSchoolchildByID(Guardians.selectedSchoolchildID);
            labelChildName.Content = child.fullName;
        }
        private void UpdateSicknessList()
        {
            attList = db.GetSicknessFromSchoolchildID(Guardians.selectedSchoolchildID);
            listViewSickness.ItemsSource = null;
            listViewSickness.ItemsSource = attList;           
        }

        private void btnDeleteNeed_Click(object sender, RoutedEventArgs e)
        {
            string drop = "00:00:00";
            string pickup = "00:00:00";
            TimeSpan has_drop = TimeSpan.Parse(drop);
            TimeSpan has_pickup = TimeSpan.Parse(pickup);

            if (selectedAttendance == null)
            {
                return;
            }
            else
            {
            db.UpdateAttendance(Guardians.selectedSchoolchildID, selectedAttendance.date, "Nej", "", has_drop, has_pickup);
            UpdateSicknessList();
            }
        }

        private void listViewSickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAttendance = (Attendance)listViewSickness.SelectedItem;
        }

        private void btnCloseSickness_Click(object sender, RoutedEventArgs e)
        {
            Guardians win = new Guardians();
            win.Show();
            this.Close();
        }
    }
}
