using kursovaya_transfer.Model;
using kursovaya_transfer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kursovaya_transfer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TransferController: ControllerBase
    {
        private readonly ITransferService _transferService;
        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<IActionResult> ViewHistory()
        {
            Response.Cookies.Append("id_user", "5");

            Request.Cookies.TryGetValue("id_user", out string? Id);
            var history = await _transferService.GetHistory(Id);
            return Ok(history);
        }

        [HttpPut]
        public async Task<IActionResult> TransferInside([FromBody] Transfer transfer)
        {
            Response.Cookies.Append("id_user", "5");
            Response.Cookies.Append("id_sender", "1");
            Response.Cookies.Append("id_recipient", "2");

            Request.Cookies.TryGetValue("id_sender", out string Id);
            Request.Cookies.TryGetValue("id_recipient", out string IdR);
            Request.Cookies.TryGetValue("id_user", out string IdU);

            string data = await _transferService.TransferInside(transfer, IdU, Id, IdR);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> TransferOutside([FromBody] Transfer transfer)
        {
            Response.Cookies.Append("id_user", "5");
            Response.Cookies.Append("id_sender", "1");
            Response.Cookies.Append("account_num", "0000 0000 0000 0000");

            Request.Cookies.TryGetValue("id_sender", out string Id);
            Request.Cookies.TryGetValue("id_user", out string IdU);
            Request.Cookies.TryGetValue("account_num", out string account_num);

            string data = await _transferService.TransferOutside(transfer, IdU, Id, account_num);
            return Ok(data);
        }
    }
}
