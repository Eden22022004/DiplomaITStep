using System.Net.Mail;
using System.Net;
using System.Security.Policy;
namespace SpaceRythm.Models;


public class EmailHelper
{
    public EmailHelperOptions Options { get; }
    public EmailHelper(EmailHelperOptions options)
    {
        Options = options;
    }

    public bool SendEmail(string userEmail, string confirmationLink, string subject)
    {
        Console.WriteLine("***EmailHelper SendEmail");
        Console.WriteLine($"EmailHelper confirmationLink: {confirmationLink}");

        MailMessage mailMessage = new()
        {
            From = new MailAddress(Options.From)
        };
        mailMessage.To.Add(new MailAddress(userEmail));
        mailMessage.Subject = subject;  // Используем переданную тему письма
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = confirmationLink;

        SmtpClient client = new()
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(Options.Login, Options.Password),
            Host = Options.Host,
            Port = Options.Port,
            EnableSsl = Options.EnableSSL
        };

        try
        {
            client.Send(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
    //public bool SendEmail(string userEmail, string confirmationLink)
    //{
    //    Console.WriteLine("***EmailHelper SendEmail");
    //    Console.WriteLine($"EmailHelper confirmationLink: {confirmationLink}");

    //    MailMessage mailMessage = new()
    //    {
    //        From = new MailAddress(Options.From)
    //    };
    //    mailMessage.To.Add(new MailAddress(userEmail));
    //    mailMessage.Subject = Options.Subject;
    //    mailMessage.IsBodyHtml = true;
    //    mailMessage.Body = confirmationLink;
    //    SmtpClient client = new()
    //    {
    //        DeliveryMethod = SmtpDeliveryMethod.Network,
    //        Credentials = new System.Net.NetworkCredential(Options.Login, Options.Password),
    //        Host = Options.Host,
    //        Port = Options.Port,
    //        EnableSsl = Options.EnableSSL
    //    };
    //    try
    //    {
    //        client.Send(mailMessage);
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.ToString());
    //    }
    //    return false;
    //}

    public bool SendEmailResetPassword(string userEmail, string resetLink)
    {
        Console.WriteLine("---EmailHelper resetLink " + resetLink);
        return SendEmail(userEmail, resetLink, "Сброс пароля");
    }
}
//public class EmailHelper
//{
//    public bool SendEmail(string userEmail, string confirmationLink)
//    {

//        Console.WriteLine("***EmailHelper SendEmail");
//        Console.WriteLine($"EmailHelper confirmationLink: {confirmationLink}");
//        MailMessage mailMessage = new MailMessage
//        {
//            From = new MailAddress("anna.mykhalchenko8@gmail.com")
//        };
//        mailMessage.To.Add(new MailAddress(userEmail));
//        mailMessage.Subject = "Ви успішно зареєстровані в Space Rythm";
//        mailMessage.IsBodyHtml = true;
//        mailMessage.Body = confirmationLink;
//        SmtpClient client = new SmtpClient
//        {
//            DeliveryMethod = SmtpDeliveryMethod.Network,
//            Credentials = new System.Net.NetworkCredential("anna.mykhalchenko8@gmail.com", "nqyd srvr nbvz szqf"),
//            Host = "smtp.gmail.com",
//            Port = 587,
//            EnableSsl = true
//        };
//        try
//        {
//            client.Send(mailMessage);
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.ToString());
//        }
//        return false;
//    }
//}
