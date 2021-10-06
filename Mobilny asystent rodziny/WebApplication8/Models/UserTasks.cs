using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class UserTasks
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Utworzono")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Zakończono")]
        public DateTime? EndedAt { get; set; }
        [Column(TypeName = "text")]
        [Display(Name = "Utworzył")]
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "Pole zadanie jest wymagane")]
        [Column(TypeName = "text")]
        [Display(Name = "Zadanie")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Pole opis jest wymagane")]
        [Display(Name = "Opis")]
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Pole grupa zadań jest wymagane")]
        [Display(Name = "Grupa zadań")]
        public string TaskGroup { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "text")]
        [Display(Name = "Przypisana osoba")]
        public string WantedUser { get; set; }
    }
}

