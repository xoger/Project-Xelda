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
	public class Story
	{
		novel text;
		public int week = 0;
        public int day = 0;
        public int Event = 1;
        Scene scene;
        List<string> options;
        ClassSelection classes;
		public Story (novel Text, ClassSelection Classes, Scene Scene)
		{
			text = Text;
            classes = Classes;
            scene = Scene;
		}
		public void events(chardatabace chardb, character player)
        {
            #region week 0, day 0
            if (week == 0 && day == 0)
            {
                if (Event == 1 && text.finished() == true)
                {
                    player.posision = new Vector2(1450, 600);
                    text.AddPlayer("Rocky", 1, false, 0);
                    text.Changetext("Hey there, welcome to the academy!", "Rocky");
                    text.ChangePos(4, "Rocky", 5);
                    text.Changetext("Woah you look dizzy", "Rocky");
                    options = new List<string>();
                    options.Add("yes");
                    options.Add("no");
                    text.ChangeEmo(1, "Rocky");
                    text.Changetext("Are you alright?", "Rocky", options);
                    Event = 3;
                }
                else if (Event == 3 && text.finished() == true)
                {
                    if (text.choise == 0)
                        Event = 4;
                    else
                        Event = 5;
                }
                else if (Event == 4 && text.finished() == true)
                {
                    text.ChangeEmo(0, "Rocky");
                    text.Changetext("Phew! I was worried for a sec", "Rocky");
                    Event = 6;
                }
                else if (Event == 5 && text.finished() == true)
                {
                    text.Changetext("Quick drink this water!", "Rocky");
                    Event = 6;
                }
                else if (Event == 6 && text.finished() == true)
                {
                    text.Changetext("Anyway, you need to deside what you want to study over the week", "Rocky");
                    text.ClassSelection();
                    text.RemovePlayer("Rocky", true, 0);
                    text.Lesson();
                    text.ChangeDay();
                    Event = 7;
                }
            }
            #endregion
            #region default
            else
            {
                if (Event == 1 && text.finished() == true)
                {
                    if (day == 0)
                        text.ClassSelection();
                    text.Lesson();
                    Event = 2;
                    text.ChangeDay();
                    
                }
            }
            #endregion
        }
    }
}

