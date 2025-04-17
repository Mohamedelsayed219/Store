using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntiy, Tkey > where TEntiy : BaseEntity<Tkey>
    {
        Expression<Func<TEntiy, bool>>? Criteria { get; set; }
        List<Expression<Func<TEntiy, object>>> IncludeExpressions { get; set; }

         Expression<Func<TEntiy, object>>? OrderBy { get; set; }
         Expression<Func<TEntiy, object>>? OrderByDescending { get; set; }

          int Skip { get; set; }
          int Take { get; set; }
          bool IsPagination { get; set; }

    }
}
