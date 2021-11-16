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
        public Material material;
        public Transform Transform { get; }
            
        public Shape(Vertex[] vertices, Material material) {
            this.vertices = vertices;
            this.material = material;
            LoadTriangleIntoBuffer();
            this.Transform = new Transform();
        }
        public Vector GetMinBounds() {
            var min = this.Transform.Matrix * this.vertices[0].position; //check this
            for (var i = 1; i < this.vertices.Length; i++) {
                min = Vector.Min(min, this.vertices[i].position);
            }

            return min;
        }
            
        public Vector GetMaxBounds() {
            var max = this.Transform.Matrix * this.vertices[0].position;
            for (var i = 1; i < this.vertices.Length; i++) {
                max = Vector.Max(max, this.vertices[i].position);
            }

            return max;
        }

        public Vector GetCenter() {
            return (GetMinBounds() + GetMaxBounds()) / 2;
        }

        public unsafe void Render()
        {
            this.material.Use();
            this.material.SetTransform(this.Transform.Matrix);
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
    }
}