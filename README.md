# AuthSystemApp 🔐  

A **WPF Authentication System** built with **C# (.NET Framework / .NET Core)** and **SQL Server**.  
This project provides **Sign Up** and **Sign In** functionality with proper validation, password hashing, and database integration.  

---

## 🚀 Features  
- **User Registration (Sign Up)**  
  - Name, Email, Password, Confirm Password fields  
  - Validates required fields  
  - Strong password policy  
  - Email format validation (`gmail.com`, `yahoo.com`, `outlook.com`)  
  - Prevents duplicate email registration  

- **User Login (Sign In)**  
  - Email & Password validation  
  - Password hashing with **SHA256**  
  - Error messages for invalid credentials  

- **UI Features**  
  - Clean WPF UI with rounded corners  
  - Toggle Password (🙈 / 🙉) visibility  
  - Real-time validation (errors clear while typing)  

- **Database (SQL Server)**  
  - `Users` table with `Id, Name, Email, Password, CreatedAt`  
  - Passwords stored securely (SHA256 hashed)  

---

## 🛠️ Technologies Used  
- **C# WPF**  
- **.NET Framework / .NET Core**  
- **SQL Server (MSSQL)**  
- **SHA256 Password Hashing**  

---

## 🗄️ Database Code  

Run this script in **SQL Server Management Studio (SSMS):**  

```sql
CREATE DATABASE AuthSystemDB;

USE AuthSystemDB;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- View all users
SELECT * FROM Users;      
