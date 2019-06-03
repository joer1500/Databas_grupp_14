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
    /// Interaction logic for Needs.xaml
    /// </summary>
    public partial class Needs : Window
    {
        public Needs()
        {
            InitializeComponent();
            UpdateNeeds();
        }

        DbOperations db = new DbOperations();



        private void UpdateNeeds() {

            List<Needs> needs = new List<Needs>();
            needs = db.GetNeedsFromSchoolchildID(Guardians.selectedSchoolchildID);
            var n = string.Join(Environment.NewLine, needs);
            label_needs.Content = n;
        }

        private void btnCloseNeeds_Click(object sender, RoutedEventArgs e)
        {
            Guardians m = new Guardians();
            m.Show();
            this.Close();
        }
    }
}
