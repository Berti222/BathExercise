using Bath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BathConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileReader fileReader = new FileReader();
            IDataReader dataReader = new DataReader();
            IObjectCreator objectCreator = new ObjectCreator();

            IDataBaseCreater dataBaseCreater = new DataBaseCreater(fileReader, dataReader, objectCreator);
            IFileWriter fileWriter = new FileWriter();

            // please Change the path !!
            string readFilePath = "D:\\eretsegiFeladat\\furdoadat.txt";
            string writeFielPath = "D:\\eretsegiFeladat\\szauna.txt";

            IEnumerable<BathArea> bathAreas = DataCreater.CreateBathArea();

            dataBaseCreater.SaveDateToDataBase(readFilePath, bathAreas);

            // Exercises call-s
            Exercises exercises = new Exercises(dataReader, fileWriter);
            exercises.Excercise2_FirstAndLastGuest();
            exercises.Excercise3_JustOneAreaOutOfDressingRoom();
            exercises.Excercise4_TheMostTimeInTheBath();
            exercises.Excercise5_StatisticOfTimeIntervals();
            exercises.Excercise6_GuestsInSaunaAndTheTimeTheWereThere(writeFielPath);
            exercises.Excercise7_HowMuchGuestUsedTheAreas();

            Console.ReadLine();
        }       
    }
}
