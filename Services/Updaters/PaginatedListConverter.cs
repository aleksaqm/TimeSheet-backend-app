using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Converters
{
    public class PaginatedListConverter <TSource, TDestination>
    {
        public static void Convert(PaginatedList<TSource> source, PaginatedList<TDestination> target)
        {
            target.CurrentPage = source.CurrentPage;
            target.PageSize = source.PageSize;
            target.HasNext = source.HasNext;
        }
        
    }
}
