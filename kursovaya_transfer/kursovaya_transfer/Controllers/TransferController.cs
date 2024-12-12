using kursovaya_transfer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kursovaya_transfer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TransferController: ControllerBase
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult ViewHistory()
        {
            Response.Cookies.Append("id_user", "1");
            if (Request.Cookies.TryGetValue("id_user", out string Id))
            {
                int.TryParse(Id, out int id);
                var data = db.Transfer.AsNoTracking().Where(c => c.user_id == id).ToList();
                return Ok(data);
            }
            return BadRequest("Ошибка");
        }

        [HttpPut]
        public IActionResult TransferInside()
        {
            Response.Cookies.Append("id_user", "1");
            Response.Cookies.Append("id_sender", "1");
            Response.Cookies.Append("id_recipient", "2");
            Response.Cookies.Append("sum", "1000");

            if (Request.Cookies.TryGetValue("id_sender", out string Id));
            {
                Request.Cookies.TryGetValue("id_recipient", out string IdR);
                Request.Cookies.TryGetValue("sum", out string Sum);
                Request.Cookies.TryGetValue("id_user", out string IdU);

                int.TryParse(IdU, out int id_user);
                int.TryParse(Id, out int id_sender);
                int.TryParse(IdR, out int id_recipi);
                float.TryParse(Sum, out float total);

                var card = db.Card.Find(id_sender);
                if (card == null)
                {
                    return NotFound("Карта 1 не найдена");
                }
                float newBal = card.balance - total;
                card.balance = newBal;

                var card1 = db.Card.Find(id_recipi);
                if (card1 == null)
                {
                    return NotFound("Карта 2 не найдена");
                }
                float newBalrec = card1.balance + total;
                card1.balance = newBalrec;

                db.Transfer.Add(new Transfer
                {
                    user_id = id_user,
                    sender_id = id_sender,
                    recipient_id = id_recipi,
                    sum = total
                });

                db.SaveChanges();

                return Ok("Баланс карты обновлен успешно");
            }
        }

        [HttpPut]
        public IActionResult TransferOutside()
        {
            Response.Cookies.Append("id_user", "1");
            Response.Cookies.Append("id_sender", "1");
            Response.Cookies.Append("id_recipient", "2");
            Response.Cookies.Append("sum", "1000");
            Response.Cookies.Append("account_num", "test");

            if (Request.Cookies.TryGetValue("id_sender", out string Id)) ;
            {
                Request.Cookies.TryGetValue("id_recipient", out string IdR);
                Request.Cookies.TryGetValue("sum", out string Sum);
                Request.Cookies.TryGetValue("id_user", out string IdU);
                Request.Cookies.TryGetValue("account_num", out string account_num);

                int.TryParse(IdU, out int id_user);
                int.TryParse(Id, out int id_sender);
                int.TryParse(IdR, out int id_recipi);
                float.TryParse(Sum, out float total);

                var card = db.Card.Find(id_sender);
                if (card == null)
                {
                    return NotFound("Карта 1 не найдена");
                }
                float newBal = card.balance - total;
                card.balance = newBal;

                Card? card1 = (from new_card in db.Card where new_card.account_number == account_num select new_card).FirstOrDefault();
                if (card1 == null)
                {
                    return NotFound("Карта 2 не найдена");
                }
                float newBalrec = card1.balance + total;
                card1.balance = newBalrec;

                db.Transfer.Add(new Transfer
                {
                    user_id = id_user,
                    sender_id = id_sender,
                    recipient_id = card1.card_id,
                    sum = total
                });

                db.SaveChanges();

                return Ok("Баланс карты обновлен успешно");
            }
        }
    }
}
