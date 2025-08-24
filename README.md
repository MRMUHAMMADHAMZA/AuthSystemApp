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

## 📂 Project Structure  
AuthSystemApp/
│── SignInWindow.xaml / .cs # Login screen
│── SignUpWindow.xaml / .cs # Registration screen
│── MainWindow.xaml / .cs # Dashboard after login
│── DatabaseHelper.cs # SQL operations
│── PasswordHelper.cs # SHA256 hashing
│── Converters.cs # Value converters (error handling)
│── App.xaml / App.xaml.cs # App entry point
