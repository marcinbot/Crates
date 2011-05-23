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

namespace editor2
{
    class Rectangle
    {
        public bool placing;
        public Geom rGeom;
        public Body rBody;
        public int xSize, ySize;
        int xSizePrev, ySizePrev;
        PhysicsSimulator physicsS;

        public Rectangle(PhysicsSimulator pS, Vector2 pos)
        {
            physicsS = pS;
            xSize = 50;
            ySize = 50;
            xSizePrev = xSize;
            ySizePrev = ySize;
            placing = true;
            rBody = BodyFactory.Instance.CreateRectangleBody(pS, 50, 50, 1);
            rBody.IgnoreGravity = true;
            rBody.Position = pos;
            rGeom = GeomFactory.Instance.CreateRectangleGeom(pS, rBody, 50, 50);
        }

        public void Update(Vector2 pos)
        {
            if (xSizePrev != xSize | ySizePrev != ySize)
            {
                rBody.Dispose();
                rBody = null;
                rBody = BodyFactory.Instance.CreateRectangleBody(physicsS, xSize, ySize, 1);
                rBody.IgnoreGravity = true;
                rGeom = GeomFactory.Instance.CreateRectangleGeom(physicsS, rBody, xSize, ySize);
            }
            if (placing)
                rBody.Position = pos;
            xSizePrev = xSize;
            ySizePrev = ySize;
        }

        public void Draw(SpriteBatch sB, Texture2D dot)
        {
            sB.Draw(dot, rBody.Position, Color.Red);
            foreach (Vector2 v in rGeom.WorldVertices)
                sB.Draw(dot, v, Color.Green);
        }
    }
}
