using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMapSystem
{
    class UserInfo
    {
        // The UserID property is the primary key for the table and is auto-generated.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
