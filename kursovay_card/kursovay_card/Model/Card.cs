using System.ComponentModel.DataAnnotations.Schema;

namespace kursovay_card.Model
{
    public class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int card_id { get; set; }
        public int user_id { get; set; }
        public string? name { get; set; }
        public string? account_number { get; set; }
        public string? data { get; set; }
        public string? cvc { get; set; }
        public float balance { get; set; }
    }
}
