using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class Vector4
    {
        private List<double> values;

        public Vector4(double a, double b, double c, double d)
        {
            this.values = new List<double> { a, b, c, d };
        }

        private Vector4(Vector3 val)
        {
            values = new List<double> { val.X, val.Y, val.Z, 1 };
        }

        public double this[int index]
        {
            get
            {
                return values[index];
            }
        }

        public static Vector4 FromVector3(Vector3 v)
        {
            return new Vector4(v);
        }

        public static Vector4 operator *(Vector4 vec, double val)
        {
            throw new NotImplementedException();
        }

        public static double operator *(Vector4 vec1, Vector4 vec2)
        {
            var sum = 0d;
            for (int i = 0; i < 4; i++)
            {
                sum += vec1[i] * vec2[i];
            }

            return sum;
        }
    }
}
