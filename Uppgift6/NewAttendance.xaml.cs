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
    /// Interaction logic for NewAttendance.xaml
    /// </summary>
    public partial class NewAttendance : Window
    {
        public NewAttendance()
        {
            InitializeComponent();
            GetAllSchoolchilds();
            SetValues();
        }
        List<Schoolchild> sc = new List<Schoolchild>();
        DateTime choosenDate = DateTime.Today;

        private void GetAllSchoolchilds()
        {
            DbOperations db = new DbOperations();
            sc = db.GetSchoolchildrenOrderByLastname();

            //Till listan:
            //listViewGuardians.ItemsSource = null;
            //listViewGuardians.ItemsSource = guardians;

            //Till combobox:
            comboBoxSchoolchilds.ItemsSource = sc;
        }

        private void SetValues()
        {
            textBoxDate.Text = choosenDate.ToShortDateString();
            comboBoxBreakfast.Items.Add("Ja");
            comboBoxBreakfast.Items.Add("Nej");
        }
    }
}
