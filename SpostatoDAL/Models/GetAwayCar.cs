using SpostatoBL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public class GetAwayCar
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [Required, ForeignKey("OwnerId")]
        public Gangster Owner { get; set; }
        [Required]
        public CarEnum Name { get; set; }
        [Range(1, 350), Required]
        public int Speed { get; set; }
        [Range(1, 100), Required]
        public int Intact { get; set; }
        [Range(1000, 5000000), Required]
        public int Value { get; set; }
        public int? CityId { get; set; }
        [Required, ForeignKey("CityId")]
        public City CurrentCity { get; set; }
    }
}
