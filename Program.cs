using System;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialization and hints
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Doublebuffer, false);
            
            //Create the window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            float[] vertices = new float[]
            {
                -.5f, -.5f, 0f,
                .5f, -.5f, 0f,
                0f, .5f, 0f
            };

            //Load the vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER,vertexBuffer);
            unsafe
            {
                fixed (float* vertex = &vertices[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
                }
                glVertexAttribPointer(0,3,GL_FLOAT,false,3 * sizeof(float), NULL);
            }
            glEnableVertexAttribArray(0);

            //Engine rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // react to widow changes.
                glDrawArrays(GL_TRIANGLES, 0, 3);
                glFlush();
            }
        }
    }
}

