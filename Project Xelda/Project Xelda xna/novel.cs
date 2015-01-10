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
    public class action
    {
        public character character;
        public bool changeemotion;
        public int emotion;
        public bool changepos;
        public int posision;
        public string text;
        public string name;
        public bool instant;
        public int speed;
        public bool changeday;
        public bool changescene;
        public int sceneset;
        public int scene;
        public bool changestat;
        public int stat;
        public int change;
        public bool addplayer;
        public bool removeplayer;
        public int effect;
        public bool classselection;
        public bool lessonselect;
    }
	public class novel
	{
		private SpriteFont font;
		private Texture2D background;
		string Block;
        List<string> options = new List<string>();
        bool showbuttons;
        public List<action> lines = new List<action>();
        public int choise;
        public bool chosen = true;
        List<Button> buttons = new List<Button>();
        Texture2D oval;
        public bool active = true;
        float count = 1;
        bool writing = true;
        bool written = false;
        bool wait = false;
        List<character> characters = new List<character>();
        characterContent charcont;
        chardatabace chardb;
		public novel (ContentManager content, characterContent Charcont, chardatabace Chardb)
		{
            charcont = Charcont;
            chardb = Chardb;
			font = content.Load<SpriteFont> ("mainfont");
			background = content.Load<Texture2D> ("textboxback");
            oval = content.Load<Texture2D>("oval");
		}
		public void Changetext (string block, string name)
		{
			Block = block;

            lines.Add(new action { text = "", name = name, });
            List<object> list = string2list(Block);
			for (int i = 0; i < list.Count; i++) {
				string letter = list [i].ToString();
				lines[lines.Count-1].text += letter;
                if (font.MeasureString(lines[lines.Count - 1].text).X > 750)
                {
					for (int j = i; list[j].ToString() != " "; j--){
                        lines[lines.Count - 1].text = lines[lines.Count - 1].text.Remove(j);
					}
					i = list.Count;
				}
			}
			Block = block;
            int count = lines[lines.Count - 1].text.Length;
            Block = Block.Remove(0, count);
            if (Block != "")
            {
                Changetext(Block, name);
            }
		}
        public void ChangePos(int pos, string name, int speed)
        {
            lines.Add(new action { changepos = true, posision = pos, name = name, speed = speed, });
        }
        public void AddPlayer(string name, int pos, bool instant, int effect)
        {
            lines.Add(new action { addplayer = true, name = name, posision = pos, instant = instant, effect = effect});
        }
        public void RemovePlayer(string name, bool instant, int effect)
        {
            lines.Add(new action { removeplayer = true, name = name, instant = instant, effect = effect });
        }
        public void ChangeEmo(int emo, string name)
        {
            lines.Add(new action {changeemotion = true, emotion = emo, name = name,});
        }
        public void ChangeDay()
        {
            lines.Add(new action { changeday = true });
        }
        public void ChangeScene(int set, int Scene)
        {
            lines.Add(new action { changescene = true, sceneset = set, scene = Scene });
        }
        public void ClassSelection()
        {
            lines.Add(new action { classselection = true });
        }
        public void ChangeStat(int Stat, int Change, character Character)
        {
            lines.Add(new action{changestat = true, stat = Stat, change = Change, character = Character});
        }
        public void Changetext(string block, string name, List<string> Options)
        {
            Changetext(block, name);
            chosen = false;
            options = Options;
            for (int j = 0; j < options.Count; j++)
            {
                buttons.Add(new Button(new Vector2(602, 300 + j * 50), oval,Options[j],font));
            }
        }
        public void Lesson()
        {
            lines.Add(new action { lessonselect = true });
        }
        public List<object> string2list(string strg)
        {
            Array chars = strg.ToCharArray();
            List<object> list = new List<object>();
            foreach (object obj in chars)
                list.Add(obj);
            return list;
        }
		public bool finished()
		{
            if (chosen == true)
            {
                if (lines.Count == 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
		}
        character name2char(string name)
        {
            for (int i = 0; i < characters.Count; i++)
            {

                if (characters[i].name == lines[0].name)
                {
                    return characters[i];
                }
            }
            return null;
        }
		public void Update (Global global, Story story, Scene scene, ClassSelection classsection, character player)
		{
            for (int t = 0; t < characters.Count; t++)
                characters[t].Update();
            if (lines.Count > 0 && lines[0].classselection == true)
            {
                classsection.Update(global, player);
                if (classsection.finished == true)
                {
                    lines.RemoveAt(0);
                    classsection.finished = false;
                }
            }
            if (lines.Count > 0 && lines[0].lessonselect == true)
            {
                lines.RemoveAt(0);
                Random rand;
                string Text;
                int change;
                lines.Insert(0, new action { changescene = true, sceneset = 0, scene = classsection.choises[story.day] });
                if (classsection.choises[story.day] < 6)
                {
                    rand = new Random();
                    change = rand.Next(2, 5);
                    Text = (player.stats.Stats[classsection.choises[story.day]].name + " went up by " + change + "!");
                    lines.Insert(1, new action { changestat = true, stat = classsection.choises[story.day], change = change, character = player });
                    change = rand.Next(2, 5);
                    lines.Insert(2, new action { changestat = true, stat = 6, change = change, character = player });
                    lines.Insert(3, new action { text = Text, name = "", });
                    lines.Insert(4, new action { text = "Fatigue went up by " + change, name = "", });
                }
                else if (classsection.choises[story.day] == 6)
                {
                    rand = new Random();
                    change = 20;
                    lines.Insert(1, new action { changestat = true, stat = classsection.choises[story.day], change = -change, character = player });
                    Text = ("Fatigue went down by " + change + "!");
                    lines.Insert(2, new action { text = Text, name = "", });
                }
            }
            if (lines.Count > 0 && lines[0].addplayer == true)
            {
                if (wait == false)
                {
                    characters.Add(new character(charcont, chardb));
                    characters[characters.Count - 1].ChangeCharacter(lines[0].name, lines[0].effect, true, lines[0].posision);
                    wait = true;
                }
                if (characters[0].trans == false && wait == true)
                {
                    wait = false;
                    lines.RemoveAt(0);
                }
            }
            if (lines.Count > 0 && lines[0].removeplayer == true)
            {
                if (wait == false)
                {
                    characters.Remove(name2char(lines[0].name));
                    lines.RemoveAt(0);
                }
            }
            if (lines.Count > 0 && lines[0].changestat == true && wait == false)
            {
                lines[0].character.stats.Stats[lines[0].stat].value += lines[0].change;
                lines.RemoveAt(0);
            }
            if (lines.Count > 0 && lines[0].changescene == true)
            {
                wait = true;
                scene.ChangeScene(lines[0].scene);
                if (global.mouse.LeftButton == ButtonState.Pressed && global.old.LeftButton == ButtonState.Released)
                {
                    scene.transtime = 1;
                    global.old = global.mouse;
                }
                if (scene.finished() == true)
                {
                    wait = false;
                    lines.RemoveAt(0);
                }
            }
            if (lines.Count > 0 && lines[0].changeday == true)
            {
                
                if (story.day == 4)
                {
                    story.day = 0;
                    story.week++;
                    story.Event = 1;
                }
                else
                {
                    story.day++;
                    story.Event = 1;
                }
                lines.RemoveAt(0);
            }
            if (lines.Count> 0 && lines[0].changepos == true)
            {
                character character = name2char(lines[0].name);
                character.ChangePosition(lines[0].posision, lines[0].speed);
                wait = true;
                if (character.posision == character.tarpos)
                    wait = false;
                else if (global.mouse.LeftButton == ButtonState.Pressed && global.old.LeftButton == ButtonState.Released)
                    character.posision = character.tarpos;
                if (wait == false)
                    lines.RemoveAt(0);
            }
            if (lines.Count > 0 && lines[0].changeemotion == true && wait == false)
            {
                name2char(lines[0].name).emotion = lines[0].emotion;
                lines.RemoveAt(0);
            }
            if (showbuttons == true && chosen == false)
                for (int i = 0; i < options.Count; i++)
                {
                    if (buttons[i].Clicked(global.mouse,global.old) == true)
                    {
                        choise = i;
                        showbuttons = false;
                        chosen = true;
                    }
                }
            if (lines.Count > 0 && showbuttons == false && writing == false)
            {
                if (global.mouse.LeftButton == ButtonState.Pressed && global.old.LeftButton == ButtonState.Released)
                {
                    written = false;
                    int j = 0;
                    for (int i = 0; j < 3; i++)
                    {
                        if (lines[i].text == null)
                        { j = 3; }
                        else
                        {
                            lines.RemoveAt(i);
                            i -= 1;
                            j += 1;
                            count = 1;
                        }
                        if (lines.Count - (i + 1) == 0)
                        {
                            j = 3;
                        }
                    }
                }
            }
            if (writing == true && global.mouse.LeftButton == ButtonState.Pressed && global.old.LeftButton == ButtonState.Released)
            {
                written = true;
                count = 255;
            }

            if (wait == false)
                count += 0.5f;
            if (lines.Count == 0)
                count = 1;
		}
		public void Draw (SpriteBatch spritebatch, float scale, ClassSelection classselection)
		{

            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].Draw(spritebatch, scale);
            }
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));

			spritebatch.Draw (background, new Rectangle(204, 620, 800,128), Color.White);
            if (lines.Count > 0)
            {
                if (lines[0].name != null && wait == false)
                {
                    spritebatch.DrawString(font, lines[0].name + ":", new Vector2(220, 630), Color.Black);
                }
                int finished = 0;
                int length = 0;
                int j = 0;
                writing = false;
                for (int i = 0; j < 3; i++)
                {
                    if (lines[i].text == null)
                    { j = 3; }
                    else
                    {
                        length += lines[i].text.Length;
                        if (count < length && finished == i && written == false)
                        {
                            spritebatch.DrawString(font, lines[i].text.Remove(lines[i].text.Length - (length - (int)count)), new Vector2(220, 660 + j * 30), Color.Black);
                            writing = true;
                        }
                        if (length <= count || written == true)
                        {
                            spritebatch.DrawString(font, lines[i].text, new Vector2(220, 660 + j * 30), Color.Black);
                            finished = i + 1;
                        }
                        j += 1;
                    }
                    if (lines.Count - (i + 1) == 0)
                        j = 3;

                    showbuttons = false;
                    if (j == 3 && lines.Count == i + 1 && length <= count && chosen == false)
                    {
                        showbuttons = true;
                        for (int w = 0; w < options.Count; w++)
                        {
                            buttons[w].Draw(spritebatch);
                        }
                    }
                }
            }
            spritebatch.End();
            if (lines.Count > 0 && lines[0].classselection == true)
            {
                classselection.Draw(spritebatch, scale);
                
            }
		}
    }
}

