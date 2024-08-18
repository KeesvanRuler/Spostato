using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public class Casino
    {
        [Key]
        public int Id { get; set; }
        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Gangster? Owner { get; set; }
        [Required]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
        public int Balance { get; set; } = 0;
    }
}
