using System.CodeDom;
using System.Data.Entity;

namespace Bath
{
    public class BathContext : DbContext
    {
        public BathContext()
            :base("name=DefaultConnection")
        {
        }
        public DbSet<BathArea> BathAreas { get; set; }
        public DbSet<BathData> BathDatas { get; set; }
    }
}
