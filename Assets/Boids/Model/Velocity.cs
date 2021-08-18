using System;
using System.Collections.Generic;
using System.Text;

namespace Boids.Model
{
    public struct Velocity
    {
        public double X;
        public double Y;

        public Velocity(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double GetAngle()
        {
            if (double.IsNaN(this.X) && double.IsNaN(this.Y)) return double.NaN;

            double angle = Math.Atan(Y / X) * 180 / Math.PI - 90;

            return X < 0 ? angle += 180 : angle;
        }

        public void SetSpeed(double speed, bool absolute = false)
        {
            if (double.IsNaN(this.X) && double.IsNaN(this.Y)) return;

            var current = Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));

            double targetX = (X / current) * speed;
            double targetY = (Y / current) * speed;

            if (absolute)
            {
                X = targetX;
                Y = targetY;
            }
            else
            {
                X += (targetX - X) * .1;
                Y += (targetY - Y) * .1;
            }
        }
    }
}