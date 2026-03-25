using Infrastructure;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Infrastructure.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : EfUnitOfWorkBase<ECommerceDbContext>, IUnitOfWork
    {
        private readonly ECommerceDbContext _context;
        private IBasketRepository _basketRepository;

        public UnitOfWork(ECommerceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IBasketRepository BasketRepository => _basketRepository ??= new BasketRepository(_context);
    }
}



