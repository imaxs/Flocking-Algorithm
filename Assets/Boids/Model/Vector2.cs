using System;
using System.Collections.Generic;
using System.Text;

namespace Boids.Model
{
    public struct Vector2
    {
        public double X;
        public double Y;

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Shift(Vector2 position)
        {
            this.X += position.X;
            this.Y += position.Y;
        }

        public void Move(Velocity vel, double stepSize)
        {
            this.X += vel.X * stepSize;
            this.Y += vel.Y * stepSize;
        }

        public static Vector2 Delta(Vector2 originalPosition, Vector2 otherPosition)
        {
            return new Vector2(otherPosition.X - originalPosition.X, otherPosition.Y - originalPosition.Y);
        }

        public static double Distance(Vector2 originalPosition, Vector2 otherPosition)
        {
            Vector2 delta = Delta(originalPosition, otherPosition);
            return Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
        }
    }
}