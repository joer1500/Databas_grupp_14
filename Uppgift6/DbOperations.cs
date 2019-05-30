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
            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY staff_id;";

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
                            sectionname = (reader.GetString(4)),
                            sectionid = (reader.GetInt32(5))
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

        public void DeleteSchedule(int id) //Ta bort schedule baserat på ID
        {
            string stmt = "DELETE FROM schedule WHERE schedule_id = @scheduleid";
            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("scheduleid", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Schedule> GetSchoolchildrenSchedule() 
         // Hämtar alla aktuella dagens scheman för alla avdelningar  
        {
            Schedule sd;
            List<Schedule> schedule = new List<Schedule>();

            string stmt = "SELECT schedule_id, schedule.schoolchild_id, date, day_off, breakfast, should_drop, should_pickup, walk_home_alone, home_with_friend, firstname, lastname, section_id FROM(schedule INNER JOIN schoolchild ON schedule.schoolchild_id = schoolchild.schoolchild_id) ORDER BY lastname ASC";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sd = new Schedule
                        {
                            id = (reader.GetInt32(0)),
                            schoolchild_id = (reader.GetInt32(1)),
                            date = (reader.GetDateTime(2)),
                            day_off = (reader.GetString(3)),
                            breakfast = (reader.GetString(4)),
                            should_drop = (reader.GetTimeSpan(5)),
                            should_pickup = (reader.GetTimeSpan(6)),
                            walk_home_alone = (reader.GetString(7)),
                            home_with_friend = (reader.GetString(8)),
                            firstname = (reader.GetString(9)),
                            lastname = (reader.GetString(10)),
                            section_id = (reader.GetInt32(11))
                        };
                        schedule.Add(sd);
                    }
                }
                return schedule;
            }
        }

        public List<Schedule> GetChildScheduleDatesFromChildID(int ID)
        {
            Schedule sd;
            List<Schedule> schedule = new List<Schedule>();

            string stmt = $"SELECT schedule_id, schoolchild_id, date, day_off, breakfast, should_drop, should_pickup, walk_home_alone, home_with_friend " +
                $"FROM schedule WHERE schoolchild_id = {ID} ORDER BY date ASC";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sd = new Schedule()
                        {
                            id = (reader.GetInt32(0)),
                            schoolchild_id = (reader.GetInt32(1)),
                            date = (reader.GetDateTime(2)),
                            day_off = (reader.GetString(3)),
                            breakfast = (reader.GetString(4)),
                            should_drop = (reader.GetTimeSpan(5)),
                            should_pickup = (reader.GetTimeSpan(6)),
                            walk_home_alone = (reader.GetString(7)),
                            home_with_friend = (reader.GetString(8))
                        };
                        schedule.Add(sd);
                    }
                }
                return schedule;
            }
        }

        public List<Guardian> GetGuardianFromSchoolchildID(int ID)
        {
            Guardian g;
            List<Guardian> guardian = new List<Guardian>();

            string stmt = $"SELECT guardian_schoolchild.guardian_id, guardian.firstname, guardian.lastname, guardian.phonenumber FROM(guardian_schoolchild INNER JOIN guardian ON guardian_schoolchild.guardian_id = guardian.guardian_id) WHERE guardian_schoolchild.schoolchild_id = {ID}";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        g = new Guardian()
                        {
                            id = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            phonenumber = (reader.GetString(3))
                        };
                        guardian.Add(g);
                    }
                }
                return guardian;
            }

        }

        public List<Schoolchild> GetChildNameFromGuardianID(int ID)
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
            TimeSpan should_drop, TimeSpan should_pickup, string walk_home_alone, string walk_with_friend)
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
                            s.sectionid = reader.GetInt32(4);
                        }
                    }
                }
                return s;
            }
        }

        public void UpdateStaff(int id, string firstname, string lastname, string profession, int sectionID) //Uppdaterar staff
        {
            string stmt = "UPDATE staff SET (firstname, lastname, profession, section_id) = (@fname, @lname, @profession, @section) WHERE staff_id = @id";
            

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
                    cmd.Parameters.AddWithValue("section", sectionID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStaff(int id) //Ta bort staff baserat på ID
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

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY firstname;";

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
                            sectionname = (reader.GetString(4)),
                            sectionid = (reader.GetInt32(5))
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

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY lastname;";

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
                            sectionname = (reader.GetString(4)),
                            sectionid = (reader.GetInt32(5))
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

            //string stmtold = "$SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY {orderby}";
            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY @order";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("order", orderby);
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
                                sectionname = (reader.GetString(4))
                            };
                            staffs.Add(s);
                        }
                    }
                }
                return staffs;
            }
        }

        public List<Schoolchild> GetSchoolchildrenOrderByLastname() // Hämtar alla skolbarn och sorterar på efternamn
        {
            Schoolchild schoolchild;
            List<Schoolchild> schoolchildren = new List<Schoolchild>();

            string stmt = "SELECT schoolchild_id, lastname, firstname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY lastname ASC";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schoolchild = new Schoolchild
                        {
                            id = reader.GetInt32(0),
                            lastname = reader.GetString(1),
                            firstname = reader.GetString(2),
                            section = reader.GetString(3)
                        };
                        schoolchildren.Add(schoolchild);
                    }
                }
                return schoolchildren;
            }
        }

        public List<Schoolchild> GetSchoolchildrenOrderByID()   // Hämtar skolbarn och sorterar efter ID
        {
            Schoolchild schoolchild;
            List<Schoolchild> schoolchildren = new List<Schoolchild>();

            string stmt = "SELECT schoolchild_id, firstname, lastname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY schoolchild_id";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                    using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schoolchild = new Schoolchild
                        {
                            id = reader.GetInt32(0),
                            firstname = reader.GetString(1),
                            lastname = reader.GetString(2),
                            section = reader.GetString(3)
                        };
                        schoolchildren.Add(schoolchild);
                    }
                }
                return schoolchildren;
            }
        }

        public List<Schoolchild> GetSchoolchildrenOrderByFirstname() // Hämtar skolbarn och sorterar efter förnamn
        {
            Schoolchild schoolchild;
            List<Schoolchild> schoolchildren = new List<Schoolchild>();

            string stmt = "SELECT schoolchild_id, lastname, firstname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY firstname ASC";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schoolchild = new Schoolchild
                        {
                            id = reader.GetInt32(0),
                            lastname = reader.GetString(1),
                            firstname = reader.GetString(2),
                            section = reader.GetString(3)
                        };
                        schoolchildren.Add(schoolchild);
                    }
                }
                return schoolchildren;
            }
        }

        public List<Schoolchild> GetSchoolchildrenOrderBySection() // Hämtar skolbarn och sorterar efter avdelning
        {
            Schoolchild schoolchild;
            List<Schoolchild> schoolchildren = new List<Schoolchild>();

            string stmt = "SELECT schoolchild_id, lastname, firstname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY section.section_id";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schoolchild = new Schoolchild
                        {
                            id = reader.GetInt32(0),
                            lastname = reader.GetString(1),
                            firstname = reader.GetString(2),
                            section = reader.GetString(3)
                        };
                        schoolchildren.Add(schoolchild);
                    }
                }
                return schoolchildren;
            }
        }

        public void AddSchoolchild(string firstname, string lastname, int section)    // Lägger till skolbarn
        {
            string stmt = "INSERT INTO schoolchild(firstname, lastname, section_id) VALUES (@firstname, @lastname, @section_id)";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("firstname", firstname);
                    cmd.Parameters.AddWithValue("lastname", lastname);
                    cmd.Parameters.AddWithValue("section_id", section);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSchoolchild (int id) //Tar bort skolbarn
        {
            string stmt = "DELETE FROM schoolchild WHERE schoolchild_id = @schoolchild_id";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                    using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("schoolchild_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ConnectGuardianSchoolchild(int schoolchild_id, int guardian_id)       // koppla ihop barn med vårdnadshavare EJ TESTAD
        {
            string stmt = "INSERT INTO guardian_schoolchild(schoolchild_id, guardian_id) VALUES (@schoolchild_id, @guardian_id)";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("schoolchild_id", schoolchild_id);
                    cmd.Parameters.AddWithValue("guardian_id", guardian_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Staff> GetAllStaffOrderByProfession() //Hämtar alla staffs och sorterar på roll
        {
            Staff s;
            List<Staff> staffs = new List<Staff>();

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY profession ASC;";

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
                            sectionname = (reader.GetString(4)),
                            sectionid = (reader.GetInt32(5))
                        };
                        staffs.Add(s);
                    }
                }
                return staffs;
            }
        }

        public List<Staff> GetAllStaffOrderBySection() //Hämtar alla staffs och sorterar på avdelning
        {
            Staff s;
            List<Staff> staffs = new List<Staff>();

            string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY section.section_id;";

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
                            sectionname = (reader.GetString(4)),
                            sectionid = (reader.GetInt32(5))
                        };
                        staffs.Add(s);
                    }
                }
                return staffs;
            }
        }

        public List<Guardian> GetAllGuardians() //Hämtar alla vårdnadshavare
        {
            Guardian g;
            List<Guardian> guardians = new List<Guardian>();
            string stmt = "SELECT guardian_id, firstname, lastname, phonenumber, address FROM guardian;";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        g = new Guardian()
                        {
                            id = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            phonenumber = (reader.GetString(3)),
                            address = (reader.GetString(4))
                        };
                        guardians.Add(g);
                    }
                }
                return guardians;
            }

        }


        public void AddNewGuardian(string fname, string lname, string phone, string address) //Lägger till ny Guardian
        {
            string stmt = "INSERT INTO guardian(firstname, lastname, phonenumber, address) VALUES (@firstname, @lastname, @pho, @addr)";

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
                    cmd.Parameters.AddWithValue("pho", phone);
                    cmd.Parameters.AddWithValue("addr", address);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGuardian(int id) //Ta bort Guardian baserat på ID
        {

            string stmt = "DELETE FROM guardian WHERE guardian_id = @guardianid";
            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("guardianid", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public Guardian GetGuardianById(int id) //Hämtar staff baserat på ID
        {
            Guardian g = new Guardian();
            string stmt = "SELECT * FROM guardian WHERE guardian_id = @guardian_id";
            using (var conn = new
                NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("guardian_id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            g.id = reader.GetInt32(0);
                            g.firstname = reader.GetString(1);
                            g.lastname = reader.GetString(2);
                            g.phonenumber = reader.GetString(3);
                            g.address = reader.GetString(4);
                        }
                    }
                }
                return g;
            }

        }


        public void UpdateGuardian(int id, string firstname, string lastname, string phonenumber, string address) //Uppdaterar guardian
        {
            string stmt = "UPDATE guardian SET (firstname, lastname, phonenumber, address) = (@fname, @lname, @phone, @addr) WHERE guardian_id = @guardid";

            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("guardid", id);
                    cmd.Parameters.AddWithValue("fname", firstname);
                    cmd.Parameters.AddWithValue("lname", lastname);
                    cmd.Parameters.AddWithValue("phone", phonenumber);
                    cmd.Parameters.AddWithValue("addr", address);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<Schoolchild> GetSchoolchildsFromGuardian(int id) //används ej, ska tas bort
        {
            Schoolchild sc;
            List<Schoolchild> children = new List<Schoolchild>();

            string stmt = $"SELECT schoolchild.firstname, schoolchild.lastname FROM guardian_schoolchild INNER JOIN schoolchild on guardian_schoolchild.schoolchild_id = schoolchild.schoolchild_id WHERE guardian_id = @gid";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("gid", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sc = new Schoolchild()
                            {                               
                                firstname = (reader.GetString(0)),
                                lastname = (reader.GetString(1))
                                
                            };
                            children.Add(sc);
                        }
                    }
                    return children;
                }
            }
        }


        public List<Guardian> GetAllGuardiansOrderbyFirstname() //Hämtar alla vårdnadshavare
        {
            Guardian g;
            List<Guardian> guardians = new List<Guardian>();
            string stmt = "SELECT guardian_id, firstname, lastname, phonenumber, address FROM guardian ORDER BY firstname;";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        g = new Guardian()
                        {
                            id = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            phonenumber = (reader.GetString(3)),
                            address = (reader.GetString(4))
                        };
                        guardians.Add(g);
                    }
                }
                return guardians;
            }
        }

        public List<Guardian> GetAllGuardiansOrderbyLastname() //Hämtar alla vårdnadshavare
        {
            Guardian g;
            List<Guardian> guardians = new List<Guardian>();
            string stmt = "SELECT guardian_id, firstname, lastname, phonenumber, address FROM guardian ORDER BY lastname;";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        g = new Guardian()
                        {
                            id = (reader.GetInt32(0)),
                            firstname = (reader.GetString(1)),
                            lastname = (reader.GetString(2)),
                            phonenumber = (reader.GetString(3)),
                            address = (reader.GetString(4))
                        };
                        guardians.Add(g);
                    }
                }
                return guardians;
            }
        }

    }
}
