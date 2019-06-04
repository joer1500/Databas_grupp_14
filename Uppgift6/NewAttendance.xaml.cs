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
    /// Interaction logic for NewAttendance.xaml
    /// </summary>
    public partial class NewAttendance : Window
    {
        public NewAttendance()
        {
            InitializeComponent();
            GetAllSchoolchilds();
            SetValues();
        }
        List<Schoolchild> sc = new List<Schoolchild>();
        DateTime choosenDate = DateTime.Today;
        DbOperations db = new DbOperations();
        Schoolchild s = new Schoolchild();
        Random slump = new Random();

        #region Metoder
        private void GetAllSchoolchilds()
        {
            sc = db.GetSchoolchildrenOrderByID();
            sc = OrderListByLastname(sc);
            //sc = db.GetSchoolchildrenOrderByLastname();
            comboBoxSchoolchilds.ItemsSource = sc;
        }

        private void SetValues()
        {
            textBoxDate.Text = choosenDate.ToShortDateString();
            comboBoxBreakfast.Items.Add("Ja");
            comboBoxBreakfast.Items.Add("Nej");
            //btnSave.IsEnabled = false;
        }

        private void SaveAttendance()
        {
            DateTime date = DateTime.Parse(textBoxDate.Text);
            int staff = slump.Next(1, 6);
            TimeSpan should_drop = TimeSpan.Parse("00:00");
            TimeSpan should_pickup = TimeSpan.Parse("00:00");
            s = (Schoolchild)comboBoxSchoolchilds.SelectedItem;
            try
            {
                if (s == null || textBoxDate.Text == null)
                {
                    return;
                }
                else
                {
                    db.AddNewAttendance(s.id, choosenDate, "", "Ja", staff);
                    db.InsertSchedule(s, choosenDate, "Nej", comboBoxBreakfast.Text, should_drop, should_pickup, "", "");
                    StaffChildren win = new StaffChildren();
                    win.Show();
                    this.Close();
                }              
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private List<Schoolchild> OrderListByLastname(List<Schoolchild> childs)
        {
            return childs = childs.OrderBy(s => s.lastname).ToList();
        }
        private void CheckInput()
        {
            s = (Schoolchild)comboBoxSchoolchilds.SelectedItem;
            if (s != null && textBoxDate.Text != "" && comboBoxBreakfast.Text != "")
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }
        #endregion

        #region Buttons
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveAttendance();
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StaffChildren win = new StaffChildren();
            win.Show();
            this.Close();
        }
        #endregion

        #region Andra events
        private void comboBoxBreakfast_DropDownClosed(object sender, EventArgs e)
        {
            CheckInput();
        }
       
        private void comboBoxSchoolchilds_DropDownClosed(object sender, EventArgs e)
        {
            CheckInput();
        }
      
        private void textBoxDate_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }

        private void label2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textBoxDate.Text = choosenDate.ToShortDateString();
            CheckInput();
        }

        #endregion
    }
}
