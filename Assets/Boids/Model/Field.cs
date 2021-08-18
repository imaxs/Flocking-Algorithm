using System;
using System.Threading;

namespace Boids.Model
{
    public class Field
    {
        public const double WeightFlock = 2.0D;
        public const double WeightAvoid = 1.0D;
        public const double WeightAlign = 1.0D;
        public const double VisionSize = 2.0D;

        private readonly double Width;
        private readonly double Height;
        private readonly int Interval;

        public readonly Boid[] Boids;
        public readonly int[,] MapData;

        public Field(int[,] mapData, double width, double height, int boidCount, int interval = 10, bool random = true)
        {
            MapData = RecalculateMapData(mapData, width, height);
            Width = width;
            Height = height;
            Boids = new Boid[boidCount];
            Interval = interval;
            BoidsInstantiation(random);
        }

        private int[,] RecalculateMapData(int[,] originalMapData, double width, double height)
        {
            float orgWidth = originalMapData.GetLength(0);
            float orgHeight = originalMapData.GetLength(1);

            int[,] result = new int[(int)width, (int)height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int rx = (int)Math.Floor(x * (orgWidth / width));
                    int ry = (int)Math.Floor(y * (orgHeight / height));
                    result[x, y] = originalMapData[rx, ry];
                }
            }

            return result;
        }

        private void BoidsInstantiation(bool random)
        {
            Random rand = random ? new Random() : new Random(0);
            for (int i = 0; i < Boids.Length; i++)
            {
                int rndX, rndY;
                do {
                    rndX = rand.Next((int)VisionSize, MapData.GetLength(0) - (int)VisionSize);
                    rndY = rand.Next((int)VisionSize, MapData.GetLength(1) - (int)VisionSize);
                } while (MapData[rndX, rndY] == 0);

                Boids[i] = new Boid(
                        xPos: rndX,
                        yPos: rndY,
                        xVel: (rand.NextDouble() - .25),
                        yVel: (rand.NextDouble() - .25),
                        targetSpeed: .5f);
            }
        }

        public void Advance(double stepSize = 1.0)
        {
            while (true)
            {
                Thread.Sleep(Interval);
                for (int i = 0; i < Boids.Length; i++)
                {
                    Boids[i].FlockWithNeighbors(Boids, VisionSize, .02 * WeightFlock);
                    Boids[i].AvoidCloseBoids(Boids, VisionSize, .025 * WeightAvoid);
                    Boids[i].AlignWithNeighbors(Boids, VisionSize, .03 * WeightAlign);
                    Boids[i].AvoidsObstacles(MapData, stepSize);
                    Boids[i].AvoidBoundaries(Width, Height, VisionSize, stepSize);
                    Boids[i].Advance(stepSize);
                }
            }
        }
    }
}