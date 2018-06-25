using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

//using Microsoft.Xna.Framework.Storage;

using TheWarSpace;

public class WarsContent
{
    public Vector2 latestShotSpeedVector;

    public SpriteBatch spriteBatch;
    
    Random random = new Random();
    
    // Textures
    Texture2D smallStar;
    Texture2D middleStar;
    Texture2D bigStar;
    Texture2D earthMoonFull;
    Texture2D planet1;
    Texture2D planet2;
    public Texture2D fume;
    public Texture2D redShot;
    public Texture2D blueShot;
    Texture2D ship1;
    Texture2D redShip;
    public Texture2D greenShipTexture;
    public Texture2D pinkShipTexture;
    public Texture2D wraithShipTexture;
    public Texture2D wideShipTexture;
    public Texture2D andromedaShipTexture;
    public Texture2D fighterShipTexture;
    public Texture2D blueBallTexture;

    // Object lists
    public ArrayList fumeObjects = new ArrayList();
    public ArrayList bgObjects = new ArrayList();
    public ArrayList planetObjects = new ArrayList();
    public ArrayList shotObjects = new ArrayList();
    
    public ArrayList ships = new ArrayList();
    public ArrayList cats = new ArrayList();
    
    // Sound
    ContentManager contentManager;
    SoundEffect engineSound;
    SoundEffectInstance engineSoundInstance;
    SoundEffect enemyEngineSound;
    SoundEffectInstance enemyEngineSoundInstance;
    SoundEffect redShotSound;
    SoundEffect blueShotSound;
    SoundEffect explosionSound;
    SoundEffect imHit;

    public void SetContentManager(ContentManager newContentManager)
    {
        contentManager = newContentManager;
    }

    public void LoadContent(Wars wars, Viewport viewport)
    {
        LoadGraphicsContent(wars, viewport);
        LoadAudioContent(wars);
    }

    public void LoadGraphicsContent(Wars wars, Viewport viewport)
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(wars.GraphicsDevice);

        smallStar = wars.Content.Load<Texture2D>(".\\graphics\\smallStar");
        middleStar = wars.Content.Load<Texture2D>(".\\graphics\\middleStar");
        bigStar = wars.Content.Load<Texture2D>(".\\graphics\\bigStar");
        earthMoonFull = wars.Content.Load<Texture2D>(".\\graphics\\EARTHMOONsm-full_alpha_small");
        planet1 = wars.Content.Load<Texture2D>(".\\graphics\\planet1_orig_alpha_small");
        planet2 = wars.Content.Load<Texture2D>(".\\graphics\\planet2_orig_alpha_small");
        fume = wars.Content.Load<Texture2D>(".\\graphics\\fume");
        redShot = wars.Content.Load<Texture2D>(".\\graphics\\redShot");
        blueShot = wars.Content.Load<Texture2D>(".\\graphics\\blueShot");

        // TODO: use this.Content to load your game content here
        ship1 = wars.Content.Load<Texture2D>(".\\graphics\\ship1");
        pinkShipTexture = wars.Content.Load<Texture2D>(".\\graphics\\convoyinterceptrapier2t_alpha_small");
        wraithShipTexture = wars.Content.Load<Texture2D>(".\\graphics\\GTF_Wraith_alpha_small");
        wideShipTexture = wars.Content.Load<Texture2D>(".\\graphics\\arc170_plans_lg_alpha_small");
        andromedaShipTexture = wars.Content.Load<Texture2D>(".\\graphics\\AndromedaOnline_alpha_small");
        fighterShipTexture = wars.Content.Load<Texture2D>(".\\graphics\\3dsthc3_alpha_small");
        blueBallTexture = wars.Content.Load<Texture2D>(".\\graphics\\blueball_small");
        redShip = wars.Content.Load<Texture2D>(".\\graphics\\alienstars-fighter");
        greenShipTexture = wars.Content.Load<Texture2D>(".\\graphics\\greenShip");
        //Viewport viewport = wars.graphics.GraphicsDevice.Viewport;
        Vector2 screenpos;
        screenpos.X = viewport.Width / 2;
        screenpos.Y = viewport.Height / 2;
/*
        Ship newShip = new Ship(ref redShip, // texture
                                screenpos, // position
                                new Vector2(0, 0), // speed vector
                                MathHelper.PiOver2, // rotation angle
                                1, // z plane position (1==fore most)
                                0); // throttle
        newShip.DrawCompensationRotationAngle = -MathHelper.PiOver2;
        newShip.ThrottlePower = Constants.THROTTLE_POWER;
        newShip.MaxSpeed = Constants.MAX_SPEED;
        newShip.RedShotSpacing = Constants.REDSHOT_SPACING;
        newShip.RedShotSpacingCounter = newShip.RedShotSpacing;
        //ships.Add(newShip); //ships[0] NOT USED!!
        ships.Add(newShip); // ==> ships[0] ==> playerIndex == 1
*/

