using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Gangster>? Gangsters { get; set; }
        public int? GangsterId { get; set; }
        [ForeignKey("GangsterId")]
        public virtual Gangster? CurrentGangster { get; set; }
        
    }
}
