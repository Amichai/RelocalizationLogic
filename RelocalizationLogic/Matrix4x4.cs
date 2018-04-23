using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    class Matrix4x4
    {
        private List<double> values;

        private static Random random = new Random();

        private Vector4 column1;
        private Vector4 column2;
        private Vector4 column3;
        private Vector4 column4;

        private Vector4 row1;
        private Vector4 row2;
        private Vector4 row3;
        private Vector4 row4;

        public Matrix4x4 AdjustWeight(double diff, int index)
        {
            var clone = new Matrix4x4(this.values.ToList());

            clone.values[index] += diff;

            clone.InitializeRowsAndColumns();

            return clone;
        }

        public Matrix4x4(List<double> values)
        {
            Debug.Assert(values.Count == 16);

            this.values = values;
            InitializeRowsAndColumns();
        }

        private void InitializeRowsAndColumns()
        {
            row1 = new Vector4(values[0], values[1], values[2], values[3]);
            row2 = new Vector4(values[4], values[5], values[6], values[7]);
            row3 = new Vector4(values[8], values[9], values[10], values[11]);
            row4 = new Vector4(values[12], values[13], values[14], values[15]);

            column1 = new Vector4(row1[0], row2[0], row3[0], row4[0]);
            column2 = new Vector4(row1[1], row2[1], row3[1], row4[1]);
            column3 = new Vector4(row1[2], row2[2], row3[2], row4[2]);
            column4 = new Vector4(row1[3], row2[3], row3[3], row4[3]);
        }

        private static Dictionary<string, string> results = new Dictionary<string, string>();

        public static Vector3 operator *(Matrix4x4 m1, Vector3 v1)
        {
            var v4 = Vector4.FromVector3(v1);
            var a1 = m1.row1 * v4;
            var a2 = m1.row2 * v4;
            var a3 = m1.row3 * v4;
            var a4 = m1.row4 * v4;

            Debug.Assert(m1.values[0] == m1.row1[0]);

            Debug.Assert(a4 == 1);

            //var key = $"{a1},{a2},{a3},{v1.X},{v1.Y},{v1.Z}";

            //if (!results.ContainsKey(key))
            //{
            //    results[key] = m1.ToString();
            //} else
            //{
            //    Debug.Assert(results[key] == m1.ToString());
            //}

            ///Assert a4 == 1
            return new Vector3(a1, a2, a3);
        }

        private static double RandomComponent()
        {
            return random.NextDouble() * 10 - 5;
        }

        public static Matrix4x4 RandomProjection()
        {
            return new Matrix4x4(Enumerable.Concat(Enumerable.Range(0, 12).Select(i => RandomComponent()), new List<double> {0, 0, 0, 1}).ToList());
        }

        public override string ToString()
        {
            return string.Join(",", values);
        }
    }
}
