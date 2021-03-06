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
    /// Interaction logic for NewStaff.xaml
    /// </summary>
    public partial class NewStaff : Window
    {
        public NewStaff()
        {
            InitializeComponent();
            textBlockSectionInfo.Visibility = Visibility.Collapsed;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Staffwindow win = new Staffwindow();
            win.Show();
            this.Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            DbOperations db = new DbOperations();
            string fname = textBoxFirstname.Text;
            string lname = textBoxLastname.Text;
            string profession = textBoxProfession.Text;

            if (textBoxSection.Text == "" || textBoxSection.Text == "Ange avdelnings-ID")
            {
                MessageBox.Show("Vänligen ange ett avdelnings-id");
                return;               
            }
            if (fname == "" || lname == "" || profession == "")
            {
                MessageBox.Show("Vänligen ange ett förnamn, efternamn, roll samt en avdelning");
                return;
            }
            if (CheckInput(textBoxSection.Text) == false) //För att kolla om "avdelning" innehåller annat än siffror
            {                
                labelSectionError.Content = "*Endast siffror mellan 1-4 i avdelning";
                return;
            }
            else
            {
                int section = int.Parse(textBoxSection.Text); 
                try
                {
                    db.AddNewStaff(fname, lname, profession, section);
                    MessageBox.Show($"{fname} {lname} är nu tillagd i personalregistret", "Titel");
                    EmptyTextboxes();

                    Staffwindow win = new Staffwindow();
                    win.Show();
                    this.Close();
                }
                catch (PostgresException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }          
                       
        }

        private void EmptyTextboxes()
        {
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxProfession.Text = "";
            textBoxSection.Text = "";
        }

        private void textBoxSection_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxSection.Text = "";
        }

        private void Icon_Question_mark_svg_png_MouseEnter(object sender, MouseEventArgs e) 
        {
            textBlockSectionInfo.Visibility = Visibility.Visible;
        }

        private void Icon_Question_mark_svg_png_MouseLeave(object sender, MouseEventArgs e) 
        {
            textBlockSectionInfo.Visibility = Visibility.Collapsed;
        }

        private void labelSection_MouseEnter(object sender, MouseEventArgs e)
        {
            textBlockSectionInfo.Visibility = Visibility.Visible;
        }

        private void labelSection_MouseLeave(object sender, MouseEventArgs e)
        {
            textBlockSectionInfo.Visibility = Visibility.Collapsed;
        }

        private bool CheckInput(string s)
        {
            foreach (char c in s)
            {
                if (Char.IsLetter(c))
                    return false;
            }
            return true;
        }


    }

       
}
