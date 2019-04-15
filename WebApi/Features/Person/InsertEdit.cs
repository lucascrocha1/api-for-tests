namespace WebApi.Features.Person
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using WebApi.Domain;
    using WebApi.Infra;

    public class InsertEdit
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public int? Id { get; set; }

            public string Name { get; set; }

            public DateTime BirthDate { get; set; }

            public string Email { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly ApiContext _apiContext;

            public QueryHandler(ApiContext apiContext)
            {
                _apiContext = apiContext;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _apiContext
                    .Set<Person>()
                    .AsNoTracking()
                    .Where(e => e.Id == request.Id)
                    .Select(e => new Command
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Email = e.Email,
                        BirthDate = e.BirthDate
                    })
                    .FirstOrDefaultAsync();
            }
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

                MapPerson(person, request);

                await _apiContext.SaveChangesAsync();
            }

            private async Task<Person> GetPerson(Command request)
            {
                if (!request.Id.HasValue)
                {
                    var person = new Person();

                    await _apiContext.AddAsync(person);

                    return person;
                }

                return await _apiContext
                    .Set<Person>()
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
            }

            private void MapPerson(Person person, Command request)
            {
                person.Name = request.Name;
                person.Email = request.Email;
                person.BirthDate = request.BirthDate;
            }
        }
    }
}