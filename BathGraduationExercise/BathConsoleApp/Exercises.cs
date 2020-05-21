using Bath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BathConsoleApp
{
    public class Exercises
    {
        private readonly IDataReader _dataReader;
        private readonly IFileWriter _fileWriter;
        private IQueryable<BathData> _bathDatas;

        public Exercises(IDataReader dataReader, IFileWriter fileWriter)
        {
            _dataReader = dataReader;
            _fileWriter = fileWriter;
            InitialiseBathDatas();
        }

        public void InitialiseBathDatas()
        {
            _bathDatas = _dataReader.ReadAllDataFromDb();
        }

        public void Excercise2_FirstAndLastGuest()
        {
            InitialiseBathDatas();
            var firstGuestCheckOut = _bathDatas.Where(b => b.GuestSate == GuestSate.CheckOut).Min(b => b.CheckingTime);
            var lastGuestCheckOut = _bathDatas.Where(b => b.GuestSate == GuestSate.CheckOut).Max(b => b.CheckingTime);

            Console.WriteLine(
@"2. feladat
Az első vendég {0}-kor lépett ki az öltözőből.
Az utolsó vendég {1}-kor lépett ki az öltözőből.",
            CreateDateTimeString(firstGuestCheckOut),
            CreateDateTimeString(lastGuestCheckOut)
                );
            Console.WriteLine();
        }

        public void Excercise3_JustOneAreaOutOfDressingRoom()
        {            
            var groupedByGuests = from b in _bathDatas
                                  group b by b.GuestId into gr
                                  select new
                                  {
                                      GusetId = gr.Key,
                                      PlacesCount = gr.GroupBy(x => x.BathAreaId).Count()
                                  };

            int result = groupedByGuests.Where(x => x.PlacesCount == 2).Count();

            Console.WriteLine($"3. feladat \nA fürdőben {result} vendég járt csak egy részlegen. ");
            Console.WriteLine();
        }

        public void Excercise4_TheMostTimeInTheBath()
        {
            var groupedByGuests = (from b in _bathDatas
                                  group b by b.GuestId into gr
                                  select new
                                  {
                                      GusetId = gr.Key,
                                      Arrival = gr.OrderBy(x => x.CheckingTime).FirstOrDefault().CheckingTime,
                                      Exit = gr.OrderByDescending(x => x.CheckingTime).FirstOrDefault().CheckingTime
                                  }).ToList();

            var results = from g in groupedByGuests
                          select new
                          {
                              g.GusetId,
                              StayTime = g.Exit - g.Arrival
                          };

            var result = results.OrderByDescending(x => x.StayTime).FirstOrDefault();
          
            Console.WriteLine($"4.feladat \nA legtöbb időt eltöltő vendég: \n{result.GusetId}.vendég {CreateTimeSpanString(result.StayTime)}");
            Console.WriteLine();
        }

        public void Excercise5_StatisticOfTimeIntervals()
        {
            var guestsArrivalTime = (from b in _bathDatas
                                    group b by b.GuestId into gr
                                    select new
                                    {
                                        gr.Key,
                                        gr.OrderBy(x => x.CheckingTime).FirstOrDefault().CheckingTime
                                    }).ToList();

            var arrivedBetween6And9 = guestsArrivalTime
                        .Where(
                            x => x.CheckingTime >= ObjectCreator.CreateDateTime(6, 0, 0) &&
                            x.CheckingTime < ObjectCreator.CreateDateTime(9, 0, 0)
                            ).Count();

            var arrivedBetween9And16 = guestsArrivalTime
                        .Where(
                            x => x.CheckingTime >= ObjectCreator.CreateDateTime(9, 0, 0) &&
                            x.CheckingTime < ObjectCreator.CreateDateTime(16, 0, 0)
                            ).Count();

            var arrivedBetween16And20 = guestsArrivalTime
                        .Where(
                            x => x.CheckingTime >= ObjectCreator.CreateDateTime(16, 0, 0) &&
                            x.CheckingTime < ObjectCreator.CreateDateTime(20, 0, 0)
                            ).Count();

            Console.WriteLine(
@"5. feladat
6 - 9 óra között {0} vendég
9 - 16 óra között {1} vendég
16 - 20 óra között {2} vendég ",
            arrivedBetween6And9,
            arrivedBetween9And16,
            arrivedBetween16And20);
            Console.WriteLine();
        }

        public void Excercise6_GuestsInSaunaAndTheTimeTheWereThere(string path)
        {
            var groupedByGuests = (from b in _bathDatas
                                   where b.BathArea.Name == "Szaunák"
                                   group b by b.GuestId into gr
                                   select new
                                   {
                                       GusetId = gr.Key,
                                       ChekIns = gr.Where(x => x.GuestSate == GuestSate.CheckIn).Select(x => x.CheckingTime),
                                       ChekOuts = gr.Where(x => x.GuestSate == GuestSate.CheckOut).Select(x => x.CheckingTime)
                                   }).ToList();

            var result = from g in groupedByGuests
                         select new
                         {
                             g.GusetId,
                             SaunaTime = TimeCounter(g.ChekIns, g.ChekOuts)
                         };

            List<string> rows = new List<string>();

            rows.Add("6. Feladat");

            foreach (var item in result)
            {
                rows.Add(item.GusetId + " : " + CreateTimeSpanString(item.SaunaTime));
            }

            _fileWriter.WriteToFile(path, rows);

            Console.WriteLine("6. Feladat \nFile Created Successfuly!!");
            Console.WriteLine();
        }

        public void Excercise7_HowMuchGuestUsedTheAreas()
        {
            var bal = _bathDatas.GroupBy(x => x.BathAreaId);

            var result = from b in _bathDatas
                         group b by b.BathArea.Name into gr
                         select new
                         {
                             BathArea = gr.Key,
                             NumsOfGuestsCount = gr.GroupBy(x => x.GuestId).Count()
                         };

            Console.WriteLine("7. feladat");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.BathArea}: {item.NumsOfGuestsCount}");
            }
        }

        private string CreateDateTimeString(DateTime time)
        {
            return $"{time.Hour}:{time.Minute}:{time.Second}";
        }
        private string CreateTimeSpanString(TimeSpan time)
        {
            return $"{time.Hours}:{time.Minutes}:{time.Seconds}";
        }

        private TimeSpan TimeCounter(IEnumerable<DateTime> arrive, IEnumerable<DateTime> exit)
        {
            var arrives = arrive.ToList();
            var exits = exit.ToList();

            TimeSpan result = new TimeSpan();
            for (int i = 0; i < exit.Count(); i++)
            {
                var ts = exits[i] - arrives[i];

                if (result == null)
                    result = ts;
                else
                    result += ts;
            }

            return result;
        }
        
    }
}
