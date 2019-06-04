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
    /// Interaction logic for NeedBySchoolchild.xaml
    /// </summary>
    public partial class NeedBySchoolchild : Window
    {
        public NeedBySchoolchild()
        {
            InitializeComponent();
            UpdateNeeds();
        }
        DbOperations db = new DbOperations();
        List<Needs> lista = new List<Needs>();
        private void UpdateNeeds()
        {
            
            lista = db.GetNeedsFromSchoolchildID(Guardians.selectedSchoolchildID);
            listViewNeeds.ItemsSource = null;
            listViewNeeds.ItemsSource = lista;
            var n = string.Join(Environment.NewLine, lista);
            label_needs.Content = lista;
        }

        

        private void btnCloseNeeds_Click(object sender, RoutedEventArgs e)
        {
            Guardians m = new Guardians();
            m.Show();
            this.Close();
        }
    }
}
