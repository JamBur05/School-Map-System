using SchoolMapSystem.Models;
using System.Windows;

namespace SchoolMapSystem
{
    public partial class frmEditTimetable : Window
    {
        private ModifyTimetable ModTimetable;
        private string Period1;
        private string Period2;
        private string Period3;
        private string Period4;
        private string Period5;
        private string username;

        public frmEditTimetable(string Username) // Constructor for the Edit Timetable form
        {
            // Intilise username
            username = Username;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Save' button is presesed
        {
            // Get the selected day from the DaySelected combo box
            var Day = cbDaySelected.SelectedItem.ToString();

            // Get the selected periods from the corresponding combo boxes
            if (cmbPeriod1 != null && cmbPeriod2 != null && cmbPeriod3 != null && cmbPeriod4 != null && cmbPeriod5 != null)
            {
                Period1 = cmbPeriod1.SelectedItem?.ToString();
                Period2 = cmbPeriod2.SelectedItem?.ToString();
                Period3 = cmbPeriod3.SelectedItem?.ToString();
                Period4 = cmbPeriod4.SelectedItem?.ToString();
                Period5 = cmbPeriod5.SelectedItem?.ToString();
            }


            // Create a new instance of the ModifyTimetable class and pass the necessary parameters
            ModTimetable = new ModifyTimetable(username, Period1, Period2, Period3, Period4, Period5, Day);

            // Call the TimetableQuery method to update the timetable in the database
            ModTimetable.TimetableQuery();
            MessageBox.Show("Saved to database!");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Return' button is presesed
        {
            frmMainWindow main = new frmMainWindow(username);
            main.Show();
            this.Close();
        }
    }
}
