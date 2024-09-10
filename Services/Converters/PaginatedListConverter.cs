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
        public static void Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination)
        {
            destination.CurrentPage = source.CurrentPage;
            destination.PageSize = source.PageSize;
            destination.HasNext = source.HasNext;
        }
        
    }
}
