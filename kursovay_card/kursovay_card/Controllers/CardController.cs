using kursah_5semestr.Services;
using kursovay_card.Model;
using kursovay_card.Object;
using kursovay_card.RabbitMQ;
using kursovay_card.Service;
using kursovaya_card;
using Microsoft.AspNetCore.Authorization;
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
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var id = _userData.GetVariable();
            var data = await _cardService.GetCards(id);
            return Ok(data);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCardInfo([FromBody] IdCardObj idCardObj)
        {
            var card = await _cardService.GetInfoCard(idCardObj.Id);
            return Ok(card);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CardObj cardObj)
        {
            var Id = _userData.GetVariable();
            await _cardService.CreateCard(cardObj, Id);
            return Ok("Карата создана успешно");
        }
    }
}
