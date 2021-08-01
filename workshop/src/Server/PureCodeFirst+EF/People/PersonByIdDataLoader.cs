using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate.DataLoader;

namespace Chat.Server.People
{
    public class PersonByIdDataLoader
        : BatchDataLoader<Guid, Person>
    {
        private readonly ChatDbContext _dbContext;

        public PersonByIdDataLoader(GreenDonut.IBatchScheduler batchScheduler, ChatDbContext dbContext):base(batchScheduler)
        {
            _dbContext = dbContext;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Person>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            return await _dbContext.People
                .Where(t => keys.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id);
        }
    }
}