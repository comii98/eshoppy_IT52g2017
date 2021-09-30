namespace EShoppy.PomocniModul.Interfejsi
{
    public interface IEmailMessage
    {
        void SendEmail(string To, string Subject, string Body, bool IsBodyHtml = false);
    }
}
