using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public abstract class Factory
    {
        [Key]
        public int Id { get; set; }
        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Gangster? Owner { get; set; }
        [Required]
        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
        public int Balance { get; set; } = 0;
        public bool IsProducing { get; set; } = false;
    }
    public class XTCFactory : Factory
    {
        public int AmountOfKilos { get; set; } = 0;
    }
    public class CocaineFactory : Factory
    {
        public int AmountOfKilos { get; set; } = 0;
    }
    public class BulletFactory : Factory
    {
        public int AmountOfBullets { get; set; } = 0;
    }

    public class CannabisPlantation : Factory
    {
        public int AmountOfKilos { get; set; } = 0;
    }
}
