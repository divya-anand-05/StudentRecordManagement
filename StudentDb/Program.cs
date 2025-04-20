// This is my StudentDb project

using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

namespace StudentDb
{
    class Program
    {
        static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("adminsettings.json")
            .Build();
        static string connectionString = config.GetConnectionString("DefaultConnection");
      

      
        static bool AdminLogin()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-------ADMIN LOGIN-------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Username: ");
            Console.ResetColor();
            string username = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Password: ");
            Console.ResetColor();
            string password = Console.ReadLine();

            string correctUserName = config["Admin:Username"];

            string storedHashedPassword = config["Admin:PasswordHash"];

            string enteredHashedPassword = HashPassword(password);

            if(username==correctUserName&&enteredHashedPassword==storedHashedPassword)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n Login successfull!\n");
                Console.ResetColor();
                return true;
            }

            
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Invalid Credentials. Access denied.");
                Console.ResetColor();
                return false;
            }

        }

        static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        static void Main(string[] args)
        {

            if (!AdminLogin())
            {
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("\n Student Record Management System");
                Console.WriteLine("-----------------------------------");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1.Add Student");
                Console.WriteLine("2.View All Students");
                Console.WriteLine("3.Updates Student");
                Console.WriteLine("4.Delete Student");
                Console.WriteLine("5.Search Student");
                Console.WriteLine("6.Export Students To CSV");
                Console.WriteLine("7.Exit");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Enter Your Choice");
                Console.ResetColor();

                switch (Console.ReadLine())
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ViewStudents();
                        break;
                    case "3":
                        UpdateStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        SearchStudent();
                        break;
                    case "6":
                        ExportStudentsToCSV();
                        break;
                    case "7":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nThank you for using the System. GoodBye!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid  choice.Try again.");
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine("\n Press any key to continue...");
                Console.ReadKey();
            }
        }
        static void AddStudent()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------------------");
            Console.WriteLine("        ADD NEW STUDENT        ");
            Console.WriteLine("-------------------------------");
            Console.ResetColor();

            string name;
            int rollNo;
            string department;
            int marks;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Enter Name: ");
                Console.ResetColor();
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name cannot be empty. Try again.");
                Console.ResetColor();
            }
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Enter Roll Number: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out rollNo) && rollNo > 0)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid RollNo. Enter a valid number greater than 0.");
                Console.ResetColor();
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Enter Department: ");
                Console.ResetColor();
                department = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(department))
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Department cannot be empty. Try again.");
                Console.ResetColor();
            }
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Enter Marks: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out marks))
                    break;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Invalid marks. Enter a numeric value.");
                Console.ResetColor();
            }


            
            SqlConnection conn = new SqlConnection(connectionString);

            string query = "INSERT INTO Students(Name, RollNo, Department, Marks) VALUES(@Name, @RollNo, @Dept, @Marks)";
            SqlCommand cmd = new SqlCommand(query, conn);


            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@RollNo", rollNo);
            cmd.Parameters.AddWithValue("@Dept", department);
            cmd.Parameters.AddWithValue("@Marks", marks);

            try
            {
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Student added Successfully!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to add student");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
            }
        }

        static void ViewStudents()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------------------");
            Console.WriteLine("        STUDENT RECORDS        ");
            Console.WriteLine("-------------------------------");
            Console.ResetColor();

           
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM Students";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\n---Student List---");

                if (!reader.HasRows)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No student records found.");
                    Console.ResetColor();
                }
                else
                {
                    while (reader.Read())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nID:{reader["StudentID"]}");
                        Console.WriteLine($"Name:{reader["Name"]}");
                        Console.WriteLine($"RollNo:{reader["RollNo"]}");
                        Console.WriteLine($"Department:{reader["Department"]}");
                        Console.WriteLine($"Marks:{reader["Marks"]}");
                        Console.ResetColor();

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
            }


        }
        static void UpdateStudent()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------------------");
            Console.WriteLine("        UPDATE STUDENT         ");
            Console.WriteLine("-------------------------------");
            Console.ResetColor();

            int id, rollNO;
            string name, department;
            int marks;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Enter Student ID to Update: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out id) && id > 0)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Id. Try again.");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Enter New Name: ");
            Console.ResetColor();
            name = Console.ReadLine();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Enter New Roll Number: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out rollNO) && rollNO > 0)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid roll number. Try again.");
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Enter New Department: ");
            Console.ResetColor();
            department = Console.ReadLine();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Enter New Marks: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out marks) && marks > 0)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid marks. Try again");
                Console.ResetColor();
            }

           
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "UPDATE Students SET Name=@Name, RollNo=@RollNo, Department=@Dept, Marks=@Marks WHERE STUDENTID=@ID";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@RollNo", rollNO);
            cmd.Parameters.AddWithValue("@Dept", department);
            cmd.Parameters.AddWithValue("@Marks", marks);
            cmd.Parameters.AddWithValue("@ID", id);

            try
            {
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Student record updated successfully!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("NO record found with the given ID.");
                    Console.ResetColor();
                }

            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
            }
        }

        static void DeleteStudent()
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------------------");
            Console.WriteLine("        DELETE STUDENT         ");
            Console.WriteLine("-------------------------------");
            Console.ResetColor();

            int id;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Enter Student ID to Delete: ");
                Console.ResetColor();
                if (int.TryParse(Console.ReadLine(), out id) && id > 0)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid  Id. Try again.");
                Console.ResetColor();


                
                SqlConnection conn = new SqlConnection(connectionString);
                string query = "DELETE FROM Students WHERE StudentID=@ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Student record deleted Successfully!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Student found with the given ID.");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error:", ex.Message);
                    Console.ResetColor();
                }
            }
        }

            static void SearchStudent()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-------------------------------");
                Console.WriteLine("        SEARCH STUDENT         ");
                Console.WriteLine("-------------------------------");
                Console.ResetColor();


                int id;
                string name;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Search by: 1.Student ID 2.Name");
                Console.ResetColor();
                string choice = Console.ReadLine();


                
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (choice == "1")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("Enter Student ID: ");
                    Console.ResetColor();
                    if (int.TryParse(Console.ReadLine(), out id) && id > 0) ;

                    cmd.CommandText = "SELECT * FROM Students WHERE StudentID=@ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                }
                else if (choice == "2")
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Enter Student Name: ");
                    Console.ResetColor();
                    name = Console.ReadLine();
                    cmd.CommandText = "SELECT * FROM Students WHERE Name LIKE @Name";
                    cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice.");
                    Console.ResetColor();
                    return;
                }

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No records found.");
                        Console.ResetColor();
                    }
                    else
                    {
                        while (reader.Read())
                        {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\nID: {reader["StudentID"]}");
                            Console.WriteLine($"Name: {reader["Name"]}");
                            Console.WriteLine($"Roll No: {reader["RollNo"]}");
                            Console.WriteLine($"Department: {reader["Department"]}");
                            Console.WriteLine($"\nMarks: {reader["Marks"]}");
                        Console.ResetColor();
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ResetColor();
                }

            }


        static void ExportStudentsToCSV()
        {

            string filePath = "students.csv";

            
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT * FROM Students", conn);
                        SqlDataReader reader = cmd.ExecuteReader();

                        using (StreamWriter writer = new StreamWriter("students.csv"))
                        {
                            writer.WriteLine("RollNo,Name,Department,Marks");

                            while (reader.Read())
                            {
                                writer.WriteLine($"{reader["RollNo"]},{reader["Name"]},{reader["Department"]},{reader["Marks"]}");
                            }
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nStudent data exported to 'students.csv'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nError: {ex.Message}");
                Console.ResetColor();
                }
                
            }


        }
    }
  