        /*
            public Texture2D greenShipTexture;
    public Texture2D pinkShipTexture;
    public Texture2D wraithShipTexture;
    public Texture2D wideShipTexture;
    public Texture2D andromedaShipTexture;
    public Texture2D fighterShipTexture;
        */
        Ship newShip = new Ship(ref pinkShipTexture,//fighterShipTexture,//andromedaShipTexture,//wideShipTexture, //pinkShipTexture, //ship1, // texture
                                screenpos, // position
                                new Vector2(0, 0), // speed vector
                                MathHelper.PiOver2, // rotation angle
                                1, // z plane position (1==fore most)
                                0, // throttle
                                Constants.SHIP_TYPE_MANUALLY_CONTROLLED); // type
        newShip.DrawCompensationRotationAngle = MathHelper.PiOver2;
        newShip.ThrottlePower = Constants.THROTTLE_POWER;
        newShip.MaxSpeed = Constants.MAX_SPEED;
        newShip.RedShotSpacing = Constants.REDSHOT_SPACING;
        newShip.RedShotSpacingCounter = newShip.RedShotSpacing;
        //ships.Add(newShip); //ships[0] NOT USED!!
        ships.Add(newShip); // ==> ships[0] ==> playerIndex == 1

        
        Cat newCat = new Cat(ref blueBallTexture, //greenShipTexture, // texture
                             150, // ship distance
                             MathHelper.ToRadians((float) random.Next(359)), // posRotAngle
                             ref newShip,
                             1); //playerIndex
        newCat.DrawCompensationRotationAngle = -MathHelper.PiOver4 - MathHelper.ToRadians(16f);
        newCat.RedShotSpacing = Constants.REDSHOT_SPACING * 0.5f;
        newCat.RedShotSpacingCounter = newShip.RedShotSpacing;
        cats.Add(newCat);

        
        newCat = new Cat(ref blueBallTexture, //greenShipTexture, // texture
                         200, // ship distance
                         MathHelper.ToRadians((float)random.Next(359)), // posRotAngle
                         ref newShip,
                         1); //playerIndex
        newCat.DrawCompensationRotationAngle = -MathHelper.PiOver4 - MathHelper.ToRadians(16f);
        newCat.RedShotSpacing = Constants.REDSHOT_SPACING * 0.5f;
        newCat.RedShotSpacingCounter = newShip.RedShotSpacing;
        cats.Add(newCat);

        newCat = new Cat(ref blueBallTexture, //greenShipTexture, // texture
                         100, // ship distance
                         MathHelper.ToRadians((float)random.Next(359)), // posRotAngle
                         ref newShip,
                         1); //playerIndex
        newCat.DrawCompensationRotationAngle = -MathHelper.PiOver4 - MathHelper.ToRadians(16f);
        newCat.RedShotSpacing = Constants.REDSHOT_SPACING * 0.5f;
        newCat.RedShotSpacingCounter = newShip.RedShotSpacing;
        cats.Add(newCat);

        newCat = new Cat(ref blueBallTexture, //greenShipTexture, // texture
                 250, // ship distance
                 MathHelper.ToRadians((float)random.Next(359)), // posRotAngle
                 ref newShip,
                 1); //playerIndex
        newCat.DrawCompensationRotationAngle = -MathHelper.PiOver4 - MathHelper.ToRadians(16f);
        newCat.RedShotSpacing = Constants.REDSHOT_SPACING * 0.1f;
        newCat.RedShotSpacingCounter = newShip.RedShotSpacing;
        cats.Add(newCat);
        

        Ship newShip2 = new Ship(ref wraithShipTexture, //greenShipTexture, // texture
                                 screenpos / 2, // position
                                 new Vector2(0, 0), // speed vector
                                 MathHelper.PiOver2, // rotation angle
                                 1, // z plane position (1==fore most)
                                 0, // throttle
                                 Constants.SHIP_TYPE_COMPUTER_CONTROLLED); // type
        newShip2.DrawCompensationRotationAngle = -MathHelper.PiOver4 - MathHelper.ToRadians(16f);
        newShip2.ThrottlePower = Constants.THROTTLE_POWER * 2f;
        newShip2.MaxSpeed = Constants.MAX_SPEED * 2f;
        newShip2.RedShotSpacing = Constants.ENEMY_REDSHOT_SPACING * 0.5f;
        newShip2.RedShotSpacingCounter = newShip.RedShotSpacing;
        ships.Add(newShip2); // ==> ships[1] ==> playerIndex == 2


