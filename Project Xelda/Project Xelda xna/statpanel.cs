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
    class statpanel
    {
        Texture2D progress;
        Texture2D greenbar;
        Texture2D redbar;
        SpriteFont font;
        SpriteFont reward;
        List<int>statchange = new List<int>();
        List<int>statcounter = new List<int>();
        CharacterStruct oldstats = new CharacterStruct();
        CharacterStruct stats = new CharacterStruct();
        List<progressbar> bars = new List<progressbar>();
        public statpanel(ContentManager content)
        {
            progress = content.Load<Texture2D>("progress");
            greenbar = content.Load<Texture2D>("greenbar");
            redbar = content.Load<Texture2D>("redbar");
            font = content.Load<SpriteFont>("mainfont");
            reward = content.Load<SpriteFont>("bigfont");
            for (int i = 0; i < 8; i++)
            {
                statchange.Add(0);
                statcounter.Add(0);
            }
            bars.Add(new progressbar(progress, greenbar, "Sword Skill", font,0, new Vector2(16, 100)));
            bars.Add(new progressbar(progress, greenbar, "Destruction", font, 0, new Vector2(16, 150)));
            bars.Add(new progressbar(progress, greenbar, "Healing", font, 0, new Vector2(16, 200)));
            bars.Add(new progressbar(progress, greenbar, "Tactics", font, 0, new Vector2(16, 250)));
            bars.Add(new progressbar(progress, greenbar, "Fitness", font, 0, new Vector2(16, 300)));
            bars.Add(new progressbar(progress, greenbar, "Hand to hand", font, 0, new Vector2(16, 350)));
            bars.Add(new progressbar(progress, redbar, "Fatigue", font, 0, new Vector2(16, 450)));
            bars.Add(new progressbar(progress, greenbar, "Likability", font, 0, new Vector2(16, 500)));
        }
        public void Update(CharacterStruct Stats)
        {
            stats = Stats;
            for (int i = 0; i < bars.Count; i++)
            {
                if (stats.Stats[i].value != oldstats.Stats[i].value)
                {
                    statchange[i] = -oldstats.Stats[i].value + stats.Stats[i].value;
                    statcounter[i] = 100;
                }

                bars[i].progress = stats.Stats[i].value;
                if (statcounter[i] > 0)
                    statcounter[i]--;
            }
            oldstats = stats.clone();
        }
        public void Draw(SpriteBatch spritebatch)
        {
            foreach (progressbar go in bars)
                go.Draw(spritebatch);
            for (int i = 0; i < 8; i++)
            {
                if (statcounter[i] > 0)
                {
                    
                    if (statcounter[i] > 50)
                        spritebatch.DrawString(reward, "+" + (statchange[i]), new Vector2(200, bars[i].posision.Y), new Color(0, 255, 0, (1 - (float)statcounter[i] / 100) * 2));
                    else
                        spritebatch.DrawString(reward, "+" + (statchange[i]), new Vector2(200, bars[i].posision.Y), new Color(0, 255, 0, (float)statcounter[i] / 50));
                }
            }
            
        }
    }
}
