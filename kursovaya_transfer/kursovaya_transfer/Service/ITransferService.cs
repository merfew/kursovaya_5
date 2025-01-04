using kursovaya_transfer.Model;
using kursovaya_transfer.Object;

namespace kursovaya_transfer.Service
{
    public interface ITransferService
    {
        Task<List<Transfer>?> GetHistory(string id);
        Task<string> TransferInside(InTransferObj transferObj, string User_Id);
        Task<string> TransferOutside(OutTransferObj transferObj, string User_Id);
    }
}