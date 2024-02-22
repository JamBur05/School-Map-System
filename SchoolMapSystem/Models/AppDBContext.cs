using System;
using Microsoft.EntityFrameworkCore;

namespace SchoolMapSystem
{
    class AppDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // This method is used to configure the database context options.
        {
            // Specify the path of the SQLite database file.
            string _db = @"dbSchoolMapSystem.db";
            // Create a connection string using the database file path.
            string cs = String.Format("Data Source={0}", _db);

            // Use SQLite as the database provider and set the connection string.
            optionsBuilder.UseSqlite(cs);
        }

        // These properties represent the database tables as DbSet objects.
        public DbSet<UserInfo> tblUserDetails { get; set; }
        public DbSet<TimetableInfo> tblTimetable { get; set; }
        public DbSet<LessonInfo> tblLesson { get; set; }
    }
}
