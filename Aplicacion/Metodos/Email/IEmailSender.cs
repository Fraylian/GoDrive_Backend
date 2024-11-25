using Dominio.Entidades;

namespace Aplicacion.Metodos.Email
{
    public interface IEmailSender
    {
        Task<bool> Execute(Emails modelo);
    }
}
