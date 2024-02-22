using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchoolMapSystem
{
    class LoginRegister
    {
        private bool isStrong;
        private char[] CAPITALS;
        private char[] LOWERCASE;
        private char[] NUMBERS;
        private char[] SPECIALS;
        private string storedUsername;
        public LoginRegister() // Constructor, intilises the constants used for checking password strength
        {
            isStrong = false;
            CAPITALS = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            LOWERCASE = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            NUMBERS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            SPECIALS = new char[] { '!', '£', '"', '#', '$', '%', '&', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', ';', ']', '^', '_', '`', '{', '|', '}', '~' };
        }

        public void GrantAccess() // Open the main window if access is granted
        {
            frmMainWindow main = new frmMainWindow(storedUsername);
            main.Show();
        }

        public bool PasswordStrengthCheck(string password) // Checks the strength of the password given based on certian factors.
        {

            if (CAPITALS.Any(password.Contains))
            {
                if (NUMBERS.Any(password.Contains))
                {
                    if (SPECIALS.Any(password.Contains))
                    {
                        if (LOWERCASE.Any(password.Contains))
                        {
                            if (password.Length >= 8)
                            {
                                if (password != "")
                                {
                                    return isStrong = true;
                                }
                            }
                        }
                    }
                }
            }
            return isStrong;
        }

        public void CreateAccount(string username, string password) // Method for creating an account and storing it in the database
        {
            storedUsername = username;

            // Create a new instance of the AppDBContext using a using block for automatic disposal
            using (AppDBContext db = new AppDBContext())
            {

                // Create a new UserInfo object with the provided username and password
                UserInfo userinfo = new UserInfo()
                {
                    Username = username,
                    Password = password
                };
                // Add the new user to the UserDetails table in the database and save changes
                db.tblUserDetails.Add(userinfo);
                Debug.WriteLine("Before SaveChanges()");
                db.SaveChanges();
                Debug.WriteLine("After SaveChanges()");
                // Get the ID of the newly created user
                var userID = db.tblUserDetails.Single(u => u.Username == username).UserID;
                // Calculate the starting ID for the user's timetable
                int ttablestart = userID * 5;

                // Construct a parameterised SQL query to add the user's timetable to the database
                var timetablequery = "INSERT INTO tblTimetable (TimetableID, UserID, Day)" +
                        "VALUES" +
                        "(@ts1, @userID, 'Monday')," +
                        "(@ts2, @userID, 'Tuesday')," +
                        "(@ts3, @userID, 'Wednesday')," +
                        "(@ts4, @userID, 'Thursday')," +
                        "(@ts5, @userID, 'Friday')";

                //Create parameters
                var parameters = new[] {
                    new SqliteParameter("@ts1", ttablestart),
                    new SqliteParameter("@ts2", ttablestart + 1),
                    new SqliteParameter("@ts3", ttablestart + 2),
                    new SqliteParameter("@ts4", ttablestart + 3),
                    new SqliteParameter("@ts5", ttablestart + 4),
                    new SqliteParameter("@userID", userID)
                };


                // Ensure the database is created and execute the SQL query
                db.Database.EnsureCreated();
                db.Database.ExecuteSqlRaw(timetablequery, parameters);
            }
        }

        public bool EmailIsValid(string input)
        {
            // Define the regular expression pattern
            string pattern = "^[a-zA-Z]{2}[0-9]{5}@woking.ac.uk$";

            // Create a Regex object to check if the input matches the pattern
            Regex regex = new Regex(pattern);

            // Check if the input matches the pattern
            bool isMatch = regex.IsMatch(input);

            // Return the result
            return isMatch;
        }
    }
}
