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
    class Credits
    {
        string strg;
        SpriteFont smallfont;
        SpriteFont bigfont;
        Vector2 posision = new Vector2(683,768);
        int gap = 0;
        List<string> people = new List<string>();
        public int stage = 3;
        public Credits(ContentManager content)
        {
            smallfont = content.Load<SpriteFont>("bigfont");
            bigfont = content.Load<SpriteFont>("hugefont");
            people.Add("!Programmer, Writer and Project Manager");
            people.Add("Mat Parr");

            people.Add("!Character Artist");
            people.Add("Sara Vanderpal");

            people.Add("!Additional Art");
            people.Add("Ahlam Shaikh");

            people.Add("!Music");
            people.Add("Aaron Dunn");

        }
        public void Update(Global global)
        {
            posision.Y -= 0.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
            {
                posision.Y = 820;
                gap = 0;
                global.stage = 0;
            }
        }
        public void Draw(SpriteBatch spritebatch, float scale, GraphicsDeviceManager graphics, Global global)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
            gap = 0;
            for (int i = 0; i < people.Count; i++)
            {
                
                if (people[i].StartsWith("!") == true)
                {
                    strg = people[i].Remove(0, 1);
                    gap += 1;
                    spritebatch.DrawString(bigfont, strg, new Vector2(posision.X, (posision.Y + (i + gap) * 40)), Color.White, 0, bigfont.MeasureString(strg) / 2, 1, 0, 1);
                }
                else
                {
                    strg = people[i];
                    spritebatch.DrawString(smallfont, strg, new Vector2(posision.X, posision.Y + (i + gap) * 40), Color.White, 0, smallfont.MeasureString(strg) / 2, 1, 0, 1);
                    if (posision.Y + (i + gap) * 40 < 0 && i == people.Count - 1)
                    {
                        posision.Y = 820;
                        gap = 0;
                        global.stage = 0;
                    }
                }
            }
            spritebatch.End();
        }
    }
}
