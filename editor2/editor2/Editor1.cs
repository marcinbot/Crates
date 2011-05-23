using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Dynamics.Springs;
using FarseerGames.FarseerPhysics.Dynamics.Joints;
using FarseerGames.FarseerPhysics.Factories;
using WindowsGame1;

using System.Diagnostics;

namespace editor2
{
    public class Editor1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PhysicsSimulator physicsSimulator;
        Camera2D camera;
        Texture2D dot;
        public byte[] bytes;
        Platform pltfrm;

        List<Rec> rects = new List<Rec>();
        List<Joi> jointedRects = new List<Joi>();
        List<Chain> chains = new List<Chain>();
        List<Swing> swings = new List<Swing>();

        SpriteFont font;
        string modeString="";
        enum Mode {None, Rectangle, Joint, Chain, Rope};
        Mode mode = Mode.None;

        MouseState mouseStatePrevious, mouseStateCurrent;
        Vector2 mPoint;
        KeyboardState kbPrevious, kbCurrent;

        public Editor1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            physicsSimulator = new PhysicsSimulator(new Vector2(0, 1500));
            physicsSimulator.BiasFactor = 0.1f;
            physicsSimulator.MaxContactsToResolve = 6;
            physicsSimulator.Iterations = 20;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            camera = new Camera2D(new Vector2(1024, 768), null, null, null, null, null, null, null, null, null);
            dot = Content.Load<Texture2D>("dot");
            pltfrm = new Platform("C:\\Documents and Settings\\Marcin\\My Documents\\Visual Studio 2008\\Projects\\WindowsGame1\\WindowsGame1\\Content\\mapData\\map", physicsSimulator);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            mode = Mode.None;
            mouseStateCurrent = Mouse.GetState();
            mPoint = new Vector2(mouseStateCurrent.X + camera.Position.X - 512, mouseStateCurrent.Y + camera.Position.Y - 381);
            kbCurrent = Keyboard.GetState();
            if (kbCurrent.IsKeyDown(Keys.Escape))
                this.Exit();
            if (kbCurrent.IsKeyDown(Keys.Right) | kbCurrent.IsKeyDown(Keys.D))
            {
                camera.Position = new Vector2(camera.Position.X + 10, camera.Position.Y);
            }
            if (kbCurrent.IsKeyDown(Keys.Left) | kbCurrent.IsKeyDown(Keys.A))
            {
                camera.Position = new Vector2(camera.Position.X - 10, camera.Position.Y);
            }
            if (kbCurrent.IsKeyDown(Keys.Up) | kbCurrent.IsKeyDown(Keys.W))
            {
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 10);
            }
            if (kbCurrent.IsKeyDown(Keys.Down) | kbCurrent.IsKeyDown(Keys.S))
            {
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 10);
            }

            if (kbCurrent.IsKeyDown(Keys.LeftControl) & kbCurrent.IsKeyDown(Keys.S))
            {
                Save();
            }
            if (kbCurrent.IsKeyDown(Keys.LeftControl) & kbCurrent.IsKeyDown(Keys.O))
            {
                Open();
            }
            if (kbCurrent.IsKeyDown(Keys.LeftControl) & kbCurrent.IsKeyDown(Keys.A))
            {
                rects.Clear();
                rects.TrimExcess();
                pltfrm.rBodies.bodies.Clear();
                pltfrm.rBodies.bodies.TrimExcess();
                pltfrm.rBodies.geoms.Clear();
                pltfrm.rBodies.geoms.TrimExcess();
            }

            if (kbCurrent.IsKeyDown(Keys.R) & kbPrevious.IsKeyUp(Keys.R))
            {
                if (rects.Count == 0)
                    rects.Add(new Rec(physicsSimulator, mPoint, 100, 100, 0));
                else
                {
                    rects[rects.Count - 1].placing = false;
                    rects.Add(new Rec(physicsSimulator, mPoint, 100, 100, 0));
                }
            }
            if (rects.Count!=0)
            {
                if (rects[rects.Count - 1].placing)
                {
                    rects[rects.Count - 1].Update(kbCurrent, mPoint, mouseStateCurrent, mouseStatePrevious);
                    mode = Mode.Rectangle;
                }
            }

            if (kbCurrent.IsKeyDown(Keys.J) & kbPrevious.IsKeyUp(Keys.J))
            {
                if (jointedRects.Count==0)
                    jointedRects.Add(new Joi(physicsSimulator, mPoint));
                else
                {
                    jointedRects[jointedRects.Count - 1].placing = false;
                    jointedRects.Add(new Joi(physicsSimulator, mPoint));
                }
            }
            if (jointedRects.Count != 0)
            {
                if (jointedRects[jointedRects.Count - 1].placing)
                {
                    jointedRects[jointedRects.Count - 1].Update(physicsSimulator, kbCurrent, mPoint, mouseStateCurrent, mouseStatePrevious);
                    mode = Mode.Joint;
                }
            }

            if (kbCurrent.IsKeyDown(Keys.C) & kbPrevious.IsKeyUp(Keys.C))
            {
                chains.Add(new Chain(mPoint));
            }
            if (chains.Count != 0)
            {
                if (!chains[chains.Count - 1].finished)
                {
                    chains[chains.Count - 1].Update(physicsSimulator, kbCurrent, mPoint, mouseStateCurrent, mouseStatePrevious);
                    mode = Mode.Chain;
                }
            }


            if (kbCurrent.IsKeyDown(Keys.T) & kbPrevious.IsKeyUp(Keys.T))
            {
                swings.Add(new Swing(mPoint));
            }
            if (swings.Count != 0)
            {
                if (!swings[swings.Count - 1].finished)
                {
                    swings[swings.Count - 1].Update(physicsSimulator, kbCurrent, mPoint, mouseStateCurrent, mouseStatePrevious);
                    mode = Mode.Rope;
                }
            }

            switch (mode)
            {
                case Mode.None:
                    modeString = "None";
                    break;
                case Mode.Rectangle:
                    modeString = "Rectangle";
                    break;
                case Mode.Joint:
                    modeString = "Joint";
                    break;
                case Mode.Chain:
                    modeString = "Chain "+chains[chains.Count-1].bodies;
                    break;
                case Mode.Rope:
                    modeString = "Swinging rope " + swings[swings.Count - 1].bodies;
                    break;
            }




            physicsSimulator.Update(gameTime.ElapsedGameTime.Milliseconds * .001f);
            mouseStatePrevious = mouseStateCurrent;
            kbPrevious = kbCurrent;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.CameraMatrix);
           
            pltfrm.Draw(spriteBatch, dot);
            //if (rec!=null)
            //    rec.Draw(spriteBatch, dot);
            foreach (Rec r in rects)
                r.Draw(spriteBatch, dot);

            foreach (Joi j in jointedRects)
            {
                j.rec.Draw(spriteBatch, dot);
                if (j.joint!=null)
                    spriteBatch.Draw(dot, j.joint.Anchor, Color.Yellow);
            }

            foreach (Chain c in chains)
            {
                spriteBatch.Draw(dot, c.startV, Color.Blue);
                spriteBatch.Draw(dot, c.endV, Color.Blue);
                if (c.chain != null)
                {
                    foreach (Body b in c.chain.Bodies)
                        spriteBatch.Draw(dot, b.Position, Color.LightBlue);
                }
            }

            foreach (Swing s in swings)
            {
                spriteBatch.Draw(dot, s.startV, Color.Blue);
                spriteBatch.Draw(dot, s.endV, Color.Blue);
                if (s.chain != null)
                {
                    foreach (Body b in s.chain.Bodies)
                        spriteBatch.Draw(dot, b.Position, Color.LightBlue);
                }
            }

            spriteBatch.DrawString(font, modeString, camera.Position - new Vector2(512,381), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Save()
        {
            FileStream fs = new FileStream("C:\\Documents and Settings\\Marcin\\My Documents\\Visual Studio 2008\\Projects\\WindowsGame1\\WindowsGame1\\Content\\mapData\\map.map", FileMode.Create, FileAccess.Write, FileShare.Read);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(rects.Count);
            foreach (Rec r in rects)
                r.Save(bw);
            bw.Write(jointedRects.Count);
            foreach (Joi j in jointedRects)
                j.Save(bw);
            bw.Write(chains.Count);
            foreach (Chain c in chains)
                c.Save(bw);
            bw.Write(swings.Count);
            foreach (Swing s in swings)
                s.Save(bw);
            bw.Close();
            fs.Close();
        }

        protected void Open()
        {
            TextReader tr = new StreamReader("C:\\Documents and Settings\\Marcin\\My Documents\\Visual Studio 2008\\Projects\\WindowsGame1\\WindowsGame1\\Content\\mapData\\map.txt");
            string line;
            rects.Clear();
            rects.TrimExcess();
            Rec rec;
            // read a line of text
            //line=tr.ReadLine();
            while ((line = tr.ReadLine()) != null)
            {
                rec = new Rec(physicsSimulator, new Vector2(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1])), Convert.ToInt32(line.Split(' ')[2]), Convert.ToInt32(line.Split(' ')[3]), (float)Convert.ToDouble(line.Split(' ')[4]));
                //rec.rBody.Position = new Vector2(Convert.ToInt32(line.Split(' ')[0]), Convert.ToInt32(line.Split(' ')[1]));
                rec.placing = false;
                rects.Add(rec);
            }

            // close the stream
            tr.Close();
        }
    }
}
