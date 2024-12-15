using kursovaya_transfer.Model;

namespace kursovaya_transfer.Service
{
    public interface ITransferService
    {
        Task<List<Transfer>> GetHistory(string id);
        Task<string> TransferInside(Transfer transfer, string User_Id, string sender_Id, string recipient_Id);
        Task<string> TransferOutside(Transfer transfer, string User_Id, string sender_Id, string account_num);
    }
}