using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class Agent
    {
        public Agent()
        {
            seenObjects = new List<ObjectPosition>();
        }

        private List<ObjectPosition> seenObjects;
        public IReadOnlyCollection<ObjectPosition> SeenObjects => seenObjects;

        internal void Push(ObjectPosition item)
        {
            seenObjects.Add(item);
        }

        public List<ObjectPosition> Transform(Matrix4x4 matrix)
        {
            return seenObjects.Select(i => i.Transform(matrix)).ToList();
        }

        public static Agent ParseFromFile(string path)
        {
            var toReturn = new Agent();

            var lines = File.ReadAllLines(path);

            var prefixBreak = @"] ";

            foreach (var line in lines)
            {
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var index = line.IndexOf(prefixBreak);

                var l = line.Substring(index + prefixBreak.Length);

                if(l[0] == '-')
                {
                    continue;
                }

                var prefixBreak2 = " --> ";

                var index2 = l.IndexOf(prefixBreak2);

                var l2 = l.Substring(index2 + prefixBreak2.Length);


                var parts = l2.Split(' ');
                var value = l.Substring(0, index2);

                var x = double.Parse(parts[2].Trim(','));
                var y = double.Parse(parts[4].Trim(','));
                var z = double.Parse(parts[6].Trim(',', ')'));

                var vec = new Vector3(x, y, z);

                var op = new ObjectPosition(vec, value, 0);

                toReturn.seenObjects.Add(op);
                Debug.Print($"{value}|{x},{y},{z}");
                //var 
            }

            return toReturn;
        }
    }
}
