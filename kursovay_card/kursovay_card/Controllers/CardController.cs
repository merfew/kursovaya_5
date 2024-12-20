using kursah_5semestr.Services;
using kursovay_card.Model;
using kursovay_card.RabbitMQ;
using kursovay_card.Service;
using kursovaya_card;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace kursovay_card.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IUserDataFunc _userData;
        IDataUpdaterService _updaterService;
        public CardController(ICardService cardService, IUserDataFunc userData, IDataUpdaterService updaterService)
        {
            _cardService = cardService;
            _userData = userData;
            _updaterService = updaterService;
            //_updaterService.Start();
        }

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            //Response.Cookies.Append("id_user", "5");
            //Request.Cookies.TryGetValue("id_user", out string? id);
            //var data = await _cardService.GetCards(userData);
            //await _updaterService.Start();
            var id = _userData.GetVariable();
            var data = await _cardService.GetCards(id);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCardInfo()
        {
            Response.Cookies.Append("id_card", "8");

            Request.Cookies.TryGetValue("id_card", out string? Id);
            var card = await _cardService.GetInfoCard(Id);
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] Card card)
        {
            //Response.Cookies.Append("id_user", "5");

            //Request.Cookies.TryGetValue("id_user", out string? Id);

            var Id = _userData.GetVariable();

            await _cardService.CreateCard(card, Id);
            return Ok("Карата создана успешно");
        }
    }
}
