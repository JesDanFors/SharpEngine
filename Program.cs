using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static Triangle triangle = new Triangle (
            new Vertex[] {
                //triangle 1
                new Vertex(new Vector(-.4f, -.4f), Color.Red),
                new Vertex(new Vector(.4f, -.4f), Color.Green),
                new Vertex(new Vector(0f, .6f), Color.Blue),
            }
        );

        private static Triangle triangle2 = new Triangle(
            new Vertex[]
            {
                new Vertex(new Vector(.4f, .4f), Color.Blue),
                new Vertex(new Vector(.8f, .4f), Color.Red),
                new Vertex(new Vector(.6f, .8f), Color.Green)
            });

        private static Triangle[] triangles = new Triangle[] {triangle, triangle2};
        
        static void Main(string[] args) {
            //screen rendering
            var window = CreateWindow();
            CreateShaderProgram();

            // engine rendering loop
            var direction = new Vector(0.003f, 0.003f);
            var multiplier = 0.99f;
            var rotate = .5f;
            while (!Glfw.WindowShouldClose(window)) {
                foreach (var triangle in triangles)
                {
                    Glfw.PollEvents(); // react to window changes (position etc.)
                    ClearScreen();
                    Render(window);

                    triangle.Scale(multiplier);
                    triangle.Rotate(rotate);
                    multiplier = triangle.CurrenScalar(multiplier);
                    direction = triangle.MoveDirect(direction);
                    
                }
            }
        }

        static void Render(Window window) {
            triangle.Render();
            triangle2.Render();
            Glfw.SwapBuffers(window);
        }

        static void ClearScreen() {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/position-color.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }
        static Window CreateWindow() {
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
            var window = Glfw.CreateWindow(1360, 740, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}