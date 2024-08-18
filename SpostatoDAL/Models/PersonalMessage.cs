using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoDAL.Models
{
    public class PersonalMessage
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        [Required]
        public string MessageContent { get; set; }
        [Required]
        public virtual Gangster Sender { get; set; }
        [Required]
        public virtual Gangster Reciever { get; set; }
        public bool Read { get; set; } = false;
    }
}
