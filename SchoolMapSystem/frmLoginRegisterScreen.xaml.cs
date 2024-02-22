using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace SchoolMapSystem
{
    /// <summary>
    /// Interaction logic for LoginRegisterScreen.xaml
    /// </summary>
    public partial class frmLoginRegisterScreen : Window
    {

        private string username;
        private string password;
        private bool userFound;
        private LoginRegister loginprocess;
        private int loginAttempts = 0;

        public frmLoginRegisterScreen()
        {
            InitializeComponent();
            loginprocess = new LoginRegister(); // Login and Register operations
        }


        private void Login_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Login' button is presesed
        {
            // Get the values entered in the username and password textboxes
            username = txtUsernameBox.Text;
            password = txtPasswordBox.Password;

            // Create a new instance of the AppDBContext class
            using (AppDBContext db = new AppDBContext())
            {
                // Check if there is a user in the database with the entered username and password
                userFound = db.tblUserDetails.Any(user => user.Username == username && user.Password == password);

                if (userFound == true) // If the user exists
                {
                    // Create a new instance of the frmMainWindow form and pass in the username
                    frmMainWindow main = new frmMainWindow(username);
                    // Show main window and close current window
                    main.Show();
                    this.Close();
                }
                else // If the user does not exist
                {
                    loginAttempts++;
                    // Display an error message to the user
                    MessageBox.Show("Invalid credentials.");
                }
            }

            if (loginAttempts == 3)
            {
                MessageBox.Show("You have had 3 attempts. The program will now close.");
                this.Close();
            }
        }

    

        private void Register_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Register' button is presesed
        {
            // Get the values entered in the username and password textboxes
            username = txtUsernameBox.Text;
            password = txtPasswordBox.Password;

            // Converts SecureString to plain text string
            IntPtr ptr = Marshal.SecureStringToBSTR(txtPasswordBox.SecurePassword);
            string plainPassword = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);


            if (loginprocess.PasswordStrengthCheck(plainPassword)) // Check if password is strong enough
            {
                if (loginprocess.EmailIsValid(username))  // Checks if the username matches the college regular expression.
                {
                    // Create account in the database and show message
                    loginprocess.CreateAccount(username, plainPassword);
                    MessageBox.Show("Registered!");
                    // Open the edit timetable form for the new user and close this form
                    frmEditTimetable details = new frmEditTimetable(username);
                    details.Show();
                    this.Close();
                }
                else // Regex failed
                {
                    MessageBox.Show("Your username must be your college email, with the correct domain.");
                }
            }
            else // Password strength fail
            {
                MessageBox.Show("Password not strong enough...");
            }
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e) // This method allows the window to be moved by clicking and dragging the mouse
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e) // Closes the application on click
        {
            Close();
        }
    }
}
