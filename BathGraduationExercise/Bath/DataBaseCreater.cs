using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bath
{
    public interface IDataBaseCreater
    {
        void SaveDateToDataBase(string path, IEnumerable<BathArea> bathAreas);
    }

    public class DataBaseCreater : IDataBaseCreater
    {
        private readonly IFileReader _fileReader;
        private readonly IDataReader _dataReader;
        private readonly IObjectCreator _objectCreator;

        public DataBaseCreater(IFileReader fileReader, IDataReader dataReader, IObjectCreator objectCreator)
        {
            _fileReader = fileReader;
            _dataReader = dataReader;
            _objectCreator = objectCreator;
        }

        public void SaveDateToDataBase(string path, IEnumerable<BathArea> bathAreas)
        {
            IEnumerable<BathData> datas = _dataReader.ReadAllDataFromDb();
            BathData bathData = datas.FirstOrDefault();

            if (bathData != null)
                return;

            string[] fileData = _fileReader.ReadFile(path);
            datas = _objectCreator.CreateAllBathDataObjects(fileData);

            CreateBathAreas(bathAreas);

            using (BathContext db = new BathContext())
            {
                db.BathDatas.AddRange(datas);
                db.SaveChanges();
            }

        }        

        private void CreateBathAreas(IEnumerable<BathArea> bathAreas)
        {
            using (BathContext db = new BathContext())
            {
                db.BathAreas.AddRange(bathAreas);
                db.SaveChanges();
            }
        }

    }
}
