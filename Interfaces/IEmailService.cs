using API.DTOs;

namespace API.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailMessageDto emailMessage);
    }
}
