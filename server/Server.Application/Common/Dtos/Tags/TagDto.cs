using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Common.Dtos.Tags
{
    public class TagDto
    {
        public string Name { get; set; }
        public DateTime? DateEdited { get; set; }
        public DateTime DateCreated { get; set; } = default!;
        public DateTime? DateDeleted { get; set; }
    }
}
