using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bath
{
    public static class DataCreater
    {
        public static IEnumerable<BathArea> CreateBathArea()
        {
            List<BathArea> bathAreas = new List<BathArea>();

            bathAreas.Add(new BathArea { Name = "Öltöző" });
            bathAreas.Add(new BathArea { Name = "Uszoda" });
            bathAreas.Add(new BathArea { Name = "Szaunák" });
            bathAreas.Add(new BathArea { Name = "Gyógyvizes medencék" });
            bathAreas.Add(new BathArea { Name = "Strand" });

            return bathAreas;
        }
    }
}
