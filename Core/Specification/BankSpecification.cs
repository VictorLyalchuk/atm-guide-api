using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public class BankSpecification
    {
        public class BankAll : Specification<Bank>
        {
            public BankAll()
            {
                Query.OrderBy(p => p.Name);
            }
        }
        public class BankByPage : Specification<Bank>
        {
            public BankByPage(int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 10;
                Query.OrderBy(p => p.Name)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize);
            }
        }
    }
}
