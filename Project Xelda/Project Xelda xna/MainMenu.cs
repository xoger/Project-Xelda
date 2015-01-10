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
    class MainMenu
    {
        List<Button> buttons = new List<Button>();
        Texture2D bigoval;
        SpriteFont font;
        public MainMenu(ContentManager content)
        {
            font = content.Load <SpriteFont>("bigfont");
            bigoval = content.Load<Texture2D>("bigoval");
            buttons.Add(new Button(new Vector2(1000, 100), bigoval, "New Game", font));
            buttons.Add(new Button(new Vector2(1000, 200), bigoval, "TBS", font));
            buttons.Add(new Button(new Vector2(1000, 300), bigoval, "Credits", font));
        }
        
        public void Update(Global global)
        {
            if (buttons[0].Clicked(global.mouse,global.old) == true)
            {
                global.stage = 2;
            }
            if (buttons[1].Clicked(global.mouse, global.old) == true)
            {
                global.stage = 4;
            }
            if (buttons[2].Clicked(global.mouse, global.old) == true)
            {
                global.stage = 3;
            }

        }
        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Button go in buttons)
                go.Draw(spritebatch);
        }
    }
}
