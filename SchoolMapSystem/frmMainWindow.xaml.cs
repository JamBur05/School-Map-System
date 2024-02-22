using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using SchoolMapSystem.Models;

namespace SchoolMapSystem
{
    public partial class frmMainWindow : Window
    {
        private Dijkstra PathFinder;
        private string storedUsername;
        private Stack NodeStack;
        private NodeDictionary nodeDict;
        private Stack PeriodStack;
        private GenereateTimetable tt;
        private int periodCounter = 0;
        private PDFSaver pdf;
        private EmailSend email;
        private Rectangle node;
        private DayOfWeek wk;


        public frmMainWindow(string userName)   // Constructor for the main window of the application
        {
            InitializeComponent();
            wk = DateTime.Today.DayOfWeek;
            NodeStack = new Stack(24);  // Stack to store nodes in final path
            PathFinder = new Dijkstra();    // Dijkstra class to find shortest path
            pdf = new PDFSaver();   // PDF Generator
            storedUsername = userName;  // Username currently in use
            tt = new GenereateTimetable();  // Path generator based on user timetable
            email = new EmailSend();    // Email sender
            PeriodStack = tt.TimetableGen(userName);    // Generate the lessons in the users timetable
            nodeDict = new NodeDictionary(NodeA1, NodeA2, NodeA3, NodeA4, NodeA5, NodeA6, NodeA7, NodeA8, NodeA9, NodeA10, NodeA11, NodeA12, NodeA13, NodeA14, NodeA15, NodeA16, NodeA17, NodeA18, NodeA19, NodeA20, NodeA21, NodeA22, NodeA23);    // Create the dictionary for the nodes
            
            if(userName != "admin")
            {
                lblUserName.Content = userName.Remove(userName.Length - 13);    // Welcome the user, with the appropriate username
            }
            else
            {
                lblUserName.Content = "Admin";
            }

            lblDayOfWeek.Content = "Today is " + wk + ".";

            // Create a new instance of the AppDBContext class.

            using (AppDBContext db = new AppDBContext())
            {
                var userID = db.tblUserDetails.Where(u => u.Username == userName).Select(u => u.UserID).FirstOrDefault();
                var timetableData = db.tblTimetable
                    .Where(t => t.UserID == userID)
                    .Include(t => t.Period1Lesson)
                    .Include(t => t.Period2Lesson)
                    .Include(t => t.Period3Lesson)
                    .Include(t => t.Period4Lesson)
                    .Include(t => t.Period5Lesson)
                    .ToList();

                dgTimetable.ItemsSource = timetableData;
            }

            if (userName == "admin")
            {
                btnAdminPanel.Visibility = Visibility.Visible;
            }

        }

        public void Colours() 
        {
            lblTime.Content = "Total time " + PathFinder.GetTime().ToString() + " seconds";

            int top = NodeStack.PointerValue();
            for (int i = 0; i < NodeStack.StackSize(); i++)
            {
                
                try
                {
                    // If the node is the starting point, set its color to red
                    if (i == top) 
                    {
                        Console.WriteLine("Debug: Node Lit {0}", NodeStack.TopValue());
                        node = nodeDict.Get(NodeStack.TopValue());
                        node.Fill = new SolidColorBrush(Color.FromRgb(230, 16, 48));
                        NodeStack.Pop();
                    }
                    // If the node is the ending point, set its color to green
                    else if (i == 0)
                    {
                        Console.WriteLine("Debug: Node Lit {0}", NodeStack.TopValue());
                        node = nodeDict.Get(NodeStack.TopValue());
                        node.Fill = new SolidColorBrush(Color.FromRgb(25, 227, 126));
                        NodeStack.Pop();

                    }
                    // Otherwise, set the color of the node to a shade of blue
                    else
                    {
                        Console.WriteLine("Debug: Node Lit {0}", NodeStack.TopValue());
                        node = nodeDict.Get(NodeStack.TopValue());
                        node.Fill = new SolidColorBrush(Color.FromRgb(0, 111, 111));
                        NodeStack.Pop();
                    }

                }
                catch (Exception)
                {
                    NodeStack.Pop();
                }
            }
        }


