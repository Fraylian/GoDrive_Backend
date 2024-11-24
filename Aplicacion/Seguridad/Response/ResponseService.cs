

namespace Aplicacion.Seguridad.Response
{
    public static class ResponseService
    {
        public static ResponseModel Respuesta(int statusCode, object Data = null, string mensaje = null) {

            bool success = false;
            if (statusCode >= 200 && statusCode < 300)
            {
                success = true;
            }

            var resultado = new ResponseModel
            {
                StatusCode = statusCode,
                Success = success,
                Mensaje = mensaje,
                Data = Data,
            };

            return resultado;


        }
    }
}
