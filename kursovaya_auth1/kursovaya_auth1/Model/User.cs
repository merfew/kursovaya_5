using System.ComponentModel.DataAnnotations.Schema;

namespace kursovaya_auth1.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; } 
        public string? name { get; set; }
        public string? surname { get; set; }
        public string? phone_number { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
