using System;
using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Chat.Server.People
{
    public class PersonQueries
    {
        /// <summary>
        /// Gets the currently logged in user.
        /// </summary>
        // [Authorize]
        public IQueryable<Person> GetMe(
            [GlobalState]string currentUserEmail,
            [Service]ChatDbContext dbContext) =>
            dbContext.People.Where(t => t.Email == currentUserEmail);


        /// <summary>
        /// Gets access to all the people known to this service.
        /// </summary>
        // [Authorize]
        public IQueryable<Person> GetPeople(
            [Service]ChatDbContext dbContext) =>
            dbContext.People;

        /// <summary>
        /// Gets a user by its email address.
        /// </summary>
        // [Authorize]
        public IQueryable<Person> GetPersonByEmailAsync(
            string email,
            [Service]ChatDbContext dbContext) =>
            dbContext.People.Where(t => t.Email == email);

        /// <summary>
        /// Gets a user by its id.
        /// </summary>
        // [Authorize]
        public IQueryable<Person> GetPersonByIdAsync(
            Guid id,
            [Service]ChatDbContext dbContext) =>
            dbContext.People.Where(t => t.Id == id);
    }
}
