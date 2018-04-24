using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class Program
    {
        private static Random random = new Random();

        static void Main(string[] args)
        {
            var groundTruth = new GroundTruth();
            groundTruth.GenerateLayout(40);

            var agentA = new Agent();
            var agentB = new Agent();

            var solver = new TransformationSolver(agentA, agentB);

            var addThreshold = .5;

            foreach (var item in groundTruth.Items)
            {
                if (random.NextDouble() < addThreshold)
                {
                    agentA.Push(item.Clone());
                }

                if (random.NextDouble() < addThreshold)
                {
                    agentB.Push(item);
                }
            }

            solver.Solve();

            ///Place n strings in 3d space
            ///randomly select a string and stream to client a or b
            ///transform to two local coordinate systems
            ///calculate the transform given sets of the strings 
        }
    }
}
