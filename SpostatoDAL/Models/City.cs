using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public enum CityName { Amsterdam, Rotterdam, Groningen, Eindhoven, Vlissingen, Beverwijk }
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Gang>? Gangs { get; set; }
        public ICollection<Gangster>? Gangsters { get; set; }
        public ICollection<GetAwayCar>? Cars { get; set; }
        public ICollection<Factory> Factorys { get; set; }
        public Casino Casino { get; set; }
    }
}