        public void ClearScene() // Method to clear the scene by setting the fill color of all nodes to white.
        {
            for (int i = 0; i <= 22; i++)
            {
                // Get the node with the specified ID from the nodeDict and set its fill color to white.
                node = nodeDict.Get(i);
                node.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private void btnGenerateTimetable_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Generate Timetable' button is presesed
        {
            if (wk.ToString() == "Saturday" || wk.ToString() == "Sunday")   // Check if it is a weekend
            {
                MessageBox.Show("It is a weekend and you have no lessons.");
                return;
            }


            // Clear the scene
            ClearScene();

            // Initilise variables and incremenet periodCounter
            int Period1 = 0; 
            int Period2 = 0;
            string lessonstart;
            string lessonend;
            periodCounter++;

            try
            {
                Period1 = PeriodStack.TopValue();   // Get the top value from the PeriodStack and remove it
                PeriodStack.Pop();
                Period2 = PeriodStack.TopValue();   // Get the top value from the PeriodStack
                XAMLPeriodCounter.Content = periodCounter.ToString() + " --> " + (periodCounter + 1).ToString();
                
                if (Period1 == 99 && Period2 != 99)     // If the user has a free, then set the starting loaction to the cafeteria.
                {
                    Period1 = 2;
                }
            }
            catch (Exception)
            {
                // Display a message box indicating the end of the day and generate the timetable again to reset the process
                MessageBox.Show("End of the day");
                PeriodStack = tt.TimetableGen(storedUsername);
                periodCounter = 0;
                return;
            }

            try
            {
                // Get the names of the lessons corresponding to the period IDs and display them in the appropriate labels.
                lessonstart = tt.GetLessonName(Period1);
                lessonend = tt.GetLessonName(Period2);

                // Run the Dijkstra algorithm to find the shortest path between the two lessons and display the path.
                lblfrmlesson.Content = "From lesson: " + lessonstart;
                lbltolesson.Content = "To lesson: " + lessonend;

                PathFinder.RunDijkstra(Period1, Period2);
                NodeStack = PathFinder.GetStack();
                Colours();
            }
            catch (Exception)
            {
                // If there is no lesson during the current period, display a message box indicating a free period.
                MessageBox.Show("Free Period!");
            }
        }

        private void btnModifyTimetable_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Modify Timetable' button is presesed
        {
            // Shows the frmEditTimetable and closes the current form
            frmEditTimetable reg = new frmEditTimetable(storedUsername);
            reg.Show();
            this.Close();
        }


        private void btnGenerateRoute_Click(object sender, RoutedEventArgs e) // Method to handle the event when the 'Generate Route' button is presesed
        {
            ClearScene();   // Clears the scene

            // Initilise variables
            string startNode = "";
            string endNode = "";
            int startID = 0;
            int endID = 0;

            try
            {
                if (cmbStart.SelectedItem == null || cmbEnd.SelectedItem == null) // Validates that the comboboxes are not empty before converting them to strings.
                {
                    throw new Exception();  // Throw an exception if either combob box is empty
                }

                XAMLPeriodCounter.Content = "";
                lblfrmlesson.Content = "From lesson: " + cmbStart.SelectedItem?.ToString()?.Split(':')[1]?.Trim();
                lbltolesson.Content = "To lesson: " + cmbEnd.SelectedItem?.ToString()?.Split(':')[1]?.Trim();

                // Convert the selected items in the combo boxes to stirngs
                startNode = cmbStart.SelectedItem.ToString();
                endNode = cmbEnd.SelectedItem.ToString();

                // Get the lessonID's for each lesson
                startID = tt.GetLessonId(startNode);
                endID = tt.GetLessonId(endNode);


                // Run Dijkstra's algorithm to find the shortest path between the starting and end node
                PathFinder.RunDijkstra(startID, endID);

                // Get the stack of nodes representing the path
                NodeStack = PathFinder.GetStack();
            }
            catch (Exception)
            {
                // Error message thrown for any execptions
                MessageBox.Show("Please check the starting and end nodes.");
                return;
            }
            // Colour the nodes in the scene based on the NodeStack
            Colours();

            using (var db = new AppDBContext())
            {
                var lesson = db.tblLesson.FirstOrDefault(l => l.LessonID == startID);
                if (lesson != null)
                {
                    lesson.SearchCount++;
                    db.SaveChanges();
                }

                lesson = db.tblLesson.FirstOrDefault(l => l.LessonID == endID);
                if (lesson != null)
                {
                    lesson.SearchCount++;
                    db.SaveChanges();
                }
            }

        }

        private async void btnEmailSend_Click(object sender, RoutedEventArgs e)
        {
            List<string> filePaths = new List<string>();
            for (int i = 0; i <= 4; i++)
            {
                string fileName = $"Period {i + 1}";
                ClearScene();
                btnGenerateTimetable_Click(sender, e);
                pdf.SaveScreenshotAsPdf(this, fileName);
                await Task.Delay(500);
                filePaths.Add(pdf.GetFilePath());
            }

            email.SendEmail(filePaths, storedUsername);
        }

        private void btnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            // Shows the frmEditTimetable and closes the current form
            frmDataVisualization admin = new frmDataVisualization();
            admin.Show();
            this.Close();
        }
    }
}
