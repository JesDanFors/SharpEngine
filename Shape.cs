using System;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine {
    public class Shape {
        protected Vertex[] vertices;
        private uint vertexArray;
        private uint vertexBuffer;
            
        public float CurrentScale { get; private set; }

        public Material material;
            
        public Shape(Vertex[] vertices, Material material) {
            this.vertices = vertices;
            this.material = material;
            this.CurrentScale = 1f;
            LoadTriangleIntoBuffer();
        }
        public Vector GetMinBounds() {
            var min = this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                min = Vector.Min(min, this.vertices[i].position);
            }

            return min;
        }
            
        public Vector GetMaxBounds() {
            var max = this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                max = Vector.Max(max, this.vertices[i].position);
            }

            return max;
        }

        public Vector GetCenter() {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }

        public void Scale(float multiplier) {
            // We first move the triangle to the center, to avoid
            // the triangle moving around while scaling.
            // Then, we move it back again.
            var center = GetCenter();
            Move(-center);
            for (var i = 0; i < this.vertices.Length; i++) {
                this.vertices[i].position *= multiplier;
            }
            Move(center);

            this.CurrentScale *= multiplier;
        }

        public void Move(Vector direction)
        {
            Matrix matrix = Matrix.Identity;
            for (var i = 0; i < this.vertices.Length; i++)
            {
                this.vertices[i].position = matrix * this.vertices[i].position;
            }
        }

        public unsafe void Render()
        {
            this.material.Use();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, this.vertexBuffer);
            fixed (Vertex* vertex = &this.vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length);
            glBindVertexArray(0);
        }

        public unsafe void LoadTriangleIntoBuffer()
        {
            vertexArray = glGenVertexArray();
            vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glBindVertexArray(0);
        }

        public Vector MoveDirect(Vector MoveDirection)
        {
            //Move the Triangle by its Direction
            Move(MoveDirection);

            //Check the X-Bounds of the Screen
            if (GetMaxBounds().x >= 1 && MoveDirection.x > 0 ||
                GetMinBounds().x <= -1 && MoveDirection.x < 0)
            {
                MoveDirection.x *= -1;
            }

            //Check the Y-Bounds of the Screen
            if (GetMaxBounds().y >= 1 && MoveDirection.y > 0 ||
                GetMinBounds().y <= -1 && MoveDirection.y < 0)
            {
                MoveDirection.y *= -1;
            }
            return MoveDirection;
        }

        public float CurrenScalar(float multiplier)
        {
            if (CurrentScale <= 0.5f)
            {
                multiplier = 1.01f;
            }

            if (CurrentScale >= 1f)
            {
                multiplier = 0.99f;
            }

            return multiplier;
        }

        public void Rotate(float degree)
        {
            var center = GetCenter();
            Move(-center);
            for (int i = 0; i < vertices.Length; i++)
            {
                var currentRotation = Vector.Angle(this.vertices[i].position);
                var distance = vertices[i].position.GetMagnitude();
                var newX = MathF.Cos(currentRotation + degree);
                var newY = MathF.Sin(currentRotation + degree);
                vertices[i].position = new Vector(newX, newY) * distance;
            }
            Move(center);
        }
    }
}