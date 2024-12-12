using System.ComponentModel.DataAnnotations.Schema;

namespace kursovaya_transfer.Model
{
    public class Transfer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int transfer_id { get; set; }
        public int user_id { get; set; }
        public int sender_id { get; set; }
        public int recipient_id { get; set; }
        public float sum { get; set; }
    }
}
