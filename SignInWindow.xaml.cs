using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AuthSystemApp
{
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            ResetField(txtEmail, txtEmailError);
            ResetField(txtPassword, txtPasswordError);

            string email = txtEmail.Text.Trim().ToLower();
            txtEmail.Text = email;

            string password = txtPassword.Visibility == Visibility.Visible ? txtPassword.Password : txtPasswordPlain.Text;

            bool hasError = false;

            // Email validation
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

            // Password validation
            if (string.IsNullOrEmpty(password))
            {
                SetError(txtPassword, txtPasswordError, "Password is required");
                hasError = true;
            }

            if (hasError) return;

            DatabaseHelper db = new DatabaseHelper();

            if (db.CheckCredentials(email, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetError(Control control, TextBlock errorTextBlock, string errorMessage)
        {
            control.Tag = errorMessage;

            if (control == txtPassword || control == txtPasswordPlain)
            {
                passwordBorder.BorderBrush = Brushes.Red;
            }
            else if (control is TextBox tb)
            {
                var border = (Border)tb.Template.FindName("border", tb);
                if (border != null)
                    border.BorderBrush = Brushes.Red;
            }

            if (errorTextBlock != null)
                errorTextBlock.Text = errorMessage;
        }

        private void ResetField(Control control, TextBlock errorTextBlock)
        {
            control.Tag = null;

            if (control == txtPassword || control == txtPasswordPlain)
            {
                passwordBorder.BorderBrush = Brushes.Gray;
            }
            else if (control is TextBox tb)
            {
                var border = (Border)tb.Template.FindName("border", tb);
                if (border != null)
                    border.BorderBrush = Brushes.Gray;
            }

            if (errorTextBlock != null)
                errorTextBlock.Text = null;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-z0-9._%+-]+@(gmail\.com|yahoo\.com|outlook\.com)$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Save caret position
            int caretIndex = txtEmail.CaretIndex;

            // Process text: remove spaces + lowercase
            string processed = txtEmail.Text.Replace(" ", "").ToLower();

            // Only update if text actually changed
            if (txtEmail.Text != processed)
            {
                txtEmail.Text = processed;

                // Restore caret safely
                txtEmail.CaretIndex = Math.Min(processed.Length, caretIndex);
            }

            // ✅ Clear error as soon as user starts typing
            ResetField(txtEmail, txtEmailError);
        }



        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ResetField(txtPassword, txtPasswordError);
            if (txtPasswordPlain.Visibility == Visibility.Visible)
                txtPasswordPlain.Text = txtPassword.Password;
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

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SignUpWindow signUp = new SignUpWindow();
            signUp.Show();
            this.Close();
        }
    }
}
