using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class FileTextReader : Reader
    {
        private StringReader sReader;

        public FileTextReader(string fileText)
        {
            sReader = new StringReader(fileText);

            if (sReader == null)
            {
                throw new Exception("Empty file");
            }
        }

        public override string Read()
        {
            return sReader.ReadLine();
        }

        public override void Close()
        {
            sReader.Close();
        }

        public override bool Done()
        {
            if (sReader.Peek() == -1)
            {
                return true;
            }
            else { return false; }
        }
    }
}

