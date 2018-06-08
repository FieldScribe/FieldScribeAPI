using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class LynxSchReader : ReaderDecorator
    {
        public LynxSchReader(Reader wrapped) : base(wrapped)
        {
            Wrapped.Read(); // Throw away first line of .sch file
        }
        public override void Close()
        {
            Wrapped.Close();
        }

        public override bool Done()
        {
            return Wrapped.Done();
        }

        public override string Read()
        {
            return Wrapped.Read();
        }

        public List<int> ReadFieldEvtNums()
        {
            List<int> fieldEvts = new List<int>();

            int i = ReadEvtNum();

            while (i != -1)
            {
                fieldEvts.Add(i);
                i = ReadEvtNum();
            }

            return fieldEvts;
        }

        private int ReadEvtNum()
        {
            if (Wrapped.Done())
            {
                return -1;
            }
            else
            {
                string[] parts = Wrapped.Read().Split(',');

                return int.Parse(parts[0]);
            }
        }
    }
}
