using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;

namespace SharpEngine
{
    public struct Matrix
    {
        public float m11; public float m12; public float m13; public float m14;
        public float m21; public float m22; public float m23; public float m24;
        public float m31; public float m32; public float m33; public float m34; 
        public float m41; public float m42; public float m43; public float m44;

        public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23,
            float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13; this.m14 = m14;
            this.m21 = m21; this.m22 = m22; this.m23 = m23; this.m24 = m24;
            this.m31 = m31; this.m32 = m32; this.m33 = m33; this.m34 = m34;
            this.m41 = m41; this.m42 = m42; this.m43 = m43; this.m44 = m44;
        }

        public static Matrix Identity => new Matrix(1, 0, 0, 0,
                                             0, 1, 0, 0,
                                             0, 0, 1, 0,
                                             0, 0, 0, 1);

        public static Vector operator *(Matrix m, Vector v)
        {
            return new Vector(m.m11 * v.x + m.m12 * v.y + m.m13 * v.z + m.m14 * 1,
                              m.m21 * v.x + m.m22 * v.y + m.m23 * v.z + m.m24 * 1,
                              m.m31 * v.x + m.m32 * v.y + m.m33 * v.z + m.m34 * 1);
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            return new Matrix(b.m11*a.m11+b.m21*a.m12+b.m31*a.m13+b.m41*a.m14,
                b.m12*a.m11+b.m22*a.m12+b.m32*a.m13+b.m42*a.m14,
                b.m13*a.m11+b.m23*a.m12+b.m33*a.m13+b.m43*a.m14,
                b.m14*a.m11+b.m24*a.m12+b.m34*a.m13+b.m44*a.m14,

                b.m11*a.m21+b.m21*a.m22+b.m31*a.m23+b.m41*a.m24,
                b.m12*a.m21+b.m22*a.m22+b.m32*a.m23+b.m42*a.m24,
                b.m13*a.m21+b.m23*a.m22+b.m33*a.m23+b.m43*a.m24,
                b.m14*a.m21+b.m24*a.m22+b.m34*a.m23+b.m44*a.m24,

                b.m11*a.m31+b.m21*a.m32+b.m31*a.m33+b.m41*a.m34,
                b.m12*a.m31+b.m22*a.m32+b.m32*a.m33+b.m42*a.m34,
                b.m13*a.m31+b.m23*a.m32+b.m33*a.m33+b.m43*a.m34,
                b.m14*a.m31+b.m24*a.m32+b.m34*a.m33+b.m44*a.m34,

                b.m11*a.m41+b.m21*a.m42+b.m31*a.m43+b.m41*a.m44,
                b.m12*a.m41+b.m22*a.m42+b.m32*a.m43+b.m42*a.m44,
                b.m13*a.m41+b.m23*a.m42+b.m33*a.m43+b.m43*a.m44,
                b.m14*a.m41+b.m24*a.m42+b.m34*a.m43+b.m44*a.m44
                );
        }

        public static Matrix Translation(Vector v)
        {
            var newMatrix = Identity;
            newMatrix.m14 = v.x;
            newMatrix.m24 = v.y;
            newMatrix.m34 = v.z;
            return newMatrix;
        }
        public static Matrix Scale(Vector v)
        {
            var newMatrix = Identity;
            newMatrix.m11 = v.x;
            newMatrix.m22 = v.y;
            newMatrix.m33 = v.z;
            return newMatrix;
        }
    }
}