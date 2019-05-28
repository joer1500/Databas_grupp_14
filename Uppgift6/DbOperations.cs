using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;


namespace Uppgift6
{
    class DbOperations
    {
        public List<Staff> GetAllStaff() //Hämtar alla staffs
        {
            Staff s;
            List<Staff> staffs = new List<Staff>();
            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY staff_id;";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        s = new Staff()
                        {
                            staffID = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            profession = (reader.GetString(3)),
                            section = (reader.GetString(4))
                        };
                        staffs.Add(s);
                    }
                }
                return staffs; 
            }            
        }

        public void AddNewStaff(string fname, string lname, string proffesion, int section) //Lägger till ny staff
        {
            string stmt = "INSERT INTO staff(firstname, lastname, profession, section_id) VALUES (@firstname, @lastname, @profession, @section_id) ";

            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("firstname", fname);
                    cmd.Parameters.AddWithValue("lastname", lname);
                    cmd.Parameters.AddWithValue("profession", proffesion);
                    cmd.Parameters.AddWithValue("section_id", section);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Schoolchild> GetChildNameFromGuardianID (int ID)
        {
            Schoolchild sc;
            List<Schoolchild> children = new List<Schoolchild>();

            string stmt = $"SELECT guardian_schoolchild.guardian_id, guardian_schoolchild.schoolchild_id, schoolchild.firstname, schoolchild.lastname " +
                $"          FROM(guardian_schoolchild INNER JOIN schoolchild ON guardian_schoolchild.schoolchild_id = schoolchild.schoolchild_id) " +
                $"          WHERE guardian_id = {ID}";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sc = new Schoolchild()
                        {
                            id = (reader.GetInt32(1)),
                            firstname = (reader.GetString(2)),
                            lastname = (reader.GetString(3)),
                        };
                        children.Add(sc);
                    }
                }
                return children;
            }

        }
        public void InsertSchedule(Schoolchild child, DateTime date, string day_off, string breakfast, 
            DateTime should_drop, DateTime should_pickup, string walk_home_alone, string walk_with_friend)
         {
            Schoolchild schoolchild;
            schoolchild = child;

            string stmt = "INSERT INTO schedule(schoolchild_id, date, day_off, breakfast, " +
                "should_drop, should_pickup, walk_home_alone, home_with_friend) " +
                "VALUES(@id, @date, @day_off, @breakfast, @drop, @pickup, @walk_alone, @walk_friend) ";

            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))

            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("id", child.id);
                    cmd.Parameters.AddWithValue("date", date);
                    cmd.Parameters.AddWithValue("day_off", day_off);
                    cmd.Parameters.AddWithValue("breakfast", breakfast);
                    cmd.Parameters.AddWithValue("drop", should_drop);
                    cmd.Parameters.AddWithValue("pickup", should_pickup);
                    cmd.Parameters.AddWithValue("walk_alone", walk_home_alone);
                    cmd.Parameters.AddWithValue("walk_friend", walk_with_friend);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Staff GetStaffByID(int id) //Hämtar staff baserat på ID
        {
            Staff s = new Staff();
            string stmt = "SELECT * FROM staff WHERE staff_id = @staff_id";
            using (var conn = new
                NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("staff_id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            s.staffID = reader.GetInt32(0);
                            s.firstname = reader.GetString(1);
                            s.lastname = reader.GetString(2);
                            s.profession = reader.GetString(3);
                        }
                    }
                }
                return s;
            }
        }

        public void UpdateStaff(int id, string firstname, string lastname, string profession) //Uppdaterar staff
        {
            string stmt = "UPDATE staff SET (firstname, lastname, profession) = (@fname, @lname, @profession) WHERE staff_id = @id";

            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("fname", firstname);
                    cmd.Parameters.AddWithValue("lname", lastname);
                    cmd.Parameters.AddWithValue("profession", profession);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStaff (int id) //Ta bort staff baserat på ID
        {

            string stmt = "DELETE FROM staff WHERE staff_id = @staffID";
            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("staffID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Staff> GetAllStaffOrderByFirstname() //Hämtar alla staffs och sorterar på förnamn
        {
            Staff s;
            List<Staff> staffs = new List<Staff>();

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY firstname;";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        s = new Staff()
                        {
                            staffID = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            profession = (reader.GetString(3)),
                            section = (reader.GetString(4))
                        };
                        staffs.Add(s);
                    }
                }
                return staffs;
            }
        }

        public List<Staff> GetAllStaffOrderByLastname() //Hämtar alla staffs och sorterar på efternamn
        {
            Staff s;
            List<Staff> staffs = new List<Staff>();

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY lastname;";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        s = new Staff()
                        {
                            staffID = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            profession = (reader.GetString(3)),
                            section = (reader.GetString(4))
                        };
                        staffs.Add(s);
                    }
                }
                return staffs;
            }
        }

        public List<Staff> GetAllStaffOrderBy(string orderby) //Under konstruktion
        {
            Staff s;
            List<Staff> staffs = new List<Staff>();

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY @order";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))                   
                using (var reader = cmd.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        s = new Staff()
                        {
                            staffID = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            profession = (reader.GetString(3)),
                            section = (reader.GetString(4))
                        };
                        staffs.Add(s);
                    }
                }
                return staffs;
            }
        }   
    }
}
