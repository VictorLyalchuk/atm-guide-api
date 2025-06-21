using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public class RegionSpecification
    {
        public class RegionAll : Specification<Region>
        {
            public RegionAll()
            {
                Query.OrderBy(p => p.Name);
            }
        }
        public class RegionByPage : Specification<Region>
        {
            public RegionByPage(int page)
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
