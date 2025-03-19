using System.ComponentModel.DataAnnotations;

namespace WebApiTemplate.Repository.Database.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
