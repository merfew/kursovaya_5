using System.ComponentModel.DataAnnotations.Schema;

namespace kursovaya_transfer
{
    public class UserData
    {
        public int user_id { get; set; }

    }
    public class UserDataFunc : IUserDataFunc
    {
        private string? myVariable = null;

        public void SetVariable(string value)
        {
            myVariable = value;
        }

        public string GetVariable()
        {
            if (myVariable == null)
            {
                return "Сообщение пустое";
            }
            return myVariable;
        }
    }

    public interface IUserDataFunc
    {
        void SetVariable(string value);
        string GetVariable();
    }
}
