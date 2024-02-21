using MimeKit;

namespace API.DTOs
{
    public class EmailMessageDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        //public EmailMessageDto(string To, string Subject, string Content)
        //{
        //    //this.To = new List<MailboxAddress>();
        //    //this.To.AddRange(To.Select(x => new MailboxAddress("email", x)));
        //    this.To = To;
        //    this.Subject = Subject;
        //    this.Content = Content;
        //}
    }
}
