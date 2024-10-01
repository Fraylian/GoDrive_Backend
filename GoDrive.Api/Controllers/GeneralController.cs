using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace GoDrive.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController: ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
