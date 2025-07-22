using BlogMagangementSystem.Common.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlogMagangementSystem.Common.Middlewares
{
    public class GlobalTransactionMiddleware : IMiddleware
    {
        private readonly BlogDbContext _context;

        public GlobalTransactionMiddleware(BlogDbContext context)
        {
            this._context = context;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            IDbContextTransaction Transaction = null!;
            try
            {
                Transaction = _context.Database.BeginTransaction();

                await next(context);
                Transaction.Commit();

            }
            catch (Exception)
            {
               Transaction?.Rollback();
                throw;
            }


        }
    }
}

