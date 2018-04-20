using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    internal struct ObjectPosition
    {
        private static string[] labels = new[] { "car", "person", "cup", "laptop", "window", "charger", "table", "chair", "bed", "plate", "bag", "straw" };

        private string Value { get; set; }
        private Vector3 Position { get; set; }

        private static int idCounter = 0;

        public int Id
        {
            get;
        }

        private static Random random = new Random();

        public ObjectPosition(Vector3 position, string value)
        {
            Id = idCounter++;
            Value = value;
            Position = position;
        }

        private static double RandomPositionComponent()
        {
            return random.NextDouble() * 20 - 10;
        }

        public void Transform(Vector3 position)
        {
            throw new NotImplementedException();
        }

        internal static ObjectPosition Random()
        {
            return new ObjectPosition(new Vector3(RandomPositionComponent(), RandomPositionComponent(), RandomPositionComponent()), 
                labels[random.Next(labels.Length)]);
        }
    }
}