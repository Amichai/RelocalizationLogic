using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    public class GradientAscent<T> where T : IPerturbable, IValueFunction, ISerializable<T>
    {
        private static Random rand = new Random();
        public GradientAscent(string path = null)
        {
            this.id = counter++;
            this.path = path;
        }
        private DateTime lastImprovement = DateTime.MinValue;
        private double improvementDiff = 0;
        private static int counter = 0;
        private int id;
        public Subject<T> NewBest = new Subject<T>();
        public bool IsPaused = false;

        private GradientAscentState<T> state;

        private int lastStepDirectionIndex;
        private int lastStepDirectionCount;

        public T Run(GradientAscentState<T> state, int maxIter)
        {
            this.state = state;
            IComparable value, lastValue;
            int iter = 0;
            int loopCount = 0;
            T current;
            do
            {
                lastValue = state.CurrentValue();
                value = lastValue;
                current = state.CurrentState();
                T optimized;
                T randomAdjustment;
                this.adjust(current, out value, out optimized, out randomAdjustment);
                if (gt(value, lastValue))
                {
                    current = optimized;
                    state.Update(current, value);
                    this.NewBest.OnNext(current);
                }
                iter = state.IterCount;
                Debug.Print("Val: {0} -ID:{1}", value, this.id);
                if (lastImprovement != DateTime.MinValue)
                {
                    Debug.Print("{0} diff: {1}, elapsed: {2} -ID:{3}",
                        iter,
                        (MatchDistance)value - (MatchDistance)lastValue,
                        DateTime.Now - lastImprovement, this.id);
                }
                lastImprovement = DateTime.Now;
                //Debug.Print("{0}\n\n", current.ToJson());
                this.IterationCount = iter;
            } while (gt(value, lastValue) && loopCount++ < maxIter);
            return current;
        }

        private string path;

        public T Run(T initial, int steps, IComparable minValue, double initialTemperature = 0)
        {
            this.state = new GradientAscentState<T>(path);
            var current = initial;
            double temperature = initialTemperature;
            IComparable value = minValue;
            IComparable lastValue = initial.Value();
            bool tryAgain = false;
            int iter = 0;
            do
            {
                if (this.IsPaused)
                {
                    Thread.Sleep(5000);
                    tryAgain = true;
                    continue;
                }
                tryAgain = false;
                lastValue = value;
                T optimized;
                T randomAdjustment;
                this.adjust(current, out value, out optimized, out randomAdjustment);
                if (value.CompareTo(lastValue) == 1)
                {
                    if (temperature == 0 || rand.NextDouble() > temperature)
                    {
                        current = optimized;
                    }
                    else
                    {
                        current = randomAdjustment;
                        value = current.Value();
                    }
                    this.state.Update(current, value);
                    this.NewBest.OnNext(current);
                }
                temperature -= .001;
                temperature = Math.Max(temperature, 0);
                iter++;
                Debug.Print("Val: {0} -ID:{1}, iter: {2}", value, this.id, iter);

                Debug.Print("Val: {0} -ID:{1}", value, this.id);
                if (lastImprovement != DateTime.MinValue)
                {
                    Debug.Print("{0} diff: {1}, elapsed: {2} -ID:{3}",
                        iter,
                        (MatchDistance)value - (MatchDistance)lastValue,
                        DateTime.Now - lastImprovement, this.id);
                }
                lastImprovement = DateTime.Now;
                //Debug.Print("{0} -ID:{1}\n\n", current.ToString(), this.id);
                this.IterationCount = iter + 1;
            } while ((gt(value, lastValue) || temperature > 0 || this.IsPaused || tryAgain) && iter < steps);
            this.ResolvedValue = value;
            Debug.Print("DONE! -ID:{0}", this.id);
            //log.Debug(string.Format("Val: {0}", value));
            //log.Debug(current.ToString());
            return current;
        }

        public IComparable ResolvedValue { get; private set; }

        private bool gt(IComparable a, IComparable b)
        {
            return a.CompareTo(b) == 1;
        }

        private void adjust(T input, out IComparable value, out T optimizedRoster, out T randomAdjustment)
        {
            List<T> options = new List<T>();

            if (input is IDynamicPerturbable)
            {
                foreach (var pertubation in ((IDynamicPerturbable)input).Pertubations(lastStepDirectionIndex, lastStepDirectionCount))
                {
                    options.Add((T)pertubation);
                }
            }
            else
            {
                foreach (var pertubation in input.Pertubations())
                {
                    options.Add((T)pertubation);
                }
            }
            optimizedRoster = this.getBest(options, out value);
            randomAdjustment = options[rand.Next(options.Count)];
        }

        private T getBest(List<T> options, out IComparable value)
        {
            var idx = options.MaxIndex(i => i.Value());

            if (idx == lastStepDirectionIndex)
            {
                lastStepDirectionCount++;
                Debug.Print("Direction count: {0}, index: {1}", lastStepDirectionCount, idx);
            }
            else
            {
                lastStepDirectionIndex = idx;
                lastStepDirectionCount = 1;
            }

            T r = options[idx];
            value = r.Value();
            //Debug.Print(value.ToString());
            return r;
        }

        public int IterationCount { get; set; }

        public GradientAscentState<T> GetState()
        {
            return this.state;
        }
    }

    public interface IDynamicPerturbable
    {
        IEnumerable<IValueFunction> Pertubations(int idx, int factor);
    }

    public interface IPerturbable
    {
        IEnumerable<IValueFunction> Pertubations();
    }

    public interface IValueFunction
    {
        IComparable Value();
    }

    public interface ISerializable<T>
    {
        string ToJson();
    }
}
