using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Storage;

namespace Project_Xelda
{
    class progressbar
    {
        Texture2D back;
        Texture2D front;
        string text;
        SpriteFont font;
        public int progress;
        public Vector2 posision;
        public progressbar(Texture2D Back, Texture2D Front, string Text, SpriteFont Font, int Progress, Vector2 Posision)
        {
            back = Back;
            front = Front;
            text = Text;
            font = Font;
            progress = Progress;
            posision = Posision;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(font, text + " " + progress, new Vector2(posision.X, posision.Y - font.MeasureString(text).Y), Color.Black);
            spritebatch.Draw(back,posision, null, Color.White);
            spritebatch.Draw(front, new Vector2 (posision.X+4,posision.Y), new Rectangle(0, 0, (int)(front.Width * ((float)progress/ (float)100)), front.Height), Color.White);
        }
    }
}
