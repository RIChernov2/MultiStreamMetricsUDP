namespace ClientUDP.Instances
{
    using System.Text;
    using ConfigurationUDP.Instances;

    public class ClientMetricTracker
    {
        private object _locker = new object();
        private int _receivedDataCount = 0;
        private int _firstReceivedPacketNumber = -1;
        private int _lastReceivedPacketNumber = 0;

        private double _averege = 0;
        private double _averegeOld = 0;
        private double _dispersia = 0;
        private double _dispersiaOld = 0;
        private double StandardDeviation => GetStandardDeviation();

        private OnlineMedianCalculator _medianCalculator = new OnlineMedianCalculator();
        private ModeTracker _modeTracker = new ModeTracker();

        public void AddValues(DataPackage dataPackage)
        {
            lock (_locker)
            {
                _receivedDataCount++;


                if (_firstReceivedPacketNumber == -1)
                {
                    _firstReceivedPacketNumber = dataPackage.OrderNumber;
                }

                _lastReceivedPacketNumber = dataPackage.OrderNumber;



                var mathTask = Task.Run(() =>
                {
                    UpdateAvarage(dataPackage.Value);
                });

                mathTask.ContinueWith(task => 
                {
                    UpdateStandardDeviationByDispercia(dataPackage.Value);
                });

                var medianTask = Task.Run(() =>  _medianCalculator.AddValue(dataPackage.Value));
                var modeTask = Task.Run(() => _modeTracker.AddValue(dataPackage.Value));

                Task.WaitAll(mathTask, medianTask, modeTask);
            }
        }

        public string GetStatisticsMessage()
        {

            if (_firstReceivedPacketNumber == -1)
            {
                return "No packets have been received yet";
            }

            StringBuilder builder = new StringBuilder();

#if DEBUG
            DateTime start = DateTime.Now;
#endif

            lock (_locker)
            {
                string offset = "...";
                string tab = $"{offset} ";
                string tab3 = $"{offset}{offset}{offset} ";
                string tab4 = $"{offset}{offset}{offset}{offset} ";

                builder.Append(Environment.NewLine);
                builder.AppendLine($"{tab}Current metrics:");
                AddPacketLossInfoMessage(builder, $"{offset}{offset} ");
                builder.AppendLine($"{tab3}Average {_averege:F6}");
                builder.AppendLine($"{tab3}Standard deviation {StandardDeviation:F6}");
                builder.AppendLine($"{tab3}Median {_medianCalculator.GetMedian():F6}");
                AddModaMessage(builder, $"{offset}{offset}{offset}{offset} ");
#if DEBUG
                builder.AppendLine($"{tab}Time spent for response (milliseconds): {(DateTime.Now - start).TotalMilliseconds:F3}");
#endif
            }

            return builder.ToString();
        }

        private void UpdateAvarage(double newValue)
        {
            _averegeOld = _averege;
            _averege = _averegeOld + (newValue - _averegeOld) / _receivedDataCount;
        }

        private void UpdateStandardDeviationByDispercia(double newValue)
        {
            if (_receivedDataCount == 1) return;


            _dispersia = (
                (_receivedDataCount - 1) * _dispersiaOld + Math.Pow(newValue - _averegeOld, 2)
                ) / _receivedDataCount;

            _dispersiaOld = _dispersia;
        }

        private double GetStandardDeviation()
        {
            return Math.Sqrt(_dispersia);
        }

        private void AddPacketLossInfoMessage(StringBuilder builder, string tab = null)
        {
            double totalPacketsSent = _lastReceivedPacketNumber - _firstReceivedPacketNumber + 1;
            double packetLoss = totalPacketsSent - _receivedDataCount;

            double percentageOfLoss = packetLoss / totalPacketsSent * 100;

            builder.AppendLine($"{tab}Total packets sent: {totalPacketsSent}");
            builder.AppendLine($"{tab}Packets lost: {packetLoss}");
            builder.AppendLine($"{tab}Packet loss percentage: {percentageOfLoss}");
        }

        private void AddModaMessage(StringBuilder builder, string tab = null)
        {
            var result = _modeTracker.GetModeValues();
            var frequency = result.frequency;
            var numbers = result.numbers;

            builder.AppendLine($"{tab}Mode(s) with frequency {frequency}: {numbers:F4}");

        }
    }
}
