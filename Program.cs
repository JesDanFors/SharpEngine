using System;
using System.Collections.Generic;
using GLFW;

namespace SharpEngine
{
    class Program {
        static float Lerp(float from, float to, float t) {
            return from + (to - from) * t;
        }

        static float GetRandomFloat(Random random, float min = 0, float max = 1) {
            return Lerp(min, max, (float)random.Next() / int.MaxValue);
        }

        static void Main(string[] args) {
            
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);

            var shape = new Triangle(material);
            shape.Transform.CurrentScale = new Vector(0.5f, 1f, 1f);
            scene.Add(shape);
            
            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            scene.Add(ground);
            
            // engine rendering loop
            const int fixedStepNumber = 30;
            const float fixedStepDuration = 1.0f / fixedStepNumber;
            const float movementSpeed = 0.5f;
            double previousFixedStep = 0.0;
            while (window.IsOpen()) {
                if (Glfw.Time > previousFixedStep + fixedStepDuration)
                {
                    previousFixedStep = Glfw.Time;

                    var walkDirection = new Vector();

                    if (window.GetKey(Keys.W))
                    {
                        walkDirection += shape.Transform.Forward;
                    }
                    if (window.GetKey(Keys.S))
                    {
                        walkDirection += shape.Transform.Backwards;
                    }
                    if (window.GetKey(Keys.A))
                    {
                        walkDirection += shape.Transform.Left;
                    }
                    if (window.GetKey(Keys.D))
                    {
                        walkDirection += shape.Transform.Right;
                    }
                    if (window.GetKey(Keys.Q))
                    {
                        var rotation = shape.Transform.Rotation;
                        rotation.z += MathF.PI*fixedStepDuration;
                        shape.Transform.Rotation = rotation;
                    }
                    if (window.GetKey(Keys.E))
                    {
                        var rotation = shape.Transform.Rotation;
                        rotation.z -= MathF.PI*fixedStepDuration;
                        shape.Transform.Rotation = rotation;
                    }

                    walkDirection = walkDirection.Normalize();
                    shape.Transform.Position += walkDirection * movementSpeed * fixedStepDuration;
                }
                window.Render();
            }
        }
    }
}