using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class GroundTruth
    {
        private List<ObjectPosition> objectPositions = new List<ObjectPosition>();

        public IReadOnlyList<ObjectPosition> Items => objectPositions;

        public GroundTruth()
        {
            //TODO: confidence values
        }

        public void GenerateLayout(int count)
        {
            for (int i = 0; i < count; i++)
            {
                objectPositions.Add(ObjectPosition.Random());
            }
        }
    }
}
