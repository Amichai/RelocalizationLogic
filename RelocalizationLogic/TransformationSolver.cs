using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class TransformationSolver
    {
        private Agent a, b;

        public TransformationSolver(Agent a, Agent b)
        {
            this.a = a;
            this.b = b;
        }

        public void Solve()
        {
            ///initialize random solution
            ///check the error
            ///For each component, take the gradient ascent
            ///
            var ga = new GradientAscent<PerturbableTransformationMatrix>();


            var randomMatrix = Matrix4x4.RandomProjection();

            int maxIter = 500;
            var minDistance = new MatchDistance(0, 0);

            int searchRadius = 60;

            //var d1 = MatchDistance(randomMatrix);

            var d1  = Value(randomMatrix, searchRadius, out var initialDistance);
            Debug.Print($"Initial: {d1.MatchCount}: {initialDistance}");


            var result = ga.Run(new PerturbableTransformationMatrix(randomMatrix, m => Value(m, searchRadius, out var dist)), maxIter, minDistance);

            //var d2 = MatchDistance(result.matrix);

            //Debug.Print($"Before after: {d1}, {d2}");

            d1 = Value(result.matrix, searchRadius, out initialDistance);
            Debug.Print($"Next: {d1.MatchCount}: {initialDistance}");

            for (int i = 0; i < 4; i++)
            {
                searchRadius -= 10;

                Debug.Print($"--- Index: {i}, search radius: {searchRadius}");

                result.SetValueFunc(m => Value(m, searchRadius, out var dist));

                result = ga.Run(result, maxIter, minDistance);

                //d2 = MatchDistance(result.matrix);

                //Debug.Print($"Before after: {d1}, {d2}");

                d1 = Value(result.matrix, searchRadius, out initialDistance);
                Debug.Print($"Next: {d1.MatchCount}: {initialDistance}");
            }

            /// Validate the results

        }

        private double MatchDistance(Matrix4x4 matrix)
        {
            var objectsA = a.Transform(matrix);
            var objectsB = b.SeenObjects;

            ////var objectsA = a.Transform(matrix);
            ////var objectsB = a.SeenObjects;

            ///For each object in A,
            ///ask if there is a corresponding object within our distance tolerance
            ///
            var totalDistance = 0d;
            foreach (var obj in objectsB)
            {
                ObjectPosition? match = objectsA.SingleOrDefault(i => i.Id == obj.Id);

                if(match == null)
                {
                    continue;
                }

                var distance = match?.Position.Distance(obj.Position);

                Debug.Print($"{distance}");

                totalDistance += distance.Value.Sqrd();
            }

            return totalDistance;
        }




        private MatchDistance Value(Matrix4x4 matrix, int searchRadius, out double totalDistance)
        {
            var objectsA = a.Transform(matrix);
            var objectsB = b.SeenObjects;

            int matchCount = 0;

            var matchDistance = 0d;
            foreach(var obj in objectsB)
            {
                if(IsMatch(obj, objectsA, searchRadius, out double distance))
                {
                    matchDistance += distance.Sqrd();
                    matchCount++;
                }
            }

            totalDistance = matchDistance;

            return new MatchDistance(matchCount, 100000 / matchDistance);
        }

        private bool IsMatch(ObjectPosition search, IList<ObjectPosition> searchSpace, double distanceTolerance, out double matchDistance)
        {
            matchDistance = 0;
            if(!searchSpace.Any(i => i.Value == search.Value))
            {
                return false;
            }

            var canditates = searchSpace.Where(i => i.Value == search.Value);

            var bestMatch = canditates.OrderBy(i => i.Position.Distance(search.Position)).First();

            var d = bestMatch.Position.Distance(search.Position);
            if (d < distanceTolerance)
            {
                matchDistance = d;
                return true;
            }

            return false;
        }
    }
}
