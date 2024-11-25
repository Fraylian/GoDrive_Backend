
namespace Aplicacion.Seguridad.Response
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Mensaje { get; set; }
        public dynamic Data { get; set; }

    }
}
