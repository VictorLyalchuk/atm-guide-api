using Core.Entities;

namespace Core.Specification
{
    public static class AccountSpecification
    {
        public static IQueryable<User> UsersByPage(this IQueryable<User> query, int page)
        {
            if(page<1)
            {
                page=1;
            }

            int pageSize = 10;

            return query
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
