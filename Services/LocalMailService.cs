using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace City_info.Services
{
    public class LocalMailService : IMailService
    {
        private string mailTo = "admin@mycompany.com";
        private string mailFrom = "noreply@mycompany.com";

        public void send(string subject, string message)
        {
            Console.WriteLine($"Mail from {mailFrom} to {mailTo}) ," + $"with {nameof(LocalMailService)}.");
            Console.WriteLine($"Message :{message}");
            Console.WriteLine($"subject : {subject}");
        }
    }
}
