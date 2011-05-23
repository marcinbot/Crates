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

namespace editor2
{
    class Swing
    {
        public Vector2 startV;
        public Vector2 endV;
        public int bodies = 2;

        bool sPlaced = false;
        bool ePlaced = false;
        public bool finished = false;

        public FarseerGames.FarseerPhysics.Dynamics.Path chain;

        public Swing(Vector2 mPoint)
        {
            startV = mPoint;
            endV = mPoint;
        }

        public void Update(PhysicsSimulator pS, KeyboardState kbState, Vector2 pos, MouseState mouseStateCurrent, MouseState mouseStatePrevious)
        {
            if (!sPlaced)
            {
                startV = pos;
                if (mouseStatePrevious.LeftButton == ButtonState.Released & mouseStateCurrent.LeftButton == ButtonState.Pressed)
                    sPlaced = true;
            }
            else if (!ePlaced)
            {
                endV = pos;
                if (mouseStatePrevious.LeftButton == ButtonState.Released & mouseStateCurrent.LeftButton == ButtonState.Pressed)
                    ePlaced = true;
            }
            else
            {
                if (kbState.IsKeyDown(Keys.Add))
                {
                    bodies += 1;
                }
                if (kbState.IsKeyDown(Keys.Subtract))
                {
                    bodies -= 1;
                }
                if (kbState.IsKeyDown(Keys.Enter))
                    Finish(pS);
            }
        }

        public void Finish(PhysicsSimulator pS)
        {
            chain = ComplexFactory.Instance.CreateChain(pS, startV, endV, bodies, 1f, 1, LinkType.RevoluteJoint);
            JointFactory.Instance.CreateFixedRevoluteJoint(pS, chain.Bodies[0], startV);
            finished = true;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write((double)startV.X);
            bw.Write((double)startV.Y);
            bw.Write((double)endV.X);
            bw.Write((double)endV.Y);
            bw.Write(bodies);
        }
    }
}
