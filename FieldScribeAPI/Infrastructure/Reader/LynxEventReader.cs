using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class LynxEventReader : ReaderDecorator
    {
        private List<int> _fieldEvtNums;
        private string _line;

        public LynxEventReader(List<int> fieldEvtNums,
            Reader wrapped) : base(wrapped)
        {
            _fieldEvtNums = fieldEvtNums;

            _fieldEvtNums.Sort();

            if(!Wrapped.Done())
            {
                _line = Wrapped.Read();
            }
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

        public List<LynxEvent> ReadEvents()
        {
            List<LynxEvent> newEvents = new List<LynxEvent>();

            LynxEvent evt = ReadEvent();

            while (evt != null)
            {
                newEvents.Add(evt);
                evt = ReadEvent();
            }

            return newEvents;
        }

        private LynxEvent ReadEvent()
        {
            if(Wrapped.Done())
            {
                return null;
            }
            else
            {
                string[] parts = _line.Split(',');

                if (_line[0] != ',' &&
                    _fieldEvtNums.BinarySearch(int.Parse(parts[0])) >= 0)
                {
                    LynxEvent evt = new LynxEvent
                    {
                        EventNum = int.Parse(parts[0]),

                        Key = new EventKey
                        {
                            RoundNum = int.Parse(parts[1]),
                            FlightNum = int.Parse(parts[2]),
                            EventName = parts[3]
                        }
                    };

                    evt.Entries = new List<LynxEntry>();

                    LynxEntry ent = ReadEntry();

                    while (ent != null)
                    {
                        evt.Entries.Add(ent);
                        ent = ReadEntry();
                    }

                    return evt;
                }
                else
                {
                    _line = Wrapped.Read();
                    return ReadEvent();
                }
            }
        }

        private LynxEntry ReadEntry()
        {
            if (Wrapped.Done())
            {
                return null;
            }
            else
            {
                _line = Wrapped.Read();

                if (_line[0] == ',')
                {
                    string[] parts = _line.Split(',');

                    LynxEntry ent = new LynxEntry
                    {
                        CompNum = int.Parse(parts[1]),
                        CompetePosition = int.Parse(parts[2])
                    };

                    return ent;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
