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
            ShowChildName();
        }
        DbOperations db = new DbOperations();
        List<Needs> lista = new List<Needs>();
        Needs selectedNeed;

        private void UpdateNeeds()
        {
            
            lista = db.GetNeedsFromSchoolchildID(Guardians.selectedSchoolchildID);
            listViewNeeds.ItemsSource = null;
            listViewNeeds.ItemsSource = lista;
            //var n = string.Join(Environment.NewLine, lista);
            //label_needs.Content = lista;
        }

        private void ShowChildName()
        {
            Schoolchild schoolchild;
            schoolchild = db.GetSchoolchildByID(Guardians.selectedSchoolchildID);
            lblChildName.Content = $"{schoolchild.fullName}";
        }

        private void btnCloseNeeds_Click(object sender, RoutedEventArgs e)
        {
            Guardians m = new Guardians();
            m.Show();
            this.Close();
        }

        private void btnAddNeed_Click(object sender, RoutedEventArgs e)
        {
            string need = txtNeed.Text;
            db.AddNeed(need, Guardians.selectedSchoolchildID);
            UpdateNeeds();
        }

        private void btnDeleteNeed_Click(object sender, RoutedEventArgs e)
        {
            if (selectedNeed == null)
            {
                return;
            }
            else
            {
            db.DeleteNeed(selectedNeed.id);
            UpdateNeeds();

            }
        }

        private void listViewNeeds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNeed = (Needs)listViewNeeds.SelectedItem;
        }
    }
}
