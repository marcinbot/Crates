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
using FarseerGames.FarseerPhysics.Factories;

using System.Diagnostics;

namespace WindowsGame1
{
    public class Platform
    {
        public Vector2 pOrigin;
        public Body pBody;
        public Geom pGeom;
        public byte[] bytes;
        public RecBody rBodies;
        private List<Body> pBodyList = new List<Body>();
        private List<Geom> pGeomList = new List<Geom>();
        private enum Triangles{LeftRightDown,RightLeftDown,RightLeftUp,LeftRightUp}

        public Platform(string path, PhysicsSimulator pS)//, int width)
        {
            bytes=File.ReadAllBytes(path+".bmp");
            int width = bytes[18];
            int heigth = bytes[22];

            for (int i = 0; i < width*heigth; i++)
                if (bytes[bytes.Length-1-i]==0)
                {
                    createRectangle(pS, i, width);
                }
                else if (bytes[bytes.Length - 1 - i] == 1)
                {
                    createTriangle(pS, i, width, Triangles.LeftRightDown);
                }
                else if (bytes[bytes.Length - 1 - i] == 2)
                {
                    createTriangle(pS, i, width, Triangles.RightLeftDown);
                }
                else if (bytes[bytes.Length - 1 - i] == 3)
                {
                    createTriangle(pS, i, width, Triangles.RightLeftUp);
                }
                else if (bytes[bytes.Length - 1 - i] == 4)
                {
                    createTriangle(pS, i, width, Triangles.LeftRightUp);
                }
            rBodies = new RecBody(path, pS);
        }

        public void Draw(SpriteBatch sB, Texture2D dot)
        {
            foreach (Geom g in pGeomList)
                foreach (Vector2 v in g.WorldVertices)
                    sB.Draw(dot, v, Color.Khaki);
            rBodies.Draw(sB, dot);
        }

        private void createRectangle(PhysicsSimulator pS, int i, int width)
        {
            Body bd = BodyFactory.Instance.CreateRectangleBody(pS, 50, 50, 1);
            //Body bd = BodyFactory.Instance.CreatePolygonBody(pS, vertices, 1);
            bd.Position = new Vector2((width * 50) - (i % width) * 50, (i - (i % width)) / width * 50);
            bd.IsStatic = true;
            pBodyList.Add(bd);
            Geom gm = GeomFactory.Instance.CreateRectangleGeom(pS, bd, 50, 50);
            //Geom gm = GeomFactory.Instance.CreatePolygonGeom(pS, bd, vertices, 10);
            gm.FrictionCoefficient = 1;
            pGeomList.Add(gm);
        }

        private void createTriangle(PhysicsSimulator pS, int i, int width, Triangles t)
        {
            Vertices vertices = new Vertices();
            Vector2 position = new Vector2(0, 0) ;
            switch(t)
            {
                case Triangles.LeftRightDown:
                    position = new Vector2((width * 50) - (i % width) * 50 - 7, (i - (i % width)) / width * 50 + 8);    
                    vertices.Add(new Vector2(50, 50));
                    vertices.Add(new Vector2(0, 50));
                    vertices.Add(new Vector2(0, 0));
                    vertices.Add(new Vector2(12.5f, 12.5f));
                    vertices.Add(new Vector2(25, 25));
                    vertices.Add(new Vector2(37.5f, 37.5f));
                    break;
                case Triangles.RightLeftDown:
                    position = new Vector2((width * 50) - (i % width) * 50 + 7, (i - (i % width)) / width * 50 + 8);
                    vertices.Add(new Vector2(50, 0));
                    vertices.Add(new Vector2(50, 50));
                    vertices.Add(new Vector2(0, 50));
                    vertices.Add(new Vector2(12.5f, 37.5f));
                    vertices.Add(new Vector2(25, 25));
                    vertices.Add(new Vector2(37.5f, 12.5f));
                    break;
                case Triangles.RightLeftUp:
                    position = new Vector2((width * 50) - (i % width) * 50 - 7, (i - (i % width)) / width * 50 - 8);
                    vertices.Add(new Vector2(0, 0));
                    vertices.Add(new Vector2(50, 0));
                    vertices.Add(new Vector2(12.5f, 37.5f));
                    vertices.Add(new Vector2(25, 25));
                    vertices.Add(new Vector2(37.5f, 12.5f));
                    vertices.Add(new Vector2(0, 50));
                    break;
                case Triangles.LeftRightUp:
                    position = new Vector2((width * 50) - (i % width) * 50 + 7, (i - (i % width)) / width * 50 - 8);
                    vertices.Add(new Vector2(0, 0));
                    vertices.Add(new Vector2(50, 0));
                    vertices.Add(new Vector2(50, 50));
                    vertices.Add(new Vector2(37.5f, 37.5f));
                    vertices.Add(new Vector2(25, 25));
                    vertices.Add(new Vector2(12.5f, 12.5f));
                    break;
            }
            
            Body bd = BodyFactory.Instance.CreatePolygonBody(pS, vertices, 1);
            bd.Position = position;
            bd.IsStatic = true;
            pBodyList.Add(bd);
            Geom gm = GeomFactory.Instance.CreatePolygonGeom(pS, bd, vertices, 5);
            gm.FrictionCoefficient = 0.4f;
            pGeomList.Add(gm);
        }
    }
}
