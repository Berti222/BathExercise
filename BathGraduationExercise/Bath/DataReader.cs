using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bath
{
    public interface IDataReader
    {
        IQueryable<BathData> ReadAllDataFromDb();
    }

    public class DataReader : IDataReader, IDisposable
    {
        private readonly BathContext _db;
        public DataReader()
        {
            _db = new BathContext();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IQueryable<BathData> ReadAllDataFromDb()
        {          
                return _db.BathDatas.Include("BathArea");  
        }
    }
}
