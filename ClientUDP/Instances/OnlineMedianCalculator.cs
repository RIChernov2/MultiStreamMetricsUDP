namespace ClientUDP.Instances
{
    using System.Collections.Generic;

    public class OnlineMedianCalculator
    {

        private static object s_locker = new object();
        private static SortedSet<double> s_maxHeap  // Descending
            = new SortedSet<double>(Comparer<double>.Create((x, y) => y.CompareTo(x)));
        private static SortedSet<double> s_minHeap = new SortedSet<double>(); // Ascending

        private static Queue<double> s_window = new Queue<double>();  // Last s_windowSize elements
        private static int s_windowSize = 1000;  // Ammount of storring elements
        private static int s_windowSizeMin = 100;
        private static int s_windowSizeMax = 10_000;

        /// <summary>
        /// Gets the number of elements stored for the calculation of the median.
        /// </summary>
        public static int GetWindowSize() => s_windowSize;

        /// <summary>
        /// Sets the number of elements stored for the calculation of the median.
        /// Valid values must be within the range (100 : 10000), inclusive.
        /// </summary>
        public static (bool success, string? errorMessage) SetWindowSize(int windowSize)
        {
            if (windowSize < s_windowSizeMin || windowSize > s_windowSizeMax)
            {
                return new(false, $"Size should be in range ({s_windowSizeMin} : {s_windowSizeMax} inclusive");
            }

            lock (s_locker)
            {
                s_windowSize = windowSize;
            }

            return new(true, null);
        }

        public void AddValue(double value)
        {
            lock (s_locker)
            {
                s_window.Enqueue(value);

                CorrectHeapsByWindowSize();

                if (s_maxHeap.Count == 0 || value <= s_maxHeap.Min)
                {
                    AddToMaxHeap(value);
                }
                else
                {
                    AddToMinHeap(value);
                }

                while (Math.Abs(s_maxHeap.Count - s_minHeap.Count) > 1)
                {
                    if (s_maxHeap.Count > s_minHeap.Count + 1)
                    {
                        MoveFromMaxToMinHeap();
                    }
                    else if (s_minHeap.Count > s_maxHeap.Count)
                    {
                        MoveFromMinToMaxHeap();
                    }
                }


            }
        }
        public double GetMedian()
        {
            lock (s_locker)
            {
                if (s_maxHeap.Count > s_minHeap.Count)
                {
                    return s_maxHeap.Min;
                }
                else
                {
                    return (s_maxHeap.Min + s_minHeap.Min) / 2.0;
                }
            }
        }

        private void CorrectHeapsByWindowSize()
        {
            if (s_window.Count > s_windowSize)
            {
                double oldest = s_window.Dequeue();
                if (s_maxHeap.Contains(oldest))
                {
                    s_maxHeap.Remove(oldest);
                }
                else
                {
                    s_minHeap.Remove(oldest);
                }
            }
        }

        private void MoveFromMaxToMinHeap()
        {
            s_minHeap.Add(s_maxHeap.Min);
            s_maxHeap.Remove(s_maxHeap.Min);
        }

        private void MoveFromMinToMaxHeap()
        {
            s_maxHeap.Add(s_minHeap.Min);
            s_minHeap.Remove(s_minHeap.Min);
        }

        private void AddToMaxHeap(double value)
        {
            if (s_maxHeap.Add(value) != true)
            {
                s_minHeap.Remove(s_minHeap.Max);
                MoveFromMaxToMinHeap();
            }
        }

        private void AddToMinHeap(double value)
        {
            if (s_minHeap.Add(value) != true)
            {
                s_maxHeap.Remove(s_maxHeap.Max);
                MoveFromMinToMaxHeap();
            }
        }

        


    }
}
