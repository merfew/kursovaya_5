using kursovaya_transfer.Model;
using kursovaya_transfer.Object;
using kursovaya_transfer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kursovaya_transfer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TransferController: ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IUserDataFunc _userData;
        public TransferController(ITransferService transferService, IUserDataFunc userData)
        {
            _transferService = transferService;
            _userData = userData;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewHistory()
        {
            var Id = _userData.GetVariable();
            var history = await _transferService.GetHistory(Id);
            return Ok(history);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> TransferInside([FromBody] InTransferObj transferObj)
        {
            var IdU = _userData.GetVariable();

            string data = await _transferService.TransferInside(transferObj, IdU);
            return Ok(data);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> TransferOutside([FromBody] OutTransferObj transferObj)
        {
            var IdU = _userData.GetVariable();

            string data = await _transferService.TransferOutside(transferObj, IdU);
            return Ok(data);
        }
    }
}
