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
            var physics = new Physics(scene);
            window.Load(scene);

            var circle = new Circle(material);
            circle.Transform.Position = Vector.Left;
            circle.velocity = Vector.Right * 0.3f;
            scene.Add(circle);
            
            var circle2 = new Circle(material);
            circle2.Transform.Position = Vector.Right * 0.5f;
            scene.Add(circle2);
            
            //ground
            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            ground.gravityScale = 0f;
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumber = 30;
            const float fixedStepDuration = 1.0f / fixedStepNumber;
            const float movementSpeed = 0.5f;
            double previousFixedStep = 0.0;
            while (window.IsOpen())
            {
                if (Glfw.Time > previousFixedStep + fixedStepDuration)
                {
                    previousFixedStep = Glfw.Time;
                    physics.Update(fixedStepDuration);
                    
                }
                window.Render();
            }
        }
    }
}