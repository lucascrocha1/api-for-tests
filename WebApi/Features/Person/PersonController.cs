namespace WebApi.Features.Person
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class PersonController : Controller
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(InsertEdit.Query query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> List(List.Query query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task Insert([FromBody]InsertEdit.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpPut]
        public async Task Edit([FromBody]InsertEdit.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpDelete]
        public async Task Delete([FromBody]Delete.Command command)
        {
            await _mediator.Send(command);
        }
    }
}