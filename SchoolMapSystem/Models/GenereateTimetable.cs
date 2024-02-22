using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;

namespace SchoolMapSystem
{
    class GenereateTimetable
    {
        private int Period1;
        private int Period2;
        private int Period3;
        private int Period4;
        private int Period5;

        public Stack TimetableGen(string Username) // Generates a timetable for a given user by querying the databse and returning a stack containing the periods
        {
            using (var db = new AppDBContext())
            {

                DayOfWeek wk = DateTime.Today.DayOfWeek;
                
                // Parameterised SQL query to get the timetable for the given user and day
                var timetablequery = "SELECT TimetableID, tblTimetable.UserID, Day, Period1LessonID, Period2LessonID, Period3LessonID, Period4LessonID, Period5LessonID FROM tblTimetable, tblUserDetails WHERE tblUserDetails.UserID = tblTimetable.UserID AND Username = @Username AND Day = @Day";

                // Define the parameters
                var parameters = new[] {
                    new SqliteParameter("@Username", Username),
                    new SqliteParameter("@Day", wk.ToString())
                };

                // Execute the query with parameters
                var timetables = db.tblTimetable.FromSqlRaw(timetablequery, parameters)
                    .Select(t => new
                    {
                        TimetableID = t.TimetableID,
                        UserID = t.UserID,
                        Day = t.Day,
                        Period1LessonID = t.Period1LessonID == null ? 0 : t.Period1LessonID,
                        Period2LessonID = t.Period2LessonID == null ? 0 : t.Period2LessonID,
                        Period3LessonID = t.Period3LessonID == null ? 0 : t.Period3LessonID,
                        Period4LessonID = t.Period4LessonID == null ? 0 : t.Period4LessonID,
                        Period5LessonID = t.Period5LessonID == null ? 0 : t.Period5LessonID
                    })
                    .ToList();

                try
                {
                    foreach (var timetable in timetables)
                    {
                        Period1 = timetable.Period1LessonID;
                        Period2 = timetable.Period2LessonID;
                        Period3 = timetable.Period3LessonID;
                        Period4 = timetable.Period4LessonID;
                        Period5 = timetable.Period5LessonID;
                    }
                }
                catch(Exception)
                {
                    MessageBox.Show("Please enter your timetable data!");
                }


                Console.WriteLine("{0} {1} {2} {3} {4}", Period1, Period2, Period3, Period4, Period5);

                Stack PeriodStack = new Stack(5); // Create new Stack object

                PeriodStack.Push(Period5);
                PeriodStack.Push(Period4);
                PeriodStack.Push(Period3);
                PeriodStack.Push(Period2);
                PeriodStack.Push(Period1);

                return PeriodStack;
            }
        }

        public string GetLessonName(int lessonId) // Getter, returns the lesson name assosciated with the given lessonid from the database
        {
            string lessonName = null;
            using (var context = new AppDBContext())
            {
                // Query the database to find the lesson with the given ID
                var lesson = context.tblLesson.FirstOrDefault(l => l.LessonID == lessonId);
                // If the lesson is found, set its name to lessonName variable
                if (lesson != null)
                {
                    lessonName = lesson.LessonName;
                }
            }
            return lessonName;
        }

        public int GetLessonId(string lessonName) // Getter, returns the lessonid assosciated with the given lesson name from the database
        {
            lessonName = lessonName.Split(':')[1].Trim();
            int lessonId = -1;
            using (var context = new AppDBContext())
            {
                // Query the database to find the lessonid with the given name
                var lesson = context.tblLesson.FirstOrDefault(l => l.LessonName == lessonName);
                // If the lesson is found, set its name to lessonID variable
                if (lesson != null)
                {
                    lessonId = lesson.LessonID;
                }
            }
            return lessonId;
        }

    }
}
