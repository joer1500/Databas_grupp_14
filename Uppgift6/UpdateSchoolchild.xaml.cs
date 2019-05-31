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
    /// Interaction logic for UpdateSchoolchild.xaml
    /// </summary>
    public partial class UpdateSchoolchild : Window
    {
        public UpdateSchoolchild()
        {
            InitializeComponent();
            UpdateSchoolchildInfo();
        }

        public void UpdateSchoolchildInfo()
        {
            Schoolchild schoolchild = new Schoolchild();
            
            DbOperations db = new DbOperations();
            schoolchild = db.GetSchoolchildByID(ManageSchoolchild.selectedSchooolchildID);

            txtID.Text = schoolchild.id.ToString();
            txtFirstname.Text = schoolchild.firstname;
            txtLastname.Text = schoolchild.lastname;
            txtSection.Text = schoolchild.sectionID.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();

            int id = int.Parse(txtID.Text);
            string firstname = txtFirstname.Text;
            string lastname = txtLastname.Text;
            int sectionID = int.Parse(txtSection.Text);

            db.UpdateSchoolchild(id, firstname, lastname, sectionID);

            ManageSchoolchild win = new ManageSchoolchild();
            win.Show();
            this.Close();

        }
    }
}
