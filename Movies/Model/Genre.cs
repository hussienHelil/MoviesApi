using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Model
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }    
    }
}
