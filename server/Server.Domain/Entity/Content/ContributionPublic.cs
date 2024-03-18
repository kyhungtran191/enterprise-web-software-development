using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionPublics")]
    public class ContributionPublic : Contribution
    {
        [MaxLength(256)]
        public string UserName { get; set; } = default!;

        [MaxLength(500)]
        public string Avatar { get; set; } = default!;
        public string FacultyName { get; set; } = default!;
                
        public int LikeQuantity { get; set; } = 0;
                
        public int Views { get; set; } = 0;
    }
}
