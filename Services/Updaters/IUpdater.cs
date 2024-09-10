using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Converters
{
    public interface IUpdater<T>
    {
        void Update(T source, T destination);
    }
}
