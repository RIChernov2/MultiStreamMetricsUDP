namespace ServerUDP.Instances
{
    using System;
    using ServerUDP.Instances.Interfaces;

    public class RandomValueGeneretor : IValueGeneretor
    {
        private Random _random;
        private int _minValue;
        private int _maxValue;

        public RandomValueGeneretor(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _random = new Random();
        }

        public double Next()
        {
            int integerPart =  _random.Next(_minValue, _maxValue);
            double fractionalPart =  _random.NextDouble();
            return integerPart + fractionalPart;
        }
    }
}
