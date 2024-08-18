using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public class Prison
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<Gangster>? Gangsters { get; set; }
        public int PrisonRegister { get; set; } = 0;
        public int BreakoutCount { get; set; } = 0;
    }
}
