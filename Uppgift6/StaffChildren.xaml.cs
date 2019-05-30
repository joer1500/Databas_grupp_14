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
    /// Interaction logic for StaffChildren.xaml
    /// </summary>
    public partial class StaffChildren : Window
    {
        public StaffChildren()
        {
            InitializeComponent();
            GetSchedules();
            SortSchedulesByDate();
            EmptyListBoxAndFill();
            UpdateLabelView();
        }

        DbOperations db = new DbOperations();
        Schedule sd;
        List<Schedule> schedule = new List<Schedule>();
        DateTime choosenDate = DateTime.Today;


        private void SortSchedulesByDate()
        {
            schedule = schedule.Where(x => x.date == choosenDate).ToList();
        }

        private void GetSchedules() {
            schedule = db.GetSchoolchildrenSchedule();
        }

        private void EmptyListBoxAndFill()
        {
            listViewSC.ItemsSource = null;
            listViewSC.ItemsSource = schedule;
        }

        private void UpdateLabelView() {
            label_today.Content = choosenDate.ToString("dddd, dd MMMM yyyy");
        }

        


        #region Sortera på avdelning
        private void SortSchedulesBySectionBlue()
        {
            schedule = schedule.Where(x => x.section_id == 1).ToList();
        }

        private void SortSchedulesBySectionGreen()
        {
            schedule = schedule.Where(x => x.section_id == 2).ToList();
        }

        private void SortSchedulesBySectionYellow()
        {
            schedule = schedule.Where(x => x.section_id == 3).ToList();
        }

        private void SortSchedulesBySectionRed()
        {
            schedule = schedule.Where(x => x.section_id == 4).ToList();
        }

        #endregion


        #region Knappar
        private void button_minus_Click(object sender, RoutedEventArgs e)
        {
            choosenDate = choosenDate.AddDays(-1);
            UpdateLabelView();

            if (rbtn_all.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                EmptyListBoxAndFill();
            }

            else if (rbtn_blue.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionBlue();
                EmptyListBoxAndFill();
            }

            else if (rbtn_red.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionRed();
                EmptyListBoxAndFill();
            }

            else if (rbtn_green.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionGreen();
                EmptyListBoxAndFill();
            }

            else if (rbtn_yellow.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionYellow();
                EmptyListBoxAndFill();
            }
        }

        private void button_plus_Click(object sender, RoutedEventArgs e)
        {
            choosenDate = choosenDate.AddDays(1);
            UpdateLabelView();

            if (rbtn_all.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                EmptyListBoxAndFill();
            }

            else if (rbtn_blue.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionBlue();
                EmptyListBoxAndFill();
            }

            else if (rbtn_red.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionRed();
                EmptyListBoxAndFill();
            }

            else if (rbtn_green.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionGreen();
                EmptyListBoxAndFill();
            }

            else if (rbtn_yellow.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();
                SortSchedulesBySectionYellow();
                EmptyListBoxAndFill();
            }
        }

        private void btnAddGuardian_Click(object sender, RoutedEventArgs e)
        {
            GuardianManage m = new GuardianManage();
            m.Show();
            this.Close();
        }

        private void arrow_back_png_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btnCloseStaffChildren_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btnAddChild_Click(object sender, RoutedEventArgs e)
        {
            NewSchoolchild win = new NewSchoolchild();
            win.Show();
            this.Close();
        }

        private void btnHandleStaff_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }


        #endregion


        #region Radiobuttons
        private void rbtn_all_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();
            EmptyListBoxAndFill();
        }

        private void rbtn_blue_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();
            SortSchedulesBySectionBlue();
            EmptyListBoxAndFill();
        }

        private void rbtn_red_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();
            SortSchedulesBySectionRed();
            EmptyListBoxAndFill();
        }

        private void rbtn_yellow_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();
            SortSchedulesBySectionYellow();
            EmptyListBoxAndFill();
        }

        private void rbtn_green_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();
            SortSchedulesBySectionGreen();
            EmptyListBoxAndFill();
        }
        #endregion


        

        private void listViewSC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sd = (Schedule)listViewSC.SelectedItem;
            List<Guardian> guardian = new List<Guardian>();
            guardian = db.GetGuardianFromSchoolchildID(sd.schoolchild_id);
            var message = string.Join(Environment.NewLine, guardian);

            if (guardian == null)
            {
                return;
            }
            else
            {
                MessageBox.Show(message);
            }
        }
    }

}
