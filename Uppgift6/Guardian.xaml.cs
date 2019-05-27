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
    /// Interaction logic for Guardian.xaml
    /// </summary>
    public partial class Guardian : Window
    {
        public Guardian()
        {
            InitializeComponent();
        }

        DbOperations db = new DbOperations();
        Schoolchild schoolchild;

        private void BtnSearchChild_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(textBoxID.Text);
            List<Schoolchild> children = new List <Schoolchild>();

            children = db.GetChildNameFromGuardianID(id);

            listBoxChildName.ItemsSource = null;
            listBoxChildName.ItemsSource = children;
        }

        private void BtnSaveNewSchedule_Click(object sender, RoutedEventArgs e)
        {
            schoolchild = (Schoolchild)listBoxChildName.SelectedItem;

            DateTime date = DateTime.Parse(textBoxDate.Text);
            string day_off = textBoxDay_of.Text.ToString();
            string breakfast = textBoxBreakfast.Text.ToString();
            DateTime should_drop = DateTime.Parse(textBoxShould_drop.Text);
            DateTime should_pickup = DateTime.Parse(textBoxShould_pickup.Text);
            string walk_home_alone = textBoxWalk_home_alone.Text.ToString();
            string walk_with_friend = textBoxHome_with_friend.Text.ToString();

            db.InsertSchedule(schoolchild, date, day_off, breakfast, should_drop, should_pickup, walk_home_alone, walk_with_friend);

            MessageBox.Show($"Ditt schema har lagts till för {schoolchild.firstname} den {textBoxDate.Text.ToString()}.");
        }
    }
}
