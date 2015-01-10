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
    public class Scene
    {
        Texture2D scene;
        Texture2D oldscene;
        public List<Effect> effects = new List<Effect>();
        public float transtime = 0;
        Vector2 posision = new Vector2(204,20);
        List<Texture2D>classscenes = new List<Texture2D>();
        public Scene(ContentManager content)
        {
            effects.Add(content.Load<Effect>("circle"));
            classscenes.Add(content.Load<Texture2D>("scene/sword training"));
            classscenes.Add(content.Load<Texture2D>("scene/destruction magic"));
            classscenes.Add(content.Load<Texture2D>("scene/healing"));
            classscenes.Add(content.Load<Texture2D>("scene/tactics"));
            classscenes.Add(content.Load<Texture2D>("scene/fitness"));
            classscenes.Add(content.Load<Texture2D>("scene/hand to hand"));
            classscenes.Add(content.Load<Texture2D>("scene/rest"));
            scene = content.Load<Texture2D>("DBack");
            oldscene = scene;
        }
        public void ChangeScene(int Scene)
        {
            if (scene != classscenes[Scene])
            {
                oldscene = scene;
                scene = classscenes[Scene];
                transtime = 0;
            }
        }
        public bool finished()
        {
            if (transtime >= 0.8f)
                return true;
            return false;
        }
        public void Draw(SpriteBatch spritebatch, float scale)
        {

            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
            spritebatch.Draw(oldscene, posision, Color.White);
            spritebatch.End();
            transtime += 0.01f;
            effects[0].Parameters["time"].SetValue(transtime);
            TransDraw(scene, Color.White, spritebatch, scale);
        }
        void TransDraw(Texture2D tex, Color colour, SpriteBatch spritebatch, float scale)
        {
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
            effects[0].Parameters["InColour"].SetValue(new Vector4((float)colour.R / 255, (float)colour.G / 255, (float)colour.B / 255, (float)colour.A / 255));
            //effects[0].Begin();
            effects[0].CurrentTechnique.Passes[0].Apply();

            spritebatch.Draw(tex, posision, Color.White);

            //effects[0].CurrentTechnique.Passes[0].End();
            //effects[0].End();
            spritebatch.End();
        }
    }
}
