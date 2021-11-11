using OpenGL;

namespace SharpEngine
{
    public class Triangle
    {
        private Vertex[] vertices;
        public float currentScale { get; private set;}
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            currentScale = 1;
        }

        public void Scale(float multiplier)
        {
            //find position
            var center = (GetMinBounds() + GetMaxBounds()) / 2; 
            //move to center
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position -= center;
            }
            //scale
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position *= multiplier;
            }
            //Move it back
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += center;
            }

            currentScale *= multiplier;

        }

        public void Move(Vector direction)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += direction;
            }
        }

        public Vector GetMaxBounds()
        {
            var max = vertices[0].position;
            for (int i = 1; i < vertices.Length; i++)
            {
                max = Vector.Max(max, vertices[i].position);
            }
            return max;
        }

        public Vector GetMinBounds()
        {
            var min = vertices[0].position;
            for (int i = 1; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
            }

            return min;
        }

        public unsafe void Render() {
            fixed (Vertex* vertex = &vertices[0]) {
                Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, Gl.GL_DYNAMIC_DRAW);
            }
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, vertices.Length);
        }
    }
}