        int screenPosX;
        int screenPosY;
        Vector2 pos;
        for (int i = 0; i < Constants.NUMBER_OF_STARS; i++)
        {
            screenPosX = random.Next(Constants.RESOLUTION_WIDTH);
            screenPosY = random.Next(Constants.RESOLUTION_HEIGHT);
            pos = new Vector2(screenPosX, screenPosY);

            int size = random.Next(10);
            int z = 1+random.Next(4);
            if (size > 5)
            {
                bgObjects.Add(new WarsObject(ref smallStar, pos, z));
            }
            else if (size > 2)
            {
                bgObjects.Add(new WarsObject(ref middleStar, pos, z));
            }
            else
            {
                bgObjects.Add(new WarsObject(ref bigStar, pos, z));
            }
        }
        /*
        screenPosX = random.Next(Constants.RESOLUTION_WIDTH);
        screenPosY = random.Next(Constants.RESOLUTION_HEIGHT);
        pos = new Vector2(screenPosX, screenPosY);
        planetObjects.Add(new WarsObject(ref earthMoonFull, pos, 50));
        */
        screenPosX = random.Next(Constants.RESOLUTION_WIDTH);
        screenPosY = random.Next(Constants.RESOLUTION_HEIGHT);
        pos = new Vector2(screenPosX, screenPosY);
        planetObjects.Add(new WarsObject(ref planet2, pos, 10));
        /*
        screenPosX = random.Next(Constants.RESOLUTION_WIDTH);
        screenPosY = random.Next(Constants.RESOLUTION_HEIGHT);
        pos = new Vector2(screenPosX, screenPosY);
        planetObjects.Add(new WarsObject(ref planet1, pos, 5));
        */
    }

    public void LoadAudioContent(Wars wars)
    {
        engineSound = contentManager.Load<SoundEffect>(".\\sound\\Engine");
        engineSoundInstance = engineSound.Play(1.0f, 0.3f, 0.0f, true); // true == looped
        engineSoundInstance.Pause();
        enemyEngineSound = contentManager.Load<SoundEffect>(".\\sound\\Engine");
        enemyEngineSoundInstance = engineSound.Play(1.0f, 0.0f, 0.0f, true); // true == looped
        enemyEngineSoundInstance.Pause();
        redShotSound = contentManager.Load<SoundEffect>(".\\sound\\Laser1");
        blueShotSound = contentManager.Load<SoundEffect>(".\\sound\\Laser3");
        explosionSound = contentManager.Load<SoundEffect>(".\\sound\\ExplosionA");
        imHit = contentManager.Load<SoundEffect>(".\\sound\\ExplosionB");
    }

    public Ship getPlayersShip(int playerIndex)
    {
        if (playerIndex - 1 >= ships.Count)
        {
            return null;
        }
        return (Ship)ships[playerIndex - 1];
    }

    public void PlayImHit()
    {
        imHit.Play(1.0f, -1.0f, 0f, false);
    }

    public void PlayEngineSound(int playerIndex)
    {
        if (playerIndex == 1)
        {
            engineSoundInstance.Play();
        }
        else
        {
            enemyEngineSoundInstance.Play();
        }
    }

    public void PauseEngineSound(int playerIndex)
    {
        if (playerIndex == 1)
        {
            engineSoundInstance.Pause();
        }
        else
        {
            enemyEngineSoundInstance.Pause();
        }
    }

    public void PlayShotSound()
    {
        redShotSound.Play();
    }

    public void PlayBlueShotSound()
    {
        blueShotSound.Play();
    }

    public void PlayExplosionSound()
    {
        explosionSound.Play();
    }

    protected Vector2 getRandomVector(int size)
    {
        int randSize = size * 2 + 1;
        int x = size - random.Next(randSize);
        int y = size - random.Next(randSize);
        return new Vector2(x, y);
    }

    public void AddFume(int playerIndex)
    {
        float posFactor = 20f;
        float speedFactor = Constants.FUME_SPEED_FACTOR;

        //Ship ship = (Ship)ships[playerIndex];
        Ship ship = getPlayersShip(playerIndex);

        for (int i = 0; i < Constants.FUME_COUNT; i++)
        {
            Vector2 fumePos = ship.Pos - ship.RotationVector * posFactor;
            Vector2 fumeSpeed = -ship.RotationVector * speedFactor;
            fumeSpeed += ship.Speed;
            fumePos += getRandomVector(3);
            fumeSpeed += getRandomVector(2); // randomize the direction (velocity) somewhat
            WarsObject newFumeObj = new WarsObject(ref fume, fumePos, fumeSpeed);
            newFumeObj.TTL = Constants.FUME_TTL;
            fumeObjects.Add(newFumeObj);
        }
    }

    public void AddShot(int playerIndex)
    {
        float posFactor = 30f;

        //Ship ship = (Ship)ships[playerIndex];
        Ship ship = getPlayersShip(playerIndex);
        ship.TotShotsFired += 1;
        ship.Counter -= 1;

        Vector2 shotPos = ship.Pos + ship.RotationVector * posFactor;
        Vector2 shotSpeed = ship.RotationVector * Constants.REDSHOT_SPEED;
        shotSpeed += ship.Speed;
        WarsObject newShotObj;
        if (playerIndex == 1)
        {
            newShotObj = new WarsObject(ref blueShot, shotPos, shotSpeed, ship.RotationAngle);
            PlayBlueShotSound();
        }
        else
        {
            newShotObj = new WarsObject(ref redShot, shotPos, shotSpeed, ship.RotationAngle);
            PlayShotSound();
        }
        newShotObj.TTL = Constants.REDSHOT_TTL;
        newShotObj.DrawCompensationRotationAngle = MathHelper.PiOver2;
        newShotObj.Owner = playerIndex;
        shotObjects.Add(newShotObj);
        latestShotSpeedVector = shotSpeed;
    }


    public void AddCatShot(int playerIndex, Cat cat)
    {
        float posFactor = 30f;

        Ship ship = getPlayersShip(playerIndex);
        cat.TotShotsFired += 1;
        cat.Counter -= 1;

        Vector2 shotPos = cat.Pos + ship.RotationVector * posFactor;
        Vector2 shotSpeed = ship.RotationVector * Constants.REDSHOT_SPEED;
        shotSpeed += ship.Speed;
        WarsObject newShotObj;
        //if (playerIndex == 1)
        //{
        //    newShotObj = new WarsObject(ref blueShot, shotPos, shotSpeed, ship.RotationAngle);
        //    PlayBlueShotSound();
        //}
        //else
        {
            newShotObj = new WarsObject(ref redShot, shotPos, shotSpeed, ship.RotationAngle);
            PlayShotSound();
        }
        newShotObj.TTL = Constants.REDSHOT_TTL;
        newShotObj.DrawCompensationRotationAngle = MathHelper.PiOver2;
        newShotObj.Owner = playerIndex;
        shotObjects.Add(newShotObj);
        latestShotSpeedVector = shotSpeed;
    }


    public Vector2 GetUnitVectorFromAngle(float angle)
    {
        Vector2 vec = new Vector2();
        Vector2 zeroAngleUnitVector = new Vector2(1, 0);
        vec.X = (float)(Math.Cos(angle));
        vec.Y = -(float)(Math.Sin(angle));
        //    rotationVector.X = (float)(Math.Cos(angle) * zeroAngleUnitVector.X - Math.Sin(angle) * zeroAngleUnitVector.Y);
        //    rotationVector.Y = -(float)(Math.Cos(angle) * zeroAngleUnitVector.Y + Math.Sin(angle) * zeroAngleUnitVector.X);
        return vec;
    }

    public void AddExplosion(int playerIndex)
    {
        //Ship ship = (Ship)ships[playerIndex];
        Ship ship = getPlayersShip(playerIndex);

        for (int i = 0; i < Constants.EXPLOSION_DEBRIS_COUNT; i++)
        {
            Vector2 expPos = ship.Pos + getRandomVector(ship1.Width);
            Vector2 debrisSpeed = ship.Speed;
            float angle = MathHelper.ToRadians((float)random.Next(360));
            debrisSpeed += GetUnitVectorFromAngle(angle) * (Constants.EXPLOSION_DEBRIS_SPEED - Constants.EXPLOSION_DEBRIS_SPEED_VARIANCE + random.Next((int)(2000f * Constants.EXPLOSION_DEBRIS_SPEED_VARIANCE)) / 1000f);
            WarsObject newDebris = new WarsObject(ref fume, expPos, debrisSpeed, ship.RotationAngle);
            newDebris.TTL = Constants.EXPLOSION_DEBRIS_TTL - Constants.EXPLOSION_DEBRIS_TTL_VARIANCE + random.Next((int)(2000f * Constants.EXPLOSION_DEBRIS_TTL_VARIANCE)) / 1000f;
            fumeObjects.Add(newDebris);
        }
    }

    public void AddShip(int type)
    {
        Vector2 pos = new Vector2(random.Next(Constants.RESOLUTION_WIDTH), random.Next(Constants.RESOLUTION_HEIGHT));
        Vector2 speed = ((Ship)getPlayersShip(1)).Speed;

        switch (type)
        {
            case 0:
                AddWraithShip(pos, speed);
                break;
            case 1:
                AddWideShip(pos, speed);
                break;
            case 2:
                AddAndromedaShip(pos, speed);
                break;
            case 3:
                AddFighterShip(pos, speed);
                break;
            default:
                Console.WriteLine("SHOULD NOT GET HERE!");
                break;
        }
    }

    public void AddWraithShip(Vector2 pos, Vector2 speed)
    {
        Ship newShip = new Ship(ref wraithShipTexture,
                                pos, // position
                                new Vector2(0, 0), // speed vector
                                MathHelper.PiOver2, // rotation angle
                                1, // z plane position (1==fore most)
                                0, // throttle
                                Constants.SHIP_TYPE_COMPUTER_CONTROLLED); // type
        newShip.DrawCompensationRotationAngle = -MathHelper.PiOver4 - MathHelper.ToRadians(16f);
        newShip.ThrottlePower = Constants.ENEMY_THROTTLE_POWER * 0.75f;
        newShip.MaxSpeed = Constants.ENEMY_MAX_SPEED * 0.75f;
        newShip.RedShotSpacing = Constants.ENEMY_REDSHOT_SPACING * 0.5f;
        newShip.RedShotSpacingCounter = newShip.RedShotSpacing;
        newShip.Speed = speed;
        //newShip.FightingPlayer = ships.Count;
        ships.Add(newShip);
    }

    public void AddWideShip(Vector2 pos, Vector2 speed)
    {
        Ship newShip = new Ship(ref wideShipTexture,
                                pos, // position
                                new Vector2(0, 0), // speed vector
                                MathHelper.PiOver2, // rotation angle
                                1, // z plane position (1==fore most)
                                0, // throttle
                                Constants.SHIP_TYPE_COMPUTER_CONTROLLED); // type
        newShip.DrawCompensationRotationAngle = -MathHelper.PiOver2;
        newShip.ThrottlePower = Constants.ENEMY_THROTTLE_POWER * 0.5f;
        newShip.MaxSpeed = Constants.ENEMY_MAX_SPEED * 2f;
        newShip.RedShotSpacing = Constants.ENEMY_REDSHOT_SPACING;
        newShip.RedShotSpacingCounter = newShip.RedShotSpacing;
        newShip.Speed = speed;
        //newShip.FightingPlayer = ships.Count;
        ships.Add(newShip);
    }

    public void AddAndromedaShip(Vector2 pos, Vector2 speed)
    {
        Ship newShip = new Ship(ref andromedaShipTexture,
                                pos, // position
                                new Vector2(0, 0), // speed vector
                                MathHelper.PiOver2, // rotation angle
                                1, // z plane position (1==fore most)
                                0, // throttle
                                Constants.SHIP_TYPE_COMPUTER_CONTROLLED); // type
        newShip.DrawCompensationRotationAngle = MathHelper.PiOver2;
        newShip.ThrottlePower = Constants.ENEMY_THROTTLE_POWER * 2f;
        newShip.MaxSpeed = Constants.ENEMY_MAX_SPEED * 0.5f;
        newShip.RedShotSpacing = Constants.ENEMY_REDSHOT_SPACING / 2f;
        newShip.RedShotSpacingCounter = newShip.RedShotSpacing;
        newShip.Speed = speed;
        //newShip.FightingPlayer = ships.Count;
        ships.Add(newShip);
    }

    public void AddFighterShip(Vector2 pos, Vector2 speed)
    {
        Ship newShip = new Ship(ref fighterShipTexture,
                                pos, // position
                                new Vector2(0, 0), // speed vector
                                MathHelper.PiOver2, // rotation angle
                                1, // z plane position (1==fore most)
                                0, // throttle
                                Constants.SHIP_TYPE_COMPUTER_CONTROLLED); // type
        newShip.DrawCompensationRotationAngle = -MathHelper.Pi;
        newShip.ThrottlePower = Constants.ENEMY_THROTTLE_POWER * 2f;
        newShip.MaxSpeed = Constants.ENEMY_MAX_SPEED * 2f;
        newShip.RedShotSpacing = Constants.ENEMY_REDSHOT_SPACING;
        newShip.RedShotSpacingCounter = newShip.RedShotSpacing;
        newShip.Speed = speed;
        //newShip.FightingPlayer = ships.Count;
        ships.Add(newShip);
    }
}


