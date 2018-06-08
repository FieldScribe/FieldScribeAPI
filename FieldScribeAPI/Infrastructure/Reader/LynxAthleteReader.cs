using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class LynxAthleteReader : ReaderDecorator
    {
        public LynxAthleteReader(Reader wrapped) : base(wrapped)
        {
            Wrapped.Read(); // Throw away first line of .ppl file
        }

        public override string Read()
        {
            return Wrapped.Read();
        }

        public override void Close()
        {
            Wrapped.Close();
        }

        public override bool Done()
        {
            return Wrapped.Done();
        }

        public List<LynxAthlete> ReadAthletes()
        {
            List<LynxAthlete> newAthletes = new List<LynxAthlete>();

            LynxAthlete a = ReadAthlete();

            while (a != null)
            {
                newAthletes.Add(a);
                a = ReadAthlete();
            }

            return newAthletes;
        }

        private LynxAthlete ReadAthlete()
        {
            if (Wrapped.Done())
            {
                return null;
            }
            else
            {
                string[] parts = Wrapped.Read().Split(',');

                LynxAthlete athlete = new LynxAthlete
                { 
                    CompNum     = int.Parse(parts[0]),
                    LastName    = parts[1],
                    FirstName   = parts[2],
                    TeamName    = parts[3],
                    Gender      = parts[5]
                };

                return athlete;
            }
        }
    }
}
