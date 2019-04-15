namespace WebApi.Domain
{
    using System;

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
    }
}