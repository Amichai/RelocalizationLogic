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

            var d1 = MatchDistance(randomMatrix);

            var result = ga.Run(new PerturbableTransformationMatrix(randomMatrix, m => Value(m, searchRadius)), maxIter, minDistance);

            var d2 = MatchDistance(result.matrix);

            Debug.Print($"Before after: {d1}, {d2}");

            //for (int i = 0; i < 4; i++)
            //{
            //    searchRadius -= 10;

            //    Debug.Print($"--- Index: {i}, search radius: {searchRadius}");

            //    result.SetValueFunc(m => Value(m, searchRadius));

            //    result = ga.Run(result, maxIter, minDistance);

            //    var d2 = MatchDistance(result.matrix);

            //    Debug.Print($"Before after: {d1}, {d2}");
            //}

            ///Validate the results
            
        }

        private double MatchDistance(Matrix4x4 matrix)
        {


            var objectsA = a.Transform(matrix);
            var objectsB = a.SeenObjects;
            ///For each object in A,
            ///ask if there is a corresponding object within our distance tolerance
            ///
            var totalDistance = 0d;
            foreach (var obj in objectsB)
            {
                var match = objectsA.Single(i => i.Id == obj.Id);
                var distance = match.Position.Distance(obj.Position);

                //Debug.Print($"{distance}");

                totalDistance += distance.Sqrd();
            }

            return totalDistance;
        }

        private MatchDistance Value(Matrix4x4 matrix, int searchRadius)
        {
            ///for each agent
            ///for each seen object
            ///check the distance between that seen object and the nearest one seen by agent b
            ///if that object doesn't exist -> don't penalize
            ///if that object does exist, it's not necessarily the same object
            ///
            /// Define a matching criteria (variable like temp), count the number of matches that we can achieve
            ///

            ///try to transform a objects to b
            ///for each object, check for a match. count the number of matches (and square the distances)
            ///

            //Debug.Print($"Matrix: {matrix.ToString()}");

            var objectsA1 = a.SeenObjects;

            var objectsA = a.Transform(matrix);
            var objectsB = b.SeenObjects;

            //Debug.Print($"{string.Join(",", objectsA.Select(i => i.Position.X.ToString()))}");
            //Debug.Print($"{string.Join(",", objectsA.Select(i => i.Position.Y.ToString()))}");
            //Debug.Print($"{string.Join(",", objectsA.Select(i => i.Position.Z.ToString()))}");

            //Debug.Print($"{string.Join(",", objectsB.Select(i => i.Position.X.ToString()))}");

            int matchCount = 0;

            var matchDistance = 0d;
            ///For each object in A,
            ///ask if there is a corresponding object within our distance tolerance
            foreach(var obj in objectsB)
            {
                if(IsMatch(obj, objectsA, searchRadius, out double distance))
                {
                    matchDistance += distance.Sqrd();
                    matchCount++;
                }
            }

            //Debug.Print($"{matchDistance}");

            return new MatchDistance(matchCount, 100000 / matchDistance);
        }

        private bool IsMatch(ObjectPosition search, IList<ObjectPosition> searchSpace, double distanceTolerance, out double matchDistance)
        {
            matchDistance = 0;
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
