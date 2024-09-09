using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext { get; set; }

        public PaginatedList()
        {
            
        }

        private PaginatedList(List<T> items, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            CurrentPage = pageNumber;
            HasNext = items.Count > pageSize;
            if(HasNext)
            {
                AddRange(items[..^1]);
            }
            else
            {
                AddRange(items);        
            }
        }
        public static PaginatedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize + 1).ToList();
            return new PaginatedList<T>(items, pageNumber, pageSize);
        }
    }
}
