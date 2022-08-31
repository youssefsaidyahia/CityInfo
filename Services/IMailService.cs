namespace City_info.Services
{
    public interface IMailService
    {
        void send(string subject, string message);
    }
}