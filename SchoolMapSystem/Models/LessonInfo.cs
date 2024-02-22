using System.ComponentModel.DataAnnotations;

namespace SchoolMapSystem
{
    class LessonInfo
    {
        [Key]// Primary key attribute
        public int LessonID { get; set; } // Lesson ID property
        public string LessonName { get; set; } // Lesson name property
        public int SearchCount { get; set; } // Search Count property
    }
}
