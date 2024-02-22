using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SchoolMapSystem.Models
{
    class ModifyTimetable   
    {                       
        private string username;
        private string lessonName1;
        private string lessonName2;
        private string lessonName3;
        private string lessonName4;
        private string lessonName5;
        private int lessonID1 = -1;
        private int lessonID2 = -1;
        private int lessonID3 = -1;
        private int lessonID4 = -1;
        private int lessonID5 = -1;
        private string Day = "";

        public ModifyTimetable(string inputUsername, string lesson1, string lesson2, string lesson3, string lesson4, string lesson5, string day) // Constructor
        {
            // Set the username property to the input username
            username = inputUsername;
            // Extract the lesson names from the lesson parameters and store them in the appropriate properties
            lessonName1 = lesson1.Split(':')[1].Trim();
            lessonName2 = lesson2.Split(':')[1].Trim();
            lessonName3 = lesson3.Split(':')[1].Trim();
            lessonName4 = lesson4.Split(':')[1].Trim();
            lessonName5 = lesson5.Split(':')[1].Trim();
            // Extract the day from the day parameter and store it in the Day property
            Day = day.Split(':')[1].Trim();
        }


        public void TimetableQuery() // Updates the timetable for the given user and day with the specified lessons
        {
            using(AppDBContext db = new AppDBContext())
            {
                // Retrieve the user ID
                var userID = db.tblUserDetails.Single(u => u.Username == username).UserID;
                
                // Construct the query to retrieve the lessons from the database
                var lessonquery = "SELECT LessonID, LessonName, SearchCount FROM tblLesson WHERE LessonName IN (@l1, @l2, @l3, @l4, @l5)";
                Console.WriteLine(lessonquery);
                
                // Create parameters for SQL query
                var parameters = new[] {
                    new SqliteParameter("@l1", lessonName1),
                    new SqliteParameter("@l2", lessonName2),
                    new SqliteParameter("@l3", lessonName3),
                    new SqliteParameter("@l4", lessonName4),
                    new SqliteParameter("@l5", lessonName5)
                };



                // Execute the query and retrieve the lessons
                var lessons = db.tblLesson.FromSqlRaw(lessonquery, parameters).ToList();

                // Match the lesson names to the lesson IDs
                int lessonID;
                foreach (var lesson in lessons)
                {
                    if (lesson.LessonName == lessonName1)
                    {
                        lessonID = lesson.LessonID;
                        lessonID1 = lessonID;
                    }
                    if (lesson.LessonName == lessonName2)
                    {
                        lessonID = lesson.LessonID;
                        lessonID2 = lessonID;
                    }
                    if (lesson.LessonName == lessonName3)
                    {
                        lessonID = lesson.LessonID;
                        lessonID3 = lessonID;
                    }
                    if (lesson.LessonName == lessonName4)
                    {
                        lessonID = lesson.LessonID;
                        lessonID4 = lessonID;
                    }
                    if (lesson.LessonName == lessonName5)
                    {
                        lessonID = lesson.LessonID;
                        lessonID5 = lessonID;
                    }
                }


                // Calculate the timetable ID based on the user ID and selected day
                int ttableID = (userID * 5);

                switch (Day)
                {
                    case "Tuesday":
                        ttableID += 1;
                        break;
                    case "Wednesday":
                        ttableID += 2;
                        break;
                    case "Thursday":
                        ttableID += 3;
                        break;
                    case "Friday":
                        ttableID += 4;
                        break;
                }

                // Retrieve the timetable from the database
                var timetable = db.tblTimetable.SingleOrDefault(t => t.TimetableID == ttableID.ToString());

                if (timetable != null)
                {
                    // Update the lesson IDs for the selected day
                    timetable.Period1LessonID = lessonID1;
                    timetable.Period2LessonID = lessonID2;
                    timetable.Period3LessonID = lessonID3;
                    timetable.Period4LessonID = lessonID4;
                    timetable.Period5LessonID = lessonID5;

                    // Save changes to the database
                    db.SaveChanges();
                }
            }
        }
    }
}
