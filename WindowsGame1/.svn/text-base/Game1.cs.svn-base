using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Diagnostics;

namespace WindowsGame1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PhysicsSimulator physicsSimulator;
        Camera2D camera;
        Texture2D bgTex, c1Tex, grassTex, boxTex, gndTex, dot;
        //Texture2D[] heroTex = new Texture2D[7];
        Texture2D[] circles = new Texture2D[4];
        Rectangle vpRect;
        Hero heros;
        //Box[] crate = new Box[16];
        Platform pltfrm;

        RevoluteJoint hand;
        Path heldRope;

        Geom picked;
        Vector2[] mPoint = new Vector2[4];
        FixedLinearSpring mouseSpring;
        MouseState mouseStatePrevious, mouseStateCurrent;
        KeyboardState kbPrevious, kbCurrent;
        RevoluteJoint mouseJoint;
        Body mouseBody;
        Color mouseColor = Color.Blue;
        Vector2 rayPoint = new Vector2(0, 0);

        int bgX = 0, bgX1 = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "gra";
            Window.AllowUserResizing = true;
            //IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;

            this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 20);  //represents 10ms
            this.IsFixedTimeStep = true;
        }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            physicsSimulator = new PhysicsSimulator(new Vector2(0, 1500));
            physicsSimulator.BiasFactor = 0.1f;
            //physicsSimulator.MaxContactsToDetect = 1;
            physicsSimulator.MaxContactsToResolve = 6;
            physicsSimulator.Iterations = 20;

            //physicsSimulator.InactivityController.ActivationDistance = 100;
            //physicsSimulator.InactivityController.MaxIdleTime = 2000;
            //physicsSimulator.InactivityController.Enabled = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mouseBody = BodyFactory.Instance.CreateRectangleBody(physicsSimulator, 20, 20, 1);
            mouseBody.IsStatic = true;
            //mouseBody.IgnoreGravity = true;
            heros = new Hero(physicsSimulator);
            camera = new Camera2D(new Vector2(1024, 768), heros.hBody.Position, null, null, null, null, null, null, null, null);
            camera.TrackingBody = heros.hBody;

            for (int i = 0; i < 4; i++)
            {
                circles[i] = Content.Load<Texture2D>("circles\\" + i);
                mPoint[i] = new Vector2(0, 0);
            }

            pltfrm = new Platform("Content\\mapData\\map", physicsSimulator);

            dot = Content.Load<Texture2D>("sprites\\dot");
            boxTex = Content.Load<Texture2D>("sprites\\box");
            //for (int i=0; i<8; i++)
            //    crate[i] = new Box(physicsSimulator, boxTex, i*100+100, 100, 95, 95, 2);
            //for (int i = 8; i < 16; i++)
            //    crate[i] = new Box(physicsSimulator, boxTex, (i-8) * 100 + 100, 200, 95, 95, 2);

            gndTex = Content.Load<Texture2D>("sprites\\ground");

            bgTex = Content.Load<Texture2D>("sprites\\sky");
            c1Tex = Content.Load<Texture2D>("sprites\\clouds");
            grassTex = Content.Load<Texture2D>("sprites\\grass");

            vpRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.LoadContent();
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            kbCurrent = Keyboard.GetState();
            if (kbCurrent.IsKeyDown(Keys.Escape))
                this.Exit();
            if (kbCurrent.IsKeyDown(Keys.Right) | kbCurrent.IsKeyDown(Keys.D))
            {
                //heros.hBody.Position = new Vector2(heros.hBody.Position.X + 10, heros.hBody.Position.Y);
                heros.hBody.ApplyImpulse(new Vector2(40, 0));
                bgX += 2;
                if (bgX1 > 1024)
                    bgX = bgX - 1024;
                bgX1 += 4;
                if (bgX1 > 1024)
                    bgX1 = bgX1 - 1024;
            }
            if (kbCurrent.IsKeyDown(Keys.Left) | kbCurrent.IsKeyDown(Keys.A))
            {
                //heros.hBody.Position = new Vector2(heros.hBody.Position.X - 10, heros.hBody.Position.Y);
                heros.hBody.ApplyImpulse(new Vector2(-40, 0));
                bgX -= 2;
                if (bgX < 0)
                    bgX = bgX + 1024;
                bgX1 -= 4;
                if (bgX1 < 0)
                    bgX1 = bgX1 + 1024;
            }
            if (((kbCurrent.IsKeyDown(Keys.Up) & kbPrevious.IsKeyUp(Keys.Up))) | kbCurrent.IsKeyDown(Keys.W) & kbPrevious.IsKeyUp(Keys.W))
            {
                //heros.hBody.Position = new Vector2(heros.hBody.Position.X, heros.hBody.Position.Y-10);
                //if (heros.hBody.GetVelocityAtLocalPoint(new Vector2(16,16)).Y>=-30)
                //    heros.hBody.ApplyImpulse(new Vector2(0, -800));
                if (hand==null)
                    heros.Jump(-3200);
            }
            if (kbCurrent.IsKeyDown(Keys.Down))
            {
                //heros.hBody.Position = new Vector2(heros.hBody.Position.X, heros.hBody.Position.Y + 10);
            }

            foreach (Path p in pltfrm.rBodies.swings)
                foreach (Body b in p.Bodies)
                    if (heros.hGeom.Collide(b.Position) & hand == null)
                    {
                        hand = JointFactory.Instance.CreateRevoluteJoint(physicsSimulator, heros.hBody, b, heros.hBody.Position);
                        heldRope = p;
                    }
            if ((kbCurrent.IsKeyDown(Keys.Up)|kbCurrent.IsKeyDown(Keys.W)) & hand != null)
                heros.Climb(ref heldRope, ref hand);
            if ((kbCurrent.IsKeyDown(Keys.Down) | kbCurrent.IsKeyDown(Keys.S)) & hand != null)
                heros.SlideDown(ref heldRope, ref hand);


            camera.Update(kbCurrent, kbPrevious);
            mouseHandle();
            physicsSimulator.Update(gameTime.ElapsedGameTime.Milliseconds * .001f);
            kbPrevious = kbCurrent;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.CameraMatrix);
            vpRect.X = (int)camera.Position.X - 512;
            vpRect.Y = (int)camera.Position.Y - 381;
            //spriteBatch.Draw(bgTex, vpRect, Color.White);

            //spriteBatch.Draw(c1Tex, new Rectangle(vpRect.X, vpRect.Y, 1024 - bgX, 768), new Rectangle(bgX, 0, 1024 - bgX, 768), Color.White);
            //spriteBatch.Draw(c1Tex, new Rectangle(vpRect.X + 1024 - bgX, vpRect.Y, bgX, 768), new Rectangle(0, 0, bgX, 768), Color.White);

            //spriteBatch.Draw(grassTex, new Rectangle(vpRect.X, vpRect.Y, 1024 - bgX1, 768), new Rectangle(bgX1, 0, 1024 - bgX1, 768), Color.White);
            //spriteBatch.Draw(grassTex, new Rectangle(vpRect.X + 1024 - bgX1, vpRect.Y, bgX1, 768), new Rectangle(0, 0, bgX1, 768), Color.White);

            //foreach(Box i in crate)
            //    spriteBatch.Draw(i.boxTex, i.boxGeom.Position, null, Color.White, i.boxGeom.Rotation, i.boxOrigin, 1, SpriteEffects.None, 0);

            //spriteBatch.Draw(gndTex, new Vector2(0,0), Color.White);

            pltfrm.Draw(spriteBatch, dot);
            heros.Draw(spriteBatch, circles[1]);
           //spriteBatch.Draw(man.spt, man.pos, null, Color.White, 0.0f, man.ctr, 1.0f, man.sE, 0);

            if (mouseJoint != null | mouseSpring != null)
            {
                Vector2 mwa = mPoint[0];
                Vector2 mba = heros.hBody.Position;
                for (int i = 0; i < 7; i++)
                {
                    spriteBatch.Draw(circles[3], new Vector2(mwa.X + i * ((mba.X - mwa.X) / 14), mwa.Y + i * ((mba.Y - mwa.Y) / 14)), null, Color.GreenYellow, 0.0f, new Vector2(16, 16), 0.9f - 0.1f * i, SpriteEffects.None, 0);
                }
                for (int i = 6; i > 0; i--)
                {
                    spriteBatch.Draw(circles[3], new Vector2(mba.X - i * ((mba.X - mwa.X) / 14), mba.Y - i * ((mba.Y - mwa.Y) / 14)), null, Color.GreenYellow, 0.0f, new Vector2(16, 16), 0.9f - 0.1f * i, SpriteEffects.None, 0);
                }
                //spriteBatch.Draw(dot, mouseJoint.CurrentAnchor, null, Color.Red, 0, new Vector2(2,2), 5, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(dot, rayPoint, Color.Red);
            for (int i = 0; i < 4; i++)
                spriteBatch.Draw(circles[i], mPoint[i], null, mouseColor, 0.0f, new Vector2(16,16), 1-0.1f*i, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void mouseHandle()
        {
            mouseStateCurrent = Mouse.GetState();
            for (int i = 3; i > 0; i--)
                mPoint[i] = mPoint[i - 1];
            mPoint[0] = new Vector2(mouseStateCurrent.X + camera.Position.X - 512, mouseStateCurrent.Y + camera.Position.Y - 381);
            mouseBody.Position = mPoint[0];
            if (staticBetween(heros.hBody.Position, mPoint[0]))
                mouseColor = Color.Red;
            else
                mouseColor = Color.Blue;
            if (mouseStatePrevious.LeftButton == ButtonState.Released && mouseStateCurrent.LeftButton == ButtonState.Pressed && !staticBetween(heros.hBody.Position, mPoint[0]))
            {
                picked = physicsSimulator.Collide(mPoint[0]);
                if (picked != null && picked != heros.hGeom)
                {
                    if (!picked.Body.IsStatic & picked.CollisionGroup == 1)
                    {
                        mouseJoint = JointFactory.Instance.CreateRevoluteJoint(physicsSimulator, mouseBody, picked.Body, mPoint[0]);
                        picked.Body.RotationalDragCoefficient = 500f;
                    }
                    if (!picked.Body.IsStatic & picked.CollisionGroup != 1)
                        mouseSpring = SpringFactory.Instance.CreateFixedLinearSpring(physicsSimulator, picked.Body, picked.Body.GetLocalPosition(mPoint[0]), mPoint[0], 200, 20);
                }
            }
            else if ((mouseStatePrevious.LeftButton == ButtonState.Pressed && mouseStateCurrent.LeftButton == ButtonState.Released) | staticBetween(heros.hBody.Position, mPoint[0]))
            {
                if (mouseJoint != null && mouseJoint.IsDisposed == false)
                {
                    mouseJoint.Dispose();
                    mouseJoint = null;
                    picked.Body.RotationalDragCoefficient = 0.001f;
                    picked = null;
                }
                if (mouseSpring != null && mouseSpring.IsDisposed == false)
                {
                    mouseSpring.Dispose();
                    mouseSpring = null;
                    picked = null;
                }
            }
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseSpring != null)
                mouseSpring.WorldAttachPoint = mPoint[0];
            mouseStatePrevious = mouseStateCurrent;
        }

        protected bool staticBetween(Vector2 p1, Vector2 p2)
        {
            //return false;
            Geom colGeom;
            bool collided = false;
            rayPoint = p1;
            for (int i=0; i<25; i++)
            {
                colGeom = physicsSimulator.Collide(rayPoint);
                if (colGeom != null)
                {
                    collided = colGeom.Body.IsStatic;
                    if (colGeom.Body.IsStatic)
                        break;
                }
                rayPoint = new Vector2(rayPoint.X - (p1.X - p2.X) / 25, rayPoint.Y  - (p1.Y - p2.Y) / 25);
            }
            return collided;
        }
    }
}
