using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Boids.Model;
using Boids.Controller;

namespace Boids
{
    public class Mono : MonoBehaviour
    {
        [SerializeField]
        private Texture2D m_GenMapTexture;

        private FlockBehavior m_FlockBehavior;
        private Boid[] m_Boids;
        private int[,] m_MapData;
        private GameObject[] m_Rats;

        private int[,] ReadMap()
        {
            int width = m_GenMapTexture.width;
            int height = m_GenMapTexture.height;

            int[,] result = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = m_GenMapTexture.GetPixel(x, y);
                    if (pixelColor.grayscale < 0.01f)
                        result[x, y] = 0;
                    else
                        result[x, y] = 1;
                }
            }

            return result;
        }

        void Start()
        {
            m_FlockBehavior = new FlockBehavior(ReadMap());
            m_Boids = m_FlockBehavior.Boids;
            m_MapData = m_FlockBehavior.MapData;
            m_Rats = new GameObject[m_Boids.Length];

            for (int i = 0; i < m_Boids.Length; i++)
            {
                m_Rats[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                m_Rats[i].gameObject.transform.name = "Rat [" + i + "]";
                m_Rats[i].transform.localScale = new Vector3(1f, 1f, 1f);
                m_Rats[i].transform.position = new Vector3((float)m_Boids[i].Position.X, 0, (float)m_Boids[i].Position.Y);
            }

            m_FlockBehavior.Start();
        }

        void Update()
        {
            for (int i = 0; i < m_Boids.Length; i++)
                m_Rats[i].transform.position = new Vector3((float)m_Boids[i].Position.X, 0, (float)m_Boids[i].Position.Y);
        }

        void OnDisable()
        {
            m_FlockBehavior.Abort();
        }

        void OnDrawGizmos()
        {
            if (m_MapData == null) return;

            Gizmos.color = Color.red;

            int w = m_MapData.GetLength(0);
            int h = m_MapData.GetLength(1);

            for (int x = 0; x < w; x++)
            {
                for (int z = 0; z < h; z++)
                {
                    if (m_MapData[x, z] == 0)
                        Gizmos.DrawWireCube(new Vector3(x, 0, z), Vector3.one);
                }
            }
        }
    }
}
