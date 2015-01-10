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
    public class CharacterStruct
	{
		public int gender;
		public int hairstyle;
		public int haircolour;
		public int eyecolour;
		public int skincolour;
		public int facestyle;
		public bool catears;
		public string name;
        public int year = 1;
        public List<Stat> Stats = new List<Stat>();
        public CharacterStruct()
        {
            Stats.Add(new Stat("Sword Skill", 0));
            Stats.Add(new Stat("Destruction", 0));
            Stats.Add(new Stat("Healing", 0));
            Stats.Add(new Stat("Tactics", 0));
            Stats.Add(new Stat("Fitness", 0));
            Stats.Add(new Stat("Hand to hand", 0));
            Stats.Add(new Stat("Fatigue", 0));
            Stats.Add(new Stat("Likability", 0));
        }
        public CharacterStruct clone()
        {
            List<Stat> Clone = new List<Stat>();
            for (int i = 0; i < this.Stats.Count; i++)
            {
                Clone.Add(this.Stats[i].clone());
            }
            return new CharacterStruct { Stats = Clone };
        }
	}
    public class Stat
    {
        public string name;
        public int value;
        public Stat(string Name, int Value)
        {
            name = Name;
            value = Value;
        }

        public Stat clone()
        {
            return new Stat (this.name, this.value );
        }
    }
    public struct face
    {
        public Texture2D detail;
        public Texture2D eyes;
        public face(Texture2D Face, Texture2D Eyes)
        {
            detail = Face;
            eyes = Eyes;
        }
    }
	public class characterContent
	{
		public List<Color> eyecolors = new List<Color>();
		public List<Color> complexions = new List<Color>();
		public List<Color> haircolours = new List<Color>();
        public List<Color> tuniccolours = new List<Color>();
		public List<Texture2D> bodys = new List<Texture2D>();
        public List<Texture2D> TBSbodys = new List<Texture2D>();
        public List<Texture2D> tunics = new List<Texture2D>();
        public List<Texture2D> TBStunics = new List<Texture2D>();
        public List<List<face>> facestyles = new List<List<face>>();
        public List<List<face>> TBSfacestyles = new List<List<face>>();
		public List<Texture2D> hairstyles = new List<Texture2D>();
        public List<Texture2D> TBShairstyles = new List<Texture2D>();
        public List<Effect> effects = new List<Effect>();
		public characterContent (ContentManager content)
		{
			//hair textures\\
			hairstyles.Add(content.Load<Texture2D>("Textures/female hair 1"));              //medium = 0
            
            //body textures\\
            bodys.Add(content.Load<Texture2D>("Textures/female body"));                 //male = 0
            
            //tunic textures\\
            tunics.Add(content.Load<Texture2D>("Textures/female tunic"));           //male = 0

            //face 0 textures\\
            facestyles.Add(new List<face>());
            facestyles[0].Add(new face(content.Load<Texture2D>("Textures/female happy face"),
                                       content.Load<Texture2D>("Textures/female happy eyes")));
            facestyles[0].Add(new face(content.Load<Texture2D>("Textures/female worried face"),
                                       content.Load<Texture2D>("Textures/female happy eyes")));
            
            //eye colours\\
			eyecolors.Add (new Color (161, 202, 241));					            //blue = 0
			eyecolors.Add (new Color (20, 255, 37));					            //green = 1
			//skin colours\\
			complexions.Add(new Color(255, 223, 196));				                //pale = 0
            complexions.Add(new Color(103, 75, 64));                                //black = 1
			//hair colours\\
			haircolours.Add (new Color (242, 218, 145));				            //blonde = 0
			haircolours.Add (new Color (160, 82, 45));					            //brunnette = 1
            haircolours.Add(new Color(20, 10, 0));                                  //black = 2
            //tunic colours\\
            tuniccolours.Add(new Color(0, 255f, 0));
            tuniccolours.Add(new Color(255f, 255f, 0));
            tuniccolours.Add(new Color(255f, 0, 0));
            //transisions\\
            effects.Add(content.Load<Effect>("circle"));                            //circle wipe = 0
		}
	}
    public class TBScharacter
    {
        characterContent charcont;
        chardatabace chardb;
        CharacterStruct stats;
        Color complexion = Color.White;
        Color haircolor = Color.White;
        Color eyecolour = Color.White;
        Color tuniccolour = Color.White;
        Texture2D skin;
        Texture2D hair;
        Texture2D face;
        Texture2D eyes;
        Texture2D tunic;
        public string name = "";
        public int emotion = 0;
        public Vector2 posision = new Vector2(0, 0);
        public TBScharacter(characterContent Charcont, chardatabace charDB, string Name)
        {
            charcont = Charcont;
            chardb = charDB;
            for (int i = 0; i < chardb.chars.Count; i++)
            {
                if (name == chardb.chars[i].name)
                {

                    stats = chardb.chars[i];
                    i = chardb.chars.Count;
                }
            }
            name = stats.name;
            skin = charcont.TBSbodys[stats.gender];
            tunic = charcont.TBStunics[stats.gender];
            tuniccolour = charcont.tuniccolours[stats.year - 1];
            complexion = charcont.complexions[stats.skincolour];
            hair = charcont.TBShairstyles[stats.hairstyle];
            haircolor = charcont.haircolours[stats.haircolour];
            face = charcont.TBSfacestyles[stats.facestyle][emotion].detail;
            eyes = charcont.TBSfacestyles[stats.facestyle][emotion].eyes;
            eyecolour = charcont.eyecolors[stats.eyecolour];
        }
        public void ChangeEmotion(int Emotion)
        {
            emotion = Emotion;
            face = charcont.TBSfacestyles[stats.facestyle][emotion].detail;
            eyes = charcont.TBSfacestyles[stats.facestyle][emotion].eyes;
        }
        public void Draw(SpriteBatch spritebatch, float scale)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));

            spritebatch.Draw(skin, posision, complexion);
            spritebatch.Draw(tunic, posision, tuniccolour);
            spritebatch.Draw(hair, posision, haircolor);
            spritebatch.Draw(face, posision, Color.White);
            spritebatch.Draw(eyes, posision, eyecolour);
                
            spritebatch.End();
        }
    }
	public class character
	{
		Color complexion = Color.White;
		Color haircolor = Color.White;
		Color eyecolour = Color.White;
        Color tuniccolour = Color.White;
		Texture2D skin;
		Texture2D hair;
		Texture2D face;
        Texture2D eyes;
        Texture2D tunic;
        public int emotion = 0;
		public Vector2 posision;
        public Vector2 tarpos;
        int speed;
		public CharacterStruct stats = new CharacterStruct();
		public bool visible = false;
        public bool changevis = false;
        public string name;
        chardatabace chardb;
        public characterContent charcont;
        public float transtime = 0;
        public bool trans = false;
        public string transision = "circle";
        Effect effect;
        public character(characterContent Charcont, chardatabace charDB)
        {
            charcont = Charcont;
            chardb = charDB;
        }
        public void ChangePosition(int pos, int Speed)
        {
            speed = Speed;
            tarpos = new Vector2(204 + pos * 150, 150);
            if (Speed == 0)
                posision = tarpos;
        }
        public void ChangeCharacter(string name, int Effect, bool newVisible, int pos)
        {
            ChangePosition(pos, 0);
            for (int i = 0; i < chardb.chars.Count; i++)
            {
                if (name == chardb.chars[i].name)
                {
                    
                    stats = chardb.chars[i];
                    i = chardb.chars.Count;
                }
            }
            if (Effect != 40)
            {
                effect = charcont.effects[Effect];
                trans = true;
                transtime = -0.05f;
            }
            else
            {
                trans = false;
            }
            if (visible == !newVisible)
            {
                changevis = true;
            }
        }
        public void Update()
		{
            name = stats.name;
			skin = charcont.bodys [stats.gender];
            tunic = charcont.tunics[stats.gender];
            tuniccolour = charcont.tuniccolours[stats.year - 1];
			complexion = charcont.complexions [stats.skincolour];
			hair = charcont.hairstyles [stats.hairstyle];
			haircolor = charcont.haircolours [stats.haircolour];
            face = charcont.facestyles[stats.facestyle][emotion].detail;
            eyes = charcont.facestyles[stats.facestyle][emotion].eyes;
			eyecolour = charcont.eyecolors [stats.eyecolour];
            if (changevis == true)
            {
                visible = !visible;
                changevis = false;
            }
            if (transtime >= 0.5f)
                trans = false;
            if (trans == true)
                transtime += 0.01f;
            if (Math.Abs(posision.X) - Math.Abs(tarpos.X) > 1 || Math.Abs(tarpos.X) - Math.Abs(posision.X) > 1)
            {
                if (posision.X < tarpos.X)
                    posision.X += speed;
                if (posision.X > tarpos.X)
                    posision.X -= speed;
            }
            else { posision = tarpos; }
		}
		public void Draw(SpriteBatch spritebatch,float scale)
		{
            if (visible == true && trans == true)
            {
                effect.Parameters["time"].SetValue(transtime);
                TransDraw(skin, complexion, spritebatch, scale);
                TransDraw(tunic, tuniccolour, spritebatch, scale);
                TransDraw(hair, haircolor, spritebatch, scale);
                TransDraw(face, Color.White, spritebatch, scale);
                TransDraw(eyes, eyecolour, spritebatch, scale);
            }
            else if (visible == true)
            {
                spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
                spritebatch.Draw(skin, new Rectangle((int)posision.X, (int)posision.Y, 250, 400), complexion);
                spritebatch.Draw(tunic, new Rectangle((int)posision.X, (int)posision.Y, 250, 400), tuniccolour);
                spritebatch.Draw(hair, new Rectangle((int)posision.X, (int)posision.Y, 250, 400), haircolor);
                spritebatch.Draw(face, new Rectangle((int)posision.X, (int)posision.Y, 250, 400), Color.White);
                spritebatch.Draw(eyes, new Rectangle((int)posision.X, (int)posision.Y, 250, 400), eyecolour);
                spritebatch.End();
            }
		}
        void TransDraw(Texture2D tex, Color colour, SpriteBatch spritebatch, float scale)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
            effect.Parameters["InColour"].SetValue(new Vector4((float)colour.R / 255, (float)colour.G / 255, (float)colour.B / 255, (float)colour.A / 255));
            //effect.Begin();
            effect.CurrentTechnique.Passes[0].Apply();

            spritebatch.Draw(tex, new Rectangle((int)posision.X, (int)posision.Y, 250, 400), Color.White);

            //effect.CurrentTechnique.Passes[0].End();
            //effect.End();
            spritebatch.End();
        }
	}
	public class chardatabace
	{
        public List<CharacterStruct> chars = new List<CharacterStruct>();
        public chardatabace()
        {
            chars.Add(new CharacterStruct
            {
                gender = 0,
                hairstyle = 0,
                haircolour = 0,
                eyecolour = 0,
                skincolour = 0,
                facestyle = 0,
                catears = false,
                year = 2,
                name = "Rocky"
            });
            chars.Add(new CharacterStruct
            {
                gender = 1,
                hairstyle = 0,
                haircolour = 1,
                eyecolour = 0,
                skincolour = 0,
                facestyle = 0,
                catears = false,
                name = "Daisy"
            });
            chars.Add(new CharacterStruct
            {
                gender = 0,
                hairstyle = 1,
                haircolour = 1,
                eyecolour = 0,
                skincolour = 0,
                facestyle = 0,
                catears = false,
                name = "Jake"
            });
            chars.Add(new CharacterStruct
            {
                gender = 0,
                hairstyle = 1,
                haircolour = 2,
                eyecolour = 0,
                skincolour = 1,
                facestyle = 0,
                catears = false,
                name = "Leroy"
            });
	    }
    }
}

