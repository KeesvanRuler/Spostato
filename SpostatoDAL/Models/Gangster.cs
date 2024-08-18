using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public enum Rank { Newbie, Goon, Pro, Boss, Godfather }
    public enum Weapon { Stiletto, Glock, Uzi, Kalashnikov }
    public enum Transport { Vespa, Golf, Astra, Bugatti, Helicopter }

    [Index(nameof(Name), IsUnique = true)]
    public class Gangster
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        [Required, Display(Name = "Naam")]
        public string Name { get; set; }
        public ICollection<GetAwayCar>? GetAwayCars { get; set; }
        [Display(Name = "Lid van")]
        public Gang? Gang { get; set; }
        [Display(Name = "Eigen gang")]
        public int? OwnGangId { get; set; }
        [ForeignKey("OwnGangId")]
        public Gang? OwnGang { get; set; }
        public Casino? Casino { get; set; }
        public Factory? Factory { get; set; }
        [Display(Name = "Huidige stad")]
        //[Required]
        public virtual City? CurrentCity { get; set; }
        public ICollection<PersonalMessage>? ReceivedMessages { get; set; }
        public ICollection<PersonalMessage>? SentMessages { get; set; }
        [Display(Name = "Persoonlijk vervoer")]
        public Transport? MeansOfTransportation { get; set; }
        [Display(Name = "Is in leven")]
        public bool IsAlive { get; set; } = true;
        public int? PrisonId { get; set; }
        [ForeignKey("PrisonId")]
        public Prison? Prison { get; set; }
        public DateTime InPrisonUntill { get; set; } = DateTime.Now;
        public int? BuyOutAmount { get; set; }
        [Display(Name = "Account aangemaakt op")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Geld op zak")]
        public int AmountOfMoneyInPocket { get; set; } = 0;
        [Display(Name = "Geld op de bank")]
        public int AmountOfMoneyInBank { get; set; } = 0;
        [Display(Name = "Hoeveelheid kogels")]
        public int AmountOfBullets { get; set; } = 0;
        [Range(0, 100), Display(Name = "Gezondheid")]
        public int HP { get; set; } = 100;
        [Range(0.0, 100.0), Display(Name = "Rank ervaring")]
        public double RankProgression { get; set; } = 0.0;
        [Range(0.0, 100.0), Display(Name = "Schiet ervaring")]
        public double ShootingSkill { get; set; } = 0.0;
        [Range(0.0, 100.0), Display(Name = "Rij ervaring")]
        public double DrivingSkill { get; set; } = 0.0;
        [Range(0.0, 100.0), Display(Name = "Uitbreek ervaring")]
        public double BreakoutSkill { get; set; } = 0.0;
        [Display(Name = "Wapen")]
        public Weapon Weapon { get; set; } = Weapon.Stiletto;
        [Display(Name = "Rang")]
        public Rank Rank { get; set; } = Rank.Newbie;
        public DateTime NextBasicCrimeAt { get; set; } = DateTime.Now;
    }
}
