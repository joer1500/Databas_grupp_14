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
            int id = int.Parse(txtID.Text);
            string firstname = txtFirstname.Text;
            string lastname = txtLastname.Text;
            int section = int.Parse(txtSection.Text);

            DbOperations db = new DbOperations();
            db.UpdateSchoolchild(id, firstname, lastname, section);
        }
    }
}
