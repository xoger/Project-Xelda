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
    public class ClassSelection
    {
        public List<int> choises = new List<int>();
        List<string> titles =new List<string>();
        List<Texture2D> classes = new List<Texture2D>();
        Texture2D fade;
        SpriteFont font;
        List<Button> buttons = new List<Button>();
        Button confirm;
        public string text = "";
        int xpos = 304;
        int xstep = 150;
        int ypos = 150;
        int ystep = 70;
        public bool finished;
        public ClassSelection(ContentManager content)
        {
            for (int i = 0;i<5;i++)
                choises.Add(0);
            titles.Add("Mon");
            titles.Add("Tues");
            titles.Add("Wed");
            titles.Add("Thurs");
            titles.Add("Fri");
            fade = content.Load<Texture2D>("fade");
            font = content.Load<SpriteFont>("bigfont");
            classes.Add (content.Load<Texture2D>("classbuttons/sword"));
            classes.Add(content.Load<Texture2D>("classbuttons/destruction"));
            classes.Add(content.Load<Texture2D>("classbuttons/healing"));
            classes.Add(content.Load<Texture2D>("classbuttons/sword"));
            classes.Add(content.Load<Texture2D>("classbuttons/sword"));
            classes.Add(content.Load<Texture2D>("classbuttons/sword"));
            classes.Add(content.Load<Texture2D>("classbuttons/sword"));
            for (int i = 0; i < 7; i++)
            {
                buttons.Add(new Button(new Vector2(xpos, ypos), classes[i]));
                ypos += ystep;
            }
            confirm = new Button(new Vector2(1000, 650), classes[0]);
        }
        public void Update(Global global, character player)
        {
            text = "";
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i].Clicked(global.mouse, global.old) == true)
                    {
                        choises[j] = i;
                    }
                    if (buttons[i].Hover(global.mouse) == true)
                    {
                        text = player.stats.Stats[i].name;
                    }
                    if (j < 4)
                        buttons[i].posision.X += xstep;
                    else
                        buttons[i].posision.X -= xstep * 4;
                }
            }
            if (confirm.Clicked(global.mouse, global.old) == true)
            {
                finished = true;
            }
            global.old = global.mouse;
        }
        public void Draw(SpriteBatch spritebatch, float scale)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
            spritebatch.Draw(fade, new Rectangle(229, 50, 750, 550), Color.White);
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (choises[j] == i)
                        buttons[i].colour = Color.Gray;
                    if (i == 0)
                    {
                        spritebatch.DrawString(font, titles[j], new Vector2(buttons[i].posision.X, 100), Color.Black, 0, font.MeasureString(titles[j]) / 2, 1, 0, 1);
                    }
                    buttons[i].Draw(spritebatch);
                    buttons[i].colour = Color.White;
                    if (j < 4)
                        buttons[i].posision.X += xstep;
                    else
                        buttons[i].posision.X -= xstep * 4;
                }
            }
            spritebatch.DrawString(font, text, new Vector2(220, 660), Color.Black);
            confirm.Draw(spritebatch);
            spritebatch.End();
            
        }
    }
}
