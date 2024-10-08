﻿using Microsoft.EntityFrameworkCore;
using SpostatoBL.Enum;
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
        public bool HasPrivateJet { get; set; } = false;
        [Display(Name = "Is in leven")]
        public bool IsAlive { get; set; } = true;
        public int? PrisonId { get; set; }
        [ForeignKey("PrisonId")]
        public Prison? Prison { get; set; }
        public DateTime InPrisonUntill { get; set; } = DateTime.Now;
        public int BreakoutAttemptsLeft { get; set; } = 5;
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
        public WeaponEnum? Weapon { get; set; } = null;
        [Display(Name = "Rang")]
        public RankEnum Rank { get; set; } = RankEnum.Straatschoffie;
        public DateTime NextBasicCrimeAt { get; set; } = DateTime.Now;
    }
}
