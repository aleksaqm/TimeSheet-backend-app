using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class UpdateCategoryDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
