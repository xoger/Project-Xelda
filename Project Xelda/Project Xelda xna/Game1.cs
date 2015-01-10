using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Storage;

namespace Project_Xelda
{
    public class Global
    {
        public int stage = 0;
        public MouseState mouse;
        public MouseState old;
        public Vector2 mousepos;
    }
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		public bool widescreen = true;
		Texture2D back;
		SaveGameStorage save = new SaveGameStorage();
		SettingsStorage settings = new SettingsStorage();
		SaveGame state = new SaveGame ();
		Storage.Settings prefs = new Storage.Settings ();
		KeyboardState oldstate;
		public bool fullscreen;
		int screenwidth;
		int screenhight;
        Scene scene;
        Texture2D mouse;
        character player;
		public chardatabace chardb;
		public characterContent charcont;
		public novel text;
		public Story story;
        public Global global = new Global();
        MainMenu menu;
        CharactorCreator creator;
        statpanel panel;
        ClassSelection classes;
        Credits credits;
        Song mainsong;
        TBS tbs;
		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false;
            graphics.SynchronizeWithVerticalRetrace = false;
		}
		void ScreenSize ()
		{
			if (fullscreen == false) {
				screenhight = 768;
				if (widescreen == false) {
					screenwidth = 1024;
				} else {
					screenwidth = 1366;
				}
				} else if (fullscreen == true) {
				screenhight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				if (widescreen == true) {
					screenwidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 16 / 9;
				} else {
					screenwidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 4 / 3;
				}
			}
			graphics.IsFullScreen = fullscreen;
			graphics.PreferredBackBufferWidth = screenwidth;
			graphics.PreferredBackBufferHeight = screenhight;
			graphics.ApplyChanges();
		}

		protected override void Initialize ()
		{
			base.Initialize ();	
		}

		protected override void LoadContent ()
		{
            menu = new MainMenu(Content);
			//widescreen = settings.Load ().widescreen;
			fullscreen = settings.Load ().fullscreen;
			ScreenSize ();
			spriteBatch = new SpriteBatch (GraphicsDevice);
			back = Content.Load<Texture2D> ("border");
            scene = new Scene(Content);
			charcont = new characterContent(Content);
			chardb = new chardatabace();
            creator = new CharactorCreator(charcont, chardb, Content);
            mouse = Content.Load<Texture2D>("mouse");
            text = new novel(Content, charcont, chardb);
            panel = new statpanel(Content);
            classes = new ClassSelection(Content);
            story = new Story(text, classes, scene);
            credits = new Credits(Content);
            mainsong = Content.Load<Song>("maintrack");
            //MediaPlayer.Play(mainsong);
            MediaPlayer.IsRepeating = true;
            tbs = new TBS(chardb, text,Content);
		}
        public void Load()
        {
            story.week = save.Load(1, false).week;
            story.day = save.Load(1, false).day;
            story.Event = save.Load(1, false).Event;



            
            //text.Changetext("");
        }
		protected override void Update (GameTime gameTime)
		{
            global.mouse = Mouse.GetState();
            global.mousepos = new Vector2(global.mouse.X, global.mouse.Y);
			//save\\
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				state.week = story.week;
				state.day = story.day;
				state.Event = story.Event;


                state.player = player.stats;

                state.date = DateTime.Now;

				prefs.widescreen = widescreen;
				prefs.fullscreen = fullscreen;
				settings.Save (prefs);
				save.Save (state,1,false);
				this.Exit ();
			}
            //load\\
            if (Keyboard.GetState().IsKeyDown(Keys.L) && oldstate.IsKeyUp(Keys.L))
                Load();
			//widescreen\\
			if (Keyboard.GetState ().IsKeyDown (Keys.W) && oldstate.IsKeyUp(Keys.W)) 
			{
				widescreen = !widescreen;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.F) && oldstate.IsKeyUp (Keys.F)) 
			{
				fullscreen = !fullscreen;
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.A) && oldstate.IsKeyUp (Keys.A)) {
				ScreenSize ();
			}
            if (global.stage == 0)
            {
                menu.Update(global);
            }
            if (global.stage == 1)
            {
                text.Update(global, story, scene, classes, player);
                story.events(chardb, player);
                panel.Update(player.stats);
            }
            if (global.stage == 2)
            {
                player = creator.Update(global);
            }
            if (global.stage == 3)
            {
                credits.Update(global);
                //global.stage = credits.stage;
            }
            if (global.stage == 4)
            {
                text.Update(global, story, scene, classes, player);
                //panel.Update(player.stats);
                tbs.Update(global);
            }
			oldstate = Keyboard.GetState ();
            global.old = global.mouse;
			base.Update (gameTime);
		}


		protected override void Draw (GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(Scale(), Scale(), 0)));
            spriteBatch.Draw(back, new Rectangle(0, 0, 1366, 768), null, Color.LightCoral);
            spriteBatch.End();
            if (global.stage == 0)//mainmenu
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(Scale(), Scale(), 0)));
                menu.Draw(spriteBatch);
                spriteBatch.End();
            }

            if (global.stage == 1)//scene
            {
                scene.Draw(spriteBatch, Scale());
                text.Draw(spriteBatch, Scale(),classes);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(Scale(), Scale(), 0)));
                
                panel.Draw(spriteBatch);

                spriteBatch.End();

                player.Draw(spriteBatch, Scale() * 0.7f);
                
            }
            if (global.stage == 2)//charactor creator
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(Scale(), Scale(), 0)));
                creator.Draw(spriteBatch);
                spriteBatch.End();

                creator.DrawPlayer(spriteBatch, Scale());
            }
            if (global.stage == 3) 
            {
                credits.Draw(spriteBatch, Scale(),graphics,global);
            }
            if (global.stage == 4)
            {
                text.Draw(spriteBatch, Scale(), classes);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(Scale(), Scale(), 0)));

                panel.Draw(spriteBatch);

                spriteBatch.End();


                tbs.Draw(spriteBatch, Scale(), GraphicsDevice);
            }
            //player.Draw(spriteBatch, Scale() * 0.7f);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(Scale(), Scale(), 0)));
            spriteBatch.Draw(mouse, global.mousepos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
		float Scale()
		{
			float Wbuff;
			float Hbuff;
			if (screenwidth == screenhight *4/3) {
				Wbuff = (float)GraphicsDevice.Viewport.Width / 1024;
				Hbuff = (float)GraphicsDevice.Viewport.Height / 768;
				return Hbuff ;
			} else {
				Wbuff = (float)GraphicsDevice.Viewport.Width / 1366;
				Hbuff = (float)GraphicsDevice.Viewport.Height / 768;
				return Wbuff ;

			}
		}
	}
}

