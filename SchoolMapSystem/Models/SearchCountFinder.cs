using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SchoolMapSystem.Models
{
    class SearchCountFinder
    {
        private List<Category> SearchCategories { get; set; } = new List<Category>();
        private List<Brush> brushes = new List<Brush>
        {
            Brushes.Gold,
            Brushes.Blue,
            Brushes.Red,
            Brushes.Green,
            Brushes.Purple,
            Brushes.Orange,
            Brushes.Pink,
            Brushes.Teal,
            Brushes.Yellow,
            Brushes.Gray,
            Brushes.LightBlue,
            Brushes.LightGreen,
            Brushes.Lavender,
            Brushes.Tan,
            Brushes.Coral,
            Brushes.Salmon
        };


        public List<Category> FindSearches()
        {
            GenereateTimetable ttable = new GenereateTimetable();

            using (var db = new AppDBContext())
            {
                var lessons = db.tblLesson.Where(l => l.SearchCount > 0).ToList();
                int total = 0;
                int brushIndex = 0;

                List<Tuple<int, int>> lessonSearchCounts = new List<Tuple<int, int>>();

                foreach (var lesson in lessons)
                {
                    int lessonID = lesson.LessonID;
                    int searchCount = lesson.SearchCount;
                    lessonSearchCounts.Add(Tuple.Create(lessonID, searchCount));
                }

                foreach (var result in lessonSearchCounts)
                {
                    int searchCount = result.Item2;
                    total = total + searchCount;
                }

                foreach (var result in lessonSearchCounts)
                {
                    int lessonID = result.Item1;
                    int searchCount = result.Item2;

                    // Find lessonname
                    string lessonname = ttable.GetLessonName(lessonID);
                    float percentage = (float)Math.Round((searchCount / (float)total) * 100);



                    SearchCategories.Add(new Category
                    {
                        Title = lessonname,
                        Percentage = percentage,
                        ColourBrush = brushes[brushIndex % brushes.Count],
                    });

                    brushIndex++;
                }

                return SearchCategories;
            }
        }
    }
}
