using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bath
{
    public interface IFileWriter
    {
        void WriteToFile(string path, IEnumerable<string> content);
    }

    public class FileWriter : IFileWriter
    {
        public void WriteToFile(string path, IEnumerable<string> content)
        {
            File.WriteAllLines(path, content);
        }
    }
}
