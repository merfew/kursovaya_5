using kursovay_card.Model;
using kursovay_card.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace kursovay_card.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CardController: ControllerBase
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            Response.Cookies.Append("id_user", "1");

            Request.Cookies.TryGetValue("id_user", out string? id);
            var data = await _cardService.GetCards(id);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCardInfo()
        {
            Response.Cookies.Append("id_card", "1");

            Request.Cookies.TryGetValue("id_card", out string? Id);
            var card = await _cardService.GetInfoCard(Id);
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] Card card)
        {
            Response.Cookies.Append("id_user", "3");

            Request.Cookies.TryGetValue("id_user", out string? Id);

            await _cardService.CreateCard(card, Id);
            return Ok("Карата создана успешно");
        }
    }
}
