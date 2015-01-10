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
    public class TBS
    {
        chardatabace chardb;
        novel text;
        float square = 32;
        float wheelzoomspeed = 1.1f;
        float mousezoomspeed = 1.05f;
        Vector2 gridpos;
        Vector2 oldgridpos;
        Vector2 abspos;
        Vector2 offset = new Vector2(0);
        Vector2 mapsize = new Vector2(50, 50);
        Texture2D grid;
        Texture2D highlight;
        Texture2D diag;
        bool inbounds = false;
        bool pan = false;
        Vector2 panlock;
        Vector2 startpos;
        float pandistance;
        bool panning = false;
        bool scroll = false;
        Vector2 mouselock;
        Vector2 gridlock;
        List<rangetile> path = new List<rangetile>();
        List<rangetile> tiles = new List<rangetile>();
        List<Vector2> obsticles = new List<Vector2>();
        List<speedtile> speedtiles = new List<speedtile>();
        List<Vector2> cross = new List<Vector2>();
        Viewport board = new Viewport { X = 204, Y = 20, Height = 600, Width = 800 };
        Viewport standard;
        public TBS(chardatabace Chardb, novel Text, ContentManager content)
        {
            
            text = Text;
            chardb = Chardb;
            grid = content.Load<Texture2D>("dot");
            highlight = content.Load<Texture2D>("highlight");
            diag = content.Load<Texture2D>("diagonal");
            cross.Add(new Vector2(0, -1));
            cross.Add(new Vector2(1, -1));
            cross.Add(new Vector2(1, 0));
            cross.Add(new Vector2(1, 1));
            cross.Add(new Vector2(0, 1));
            cross.Add(new Vector2(-1, 1));
            cross.Add(new Vector2(-1, 0));
            cross.Add(new Vector2(-1, -1));

            for (int x = 21; x < 30; x++)
                for(int y = 3; y< 10; y++)
                    speedtiles.Add(new speedtile(2, 1, 0.5f, 1, new Vector2(x,y)));

            for (int i = 3; i < 10; i++)
                obsticles.Add(new Vector2(10, i));
            for (int i = 5; i < 10; i++)
                obsticles.Add(new Vector2(i, 10));
            for (int i = 3; i < 10; i++)
                obsticles.Add(new Vector2(13, i));
            for (int i = 14; i < 14+5; i++)
                obsticles.Add(new Vector2(i, 10));


            
        }
        void focus(Vector2 abs, Vector2 target)
        {
            offset = target - (abs - offset);
        }
        public void Update(Global global)
        {
            gridpos = abs2grid(global.mousepos);
            abspos = grid2abs(gridpos);

            if (gridpos.X < mapsize.X &&
                gridpos.X >= 0 &&
                gridpos.Y < mapsize.Y &&
                gridpos.Y >= 0 &&
                global.mousepos.X > board.X &&
                global.mousepos.X < board.X + board.Width &&
                global.mousepos.Y > board.Y &&
                global.mousepos.Y < board.Y + board.Height)
                inbounds = true;
            else
                inbounds = false;


            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Right))
                offset.X -= 1;
            if (key.IsKeyDown(Keys.Left))
                offset.X += 1;
            if (key.IsKeyDown(Keys.Down))
                offset.Y -= 1;
            if (key.IsKeyDown(Keys.Up))
                offset.Y += 1;
            #region zoom
            if (global.mouse.RightButton == ButtonState.Pressed && global.old.RightButton == ButtonState.Released)
            {
                scroll = true;
                mouselock = global.mousepos;
                gridlock = abs2Fgrid(global.mousepos);
            }
            if (global.old.RightButton == ButtonState.Pressed && global.mouse.RightButton == ButtonState.Released)
            {
                scroll = false;
            }
            if (scroll == true)
            {
                if ((global.mouse.Y < (int)mouselock.Y))
                {
                    
                        square = (square * mousezoomspeed);
                    
                    focus(grid2abs(gridlock) + new Vector2(204, 20), mouselock);
                }
                if ((global.mouse.Y > (int)mouselock.Y))
                {
                    if (square / mousezoomspeed > 2)
                    {
                        square = (square / mousezoomspeed);
                        focus(grid2abs(gridlock) + new Vector2(204, 20), mouselock);
                    }
                }
                Mouse.SetPosition((int)mouselock.X, (int)mouselock.Y);
                global.mousepos = mouselock;
            }
            if (global.mouse.ScrollWheelValue > global.old.ScrollWheelValue)
            {
                Vector2 gridsquare = abs2Fgrid(global.mousepos);

                square = (square * wheelzoomspeed);

                focus(grid2abs(gridsquare) + new Vector2(204, 20), global.mousepos);
            }
            if (global.mouse.ScrollWheelValue < global.old.ScrollWheelValue)
            {
                Vector2 gridsquare = abs2Fgrid(global.mousepos);
                if (square / wheelzoomspeed > 2)
                {
                    square = (square / wheelzoomspeed);
                    focus(grid2abs(gridpos) + (-abspos + global.mousepos), global.mousepos);
                }
                focus(grid2abs(gridsquare) + new Vector2(204, 20), global.mousepos);
            }
            #endregion
            if (gridpos != oldgridpos)
            {
                path = new List<rangetile>();
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].position == gridpos)
                    {
                        path.Add(tiles[i]);
                        for (int j = 0; j < tiles.Count; j++)
                        {

                            if (tiles[j].position == path[path.Count - 1].prepotision)
                            {
                                path.Add(tiles[j]);
                                j = 0;
                            }
                        }
                        if (tiles.Count > 0)
                        i = tiles.Count;
                    }
                }
            }
            if (tiles.Count > 0)
                path.Add(tiles[0]);
            
            if (global.mouse.LeftButton == ButtonState.Released &&
                global.old.LeftButton == ButtonState.Pressed &&
                inbounds == true &&
                panning == false)
            {
                FindRange();
                path = new List<rangetile>();
                path.Add(new rangetile { position = gridpos });
            }
            #region pan
            if (global.old.LeftButton == ButtonState.Released && global.mouse.LeftButton == ButtonState.Pressed && inbounds == true && pan == false)
            {
                pan = true;
                panlock = abs2Fgrid(global.mousepos);
                startpos = global.mousepos;
                pandistance = 0;
            }
            if (pan == false)
                panning = false;
            if (pan == true)
            {
                //offset += new Vector2(global.mousepos.X - global.old.X, global.mousepos.Y - global.old.Y);
                focus(grid2abs(panlock) + new Vector2(204,20), global.mousepos);
                pandistance += Vector2.Distance(global.mousepos, new Vector2(global.old.X, global.old.Y));
                if (pandistance > 5)
                    panning = true;
                if (global.mouse.LeftButton == ButtonState.Released)
                {
                    pan = false;
                }
            }
            #endregion
            if (mapsize.X * square < 400)
                square = (int)400 /(int) mapsize.X;

            if (offset.X > 200)
                if (pan == true)
                {
                    global.mousepos.X -= offset.X - 200;
                    Mouse.SetPosition((int)global.mousepos.X, (int)global.mousepos.Y);
                    focus(grid2abs(panlock) + new Vector2(204, 20), global.mousepos);
                }
            
            if (offset.Y > 150)
                if (pan == true)
                {
                    global.mousepos.Y -= offset.Y - 150;
                    Mouse.SetPosition((int)global.mousepos.X, (int)global.mousepos.Y);
                    focus(grid2abs(panlock) + new Vector2(204, 20), global.mousepos);
                }

            if (offset.X + mapsize.X * square < 600)
                if (pan == true)
                {
                    global.mousepos.X -= offset.X - (600 - mapsize.X * square);
                    Mouse.SetPosition((int)global.mousepos.X, (int)global.mousepos.Y);
                    focus(grid2abs(panlock) + new Vector2(204, 20), global.mousepos);
                }
                //offset.X -= offset.X - (600 - mapsize.X * square);

            if (offset.Y + mapsize.Y * square < 450)
                if (pan == true)
                {
                    global.mousepos.Y -= offset.Y - (450 - mapsize.Y * square);
                    Mouse.SetPosition((int)global.mousepos.X, (int)global.mousepos.Y);
                    focus(grid2abs(panlock) + new Vector2(204, 20), global.mousepos);
                }
                //offset.Y = 450 - mapsize.X * square;

            oldgridpos = gridpos;
        }
        Vector2 abs2grid(Vector2 abs)
        {
            Vector2 i;
            i.X = (int)(abs.X - 204 - offset.X) / (int)square;
            if (abs.X < 204 + offset.X)
                i.X--;
            i.Y = (int)(abs.Y - 20 - offset.Y) / (int)square;
            if (abs.Y < 20 + offset.Y)
                i.Y--;
            return i;
        }
        Vector2 abs2Fgrid(Vector2 abs)
        {
            Vector2 i;
            i.X = (abs.X - 204 - offset.X) / (int)square;
            if (abs.X < 204 + offset.X)
                i.X--;
            i.Y = (abs.Y - 20 - offset.Y) / (int)square;
            if (abs.Y < 20 + offset.Y)
                i.Y--;
            return i;
        }
        Vector2 grid2abs(Vector2 grid)
        {
            Vector2 i;
            i.X = ((int)square * grid.X)  + offset.X;
            i.Y = ((int)square * grid.Y)  + offset.Y;
            return i;
        }
        public float pythag(float first, float second)
        {
            return (float)Math.Sqrt(Math.Pow(first, 2) + Math.Pow(second, 2));
        }
        void FindRange()
        {
            tiles = new List<rangetile>();
            tiles.Add(new rangetile { position = gridpos, steps = 7 });
            

            bool finished = false;
            while (finished == false)
            {
                int count = tiles.Count;
                for (int j = 0; j < count; j++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        bool speed = false;
                        float umph = 1;
                        for (int r = 0; r < speedtiles.Count; r++)
                            if (tiles[j].position + cross[c] == speedtiles[r].position)
                            {
                                umph = speedtiles[r].directions[c];
                                speed = true;
                            }
                        if (speed == false)
                            umph = pythag(umph * cross[c].X, umph * cross[c].Y);
                        if (tiles[j].New == true && tiles[j].steps >= umph)
                        {
                            bool add = true;
                            bool remove = false;
                            int remdex = 0;
                            for (int r = 0; r < obsticles.Count; r++)
                            {
                                if (tiles[j].position + cross[c] == obsticles[r])
                                    add = false;
                                if (tiles[j].position + new Vector2(cross[c].X, 0) == obsticles[r])
                                    add = false;
                                if (tiles[j].position + new Vector2(0, cross[c].Y) == obsticles[r])
                                    add = false;
                            }
                            if (tiles[j].position.X + cross[c].X >= mapsize.X || tiles[j].position.X + cross[c].X < 0)
                                add = false;
                            if (tiles[j].position.Y + cross[c].Y >= mapsize.Y || tiles[j].position.Y + cross[c].Y < 0)
                                add = false;
                            if (add == true)
                                for (int r = 0; r < tiles.Count; r++)
                                    if (tiles[j].position + cross[c] == tiles[r].position)
                                    {
                                        if (tiles[j].steps - umph <= tiles[r].steps)
                                            add = false;
                                        else
                                        {
                                            remove = true;
                                            remdex = r;
                                        }
                                        r = tiles.Count;
                                    }
                            
                            if (add == true)
                            {
                                tiles.Add(new rangetile { position = tiles[j].position + cross[c], prepotision = tiles[j].position, steps = tiles[j].steps - umph });
                            }
                            if (remove == true)
                            {
                                tiles.RemoveAt(remdex);
                                count--;
                                if (remdex < j)
                                    j--;
                            }
                        }
                    }
                    tiles[j].New = false;
                }
                finished = true;
                for (int j = 0; j < tiles.Count; j++)
                {
                    if (tiles[j].New == true)
                        finished = false;
                }
            }
        }
        
        public void Draw(SpriteBatch spritebatch, float scale, GraphicsDevice graphics)
        {
            standard = graphics.Viewport;
            graphics.Viewport = board;
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, 0)));
            spritebatch.Draw(highlight, new Rectangle(0, 0, board.Width, board.Height), Color.Black);
            

            foreach (speedtile speedtile in speedtiles)
            {
                spritebatch.Draw(highlight, tile(speedtile.position), Color.LightSkyBlue);
            }
            foreach (Vector2 posision in obsticles)
            {
                spritebatch.Draw(highlight, tile(posision), Color.Red);
            }
            for (int j = 0; j < tiles.Count; j++)
            {
                spritebatch.Draw(highlight, tile(tiles[j].position), new Color(1, 1, 1, 0.5f));
            }
            for (int j = 0; j < path.Count; j++)
            {
                spritebatch.Draw(highlight, tile(path[j].position), Color.Blue);
            }
            for (int i = 0; i < mapsize.X + 1; i++)
            {
                spritebatch.Draw(grid, new Rectangle((int)grid2abs(new Vector2(i, 0)).X, (int)grid2abs(new Vector2(i, 0)).Y, 1, (int)mapsize.Y * (int)square), Color.Blue);
            }
            for (int i = 0; i < mapsize.Y + 1; i++)
            {
                spritebatch.Draw(grid, new Rectangle((int)grid2abs(new Vector2(0, i)).X, (int)grid2abs(new Vector2(0, i)).Y, (int)mapsize.X * (int)square, 1), Color.Blue);
            }
            spritebatch.End();
            graphics.Viewport = standard;
        }
        Rectangle tile(Vector2 position)
        {

            return new Rectangle((int)grid2abs(position).X + 1, (int)grid2abs(position).Y + 1, (int)square-1, (int)square-1);
        }
    }
}
