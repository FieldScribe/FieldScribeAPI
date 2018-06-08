using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class CSV : ReaderDecorator
    {
        private string[] parseString;
        public int i = -1; // Index

        public CSV(Reader wrapped) : base(wrapped)
        {
            parseString = Wrapped.Read().Split(',');
        }

        public string GetToken()
        {
            i++;

            if (i == parseString.Length)
            {
                i = 0;
                parseString = Wrapped.Read().Split(',');
            }

            return parseString[i];
        }

        public override string Read()
        {
            return GetToken();
        }

        public override void Close()
        {
            Wrapped.Close();
        }

        public override bool Done()
        {
            return Wrapped.Done();
        }
    }
}
