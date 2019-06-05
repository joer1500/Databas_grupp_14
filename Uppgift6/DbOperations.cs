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

        public List<Needs> GetNeedsFromSchoolchildID(int ID)
        {
            Needs needs;
            List<Needs> needsList = new List<Needs>();

            string stmt = "SELECT need_id, schoolchild_id, need FROM schoolchild_need WHERE schoolchild_id = @id";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("id", ID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            needs = new Needs()
                            {
                                id = reader.GetInt32(0),
                                child_id = reader.GetInt32(1),
                                need = reader.GetString(2),
                            };
                            needsList.Add(needs);
                        }
                    }
                    return needsList;
                }
            }
        }

        public void AddNeed(string need, int schoolchildID)
        {
            string stmt = "INSERT INTO schoolchild_need (schoolchild_id, need) VALUES (@schoolchild_id, @need)";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("schoolchild_id", schoolchildID);
                    cmd.Parameters.AddWithValue("need", need);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteNeed(int needID)
        {
            string stmt = "DELETE FROM schoolchild_need WHERE need_id = @need_id";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("need_id", needID);
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
                            //section_id = (reader.GetInt32(11)),
                            //attendance = (reader.GetString(12)),
                            //sick = (reader.GetString(13))
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

        //public List<Staff> GetAllStaffOrderByFirstname() // Används ej Hämtar alla staffs och sorterar på förnamn
        //{
        //    Staff s;
        //    List<Staff> staffs = new List<Staff>();

        //    string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY firstname;";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                s = new Staff()
        //                {
        //                    staffID = (reader.GetInt32(0)),
        //                    firstname = (reader.GetString(1)),
        //                    lastname = (reader.GetString(2)),
        //                    profession = (reader.GetString(3)),
        //                    sectionname = (reader.GetString(4)),
        //                    sectionid = (reader.GetInt32(5))
        //                };
        //                staffs.Add(s);
        //            }
        //        }
        //        return staffs;
        //    }
        //}

        //public List<Staff> GetAllStaffOrderByLastname() // Används ej Hämtar alla staffs och sorterar på efternamn
        //{
        //    Staff s;
        //    List<Staff> staffs = new List<Staff>();

        //    string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY lastname;";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                s = new Staff()
        //                {
        //                    staffID = (reader.GetInt32(0)),
        //                    firstname = (reader.GetString(1)),
        //                    lastname = (reader.GetString(2)),
        //                    profession = (reader.GetString(3)),
        //                    sectionname = (reader.GetString(4)),
        //                    sectionid = (reader.GetInt32(5))
        //                };
        //                staffs.Add(s);
        //            }
        //        }
        //        return staffs;
        //    }
        //}

        //public List<Staff> GetAllStaffOrderBy(string orderby) // Används ej. Under konstruktion
        //{
        //    Staff s;
        //    List<Staff> staffs = new List<Staff>();

        //    //string stmtold = "$SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY {orderby}";
        //    string stmt = "SELECT staff_id, firstname, lastname, profession, section_name FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY @order";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand())
        //        {
        //            cmd.Connection = conn;
        //            cmd.CommandText = stmt;
        //            cmd.Parameters.AddWithValue("order", orderby);
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    s = new Staff()
        //                    {
        //                        staffID = (reader.GetInt32(0)),
        //                        firstname = (reader.GetString(1)),
        //                        lastname = (reader.GetString(2)),
        //                        profession = (reader.GetString(3)),
        //                        sectionname = (reader.GetString(4))
        //                    };
        //                    staffs.Add(s);
        //                }
        //            }
        //        }
        //        return staffs;
        //    }
        //}

        //public List<Schoolchild> GetSchoolchildrenOrderByLastname() // Används ej Hämtar alla skolbarn och sorterar på efternamn
        //{
        //    Schoolchild schoolchild;
        //    List<Schoolchild> schoolchildren = new List<Schoolchild>();

        //    string stmt = "SELECT schoolchild_id, lastname, firstname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY lastname ASC";
        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                schoolchild = new Schoolchild
        //                {
        //                    id = reader.GetInt32(0),
        //                    lastname = reader.GetString(1),
        //                    firstname = reader.GetString(2),
        //                    section = reader.GetString(3)
        //                };
        //                schoolchildren.Add(schoolchild);
        //            }
        //        }
        //        return schoolchildren;
        //    }
        //}

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

        //public List<Schoolchild> GetSchoolchildrenOrderByFirstname() // Används ej Hämtar skolbarn och sorterar efter förnamn
        //{
        //    Schoolchild schoolchild;
        //    List<Schoolchild> schoolchildren = new List<Schoolchild>();

        //    string stmt = "SELECT schoolchild_id, lastname, firstname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY firstname ASC";
        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                schoolchild = new Schoolchild
        //                {
        //                    id = reader.GetInt32(0),
        //                    lastname = reader.GetString(1),
        //                    firstname = reader.GetString(2),
        //                    section = reader.GetString(3)
        //                };
        //                schoolchildren.Add(schoolchild);
        //            }
        //        }
        //        return schoolchildren;
        //    }
        //}

        //public List<Schoolchild> GetSchoolchildrenOrderBySection() // Används ej Hämtar skolbarn och sorterar efter avdelning
        //{
        //    Schoolchild schoolchild;
        //    List<Schoolchild> schoolchildren = new List<Schoolchild>();

        //    string stmt = "SELECT schoolchild_id, lastname, firstname, section_name FROM schoolchild INNER JOIN section ON schoolchild.section_id = section.section_id ORDER BY section.section_id";
        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                schoolchild = new Schoolchild
        //                {
        //                    id = reader.GetInt32(0),
        //                    lastname = reader.GetString(1),
        //                    firstname = reader.GetString(2),
        //                    section = reader.GetString(3)
        //                };
        //                schoolchildren.Add(schoolchild);
        //            }
        //        }
        //        return schoolchildren;
        //    }
        //}

        public Schoolchild GetSchoolchildByID(int id)
        {
            string stmt = "SELECT * FROM schoolchild WHERE schoolchild_id = @schoolchild_id";
            Schoolchild schoolchild = new Schoolchild();

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("schoolchild_id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schoolchild.id = reader.GetInt32(0);
                            schoolchild.firstname = reader.GetString(1);
                            schoolchild.lastname = reader.GetString(2);
                            schoolchild.sectionID = reader.GetInt32(3);
                        }
                    }

                }
            }
            return schoolchild;
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

        public void UpdateSchoolchild(int id, string firstname, string lastname, int sectionID)
        {
            string stmt = "UPDATE schoolchild SET (firstname, lastname, section_id) = (@firstname, @lastname, @section_id) WHERE schoolchild_id = @id";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("firstname", firstname);
                    cmd.Parameters.AddWithValue("lastname", lastname);
                    cmd.Parameters.AddWithValue("section_id", sectionID);
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

        public void ConnectGuardianSchoolchild(int schoolchild_id, int guardian_id)       // koppla ihop barn med vårdnadshavare
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

        //public List<Staff> GetAllStaffOrderByProfession() //Används ej, kan tas bort. Hämtar alla staffs och sorterar på roll
        //{
        //    Staff s;
        //    List<Staff> staffs = new List<Staff>();

        //    string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY profession ASC;";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                s = new Staff()
        //                {
        //                    staffID = (reader.GetInt32(0)),
        //                    firstname = (reader.GetString(1)),
        //                    lastname = (reader.GetString(2)),
        //                    profession = (reader.GetString(3)),
        //                    sectionname = (reader.GetString(4)),
        //                    sectionid = (reader.GetInt32(5))
        //                };
        //                staffs.Add(s);
        //            }
        //        }
        //        return staffs;
        //    }
        //}

        //public List<Staff> GetAllStaffOrderBySection() //Används ej, Hämtar alla staffs och sorterar på avdelning
        //{
        //    Staff s;
        //    List<Staff> staffs = new List<Staff>();

        //    string stmt = "SELECT staff_id, firstname, lastname, profession, section_name, staff.section_id FROM staff INNER JOIN section on staff.section_id = section.section_id ORDER BY section.section_id;";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                s = new Staff()
        //                {
        //                    staffID = (reader.GetInt32(0)),
        //                    firstname = (reader.GetString(1)),
        //                    lastname = (reader.GetString(2)),
        //                    profession = (reader.GetString(3)),
        //                    sectionname = (reader.GetString(4)),
        //                    sectionid = (reader.GetInt32(5))
        //                };
        //                staffs.Add(s);
        //            }
        //        }
        //        return staffs;
        //    }
        //}

        public List<Pickup> GetAllAllowedPickupBySchoolchildID(int schoolchildID)    // Hämtar alla godkända hämtare för ett barn
        {
            Pickup pickup;
            List<Pickup> pickups = new List<Pickup>();

            string stmt = "SELECT pickup_id, pickup_firstname, pickup_lastname, pickup_relation FROM schoolchild_pickup WHERE schoolchild_id = @schoolchild_id";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("schoolchild_id", schoolchildID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pickup = new Pickup()
                            {
                                PickupID = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2),
                                Relation = reader.GetString(3)
                            };
                            pickups.Add(pickup);
                        }
                    }
                    return pickups;
                }
            }
        }

        public void AddNewPickup(int schoolchildID, string firstname, string lastname, string relation)   // Lägger till person som får hämta
        {
            string stmt = "INSERT INTO schoolchild_pickup(schoolchild_id, pickup_firstname, pickup_lastname, pickup_relation) VALUES (@schoolchild_id, @firstname, @lastname, @relation)";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("schoolchild_id", schoolchildID);
                    cmd.Parameters.AddWithValue("firstname", firstname);
                    cmd.Parameters.AddWithValue("lastname", lastname);
                    cmd.Parameters.AddWithValue("relation", relation);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePickup (int pickupID)    // Tar bort person som får hämta
        {
            string stmt = "DELETE FROM schoolchild_pickup WHERE pickup_id = @pickup_id";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("pickup_id", pickupID);
                    cmd.ExecuteNonQuery();
                }
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


        //public List<Schoolchild> GetSchoolchildsFromGuardian(int id) //används ej, ska tas bort
        //{
        //    Schoolchild sc;
        //    List<Schoolchild> children = new List<Schoolchild>();

        //    string stmt = $"SELECT schoolchild.firstname, schoolchild.lastname FROM guardian_schoolchild INNER JOIN schoolchild on guardian_schoolchild.schoolchild_id = schoolchild.schoolchild_id WHERE guardian_id = @gid";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand())
        //        {
        //            cmd.Connection = conn;
        //            cmd.CommandText = stmt;
        //            cmd.Parameters.AddWithValue("gid", id);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    sc = new Schoolchild()
        //                    {                               
        //                        firstname = (reader.GetString(0)),
        //                        lastname = (reader.GetString(1))
                                
        //                    };
        //                    children.Add(sc);
        //                }
        //            }
        //            return children;
        //        }
        //    }
        //}


        //public List<Guardian> GetAllGuardiansOrderbyFirstname() // Används ej Hämtar alla vårdnadshavare
        //{
        //    Guardian g;
        //    List<Guardian> guardians = new List<Guardian>();
        //    string stmt = "SELECT guardian_id, firstname, lastname, phonenumber, address FROM guardian ORDER BY firstname;";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                g = new Guardian()
        //                {
        //                    id = (reader.GetInt32(0)),
        //                    firstname = (reader.GetString(1)),
        //                    lastname = (reader.GetString(2)),
        //                    phonenumber = (reader.GetString(3)),
        //                    address = (reader.GetString(4))
        //                };
        //                guardians.Add(g);
        //            }
        //        }
        //        return guardians;
        //    }
        //}

        //public List<Guardian> GetAllGuardiansOrderbyLastname() // Används ej Hämtar alla vårdnadshavare
        //{
        //    Guardian g;
        //    List<Guardian> guardians = new List<Guardian>();
        //    string stmt = "SELECT guardian_id, firstname, lastname, phonenumber, address FROM guardian ORDER BY lastname;";

        //    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new NpgsqlCommand(stmt, conn))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                g = new Guardian()
        //                {
        //                    id = (reader.GetInt32(0)),
        //                    firstname = (reader.GetString(1)),
        //                    lastname = (reader.GetString(2)),
        //                    phonenumber = (reader.GetString(3)),
        //                    address = (reader.GetString(4))
        //                };
        //                guardians.Add(g);
        //            }
        //        }
        //        return guardians;
        //    }
        //}


        public List<Schoolchild> GetSchoolchildrenFromSelectedSection(int section) // Hämtar skolbarn utifrån avdelning
        {
            Schoolchild schoolchild;
            List<Schoolchild> schoolchildren = new List<Schoolchild>();

            string stmt = "SELECT section_id, firstname, lastname FROM schoolchild WHERE section_id = @sectionid";
            using (var conn = new
                NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("sectionid", section);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schoolchild = new Schoolchild
                            {
                                id = reader.GetInt32(0),
                                firstname = reader.GetString(1),
                                lastname = reader.GetString(2),
                            };
                            schoolchildren.Add(schoolchild);
                        }
                    }
                    return schoolchildren;
                }
            }
        }

        public void DeleteConnectionGuardianSchoolchild(int schoolchild_id, int guardian_id)//Ta bort koppling mellan barn och vårdnadshavare
        {
            string stmt = "DELETE FROM guardian_schoolchild WHERE guardian_id = @gid AND schoolchild_id = @sid";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("gid", guardian_id);
                    cmd.Parameters.AddWithValue("sid", schoolchild_id);
                    cmd.Parameters.AddWithValue("guardian_id", guardian_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Attendance> GetAttendances() //Hämtar närvaro-lista
        {

            Attendance att;
            List<Attendance> attendances = new List<Attendance>();

            string stmt = "SELECT attendance.attendance_id, attendance.schoolchild_id, attendance.date, attendance.attendance, attendance.sick, attendance.attendance_staff, schoolchild.firstname, schoolchild.lastname, schoolchild.section_id FROM attendance INNER JOIN schoolchild on attendance.schoolchild_id = schoolchild.schoolchild_id ORDER BY lastname";
            using (var conn = new
                NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            att = new Attendance
                            {
                                id = (reader.GetInt32(0)),
                                schoolchild = (reader.GetInt32(1)),
                                date = (reader.GetDateTime(2)),
                                attendance = (reader.GetString(3)),
                                sick = (reader.GetString(4)),
                                staff = (reader.GetInt32(5)),
                                firstname = (reader.GetString(6)),
                                lastname = (reader.GetString(7)),
                                section_id = (reader.GetInt32(8))
                            };
                            attendances.Add(att);
                        }
                    }
                    return attendances;
                }
            }
        }
        public void AddNewAttendance(int schoolchild, DateTime date, string sick, string attendance, int staff)
        {
            string stmt = "INSERT INTO attendance(schoolchild_id, date, sick, attendance, attendance_staff) VALUES (@sid, @date, @sick, @att, @staff)";

            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("sid", schoolchild);
                    cmd.Parameters.AddWithValue("date", date);
                    cmd.Parameters.AddWithValue("sick", sick);
                    cmd.Parameters.AddWithValue("att", attendance);
                    cmd.Parameters.AddWithValue("staff", staff);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateAttendance(int schoolchild, DateTime date, string sick, string attendance)
        {
            string stmt = "UPDATE attendance SET (schoolchild_id, date, sick, attendance) = (@id, @dt, @sick, @att) WHERE schoolchild_id = @id AND date = @dt;";

            using (var conn = new
            NpgsqlConnection(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = stmt;
                    cmd.Parameters.AddWithValue("id", schoolchild);
                    cmd.Parameters.AddWithValue("dt", date);
                    cmd.Parameters.AddWithValue("sick", sick);
                    cmd.Parameters.AddWithValue("att", attendance);
                    cmd.ExecuteNonQuery();
                }
            }
        }


       
    }


}
