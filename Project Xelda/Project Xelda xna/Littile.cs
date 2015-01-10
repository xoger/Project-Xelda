using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Storage;

namespace Project_Xelda
{
    public class rangetile
    {
        public Vector2 position;
        public Vector2? prepotision;
        public float steps;
        public bool New = true;
    }
    public class speedtile 
    {
        public Vector2 position;
        public List<float> directions = new List<float>();
        public speedtile (float north, float east, float south, float west, Vector2 Position)
        {
            this.position = Position;
            directions.Add(north);
            directions.Add(pythag(north,east));
            directions.Add(east);
            directions.Add(pythag(south, east));
            directions.Add(south);
            directions.Add(pythag(south, west));
            directions.Add(west);
            directions.Add(pythag(north, west));
        }
        public float pythag(float first, float second)
        {
            return (float)Math.Sqrt(Math.Pow(first, 2) + Math.Pow(second, 2));
        }
    }
}
