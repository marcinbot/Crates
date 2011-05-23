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
using FarseerGames.FarseerPhysics.Factories;
using System.IO;

namespace editor2
{
    class Rec
    {
        public bool placing;
        public Geom rGeom;
        public Body rBody;
        public int width, heigth;
        int widthPrev, heigthPrev;
        public float rotation;
        PhysicsSimulator physicsS;

        public Rec(PhysicsSimulator pS, Vector2 pos, int wdth, int hgth, float rot)
        {
            physicsS = pS;
            width = wdth;
            heigth = hgth;
            rotation = rot;
            widthPrev = width;
            heigthPrev = heigth;
            placing = true;
            rBody = BodyFactory.Instance.CreateRectangleBody(pS, width, heigth, 1);
            rBody.IgnoreGravity = true;
            rBody.Position = pos;
            rBody.Rotation = rotation;
            rGeom = GeomFactory.Instance.CreateRectangleGeom(pS, rBody, width, heigth);
        }

        public void Update(KeyboardState kbState, Vector2 pos, MouseState mouseStateCurrent, MouseState mouseStatePrevious)
        {
            if (mouseStatePrevious.LeftButton == ButtonState.Released && mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                placing = false;
            }
            if (kbState.IsKeyDown(Keys.PageUp))
            {
                width += 20;
            }
            if (kbState.IsKeyDown(Keys.PageDown))
            {
                width -= 20;
            }
            if (kbState.IsKeyDown(Keys.Home))
            {
                heigth += 20;
            }
            if (kbState.IsKeyDown(Keys.End))
            {
                heigth -= 20;
            }
            if (kbState.IsKeyDown(Keys.Insert))
            {
                rBody.Rotation += 0.01f;
            }
            if (kbState.IsKeyDown(Keys.Delete))
            {
                rBody.Rotation -= 0.01f;
            }
            if (widthPrev != width | heigthPrev != heigth)
            {
                rBody.Dispose();
                rBody = null;
                rBody = BodyFactory.Instance.CreateRectangleBody(physicsS, width, heigth, 1);
                rBody.IgnoreGravity = true;
                rGeom = GeomFactory.Instance.CreateRectangleGeom(physicsS, rBody, width, heigth);
            }
            if (placing)
                rBody.Position = pos;
            widthPrev = width;
            heigthPrev = heigth;
        }

        public void Draw(SpriteBatch sB, Texture2D dot)
        {
            sB.Draw(dot, rBody.Position, Color.Red);
            foreach (Vector2 v in rGeom.WorldVertices)
                sB.Draw(dot, v, Color.Green);
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((double) rBody.Position.X);
            bw.Write((double) rBody.Position.Y);
            bw.Write(width);
            bw.Write(heigth);
            bw.Write((double) rBody.Rotation);
        }
    }
}
