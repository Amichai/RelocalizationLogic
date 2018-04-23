using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class Agent
    {
        public Agent()
        {
            projectionMatrix = Matrix4x4.RandomProjection();
            seenObjects = new List<ObjectPosition>();
        }

        private List<ObjectPosition> seenObjects;
        public IReadOnlyCollection<ObjectPosition> SeenObjects => seenObjects;

        private Matrix4x4 projectionMatrix;

        internal void Push(ObjectPosition item)
        {
            seenObjects.Add(item);
        }

        public List<ObjectPosition> Transform(Matrix4x4 matrix)
        {
            return seenObjects.Select(i => i.Transform(matrix)).ToList();
        }
    }
}
