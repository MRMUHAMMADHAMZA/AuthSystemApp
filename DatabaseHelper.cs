using System;
using System.Data.SqlClient;
using System.Windows;

namespace AuthSystemApp
{
    public class DatabaseHelper
    {
        private string connectionString = @"Server=DESKTOP-6RDGPRS\SQLEXPRESS;Database=AuthSystemDB;Trusted_Connection=True;";

        // Save user with hashed password
        public void SaveUser(string name, string email, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(password)); // HASHED
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message);
            }
        }

        // Check if email already exists
        public bool IsEmailExists(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking email: " + ex.Message);
                return false;
            }
        }

        // Check login credentials (email & password)
        public bool CheckCredentials(string email, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT Password FROM Users WHERE Email=@Email";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string hashedPassword = result.ToString();
                            return hashedPassword == PasswordHelper.HashPassword(password);
                        }
                        else
                        {
                            return false; // Email not found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking credentials: " + ex.Message);
                return false;
            }
        }
    }
}
