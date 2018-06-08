using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public abstract class Reader
    {
        abstract public string Read();
        abstract public void Close();
        abstract public bool Done();
    }
}
