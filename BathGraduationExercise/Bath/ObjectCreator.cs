using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bath
{
    public interface IObjectCreator
    {
        IEnumerable<BathData> CreateAllBathDataObjects(string[] datas);
    }

    public class ObjectCreator : IObjectCreator
    {
        public IEnumerable<BathData> CreateAllBathDataObjects(string[] datas)
        {
            List<BathData> result = new List<BathData>();

            foreach (string data in datas)
            {
                string[] dataPieces = data.Split(' ');

                result.Add(CreateBathDataObject(dataPieces));
            }

            return result;
        }

        private BathData CreateBathDataObject(string[] dataPieces)
        {
            int hour = int.Parse(dataPieces[3]);
            int minute = int.Parse(dataPieces[4]);
            int second = int.Parse(dataPieces[5]);

            BathData bathData = new BathData
            {
                GuestId = dataPieces[0],
                BathAreaId = int.Parse(dataPieces[1]) + 1,
                GuestSate = (GuestSate)int.Parse(dataPieces[2]),
                CheckingTime = CreateDateTime(hour, minute, second)
            };

            return bathData;
        }

        public static DateTime CreateDateTime(int hour, int minute, int second)
        {
            return new DateTime(2020, 1, 1, hour, minute, second);
        }
    }
}
