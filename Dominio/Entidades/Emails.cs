
namespace Dominio.Entidades
{
    public class Emails
    {
        public ContentEmail From { get; set; }
        public List<Personalization> Personalizations { get; set; }
        public List<ContentBody> Content { get; set; }
    }

    public class Personalization
    {
        public string subject { get; set; }
        public List<ContentEmail> To { get; set; }
    }

    public class ContentEmail
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
    public class ContentBody
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
