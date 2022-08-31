namespace City_info.Services
{
    public class CloudMailService
    {
        private string mailTo = "admin@mycompany.com";
        private string mailFrom = "noreply@mycompany.com";

        public void send(string subject, string message)
        {
            Console.WriteLine($"Mail from {mailFrom} to {mailTo}) ," + $"with {nameof(CloudMailService)}.");
            Console.WriteLine($"Message :{message}");
            Console.WriteLine($"subject : {subject}");
        }
    }
}
