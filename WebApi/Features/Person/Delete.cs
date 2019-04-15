namespace WebApi.Features.Person
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using WebApi.Domain;
    using WebApi.Infra;

    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ApiContext _apiContext;

            public CommandHandler(ApiContext apiContext)
            {
                _apiContext = apiContext;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var person = await GetPerson(request);

                if (person is null)
                    return;

                _apiContext.Remove(person);

                await _apiContext.SaveChangesAsync();
            }

            private async Task<Person> GetPerson(Command request)
            {
                return await _apiContext
                    .Set<Person>()
                    .Where(e => e.Id == request.Id)
                    .FirstOrDefaultAsync();
            }
        }
    }
}