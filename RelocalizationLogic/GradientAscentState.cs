using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    public class GradientAscentState<T> where T : ISerializable<T>
    {
        public GradientAscentState(string path)
        {
            this.Path = path;
        }

        internal IComparable CurrentValue()
        {
            return value;
        }

        public JObject ToJson()
        {
            var toReturn = new JObject();
            toReturn["Iter"] = this.IterCount;
            toReturn["Value"] = this.value.ToString();
            toReturn["State"] = this.state.ToJson();
            return toReturn;
        }

        public T CurrentState()
        {
            return state;
        }

        public int IterCount { get; set; }
        private T state;
        private IComparable value;

        public void Update(T state, IComparable value)
        {
            this.state = state;
            this.value = value;
            this.IterCount++;
        }

        public static GradientAscentState<T> Load(string path, Func<string, T> constructor)
        {
            var content = File.ReadAllText(path);
            var json = JObject.Parse(content);
            var state = new GradientAscentState<T>(path);
            state.IterCount = json["Iter"].Value<int>();
            state.value = json["Value"].Value<double>();
            var stateContent = json["State"].ToString();
            state.state = constructor(stateContent);
            return state;
        }

        public string Path { get; private set; }

        public void Save()
        {
            var content = this.ToJson().ToString();
            File.WriteAllText(this.Path, content);
        }
    }
}
