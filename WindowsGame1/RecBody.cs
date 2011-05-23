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
using FarseerGames.FarseerPhysics.Dynamics.Joints;
using FarseerGames.FarseerPhysics.Factories;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using System.Diagnostics;

namespace WindowsGame1
{
    public class RecBody
    {
        public List<Geom> geoms = new List<Geom>();
        public List<Body> bodies = new List<Body>();
        public List<FixedRevoluteJoint> fJoints = new List<FixedRevoluteJoint>();
        public List<FarseerGames.FarseerPhysics.Dynamics.Path> chains = new List<FarseerGames.FarseerPhysics.Dynamics.Path>();
        public List<FarseerGames.FarseerPhysics.Dynamics.Path> swings = new List<FarseerGames.FarseerPhysics.Dynamics.Path>();
        
        public RecBody(string path, PhysicsSimulator pS)
        {
            FileStream fs = new FileStream("Content\\mapData\\map.map", FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(fs);
            Body bd;
            Geom gm;
            float X;
            float Y;
            int width;
            int heigth;
            float rot;
            int number = br.ReadInt32();
            for (int i = 0; i < number; i++)
            {
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                width = br.ReadInt32();
                heigth = br.ReadInt32();
                rot = (float)br.ReadDouble();
                bd = BodyFactory.Instance.CreateRectangleBody(pS, width, heigth, 1);
                bd.Position = new Vector2(X, Y);
                bd.Rotation = rot;
                //bd.Enabled = false;
                gm = GeomFactory.Instance.CreateRectangleGeom(pS, bd, width, heigth);
                gm.FrictionCoefficient = 0.5f;
                bodies.Add(bd);
                geoms.Add(gm);
            }
            number = br.ReadInt32();
            for (int i = 0; i < number; i++)
            {
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                width = br.ReadInt32();
                heigth = br.ReadInt32();
                rot = (float)br.ReadDouble();
                bd = BodyFactory.Instance.CreateRectangleBody(pS, width, heigth, 1);
                bd.Position = new Vector2(X, Y);
                bd.Rotation = rot;
                gm = GeomFactory.Instance.CreateRectangleGeom(pS, bd, width, heigth);
                gm.FrictionCoefficient = 0.5f;
                gm.CollisionGroup = 2;
                bodies.Add(bd);
                geoms.Add(gm);
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                JointFactory.Instance.CreateFixedRevoluteJoint(pS, bd, new Vector2(X, Y));
            }
            number = br.ReadInt32();
            for (int i = 0; i < number; i++)
            {
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                Vector2 start = new Vector2(X, Y);
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                Vector2 end = new Vector2(X, Y);
                width = br.ReadInt32();
                FarseerGames.FarseerPhysics.Dynamics.Path ch = ComplexFactory.Instance.CreateChain(pS, start, end, width, 1f, 2, LinkType.RevoluteJoint);
                foreach (Joint j in ch.Joints)
                {
                    j.Softness = 0.001f;
                    j.Breakpoint = 50f;
                }
                JointFactory.Instance.CreateFixedRevoluteJoint(pS, ch.Bodies[0], start);
                JointFactory.Instance.CreateFixedRevoluteJoint(pS, ch.Bodies[ch.Bodies.Count - 1], end);
                chains.Add(ch);
            }
            number = br.ReadInt32();
            for (int i = 0; i < number; i++)
            {
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                Vector2 start = new Vector2(X, Y);
                X = (float)br.ReadDouble();
                Y = (float)br.ReadDouble();
                Vector2 end = new Vector2(X, Y);
                width = br.ReadInt32();
                FarseerGames.FarseerPhysics.Dynamics.Path ch = ComplexFactory.Instance.CreateChain(pS, start, end, 15, 20, 1f, 3, LinkType.RevoluteJoint);
                foreach (Joint j in ch.Joints)
                {
                    j.Softness = 0;
                    j.Breakpoint = 100f;
                }
                Joint k = JointFactory.Instance.CreateFixedRevoluteJoint(pS, ch.Bodies[0], start);
                swings.Add(ch);
            }
        }

        public void Draw(SpriteBatch sB, Texture2D dot)
        {
            foreach (Geom g in geoms)
            {
                sB.Draw(dot, g.Body.Position, Color.Red);
                foreach (Vector2 v in g.WorldVertices)
                    sB.Draw(dot, v, Color.LightBlue);
            }
            foreach (FarseerGames.FarseerPhysics.Dynamics.Path c in chains)
                foreach (Geom g in c.Geoms)
                    foreach (Vector2 v in g.WorldVertices)
                        sB.Draw(dot, v, Color.LawnGreen);
            foreach (FarseerGames.FarseerPhysics.Dynamics.Path s in swings)
                foreach (Geom g in s.Geoms)
                    foreach (Vector2 v in g.WorldVertices)
                        sB.Draw(dot, v, Color.Pink);
        }
    }
}
