﻿using System;
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
            GetAttendance();
            FilterAttendancesByDate();
            UpdateAttendanceList();
            UpdateLabelView();
        }

        DbOperations db = new DbOperations();
        List<Schedule> schedule = new List<Schedule>();
        List<Attendance> attendances = new List<Attendance>();
        DateTime choosenDate = DateTime.Today;



        private void SortSchedulesByDate()
        {
            schedule = schedule.Where(x => x.date == choosenDate).ToList();
        }

        private void FilterAttendancesByDate()
        {
            attendances = attendances.Where(x => x.date == choosenDate).ToList();
        }

        private void GetSchedules()
        {
            schedule = db.GetSchoolchildrenSchedule();          
        }

        private void GetAttendance()
        {
            attendances = db.GetAttendances();
        }

        private void EmptyListBoxAndFill()
        {
            listViewSC.ItemsSource = null;
            listViewSC.ItemsSource = schedule;
        }

        private void UpdateAttendanceList()
        {
            if (listViewAttendance == null)
            {
                return;
            }

            foreach (var schoolchild in attendances)
            {
                if (schoolchild.attendance == "True")
                {
                    
                }
            }
            listViewAttendance.ItemsSource = null;
            listViewAttendance.ItemsSource = attendances;
        }

        private void UpdateLabelView() {
            label_today.Content = choosenDate.ToString("dddd, dd MMMM yyyy");
        }
       


        #region Sortera på avdelning
        private void SortSchedulesBySectionBlue()
        {
            schedule = schedule.Where(x => x.section_id == 1).ToList();
            attendances = attendances.Where(x => x.section_id == 1).ToList();
        }

        private void SortSchedulesBySectionGreen()
        {
            schedule = schedule.Where(x => x.section_id == 2).ToList();
            attendances = attendances.Where(x => x.section_id == 2).ToList();
        }

        private void SortSchedulesBySectionYellow()
        {
            schedule = schedule.Where(x => x.section_id == 3).ToList();
            attendances = attendances.Where(x => x.section_id == 3).ToList();
        }

        private void SortSchedulesBySectionRed()
        {
            schedule = schedule.Where(x => x.section_id == 4).ToList();
            attendances = attendances.Where(x => x.section_id == 4).ToList();
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

                GetAttendance();
                FilterAttendancesByDate();
                UpdateAttendanceList();
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

                GetAttendance();
                FilterAttendancesByDate();

                EmptyListBoxAndFill();
                UpdateAttendanceList();
            }

            else if (rbtn_blue.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();

                GetAttendance();
                FilterAttendancesByDate();

                SortSchedulesBySectionBlue();

                EmptyListBoxAndFill();
                UpdateAttendanceList();
            }

            else if (rbtn_red.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();

                GetAttendance();
                FilterAttendancesByDate();

                SortSchedulesBySectionRed();

                EmptyListBoxAndFill();
                UpdateAttendanceList();
            }

            else if (rbtn_green.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();

                GetAttendance();
                FilterAttendancesByDate();

                SortSchedulesBySectionGreen();

                EmptyListBoxAndFill();
                UpdateAttendanceList();
            }

            else if (rbtn_yellow.IsChecked == true)
            {
                GetSchedules();
                SortSchedulesByDate();

                GetAttendance();
                FilterAttendancesByDate();

                SortSchedulesBySectionYellow();

                EmptyListBoxAndFill();
                UpdateAttendanceList();
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
            ManageSchoolchild win = new ManageSchoolchild();
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

            GetAttendance();
            FilterAttendancesByDate();

            EmptyListBoxAndFill();
            UpdateAttendanceList();
        }

        private void rbtn_blue_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();

            GetAttendance();
            FilterAttendancesByDate();

            SortSchedulesBySectionBlue();

            EmptyListBoxAndFill();
            UpdateAttendanceList();
        }

        private void rbtn_red_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();

            GetAttendance();
            FilterAttendancesByDate();

            SortSchedulesBySectionRed();

            EmptyListBoxAndFill();
            UpdateAttendanceList();
        }

        private void rbtn_yellow_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();

            GetAttendance();
            FilterAttendancesByDate();

            SortSchedulesBySectionYellow();

            EmptyListBoxAndFill();
            UpdateAttendanceList();
        }

        private void rbtn_green_Checked(object sender, RoutedEventArgs e)
        {
            GetSchedules();
            SortSchedulesByDate();

            GetAttendance();
            FilterAttendancesByDate();

            SortSchedulesBySectionGreen();

            EmptyListBoxAndFill();
            UpdateAttendanceList();
        }
        #endregion


        

        //private void listViewSC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    ChildProfile m = new ChildProfile();
        //    m.Show();
        //    this.Close();
        //}

       

        private void listViewSC_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            ChildProfile m = new ChildProfile();
            m.Show();
            this.Close();
        }

        private void btnSaveAttendance_Click(object sender, RoutedEventArgs e)
        {
            foreach (var schoolchild in attendances)
            {
                if (schoolchild.attendance == "Ja")
                {
                    MessageBox.Show($"{schoolchild.firstname} {schoolchild.attendance}");
                }
            }   
        }

        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var schoolchild in attendances)
            {
                if (schoolchild.attendance == "True")
                {
                    MessageBox.Show("Meddelande");
                }
            }

        }
    }

}
