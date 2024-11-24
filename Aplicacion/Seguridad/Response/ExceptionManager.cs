using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Aplicacion.Seguridad.Response
{
    public class ExceptionManager : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(ResponseService.Respuesta(StatusCodes.Status500InternalServerError, null, context.Exception.Message));
        }
    }
}
