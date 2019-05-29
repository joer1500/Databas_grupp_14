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
        Schedule schedule;


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
                TimeSpan should_drop = TimeSpan.Parse(textBoxShould_drop.Text);
                TimeSpan should_pickup = TimeSpan.Parse(textBoxShould_pickup.Text);
                string walk_home_alone = textBoxWalk_home_alone.Text.ToString();
                string walk_with_friend = textBoxHome_with_friend.Text.ToString();

                db.InsertSchedule(schoolchild, date, day_off, breakfast, should_drop, should_pickup, walk_home_alone, walk_with_friend);

                MessageBox.Show($"Ditt schema har lagts till för {schoolchild.firstname} den {textBoxDate.Text.ToString()}.");

                ScheduleList();
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

            ScheduleList();
        }

        private void listBox_ChildSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            schedule = (Schedule)listBox_ChildSchedule.SelectedItem;

            if (schedule == null)
            {
                ClearAllTxt();
            }
            else
            {
                txt_day_off.Text = schedule.day_off;
                txt_breakfast.Text = schedule.breakfast;
                txt_drop.Text = schedule.should_drop.ToString("hh\\:mm");
                txt_pickup.Text = schedule.should_pickup.ToString("hh\\:mm");
                txt_home_alone.Text = schedule.walk_home_alone.ToString();
                txt_home_with_friend.Text = schedule.home_with_friend.ToString();
            }
        }

        private void ScheduleList() {
            List<Schedule> schedule = new List<Schedule>();
            schedule = db.GetChildScheduleDatesFromChildID(schoolchild.id);

            listBox_ChildSchedule.ItemsSource = null;
            listBox_ChildSchedule.ItemsSource = schedule;
        }

        private void ClearAllTxt() {
            txt_day_off.Text = null;
            txt_breakfast.Text = null;
            txt_drop.Text = null;
            txt_pickup.Text = null;
            txt_home_alone.Text = null;
            txt_home_with_friend.Text = null;
        }

        private void Remove_schedule_Click(object sender, RoutedEventArgs e)
        {
            if (schedule == null)
            {
                MessageBox.Show("Du måste välja ett schema i listan.");
            }
            else
            {
                db.DeleteSchedule(schedule.id);
                ScheduleList();
            }
        }
    }
}
