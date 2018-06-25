using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class WarsObject
{
    protected Vector2 pos;
    protected Texture2D texture;
    protected Vector2 origin;
    protected Vector2 speed;
    protected float rotationAngle;
    protected float drawCompensationRotationAngle;
    protected Vector2 rotationVector;
    protected bool rotationVectorIsDirty;
    protected Vector2 zeroDegreeUnitVector;
    protected int z;
    protected float ttl;
    int owningPlayer; // 0 == no owner

    public WarsObject(ref Texture2D t, Vector2 p, Vector2 s, float a, int z)
    {
        this.texture = t;
        this.origin.X = t.Width / 2;
        this.origin.Y = t.Height / 2;
        this.pos = p;
        this.speed = s;
        this.rotationAngle = a;
        this.rotationVectorIsDirty = true;
        this.zeroDegreeUnitVector = new Vector2(1, 0);
        this.z = z;
        this.ttl = 0;
        this.drawCompensationRotationAngle = 0;
        this.owningPlayer = 0;

    }
    public WarsObject(ref Texture2D t, Vector2 p, Vector2 s, float a)
    {
        this.texture = t;
        this.origin.X = t.Width / 2;
        this.origin.Y = t.Height / 2;
        this.pos = p;
        this.speed = s;
        this.rotationAngle = a;
        this.rotationVectorIsDirty = true;
        this.zeroDegreeUnitVector = new Vector2(1, 0);
        this.z = 1;
        this.ttl = 0;
        this.drawCompensationRotationAngle = 0;
        this.owningPlayer = 0;
    }
    public WarsObject(ref Texture2D t, Vector2 p, Vector2 s)
    {
        this.texture = t;
        this.origin.X = t.Width / 2;
        this.origin.Y = t.Height / 2;
        this.pos = p;
        this.speed = s;
        this.rotationAngle = 0;
        this.rotationVectorIsDirty = true;
        this.zeroDegreeUnitVector = new Vector2(1, 0);
        this.z = 1;
        this.ttl = 0;
        this.drawCompensationRotationAngle = 0;
        this.owningPlayer = 0;
    }
    public WarsObject(ref Texture2D t, Vector2 p, int z)
    {
        this.texture = t;
        this.origin.X = t.Width / 2;
        this.origin.Y = t.Height / 2;
        this.pos = p;
        this.speed = new Vector2(0, 0);
        this.rotationAngle = 0;
        this.rotationVectorIsDirty = true;
        this.zeroDegreeUnitVector = new Vector2(1, 0);
        this.z = z;
        this.ttl = 0;
        this.drawCompensationRotationAngle = 0;
        this.owningPlayer = 0;
    }
    public WarsObject(ref Texture2D t, Vector2 p)
    {
        this.texture = t;
        this.origin.X = t.Width / 2;
        this.origin.Y = t.Height / 2;
        this.pos = p;
        this.speed = new Vector2(0, 0);
        this.rotationAngle = 0;
        this.rotationVectorIsDirty = true;
        this.zeroDegreeUnitVector = new Vector2(1, 0);
        this.z = 1;
        this.ttl = 0;
        this.drawCompensationRotationAngle = 0;
        this.owningPlayer = 0;
    }

    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public int Owner
    {
        get { return owningPlayer; }
        set { owningPlayer = value; }
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
            rotationAngle = MathHelper.WrapAngle(value);
            /*
            rotationAngle = value % MathHelper.TwoPi;
            if (rotationAngle < 0)
            {
                rotationAngle = MathHelper.TwoPi - rotationAngle;
            }
            */
        }
    }

    public Vector2 RotationVector
    {
        get
        {
            if (rotationVectorIsDirty)
            {
                //rotationVector.X = (float)(Math.Cos(rotationAngle) * zeroDegreeUnitVector.X - Math.Sin(rotationAngle) * zeroDegreeUnitVector.Y);
                //rotationVector.Y = -(float)(Math.Cos(rotationAngle) * zeroDegreeUnitVector.Y + Math.Sin(rotationAngle) * zeroDegreeUnitVector.X);
                rotationVector.X = (float)(Math.Cos(rotationAngle));
                rotationVector.Y = -(float)(Math.Sin(rotationAngle));
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
        pos += speed / z;
    }
} // class WarsObject


public class ShootingObject : WarsObject
{
    private float throttle;
    private float throttlePower;
    private float maxSpeed;
    private float redShotSpacing;
    private float redShotSpacingCounter;
    private int totShotsFired;
    private int counter;

    public ShootingObject(ref Texture2D t, Vector2 p, Vector2 s, float a, int z, float throttle)
        : base(ref t, p, s, a, z)
    {
        this.throttle = throttle;
        this.totShotsFired = 0;
        this.counter = 0;
    }

    public int Counter
    {
        get { return counter; }
        set { counter = value; }
    }

    public int TotShotsFired
    {
        get { return totShotsFired; }
        set { totShotsFired = value; }
    }

    public float RedShotSpacingCounter
    {
        get { return redShotSpacingCounter; }
        set { redShotSpacingCounter = value; }
    }

    public float RedShotSpacing
    {
        get { return redShotSpacing; }
        set { redShotSpacing = value; }
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

    public float ThrottlePower
    {
        get { return throttlePower; }
        set { throttlePower = value; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    public void updateSpeed()
    {
        if (throttle == 0)
        {
            return;
        }
        speed += throttle * RotationVector;
        //Console.WriteLine("RotVect: {0}", this.RotationVector.ToString());
        //Console.WriteLine("SpeedVect: {0}", speed.ToString());
        if (speed.Length() > maxSpeed)
        {
            //Console.WriteLine("NORM!!!!!! {0}", speed.Length());
            speed.Normalize();
            speed *= maxSpeed;
        }
    }
} // class ShootingObject


public class Ship : ShootingObject
{
    private int fightingPlayer;
    private int shipType;

    public Ship(ref Texture2D t, Vector2 p, Vector2 s, float a, int z, float throttle, int shipType)
        : base(ref t, p, s, a, z, throttle)
    {
        this.fightingPlayer = 1;
        this.shipType = shipType;
    }

    public int ShipType
    {
        get { return shipType; }
    }

    public int FightingPlayer
    {
        get { return fightingPlayer; }
        set { fightingPlayer = value; }
    }
} // class Ship


public class Cat : ShootingObject
{
    private int owningPlayerIndex;
    private int shipDistance;
    private float posRotationAngle;
    private Vector2 posRotationVector;
    private bool posRotationVectorIsDirty;

    public Cat(ref Texture2D t, int shipDistance, float posRotAngle, ref Ship owningShip, int owningPlayerIndex)
        : base(ref t, owningShip.Pos, owningShip.Speed, owningShip.RotationAngle, owningShip.Z, 0 /*throttle*/)
    {
        this.shipDistance = shipDistance;
        this.owningPlayerIndex = owningPlayerIndex;
        this.posRotationAngle = posRotAngle;
    }

    public int Owner
    {
        get { return owningPlayerIndex; }
    }

    public int ShipDistance
    {
        get { return shipDistance; }
    }

    public float PosRotationAngle
    {
        get { return posRotationAngle; }
        set
        {
            posRotationVectorIsDirty = true;
            posRotationAngle = MathHelper.WrapAngle(value);
        }
    }

    public Vector2 PosRotationVector
    {
        get
        {
            if (posRotationVectorIsDirty)
            {
                posRotationVector.X = (float)(Math.Cos(posRotationAngle));
                posRotationVector.Y = -(float)(Math.Sin(posRotationAngle));
                posRotationVectorIsDirty = false;
            }
            return posRotationVector;
        }
    }

} // class Cat



