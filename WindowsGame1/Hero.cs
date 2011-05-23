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

namespace WindowsGame1
{
    class Hero
    {
        //0 - lewa dolna cz. reki
        //1 - lewa gorna cz. reki
        //2 - prawa dolna ''
        //3 - '' gorna ''
        //4 - dolna cz. nogi
        //5 - gorna ''
        //6 - tors

        //public Texture2D hTex;
        //ublic Vector2 hOrigin;
        public Body hBody;
        public Geom hGeom;
        //public Texture2D hTex = new Texture2D;
        private bool airborne;

        public Hero(PhysicsSimulator pS)
        {
            hBody = BodyFactory.Instance.CreateEllipseBody(pS, 32, 32, 2);
            //hBody = BodyFactory.Instance.CreateRectangleBody(pS, 64, 64, 1);
            hBody.Position = new Vector2(512, 381);
            //hBody.IsStatic = true;
            //hBody.IgnoreGravity = true;
            hGeom = GeomFactory.Instance.CreateEllipseGeom(pS, hBody, 32, 32, 12);
            hGeom.FrictionCoefficient = 1f;
            hGeom.CollisionGroup = 3;
            hGeom.OnSeparation += onSeperation;
            hGeom.OnCollision += onCollision;
            //hGeom = GeomFactory.Instance.CreateRectangleGeom(pS, hBody, 64, 64);
        }

        public void Draw(SpriteBatch sB, Texture2D tex)
        {
            sB.Draw(tex, hBody.Position, null, Color.GreenYellow, 0.0f, new Vector2(16, 16), 2, SpriteEffects.None, 0);
            foreach(Vector2 v in hGeom.WorldVertices)
                sB.Draw(tex, v, null, Color.Yellow, 0.0f, new Vector2(16, 16), 0.2f, SpriteEffects.None, 0);
        }

        public void Jump(float force)
        {
            if (!airborne)
            {
                hBody.ApplyImpulse(new Vector2(0, force));
                airborne = true;
            }
        }

        public void Climb(ref Path p, ref RevoluteJoint j)
        {
            if (p.Bodies.IndexOf(j.Body2) != 0)
            {
                Vector2 nextPart = p.Bodies[p.Bodies.IndexOf(j.Body2) - 1].Position;
                j.Dispose();
                j = null;
                hBody.Position = new Vector2(hBody.Position.X - (hBody.Position.X - nextPart.X) / 3, hBody.Position.Y - (hBody.Position.Y - nextPart.Y) / 3);
            }
        }

        public void SlideDown(ref Path p, ref RevoluteJoint j)
        {
            if (p.Bodies.IndexOf(j.Body2) != p.Bodies.Count - 1)
            {
                Vector2 nextPart = p.Bodies[p.Bodies.IndexOf(j.Body2) + 1].Position;
                j.Dispose();
                j = null;
                hBody.Position = new Vector2(hBody.Position.X - (hBody.Position.X - nextPart.X) / 3, hBody.Position.Y - (hBody.Position.Y - nextPart.Y) / 3);
            }
            else
            {
                j.Dispose();
                j = null;
                hBody.Position = new Vector2(hBody.Position.X, hBody.Position.Y + 10);
            }
        }

        private bool onCollision(Geom geom1, Geom geom2, ContactList contactList)
        {
            airborne = false;
            return true;
        }
        private void onSeperation(Geom geom1, Geom geom2)
        {
            //airborne = true;
        }
        
    }
}
