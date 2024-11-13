namespace ClientUDP.Instances
{
    using System.Collections.Generic;
    using System.Text;

    public class ModeTracker
    {
        private object _lock = new object();
        private readonly LinkedList<ModeEntry> _modeQueue = new LinkedList<ModeEntry>();
        private readonly Dictionary<double, int> _valueFrequencyMap = new Dictionary<double, int>();
        private readonly int _preciseness;

        public ModeTracker(int preciseness = 2)
        {
            if (preciseness <= 0) throw new ArgumentException("Preciseness should be positve");

            _preciseness = preciseness;
            _modeQueue.AddFirst(new ModeEntry(0,0));
        }
        public void AddValue(double value)
        {
            double rounded = Math.Round(value, _preciseness, MidpointRounding.AwayFromZero);

            lock (_lock)
            {
                if (_valueFrequencyMap.ContainsKey(rounded))
                {
                    _valueFrequencyMap[rounded]++;
                }
                else
                {
                    _valueFrequencyMap[rounded] = 1;
                }

                int frequency = _valueFrequencyMap[rounded];

                if (frequency >= _modeQueue.First?.Value.Frequency)
                {
                    _modeQueue.AddFirst(new ModeEntry(rounded, frequency));

                    while (_modeQueue.Last?.Value.Frequency < frequency)
                    {

                        _modeQueue.RemoveLast();
                    }
                }
            }
            
        }

        public (string numbers, int frequency) GetModeValues()
        {
            lock (_lock)
            {
                var numbers = $"[ {string.Join(", ", _modeQueue.Select(x => x.Value))} ]"; // builder inside
                return (numbers, _modeQueue.First.Value.Frequency);
            }
        }

    }

    public record ModeEntry(double Value, int Frequency);
}
