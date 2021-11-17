namespace SharpEngine
{
    public class Physics
    {
        readonly Scene scene;

        public Physics(Scene scene)
        {
            this.scene = scene;
        }

        public void Update(float deltaTime)
        {
            var gravityAcceleration = Vector.Backwards * 9.819649f/100;
            for (int i = 0; i < this.scene.shapes.Count; i++)
            {
                Shape shape = this.scene.shapes[i];
                //linear velocity
                shape.Transform.Position += shape.velocity * deltaTime;
                //F = m*a
                var acceleration = shape.LinearForce * shape.Mass;
                acceleration += gravityAcceleration * shape.gravityScale;
                //linear acceleration
                shape.Transform.Position += acceleration * deltaTime * deltaTime / 2;
                shape.velocity += acceleration * deltaTime;
            }
            
        }
    }
}