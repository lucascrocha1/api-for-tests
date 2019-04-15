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

    public class List
    {
        public class Query : IRequest<Dto[]>
        {
            public int PageSize { get; set; }

            public int PageIndex { get; set; }

            public string Filter { get; set; }
        }

        public class Dto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Email { get; set; }

            public DateTime BirthDate { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Dto[]>
        {
            private readonly ApiContext _apiContext;

            public QueryHandler(ApiContext apiContext)
            {
                _apiContext = apiContext;
            }

            public async Task<Dto[]> Handle(Query request, CancellationToken cancellationToken)
            {
                var persons = GetPersons();

                persons = FilterPersons(persons, request);

                persons = PaginatePersons(persons, request);

                return await persons.ToArrayAsync();
            }

            private IQueryable<Dto> GetPersons()
            {
                return _apiContext
                    .Set<Person>()
                    .AsNoTracking()
                    .Select(e => new Dto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Email = e.Email,
                        BirthDate = e.BirthDate
                    })
                    .AsQueryable();
            }

            private IQueryable<Dto> FilterPersons(IQueryable<Dto> persons, Query request)
            {
                if (string.IsNullOrEmpty(request.Filter))
                    return persons;

                return persons
                    .Where(e => e.Name.Contains(request.Filter) || e.Email.Contains(request.Filter));
            }

            private IQueryable<Dto> PaginatePersons(IQueryable<Dto> persons, Query request)
            {
                if (!string.IsNullOrEmpty(request.Filter))
                    return persons;

                return persons
                        .Skip((request.PageIndex - 1) * request.PageSize)
                        .Take(request.PageSize);
            }
        }
    }
}
