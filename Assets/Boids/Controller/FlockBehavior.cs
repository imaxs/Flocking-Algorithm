using System.Collections;
using System.Collections.Generic;
using Boids.Model;

namespace Boids.Controller
{
    public class FlockBehavior : BaseThread
    {
        private Field m_Field;
        public ref readonly Boid[] Boids => ref m_Field.Boids;
        public ref readonly int[,] MapData => ref m_Field.MapData;

        public FlockBehavior(int[,] mapData)
        {
            m_Field = new Field(
                        mapData: mapData,
                        width: 100,
                        height: 100,
                        boidCount: 200,
                        interval: 5,
                        random: true);
        }

        protected override void ThreadWork()
        {
            m_Field.Advance(0.2f);
        }
    }
}