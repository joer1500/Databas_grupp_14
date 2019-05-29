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
using Npgsql;


namespace Uppgift6
{
    /// <summary>
    /// Interaction logic for Guardian.xaml
    /// </summary>
    public partial class Guardians : Window
    {
        public Guardians()
        {
            InitializeComponent();

            List<Schoolchild> children = new List<Schoolchild>();

            int id = int.Parse(MainWindow.SetValueForList);
            children = db.GetChildNameFromGuardianID(id);

            listBoxChildName.ItemsSource = null;
            listBoxChildName.ItemsSource = children;
        }

        DbOperations db = new DbOperations();
        Schoolchild schoolchild;


        private void BtnSaveNewSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxChildName.SelectedItem == null)
            {
                MessageBox.Show("Du måste välja ett barn i listan.");
            }

            else
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

                MessageBox.Show($"Ditt schema har lagts till för {schoolchild.firstname} den {textBoxDate.Text}.");
            }
        }

        private void btnCloseGuardian_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void listBoxChildName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                schoolchild = (Schoolchild)listBoxChildName.SelectedItem;
                label_child_schema.Content = schoolchild + "  schema";

                int schoolchild_id = 1; // = Hämta vald schoolchild ID här

                List<Schedule> schedule = new List<Schedule>();
                schedule = db.GetChildScheduleDatesFromChildID(schoolchild_id);

                listBox_ChildSchedule.ItemsSource = null;
                listBox_ChildSchedule.ItemsSource = schedule;
        }

    }
}
