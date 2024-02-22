using System.ComponentModel.DataAnnotations;

namespace SchoolMapSystem
{
    class TimetableInfo //This class represents the timetable information for a particular user for a particular day
    { 
        [Key]
        public string TimetableID { get; set; }
        public int UserID { get; set; }
        public string Day { get; set; }
        public int Period1LessonID { get; set; }
        public int Period2LessonID { get; set; }
        public int Period3LessonID { get; set; }
        public int Period4LessonID { get; set; }
        public int Period5LessonID { get; set; }

        public virtual LessonInfo Period1Lesson { get; set; }
        public virtual LessonInfo Period2Lesson { get; set; }
        public virtual LessonInfo Period3Lesson { get; set; }
        public virtual LessonInfo Period4Lesson { get; set; }
        public virtual LessonInfo Period5Lesson { get; set; }
    }
}
