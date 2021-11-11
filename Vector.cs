using System;

namespace SharpEngine
{
    public struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }
        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }

        public static Vector operator +(Vector v, Vector u)
        {
            return new Vector(v.x + u.x, v.y + u.y, v.z + u.z);
        }public static Vector operator -(Vector v, Vector u)
        {
            return new Vector(v.x - u.x, v.y - u.y, v.z - u.z);
        }
        public static Vector Max(Vector a, Vector b)
        {
            Vector c = new Vector();
            c.x = MathF.Max(a.x, b.x);
            c.y = MathF.Max(a.y, b.y);
            return c;
        }
        public static Vector Min(Vector a, Vector b)
        {
            Vector c = new Vector(0,0)
            {
                x = MathF.Min(a.x, b.x),
                y = MathF.Min(a.y, b.y)
            };
            return c;
        }
    }
}