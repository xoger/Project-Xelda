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
    class CharactorCreator
    {
        Texture2D left;
        Texture2D right;
        Texture2D bigoval;
        public character player;
        SpriteFont font;
        List<Button> buttons = new List<Button>();
        characterContent charcont;
        Random random = new Random();
        public CharactorCreator(characterContent Charcont, chardatabace chardb, ContentManager content)
        {
            charcont = Charcont;
            left = content.Load<Texture2D>("Left");
            right = content.Load<Texture2D>("Right");
            bigoval = content.Load<Texture2D>("bigoval");
            font = content.Load<SpriteFont>("bigfont");
            player = new character(charcont, chardb);
            Random();
            player.visible = true;
            player.ChangePosition(0, 0);
            player.Update();
            buttons.Add(new Button(new Vector2(750, 200), right));
            buttons.Add(new Button(new Vector2(500, 200), left)); 
            buttons.Add(new Button(new Vector2(750, 300), right)); 
            buttons.Add(new Button(new Vector2(500, 300), left)); 
            buttons.Add(new Button(new Vector2(750, 400), right)); 
            buttons.Add(new Button(new Vector2(500, 400), left)); 
            buttons.Add(new Button(new Vector2(750, 500), right)); 
            buttons.Add(new Button(new Vector2(500, 500), left)); 
            buttons.Add(new Button(new Vector2(750, 600), right)); 
            buttons.Add(new Button(new Vector2(500, 600), left));
            buttons.Add(new Button(new Vector2(1000, 500), bigoval,"Random",font));
            buttons.Add(new Button(new Vector2(1000, 600), bigoval,"Finish",font));
        }
        public character Update(Global global)
        {
            //player.stats.name = "palyer";
            player.stats.gender = Preference(buttons[0], buttons[1], player.stats.gender, charcont.bodys.Count, global);
            player.stats.hairstyle = Preference(buttons[2], buttons[3], player.stats.hairstyle, charcont.hairstyles.Count, global);
            player.stats.haircolour = Preference(buttons[4], buttons[5], player.stats.haircolour, charcont.haircolours.Count, global);
            player.stats.eyecolour = Preference(buttons[6], buttons[7], player.stats.eyecolour, charcont.eyecolors.Count, global);
            player.stats.skincolour = Preference(buttons[8], buttons[9], player.stats.skincolour, charcont.complexions.Count, global);
            player.Update();
            if (buttons[10].Clicked(global.mouse,global.old) == true)
            {
                Random();
            }
            if (buttons[11].Clicked(global.mouse,global.old) == true)
            {
                global.stage = 1;
            }
            global.old = global.mouse;
            return player;
        }
        void Random()
        {
            player.stats.gender = random.Next(0, charcont.bodys.Count);
            player.stats.hairstyle = random.Next(0, charcont.hairstyles.Count);
            player.stats.haircolour = random.Next(0, charcont.haircolours.Count);
            player.stats.eyecolour = random.Next(0, charcont.eyecolors.Count);
            player.stats.skincolour = random.Next(0, charcont.complexions.Count);
        }
        int Preference(Button Rbutton, Button Lbutton, int stat, int count, Global global)
        {
            if (Rbutton.Clicked(global.mouse, global.old) == true)
            {
                if (stat < count - 1)
                {
                    stat += 1;
                }
                else
                {
                    stat = 0;
                }
            }
            if (Lbutton.Clicked(global.mouse, global.old) == true)
            {
                if (stat > 0)
                {
                    stat -= 1;
                }
                else
                {
                    stat = count - 1;
                }
            }
            return stat;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Button go in buttons)
                go.Draw(spritebatch);
            if (player.stats.gender == 0)
                spritebatch.DrawString(font, "Male", new Vector2(625, 200), Color.Black, 0, font.MeasureString("Male") / 2, 1, 0, 1);
            else
                spritebatch.DrawString(font, "Female", new Vector2(625, 200), Color.Black, 0, font.MeasureString("Female") / 2, 1, 0, 1);
            spritebatch.DrawString(font, "Hair Style " + (player.stats.hairstyle + 1)
                , new Vector2(625, 300), Color.Black, 0, font.MeasureString("Hair Style " + (player.stats.hairstyle + 1)) / 2, 1, 0, 1);
            spritebatch.DrawString(font, "Hair Colour " + (player.stats.haircolour + 1)
                , new Vector2(625, 400), Color.Black, 0, font.MeasureString("Hair Colour " + (player.stats.haircolour + 1)) / 2, 1, 0, 1);
            spritebatch.DrawString(font, "Eye Colour " + (player.stats.eyecolour + 1)
                , new Vector2(625, 500), Color.Black, 0, font.MeasureString("Eye Colour " + (player.stats.eyecolour + 1)) / 2, 1, 0, 1);
            spritebatch.DrawString(font, "Skin Colour " + (player.stats.skincolour + 1)
                , new Vector2(625, 600), Color.Black, 0, font.MeasureString("Skin Colour " + (player.stats.skincolour + 1)) / 2, 1, 0, 1);
        }
        public void DrawPlayer(SpriteBatch spritebatch, float scale)
        {
            player.Draw(spritebatch, scale);
        }
    }
}
