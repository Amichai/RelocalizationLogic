using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    internal struct ObjectPosition
    {
        private static string[] labels = new[] { "car", "person", "cup", "laptop", "window", "charger", "table", "chair", "bed", "plate", "bag", "straw" };

        public string Value { get; }
        public Vector3 Position { get;  }

        private static int idCounter = 0;

        public int Id
        {
            get;
        }

        private static Random random = new Random();

        public ObjectPosition(Vector3 position, string value, int id)
        {
            Id = id;
            Value = value;
            Position = position;
        }

        internal ObjectPosition Clone()
        {
            return new ObjectPosition(new Vector3(Position.X, Position.Y, Position.Z), Value, Id);
        }

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

        public ObjectPosition Transform(Matrix4x4 matrix)
        {
            //Debug.Print($"{Position}");
            //matrix.PrintWeights();
            //Debug.Print($"{matrix * Position}");

            return new ObjectPosition(matrix * Position, Value, Id);
        }

        internal static ObjectPosition Random()
        {
            return new ObjectPosition(new Vector3(RandomPositionComponent(), RandomPositionComponent(), RandomPositionComponent()), 
                labels[random.Next(labels.Length)]);
        }
    }
}