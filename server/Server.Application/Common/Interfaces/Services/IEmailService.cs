using Server.Contracts.Common;

namespace Server.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        void SendEmail(MailRequest mailRequest);
    }
}
