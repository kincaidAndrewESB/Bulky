using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key] //not required as Id will be recognised as Primary Key
        public int Id { get; set; }
        [Required]//data anotation
        [MaxLength(30)]
        [DisplayName("Category Name")] 
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="The field Display Order must be between 1 and 100")]//tag helpers
        public int DisplayOrder { get; set; }
    }


}
