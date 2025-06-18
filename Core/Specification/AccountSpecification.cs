using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public static class AccountSpecification
    {
        public static IQueryable<User>UserByPage(this IQueryable<User> query, int page)
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
