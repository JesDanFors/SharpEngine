using System.Numerics;
using System.Runtime.InteropServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine {
    public class Triangle {
            
        Vertex[] vertices;
            
        public float CurrentScale { get; private set; }
            
        public Triangle(Vertex[] vertices) {
            this.vertices = vertices;
            this.CurrentScale = 1f;
            Render(LoadTriangleIntoBuffer());
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
            Move(center*-1);
            for (var i = 0; i < this.vertices.Length; i++) {
                this.vertices[i].position *= multiplier;
            }
            Move(center);

            this.CurrentScale *= multiplier;
        }

        public void Move(Vector direction) {
            for (var i = 0; i < this.vertices.Length; i++) {
                this.vertices[i].position += direction;
            }
        }

        public unsafe void Render(uint vertexArray)
        {
            glBindVertexArray(vertexArray);
            
            fixed (Vertex* vertex = &this.vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * this.vertices.Length, vertex, Gl.GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length);
        }

        public unsafe uint LoadTriangleIntoBuffer()
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            return vertexArray;
        }

        public Vector MoveDirection(Vector direction)
        {
            //Move the Triangle by its Direction
            Move(direction);

            //Check the X-Bounds of the Screen
            if (GetMaxBounds().x >= 1 && direction.x > 0 ||
                GetMinBounds().x <= -1 && direction.x < 0)
            {
                direction.x *= -1;
            }

            //Check the Y-Bounds of the Screen
            if (GetMaxBounds().y >= 1 && direction.y > 0 ||
                GetMinBounds().y <= -1 && direction.y < 0)
            {
                direction.y *= -1;
            }
            return direction;
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
    }
}