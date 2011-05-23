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

using System.Diagnostics;

namespace editor2
{
    class Joi
    {
        public FixedRevoluteJoint joint;
        public Rec rec;
        public bool placing = true;

        public Joi(PhysicsSimulator pS, Vector2 mPoint)
        {
            rec = new Rec(pS, mPoint, 100, 100, 0);
        }

        public void Update(PhysicsSimulator pS, KeyboardState kbState, Vector2 pos, MouseState mouseStateCurrent, MouseState mouseStatePrevious)
        {
            if (!rec.placing)
                placing = Place(pS, mouseStateCurrent, mouseStatePrevious, pos);
            if (rec.placing)
                rec.Update(kbState, pos, mouseStateCurrent, mouseStatePrevious);
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed & mouseStatePrevious.LeftButton == ButtonState.Released & rec.placing)
                rec.placing = false;
        }

        public bool Place(PhysicsSimulator pS, MouseState mouseStateCurrent, MouseState mouseStatePrevious, Vector2 mPoint)
        {
            if (mouseStatePrevious.LeftButton == ButtonState.Released && mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                joint = JointFactory.Instance.CreateFixedRevoluteJoint(pS, rec.rBody, mPoint);
                return false;
            }
            return true;
        }

        public void Save(BinaryWriter bw)
        {
            rec.Save(bw);
            bw.Write((double)joint.Anchor.X);
            bw.Write((double)joint.Anchor.Y);
        }
    }
}
