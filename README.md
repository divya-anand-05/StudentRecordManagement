# 🎓 Student Record Management System (C# Console Application)

This is a simple yet functional **Student Record Management System** developed in **C# (Console App)** with **SQL Server** database integration. The project allows users to manage student information with full **CRUD operations** using **ADO.NET**.

---

## 🧠 Features

- ✅ Add new student records  
- ✅ View all student data  
- ✅ Update existing student details  
- ✅ Delete student records  
- ✅ Search student by ID or name  
- ✅ Connects to SQL Server using ADO.NET

---

## 🛠 Technologies Used

- **C#** (.NET 6 or higher)
- **SQL Server**
- **ADO.NET**
- **Visual Studio**
- **Console Application**

---

## 🗂️ Folder Structure

StudentRecordManagement/ 

├── Program.cs

├── Database Table: Students

└── README.md 


---

## 🧪 How to Run the Project

1. **Clone this repo**  
   ```bash
   git clone https://github.com/divya-anand-05/StudentRecordManagement.git


2.**Open in Visual Studio**

    Open StudentRecordManagement.sln

3.**Set up your SQL Server database**
Run this script in SSMS:

CREATE DATABASE StudentDB;

GO

USE StudentDB;

CREATE TABLE Students (

    StudentID INT PRIMARY KEY IDENTITY,
    
    Name NVARCHAR(100),
    
    RollNo NVARCHAR(50),
    
    Department NVARCHAR(50),
    
    Marks INT

);

4.**Update your connection string in Program.cs**
Replace with your correct local SQL connection:

string connectionString = "Data Source=.;Initial Catalog=StudentDB;Integrated Security=True";

5.**Build and Run**

Use the console menu to perform CRUD operations

## 📷 Sample Output

Student Record Management System
1. Add Student
2. View All Students
3. Update Student
4. Delete Student
5. Search Student
6. Exit

## 👩‍💻 Developer

Divyadharshini Anandhakumar

Aspiring Web & .NET Developer

📍 Dindigul, Tamil Nadu

📧 divyadharshinianandhakumar@gmail.com

    🔗 GitHub

    🌐 Portfolio

    💼 LinkedIn

## 🙌 Support or Feedback

If you found this helpful or have suggestions, feel free to ⭐ star the repo and connect with me on LinkedIn!

