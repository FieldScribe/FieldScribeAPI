using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public abstract class ReaderDecorator : Reader
    {
        private Reader wrappedReader;

        protected ReaderDecorator(Reader wrapped)
        {
            wrappedReader = wrapped;
        }

        protected Reader Wrapped { get { return wrappedReader; } }
    }
}
