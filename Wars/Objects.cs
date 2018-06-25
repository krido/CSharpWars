using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheWarsObjects
{

    public class Object
    {
        protected Vector2 pos;
        protected Texture2D texture;
        protected Vector2 speed;
        protected float rotationAngle;
        protected float drawCompensationRotationAngle;
        protected Vector2 rotationVector;
        protected bool rotationVectorIsDirty;
        protected Vector2 zeroDegreeUnitVector;
        protected int z;
        protected float ttl;

        public Object(ref Texture2D t, Vector2 p, Vector2 s, float a, int z)
        {
            this.texture = t;
            this.pos = p;
            this.speed = s;
            this.rotationAngle = a;
            this.rotationVectorIsDirty = true;
            this.zeroDegreeUnitVector = new Vector2(1, 0);
            this.z = z;
            this.ttl = 0;
            this.drawCompensationRotationAngle = 0;
        }
        public Object(ref Texture2D t, Vector2 p, Vector2 s, float a)
        {
            this.texture = t;
            this.pos = p;
            this.speed = s;
            this.rotationAngle = a;
            this.rotationVectorIsDirty = true;
            this.zeroDegreeUnitVector = new Vector2(1, 0);
            this.z = 1;
            this.ttl = 0;
            this.drawCompensationRotationAngle = 0;
        }
        public Object(ref Texture2D t, Vector2 p, Vector2 s)
        {
            this.texture = t;
            this.pos = p;
            this.speed = s;
            this.rotationAngle = 0;
            this.rotationVectorIsDirty = true;
            this.zeroDegreeUnitVector = new Vector2(1, 0);
            this.z = 1;
            this.ttl = 0;
            this.drawCompensationRotationAngle = 0;
        }
        public Object(ref Texture2D t, Vector2 p, int z)
        {
            this.texture = t;
            this.pos = p;
            this.speed = new Vector2(0, 0);
            this.rotationAngle = 0;
            this.rotationVectorIsDirty = true;
            this.zeroDegreeUnitVector = new Vector2(1, 0);
            this.z = z;
            this.ttl = 0;
            this.drawCompensationRotationAngle = 0;
        }
        public Object(ref Texture2D t, Vector2 p)
        {
            this.texture = t;
            this.pos = p;
            this.speed = new Vector2(0, 0);
            this.rotationAngle = 0;
            this.rotationVectorIsDirty = true;
            this.zeroDegreeUnitVector = new Vector2(1, 0);
            this.z = 1;
            this.ttl = 0;
            this.drawCompensationRotationAngle = 0;
        }

        public float getDrawRotationAngle()
        {
            return -rotationAngle + drawCompensationRotationAngle;
        }

        public float DrawCompensationRotationAngle
        {
            get { return drawCompensationRotationAngle; }
            set { drawCompensationRotationAngle = value; }
        }

        public float TTL
        {
            get { return ttl; }
            set { ttl = value; }
        }

        // returns true if object should be removed, false otherwise
        public bool updateTTL(float seconds)
        {
            ttl -= seconds;
            if (ttl <= 0)
            {
                ttl = 0;
                return true;
            }
            return false;
        }

        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public float RotationAngle
        {
            get { return rotationAngle; }
            set
            {
                rotationVectorIsDirty = true;
                rotationAngle = value % MathHelper.TwoPi;
                if (rotationAngle < 0)
                {
                    rotationAngle = MathHelper.TwoPi - rotationAngle;
                }
            }
        }

        public Vector2 RotationVector
        {
            get
            {
                if (rotationVectorIsDirty)
                {
                    rotationVector.X = (float)(Math.Cos(rotationAngle) * zeroDegreeUnitVector.X - Math.Sin(rotationAngle) * zeroDegreeUnitVector.Y);
                    rotationVector.Y = -(float)(Math.Cos(rotationAngle) * zeroDegreeUnitVector.Y + Math.Sin(rotationAngle) * zeroDegreeUnitVector.X);
                    rotationVectorIsDirty = false;
                }
                return rotationVector;
            }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        public float Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public void move()
        {
            this.Pos += this.Speed / z;
        }
    } // class Object


    public class Ship : Object
    {
        private float throttle;

        public Ship(ref Texture2D t, Vector2 p, Vector2 s, float a, int z, float throttle)
            : base(ref t, p, s, a, z)
        {
            this.throttle = throttle;
        }

        public float Throttle
        {
            get { return throttle; }
            set
            {
                if (value < 0)
                {
                    throttle = 0;
                }
                else
                {
                    throttle = value;
                }
            }
        }

        public void updateSpeed()
        {
            if (throttle == 0)
            {
                return;
            }
            this.Speed += this.Throttle * this.RotationVector;
            //Console.WriteLine("RotVect: {0}", this.RotationVector.ToString());
            if (this.Speed.Length() > Constants.MAX_SPEED)
            {
                Console.WriteLine("NORM!!!!!! {0}", this.Speed.Length());
                this.speed.Normalize();
                this.Speed *= Constants.MAX_SPEED;
            }
        }
    } // class Ship

} // namespace TheWarsObjects 