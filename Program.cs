using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    struct Vector
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
    class Program
    {
        static Vector[] vertices = new Vector[]{
            new Vector(-.1f, -.1f),
            new Vector(.1f, -.1f),
            new Vector(0f, .1f),
            //new Vector(.4f, .4f),
            //new Vector(.6f, .4f),
            //new Vector(.5f, .6f)
        };

        private const int vertexX = 0;
        private const int vertexY = 1;
        private const int vertexSize = 3;
        static void Main(string[] args) {
            var window = Window();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();

            // engine rendering loop
            var direction = new Vector(0.003f, 0.003f);
            var multiplier = .999f;
            float scale = 1f;
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                //manipulation goes in here
                for (int i = vertexX; i < vertices.Length; i++)
                {
                    vertices[i] += direction;
                }
                
                //min
                var min = vertices[0];
                for (int i = 1; i < vertices.Length; i++)
                {
                    min = Vector.Min(min, vertices[i]);
                }
                //max
                var max = vertices[0];
                for (int i = 1; i < vertices.Length; i++)
                {
                    max = Vector.Max(max, vertices[i]);
                }
                //find position
                var center = (min + max) / 2; 
                //move to center
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] -= center;
                }
                //scale
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] *= multiplier;
                }
                scale *= multiplier;
                if (scale <= .5f)
                {
                    multiplier = 1.001f;
                }if (scale >= 1f)
                {
                    multiplier = .999f;
                }
                //Move it back
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += center;
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].x >= 1 && direction.x > 0 || vertices[i].x <= -1 && direction.x < 0)
                    {
                        direction.x *= -1;
                        break;
                    }
                }
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].y >= 1 && direction.y > 0 || vertices[i].y <= -1 && direction.y < 0)
                    {
                        direction.y *= -1;
                        break;
                    }
                }
                UpdateTriangleBuffer();

            }
        }

        private static void ClearScreen()
        {
            glClearColor(0.0f, 0.5f, 0.5f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        private static void Render(Window window)
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            Glfw.SwapBuffers(window);
        }

        private static unsafe void LoadTriangleIntoBuffer()
        {
            // load the vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            UpdateTriangleBuffer();
            glVertexAttribPointer(0, vertexSize, GL_FLOAT, false, vertexSize * sizeof(float), NULL);
            glEnableVertexAttribArray(0);
        }

        static unsafe void UpdateTriangleBuffer()
        {
            fixed (Vector* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }

        private static void CreateShaderProgram()
        {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/screen-coordinates.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/red.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        private static Window Window()
        {
            // initialize and configure
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(512, 768/2, "SharpEngine", Monitor.None, GLFW.Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}