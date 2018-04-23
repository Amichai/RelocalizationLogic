using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class PerturbableTransformationMatrix : IPerturbable, IValueFunction, ISerializable<PerturbableTransformationMatrix>
    {
        public Matrix4x4 matrix;
        private Func<Matrix4x4, MatchDistance> evalFunc;

        public void SetValueFunc(Func<Matrix4x4, MatchDistance> evalFunc)
        {
            this.evalFunc = evalFunc;
        }

        public PerturbableTransformationMatrix(Matrix4x4 matrix, Func<Matrix4x4, MatchDistance> evalFunc)
        {
            this.matrix = matrix;
            this.evalFunc = evalFunc;
        }

        ///TODO: step size should be dynamic
        public IEnumerable<IValueFunction> Pertubations()
        {
            var toReturn = new List<PerturbableTransformationMatrix>();
            AddPertubations(toReturn, 5);
            AddPertubations(toReturn, 1);
            AddPertubations(toReturn, .5);
            AddPertubations(toReturn, .1);
            AddPertubations(toReturn, .05);
            AddPertubations(toReturn, .01);

            return toReturn;
        }

        private void AddPertubations(List<PerturbableTransformationMatrix> toReturn, double stepSize)
        {
            for (int i = 0; i < 12; i++)
            {
                toReturn.Add(new PerturbableTransformationMatrix(matrix.AdjustWeight(stepSize, i), evalFunc));
            }

            for (int i = 0; i < 12; i++)
            {
                toReturn.Add(new PerturbableTransformationMatrix(matrix.AdjustWeight(-stepSize, i), evalFunc));
            }
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public IComparable Value()
        {
            return evalFunc(matrix);
        }
    }
}
