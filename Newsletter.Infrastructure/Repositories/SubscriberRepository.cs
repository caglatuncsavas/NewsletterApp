using GenericRepository;
using Newsletter.Domain.Entities;
using Newsletter.Domain.Repositories;
using Newsletter.Infrastructure.Context;

namespace Newsletter.Infrastructure.Repositories;

internal sealed class SubscriberRepository : Repository<Subscriber, ApplicationDbContext>, ISubscriberRepository
{
    public SubscriberRepository(ApplicationDbContext context) : base(context)
    {
    }
}

