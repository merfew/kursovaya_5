using kursovay_card.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace kursovay_card.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CardController: ControllerBase
    {
        AppDbContext db = new AppDbContext();
        //[HttpGet]
        //public IActionResult GetCards()
        //{
        //    var headers = HttpContext.Request.Headers;
        //    if (headers.TryGetValue("id", out var Id))
        //    {
        //        int.TryParse(Id, out int id);
        //        var data = db.Card.AsNoTracking().Where(c => c.user_id == id).ToList();
        //        return Ok(data);
        //    }
        //    return BadRequest("Ошибка");
        //}

        [HttpGet]
        public IActionResult GetCards()
        {
            Response.Cookies.Append("id_user", "1");
            if (Request.Cookies.TryGetValue("id_user", out string Id))
            {
                int.TryParse(Id, out int id);
                var data = db.Card.AsNoTracking().Where(c => c.user_id == id).ToList();
                return Ok(data);
            }
            return BadRequest("Ошибка");
        }

        //[HttpGet]
        //public IActionResult GetCardInfo()
        //{
        //    var headers = HttpContext.Request.Headers;
        //    var db = new AppDbContext();
        //    if (headers.TryGetValue("id", out var Id))
        //    {
        //        int.TryParse(Id, out int id);
        //        var data = (from cards in db.Card where cards.card_id == id select cards);
        //        List<Card> card = new List<Card>();
        //        foreach (var u in data)
        //        {
        //            card.Add(u);
        //        }
        //        return Ok(data);
        //    }
        //    return BadRequest("Ошибка");
        //}

        [HttpGet]
        public IActionResult GetCardInfo()
        {
            var db = new AppDbContext();
            Response.Cookies.Append("id_card", "1");
            if (Request.Cookies.TryGetValue("id_card", out string Id))
            {
                int.TryParse(Id, out int id);
                var data = (from cards in db.Card where cards.card_id == id select cards);
                List<Card> card = new List<Card>();
                foreach (var u in data)
                {
                    card.Add(u);
                }
                return Ok(data);
            }
            return BadRequest("Ошибка");
        }

        [HttpPost]
        public IActionResult CreateCard([FromBody] Card card)
        {
            Response.Cookies.Append("id_user", "3");
            if (Request.Cookies.TryGetValue("id_user", out string Id))
            {
                int.TryParse(Id, out int id);
                db.Card.Add(new Card
                {
                    user_id = id,
                    name = card.name,
                    account_number = card.account_number,
                    data = card.data,
                    cvc = card.cvc,
                    balance = card.balance
                });
                db.SaveChanges();
                return Ok("Карта создана успешно");
            }
            return BadRequest();
        }
    }
}
