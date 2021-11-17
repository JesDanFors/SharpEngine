using System;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine {
    public class Transform {
        
        Matrix transform = Matrix.Identity;
        public Vector CurrentScale { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }

        public Matrix Matrix => Matrix.Translation(Position) * Matrix.Rotation(Rotation) * Matrix.Scale(CurrentScale);

        public Vector Forward => Matrix.Transform(Matrix, Vector.Forward, 0);
        public Vector Backwards => Matrix.Transform(Matrix, Vector.Backwards, 0);
        public Vector Left => Matrix.Transform(Matrix, Vector.Left, 0);
        public Vector Right => Matrix.Transform(Matrix, Vector.Right, 0);

        public Transform()
        {
            this.CurrentScale = new Vector(1, 1, 1);
        }
        public void Move(Vector direction)
        {
            this.Position += direction;
        }

        public void Scale(float magnitude)
        {
            CurrentScale *= magnitude;
        }
        public void Rotate(float degree)
        {
            var rotation = this.Rotation;
            rotation.z += degree;
            this.Rotation = rotation;
        }
    }
}