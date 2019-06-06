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

            ViewLoggedInGuardian();
            List<Schoolchild> children = new List<Schoolchild>();

            int id = int.Parse(MainWindow.SetValueForList);
            children = db.GetChildNameFromGuardianID(id);

            listBoxChildName.ItemsSource = null;
            listBoxChildName.ItemsSource = children;

            CreateValuesForCombobox();
        }

        public static int selectedSchoolchildID;
        DbOperations db = new DbOperations();
        Schoolchild schoolchild;
        Schedule schedule;
        List<Schedule> schedules = new List<Schedule>();
        Random slump = new Random();

        private void ViewLoggedInGuardian()
        {
            DbOperations db = new DbOperations();
            Guardian loggedInGuardian;
            int id = int.Parse(MainWindow.SetValueForList);
            loggedInGuardian = db.GetGuardianById(id);

            lblLoggedInGuardian.Content = $"{loggedInGuardian.firstname} {loggedInGuardian.lastname}";
        }

        private void BtnSaveNewSchedule_Click(object sender, RoutedEventArgs e)
        {
            int staffSlump = slump.Next(1, 6);
            if (listBoxChildName.SelectedItem == null)
            {
                ErrorMessageSelectChildInList();
            }

            else
            {
                string messageOLD = errorMessage(textBoxDate.Text, textBoxBreakfast.Text, textBoxDay_of.Text, textBoxShould_drop.Text, 
                   textBoxShould_pickup.Text, textBoxWalk_home_alone.Text, textBoxHome_with_friend.Text);


                string message = errorMessage(textBoxDate.Text, comboBoxBreakFast.Text, comboBoxDayOff.Text, textBoxShould_drop.Text,
                   textBoxShould_pickup.Text, comboBoxWalkHomeAlone.Text, comboBoxHomeWithFriend.Text);

                if (message == "")
                {
                    DateTime date = DateTime.Parse(textBoxDate.Text);
                    string day_off = comboBoxDayOff.Text;
                    string breakfast = comboBoxBreakFast.Text;
                    TimeSpan should_drop = TimeSpan.Parse(textBoxShould_drop.Text);
                    TimeSpan should_pickup = TimeSpan.Parse(textBoxShould_pickup.Text);
                    string walk_home_alone = comboBoxWalkHomeAlone.Text;
                    string walk_with_friend = comboBoxHomeWithFriend.Text;

                    string drop = "00:00:00";
                    string pickup = "00:00:00";

                    TimeSpan has_drop = TimeSpan.Parse(drop);
                    TimeSpan has_pickup = TimeSpan.Parse(pickup);


                    schoolchild = (Schoolchild)listBoxChildName.SelectedItem;

                    bool dayOff;

                    dayOff = CheckIfDayOffIsTrue(day_off);

                    if (dayOff)
                    {
                        SetValuesForNullValues(schoolchild, date, day_off, breakfast, should_drop, should_pickup, walk_home_alone, walk_with_friend);
                    }

                    else
                    {
                        try
                        {
                            db.InsertSchedule(schoolchild, date, day_off, breakfast, should_drop, should_pickup, walk_home_alone, walk_with_friend);
                            db.AddNewAttendance(schoolchild.id, date, "Nej", "", staffSlump, has_drop, has_pickup);
                        }
                        catch (PostgresException ex)
                        {
                            MessageBox.Show(ex.Message);                           
                        }
                        
                    }

                    MessageBox.Show($"Ditt schema har lagts till för {schoolchild.firstname} den {textBoxDate.Text.ToString()}.");
                }

                else
                {
                    MessageBox.Show($"{messageOLD}");
                }

                GetScheduleList();
                ShowScheduleForSelectedWeek();
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
            selectedSchoolchildID = schoolchild.id;
            label_child_schema.Content = $"{schoolchild}s schema";

            GetScheduleList();
            comboBoxWeeks.Text = "Alla veckor";

            listBox_ChildSchedule.ItemsSource = null;
            listBox_ChildSchedule.ItemsSource = schedules;
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

        private void GetScheduleList()
        {          
            schedules = db.GetChildScheduleDatesFromChildID(schoolchild.id);
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
            DateTime attDate = schedule.date;
            

            if (schedule == null)
            {
                MessageBox.Show("Du måste välja ett schema i listan.");
            }
            else
            {
                int attID = schedule.schoolchild_id;
                db.DeleteSchedule(schedule.id);
                db.DeleteAttendance(attDate, attID);
                GetScheduleList();
                ShowScheduleForSelectedWeek();
            }
        }

        private string errorMessage(string date, string breakfast, string day_off, string drop, string pickup,
            string walk_alone, string walk_friend)
        {

            string message = "";
            if (day_off.ToLower() == "ja")
            {
                
                if (date == "")
                {
                    message = "Vänligen fyll i datum innan du sparar.";
                }
            }
            else
            {
                message = "Vänligen fyll i dessa textrutor innan du sparar: ";
                if (date == "")
                {
                    message += "Datum, ";
                }
                if (breakfast == "")
                {
                    message += "Frukost, ";
                }
                if (drop == "00:00")
                {
                    message += "Lämnas, ";
                }
                if (pickup == "00:00")
                {
                    message += "Hämtas, ";
                }
                if (walk_alone == "")
                {
                    message += "Gå hem själv, ";
                }
                if (walk_friend == "")
                {
                    message += "Gå hem med kompis. ";
                }

                if (message == "Vänligen fyll i dessa textrutor innan du sparar: ")
                {
                    message = "";
                }
            }

            return message;
        }

         private bool CheckIfDayOffIsTrue(string day_off)
        {
            bool dayOff= false;

            if (day_off.ToLower() == "ja")
            {
                dayOff = true;
            }

            return dayOff;
        }

        private void SetValuesForNullValues(Schoolchild schoolchild, DateTime date, string day_off, string breakfast, TimeSpan drop, TimeSpan pickup,
            string walk_alone, string walk_friend)
        {
            string tid = "00:00";
            breakfast = "Nej";
            drop = TimeSpan.Parse(tid);
            pickup = TimeSpan.Parse(tid);
            walk_alone = "Nej";
            walk_friend = "Nej";

            db.InsertSchedule(schoolchild, date, day_off, breakfast, drop, pickup, walk_alone, walk_friend);
        }

        private List<Schedule> GetShceduleFromSelectedWeek()
        {
            string vecka = (string)comboBoxWeeks.SelectedItem;
            List<Schedule> schedulesForSelectedWeek = new List<Schedule>();

            if (vecka == "Vecka 21")
            {
                foreach (Schedule s in schedules)
                {
                    if (s.date > DateTime.Parse("2019-05-19") && s.date < DateTime.Parse("2019-05-27"))
                    {
                        schedulesForSelectedWeek.Add(s);

                    }
                }
            }

            if (vecka == "Vecka 22") 
            {
                foreach (Schedule s in schedules)
                {
                    if (s.date > DateTime.Parse("2019-05-26") && s.date < DateTime.Parse("2019-06-03"))
                    {
                        schedulesForSelectedWeek.Add(s);

                    }
                }
            }

            if (vecka == "Vecka 23") 
            {
                foreach (Schedule s in schedules)
                {
                    if (s.date > DateTime.Parse("2019-06-02") && s.date < DateTime.Parse("2019-06-10"))
                    {
                        schedulesForSelectedWeek.Add(s);

                    }
                }
            }

            if (vecka == "Vecka 24")
            {
                foreach (Schedule s in schedules)
                {
                    if (s.date > DateTime.Parse("2019-06-09") && s.date < DateTime.Parse("2019-06-17"))
                    {
                        schedulesForSelectedWeek.Add(s);

                    }
                }
            }

            if (vecka == "Vecka 25")
            {
                foreach (Schedule s in schedules)
                {
                    if (s.date > DateTime.Parse("2019-06-16") && s.date < DateTime.Parse("2019-06-24"))
                    {
                        schedulesForSelectedWeek.Add(s);

                    }
                }
            }

            if (vecka == "Alla veckor")
            {
                foreach (Schedule s in schedules)
                {
                    schedulesForSelectedWeek.Add(s);
                }

            }

            return schedulesForSelectedWeek;
        }

        private void ComboBoxWeeks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*List<Schedule> schedulesForSelectedWeek = new List<Schedule>();
            schedulesForSelectedWeek = GetShceduleFromSelectedWeek();

            listBox_ChildSchedule.ItemsSource = null;
            listBox_ChildSchedule.ItemsSource = schedulesForSelectedWeek;*/

            ShowScheduleForSelectedWeek();
        }

        private void btnPickup_Click(object sender, RoutedEventArgs e)
        {
            schoolchild = (Schoolchild)listBoxChildName.SelectedItem;
            if (schoolchild == null)
            {
                ErrorMessageSelectChildInList();
            }

            else
            {
                Pickups win = new Pickups();
                win.Show();
                this.Close();
            }
            
        }

        private void CreateValuesForCombobox()
        {
            List<string> veckor = new List<string>();
            List<string> yesno = new List<string>();

            string allaVeckor, v21, v22, v23, v24, v25;
            allaVeckor = "Alla veckor";
            v21 = "Vecka 21";
            v22 = "Vecka 22";
            v23 = "Vecka 23";
            v24 = "Vecka 24";
            v25 = "Vecka 25";

            veckor.Add(allaVeckor);
            veckor.Add(v21);
            veckor.Add(v22);
            veckor.Add(v23);
            veckor.Add(v24);
            veckor.Add(v25);

            string yes = "Ja";
            string no = "Nej";
            yesno.Add(yes);
            yesno.Add(no);


            comboBoxWeeks.ItemsSource = veckor;
            comboBoxWeeks.Text = allaVeckor.ToString();

            comboBoxDayOff.ItemsSource = yesno;
            comboBoxBreakFast.ItemsSource = yesno;
            comboBoxWalkHomeAlone.ItemsSource = yesno;
            comboBoxHomeWithFriend.ItemsSource = yesno;
            comboBoxDayOff.Text = no;

            textBoxDateSick.Text = DateTime.Today.ToShortDateString();
            textBoxDate.Text = DateTime.Today.ToShortDateString();
        }

        private void btnOpenNeeds_Click(object sender, RoutedEventArgs e)
        {
            if (schoolchild == null)
            {
                ErrorMessageSelectChildInList();
            }
            else
            {
                NeedBySchoolchild m = new NeedBySchoolchild();
                m.Show();
                this.Close();
            }          
        }

        private void ButtonSick_Click(object sender, RoutedEventArgs e)
        {
          schoolchild = (Schoolchild)listBoxChildName.SelectedItem;
          string drop = "00:00:00";
          string pickup = "00:00:00";
          TimeSpan has_drop = TimeSpan.Parse(drop);
          TimeSpan has_pickup = TimeSpan.Parse(pickup);
          TimeSpan nullValue = TimeSpan.Parse(pickup);

            if (schoolchild == null)
            {
                ErrorMessageSelectChildInList();
            }
            else
            {
                if (textBoxDateSick.Text == "")
                {
                    MessageBox.Show("Vänligen fyll i ett datum för sjukanmälan.");
                }
                else
                {
                    DateTime sickDate;
                    sickDate = DateTime.Parse(textBoxDateSick.Text);
                    int staffSlump = slump.Next(1, 6);

                    //db.InsertSchedule(schoolchild, sickDate, "", "", nullValue, nullValue, "", "");
                    db.UpdateAttendance(schoolchild.id, sickDate, "Ja", "Nej", has_drop, has_pickup);
                    MessageBox.Show($"{schoolchild.firstname} är nu sjukanmäld för {textBoxDateSick.Text.ToString()}");
                    textBoxDateSick.Clear();
                }
            }          

        }

        private void ErrorMessageSelectChildInList()
        {
            MessageBox.Show("Vänligen välj ett barn i listan.");
        }

        private void ShowScheduleForSelectedWeek()
        {
            List<Schedule> schedulesForSelectedWeek = new List<Schedule>();
            schedulesForSelectedWeek = GetShceduleFromSelectedWeek();

            listBox_ChildSchedule.ItemsSource = null;
            listBox_ChildSchedule.ItemsSource = schedulesForSelectedWeek;
        }

        private void TextBoxShould_drop_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxShould_drop.Clear();
        }

        private void TextBoxShould_pickup_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxShould_pickup.Clear();
        }

        private void buttonHistoricalSick_Click(object sender, RoutedEventArgs e)
        {
            if (schoolchild == null)
            {
                return;
            }
            else
            {
            schoolchild = (Schoolchild)listBoxChildName.SelectedItem;
            selectedSchoolchildID = schoolchild.id;

            HistoricalSickness win = new HistoricalSickness();
            win.Show();
            this.Close();
            }
        }
    }       
}
