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
    public class Button
    {
        public Vector2 posision;
        public Texture2D texture;
        string Label;
        public SpriteFont font;
        public Color colour = Color.White;
        public Button(Vector2 Posision, Texture2D Texture)
        {
            texture = Texture;
            posision = Posision;
            Label = "";
        }
        public Button(Vector2 Posision, Texture2D Texture, string label, SpriteFont Font)
        {
            texture = Texture;
            posision = Posision;
            Label = label;
            font = Font;
        }
        public bool Clicked(MouseState mouse, MouseState old)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouse.Y + texture.Height / 2 > posision.Y &&
                    mouse.Y + texture.Height / 2 < posision.Y + texture.Height &&
                    mouse.X - texture.Width / 2 > posision.X - texture.Width &&
                    mouse.X + texture.Width / 2 < posision.X + texture.Width &&
                    old.LeftButton == ButtonState.Released)
                {
                    old = mouse;
                    return true;
                }
            }
            old = mouse;
            return false;
        }
        public bool Hover(MouseState mouse)
        {
            if (mouse.Y + texture.Height / 2 > posision.Y &&
                mouse.Y + texture.Height / 2 < posision.Y + texture.Height &&
                mouse.X - texture.Width / 2 > posision.X - texture.Width &&
                mouse.X + texture.Width / 2 < posision.X + texture.Width)
            {
                return true;
            }
            return false;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, posision, null, colour, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 1);
            if (Label != "")
                spritebatch.DrawString(font, Label, posision, Color.White, 0, new Vector2(font.MeasureString(Label).X / 2,  font.MeasureString(Label).Y / 2.5f), 1, 0, 1);
        }
    }
}