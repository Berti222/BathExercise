using System.ComponentModel.DataAnnotations;

namespace Bath
{
    public class BathArea
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}