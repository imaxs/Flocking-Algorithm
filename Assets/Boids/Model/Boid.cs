using System;
using System.Collections.Generic;

namespace Boids.Model
{
    public struct Boid
    {
        public Vector2 Position;
        public Velocity Velocity;
        public double TargetSpeed;

        public Boid(double xPos, double yPos, double xVel, double yVel, double targetSpeed = 1.0D) :
            this(new Vector2(xPos, yPos), new Velocity(xVel, yVel), targetSpeed)
        {}

        public Boid(Vector2 original, Velocity velocity, double targetSpeed = 1.0D)
        {
            Position = original;
            Velocity = velocity;
            TargetSpeed = targetSpeed;
        }

        public void FlockWithNeighbors(Boid[] boids, double vision, double weight)
        {
            int neighborCount = 0;

            double centerX = 0;
            double centerY = 0;

            for (int i = 0; i < boids.Length; i++)
            {
                Vector2 boidPos = boids[i].Position;
                if (Vector2.Distance(boidPos, this.Position) < vision)
                {
                    centerX += boidPos.X;
                    centerY += boidPos.Y;
                    neighborCount++;
                }
            }

            centerX /= neighborCount;
            centerY /= neighborCount;

            Velocity.X += (centerX - this.Position.X) * weight;
            Velocity.Y += (centerY - this.Position.Y) * weight;
        }

        public void AlignWithNeighbors(Boid[] boids, double vision, double weight)
        {
            int neighborCount = 0;

            double meanVelX = 0;
            double meanVelY = 0;

            for (int i = 0; i < boids.Length; i++)
            {
                Vector2 boidPos = boids[i].Position;
                if (Vector2.Distance(boidPos, this.Position) < vision)
                {
                    Velocity vel = boids[i].Velocity;
                    meanVelX += vel.X;
                    meanVelY += vel.Y;
                    neighborCount++;
                }
            }

            meanVelX /= neighborCount;
            meanVelY /= neighborCount;

            this.Velocity.X -= (this.Velocity.X - meanVelX) * weight;
            this.Velocity.Y -= (this.Velocity.Y - meanVelY) * weight;
        }

        // Method checks for nearby boids and steers away
        public void AvoidCloseBoids(Boid[] boids, double vision, double weight)
        {
            for (int i = 0; i < boids.Length; i++)
            {
                Vector2 boidPos = boids[i].Position;
                double closeness = vision - Vector2.Distance(boidPos, this.Position);
                if (closeness > 0)
                {
                    // Avoid with a magnitude correlated to closeness
                    Velocity.X -= (boidPos.X - this.Position.X) * weight * closeness;
                    Velocity.Y -= (boidPos.Y - this.Position.Y) * weight * closeness;
                }
            }
        }

        public void AvoidsObstacles(int[,] mapData, double turn)
        {
            int h = (int)this.Position.X + (int)Math.Ceiling(this.Velocity.X);
            int v = (int)this.Position.Y + (int)Math.Ceiling(this.Velocity.Y);

            if (mapData[h, v] == 0)
            {
                if (mapData[h - 1, v] == 1)
                    Velocity.X -= turn;
                else
                    Velocity.X += turn;

                if (mapData[h, v - 1] == 1)
                    Velocity.Y -= turn;
                else
                    Velocity.Y += turn;
            }
        }

        public void AvoidBoundaries(double width, double height, double padding, double turn)
        {
            if (this.Position.X < padding) this.Velocity.X += turn;
            if (this.Position.Y < padding) this.Velocity.Y += turn;
            if (this.Position.X > width - padding) this.Velocity.X -= turn;
            if (this.Position.Y > height - padding) this.Velocity.Y -= turn;
        }

        public void Advance(double stepSize)
        {
            this.Velocity.SetSpeed(TargetSpeed);
            this.Position.Move(this.Velocity, stepSize);
        }
    }
}