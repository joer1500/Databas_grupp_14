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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;


namespace Uppgift6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()

        {
            InitializeComponent();
            GetAllGuardians();
           
        }
        List<Guardian> guardians = new List<Guardian>();
        public static string SetValueForList = "";

        public void GetListValue()
         {

         }

        private void btnStaff_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren win = new StaffChildren();
            win.Show();
            this.Close();
        }

        private void BtnGuardian_Click(object sender, RoutedEventArgs e)
        {
            //orginal
            //Guardian guardian = (Guardian)listViewGuardians.SelectedItem;
            //SetValueForList = guardian.id.ToString();


            Guardian guardian = (Guardian)comboBoxGuardians.SelectedItem;
            if (guardian == null)
            {
                return;
            }
            SetValueForList = guardian.id.ToString();

            Guardians Guardiandwin = new Guardians();
            Guardiandwin.Show();
            this.Close();           
        }

        private void GetAllGuardians()
        {
            DbOperations db = new DbOperations();
            guardians = db.GetAllGuardians();

            //Till listan
            listViewGuardians.ItemsSource = null;
            listViewGuardians.ItemsSource = guardians;
            
            //Till combobox
            comboBoxGuardians.ItemsSource = guardians;
        }
       
    }
}
