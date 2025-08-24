using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AuthSystemApp
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            // Reset errors
            ResetField(txtName, txtNameError);
            ResetField(txtEmail, txtEmailError);
            ResetField(txtPassword, txtPasswordError, borderPassword);
            ResetField(txtConfirmPassword, txtConfirmPasswordError, borderConfirmPassword);

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim().ToLower();
            txtEmail.Text = email;
            string password = txtPassword.Visibility == Visibility.Visible ? txtPassword.Password : txtPasswordPlain.Text;
            string confirmPassword = txtConfirmPassword.Visibility == Visibility.Visible ? txtConfirmPassword.Password : txtConfirmPasswordPlain.Text;

            bool hasError = false;

            // Validate Name
            if (string.IsNullOrEmpty(name))
            {
                SetError(txtName, txtNameError, "Name is required");
                hasError = true;
            }

            // Validate Email
            if (string.IsNullOrEmpty(email))
            {
                SetError(txtEmail, txtEmailError, "Email is required");
                hasError = true;
            }
            else if (!IsValidEmail(email))
            {
                SetError(txtEmail, txtEmailError, "Invalid email format (e.g., user@gmail.com)");
                hasError = true;
            }

            // Validate Password
            if (string.IsNullOrEmpty(password))
            {
                SetError(txtPassword, txtPasswordError, "Password is required", borderPassword);
                hasError = true;
            }
            else if (!IsStrongPassword(password))
            {
                SetError(txtPassword, txtPasswordError,
                    "Password must be at least 8 chars, include uppercase,\nlowercase, digit & special char", borderPassword);
                hasError = true;
            }

            // Validate Confirm Password
            if (string.IsNullOrEmpty(confirmPassword))
            {
                SetError(txtConfirmPassword, txtConfirmPasswordError, "Confirm Password is required", borderConfirmPassword);
                hasError = true;
            }
            else if (password != confirmPassword)
            {
                SetError(txtConfirmPassword, txtConfirmPasswordError, "Passwords do not match", borderConfirmPassword);
                hasError = true;
            }

            if (hasError) return;

            DatabaseHelper db = new DatabaseHelper();
            if (db.IsEmailExists(email))
            {
                SetError(txtEmail, txtEmailError, "Email already registered");
                return;
            }

            db.SaveUser(name, email, password);
            MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear fields
            txtName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtPasswordPlain.Clear();
            txtConfirmPasswordPlain.Clear();
            btnTogglePassword.Content = "🙈";
            btnToggleConfirmPassword.Content = "🙈";
            txtPassword.Visibility = Visibility.Visible;
            txtPasswordPlain.Visibility = Visibility.Collapsed;
            txtConfirmPassword.Visibility = Visibility.Visible;
            txtConfirmPasswordPlain.Visibility = Visibility.Collapsed;
            borderPassword.BorderBrush = Brushes.Gray;
            borderConfirmPassword.BorderBrush = Brushes.Gray;
        }

        private void SetError(Control control, TextBlock errorTextBlock, string errorMessage, Border border = null)
        {
            control.Tag = errorMessage;
            if (border == null)
            {
                border = control.Template?.FindName("Border", control) as Border ?? FindWrapperBorder(control);
            }
            if (border != null) border.BorderBrush = Brushes.Red;
            if (errorTextBlock != null) errorTextBlock.Text = errorMessage;
        }

        private void ResetField(Control control, TextBlock errorTextBlock, Border border = null)
        {
            control.Tag = null;
            if (border == null)
            {
                border = control.Template?.FindName("Border", control) as Border ?? FindWrapperBorder(control);
            }
            if (border != null) border.BorderBrush = Brushes.Gray;
            if (errorTextBlock != null) errorTextBlock.Text = null;
        }

        private Border FindWrapperBorder(Control control)
        {
            var parent = VisualTreeHelper.GetParent(control);
            if (parent is Grid grid) return grid.Children.OfType<Border>().FirstOrDefault();
            while (parent != null && !(parent is Border)) parent = VisualTreeHelper.GetParent(parent);
            return parent as Border;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-z0-9._%+-]+@(gmail\.com|yahoo\.com|outlook\.com)$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private bool IsStrongPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e) => ResetField(txtName, txtNameError);

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Save current caret position
            int caretIndex = txtEmail.CaretIndex;

            // Clean input: remove spaces + convert to lowercase
            string processed = txtEmail.Text.Replace(" ", "").ToLower();

            // Only update if text actually changed
            if (txtEmail.Text != processed)
            {
                txtEmail.Text = processed;

                // Restore caret safely
                txtEmail.CaretIndex = Math.Min(processed.Length, caretIndex);
            }

            // ✅ Clear error message immediately when user types
            ResetField(txtEmail, txtEmailError);
        }



        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ResetField(txtPassword, txtPasswordError, borderPassword);
            if (txtPasswordPlain.Visibility == Visibility.Visible)
                txtPasswordPlain.Text = txtPassword.Password;
        }

        private void TxtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ResetField(txtConfirmPassword, txtConfirmPasswordError, borderConfirmPassword);
            if (txtConfirmPasswordPlain.Visibility == Visibility.Visible)
                txtConfirmPasswordPlain.Text = txtConfirmPassword.Password;
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtPasswordPlain.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordPlain.Visibility = Visibility.Visible;
                btnTogglePassword.Content = "🙉";
                txtPasswordPlain.Focus();
                txtPasswordPlain.CaretIndex = txtPasswordPlain.Text.Length;
            }
            else
            {
                txtPassword.Password = txtPasswordPlain.Text;
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordPlain.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "🙈";
                txtPassword.Focus();
            }
        }

        private void BtnToggleConfirmPassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtConfirmPassword.Visibility == Visibility.Visible)
            {
                txtConfirmPasswordPlain.Text = txtConfirmPassword.Password;
                txtConfirmPassword.Visibility = Visibility.Collapsed;
                txtConfirmPasswordPlain.Visibility = Visibility.Visible;
                btnToggleConfirmPassword.Content = "🙉";
                txtConfirmPasswordPlain.Focus();
                txtConfirmPasswordPlain.CaretIndex = txtConfirmPasswordPlain.Text.Length;
            }
            else
            {
                txtConfirmPassword.Password = txtConfirmPasswordPlain.Text;
                txtConfirmPassword.Visibility = Visibility.Visible;
                txtConfirmPasswordPlain.Visibility = Visibility.Collapsed;
                btnToggleConfirmPassword.Content = "🙈";
                txtConfirmPassword.Focus();
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow();
            signInWindow.Show();
            this.Close();
        }
    }
}
