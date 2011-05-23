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

namespace WindowsGame1
{
    class Box
    {
        public Geom boxGeom;
        public Texture2D boxTex;
        public Vector2 boxOrigin;
        public Body boxBody;
        float wdth, hgth;

        public Box(PhysicsSimulator pS, Texture2D tex, float x, float y, float width, float heigth, float mass)
        {
            boxTex = tex;
            wdth = width;
            hgth = heigth;
            boxBody = BodyFactory.Instance.CreateRectangleBody(pS, width, heigth, mass);
            boxBody.Position = new Vector2(x, y);
            boxGeom = GeomFactory.Instance.CreateRectangleGeom(pS, boxBody, 0.9f*width, 0.9f*heigth);
            boxGeom.FrictionCoefficient = 0.8f;

            boxOrigin = new Vector2(boxTex.Width / 2, boxTex.Height / 2);
        }
    }
}
