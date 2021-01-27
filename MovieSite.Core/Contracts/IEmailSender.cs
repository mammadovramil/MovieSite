namespace MovieSite.Core.Contracts
{
    public interface IEmailSender
    {
        void SendEmail(string from, string to, string subject, string html);
    }
}
