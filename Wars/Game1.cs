using System;
//using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

using WarsUtils;

//using Microsoft.DirectX;
//using Microsoft.DirectX.DirectInput;

namespace TheWarSpace
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Wars : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Random random = new Random();
        float counter = 0;
        WarsContent warsContent = new WarsContent();
        WarsControl warsControl = new WarsControl();
        int numberOfPlayers = 2;
        bool whiteFlash = false;
        float addShipCounter = 0;
        Microsoft.DirectX.DirectInput.Device joystickDevice;
        Microsoft.DirectX.DirectInput.JoystickState joystickState;

        public Wars()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Constants.RESOLUTION_HEIGHT;
            graphics.PreferredBackBufferWidth = Constants.RESOLUTION_WIDTH;
            graphics.IsFullScreen = Constants.RESOLUTION_FULLSCREEN;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Microsoft.DirectX.DirectInput.DeviceList dl = Microsoft.DirectX.DirectInput.Manager.GetDevices(
                Microsoft.DirectX.DirectInput.DeviceType.Joystick, Microsoft.DirectX.DirectInput.EnumDevicesFlags.AttachedOnly);
            while (dl.Count > 0 && dl.MoveNext())
            {
                Microsoft.DirectX.DirectInput.DeviceInstance di = (Microsoft.DirectX.DirectInput.DeviceInstance)dl.Current;
                if (di.DeviceType == Microsoft.DirectX.DirectInput.DeviceType.Joystick)
                {
                    //InitializeJoystick( di );
                    Console.WriteLine("DirectX joystick: {0}", di);

                    // create a device from this controller.
                    joystickDevice = new Microsoft.DirectX.DirectInput.Device(di.InstanceGuid);
                    //joystickDevice.SetCooperativeLevel(this,
                    //    Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Background | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.NonExclusive);

                    // Tell DirectX that this is a Joystick.
                    joystickDevice.SetDataFormat(Microsoft.DirectX.DirectInput.DeviceDataFormat.Joystick);
                    // Finally, acquire the device.
                    joystickDevice.Acquire();

                    // Find the capabilities of the joystick
                    Microsoft.DirectX.DirectInput.DeviceCaps cps = joystickDevice.Caps;
                    // number of Axes
                    Console.WriteLine("Joystick Axis: " + cps.NumberAxes);
                    // number of Buttons
                    Console.WriteLine("Joystick Buttons: " + cps.NumberButtons);
                    // number of PoV hats
                    Console.WriteLine("Joystick PoV hats: " + cps.NumberPointOfViews);

                    break;
                }
            }

            // Sound
            warsContent.SetContentManager(new ContentManager(this.Services, @"Content\"));
            //contentManager = new ContentManager(this.Services, @"Content\");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            warsContent.LoadContent(this, graphics.GraphicsDevice.Viewport);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                warsContent.AddExplosion(1);
                warsContent.PlayExplosionSound();
            }

            /*
            Keys[] keys = Keyboard.GetState().GetPressedKeys();
            foreach(Keys key in keys)
            {
                Console.WriteLine("Key pressed: {0}", key);
            }
            */

            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            addShipCounter -= elapsed;
            if (Keyboard.GetState().IsKeyDown(Keys.Y) && addShipCounter <= 0)
            {
                addShipCounter = 0.3f;
                warsContent.AddShip(random.Next(4));
            }

            counter -= elapsed;
            if (counter <= 0)
            {
                counter = 3f;
                //Console.WriteLine("elapsed game time: {0}, elapsed real time: {1}", elapsed, (float)gameTime.ElapsedRealTime.TotalSeconds);
                if (warsContent.ships.Count == 1)
                {
                    warsContent.AddShip(random.Next(4));
                }

                //warsContent.AddShip();
                //Console.WriteLine("Angle: {0}", MathHelper.ToDegrees(((Ship)warsContent.ships[0]).RotationAngle));
            }


            // Joystick
            try
            {
                // poll the joystick
                joystickDevice.Poll();
                // update the joystick state field
                joystickState = joystickDevice.CurrentJoystickState;
            }
            catch (Exception err)
            {
                // we probably lost connection to the joystick
                // was it unplugged or locked by another application?
                Console.WriteLine(err.Message);
            }


            // TODO: Add your update logic here
            foreach (Cat cat in warsContent.cats)
            {
                cat.RedShotSpacingCounter -= elapsed;
            }
            bool playerAbove1throttle = false;
            for (int player = 1; player <= warsContent.ships.Count; player++)
            {
                //Ship ship = (Ship)warsContent.ships[player];
                Ship ship = warsContent.getPlayersShip(player);

                Commands com = warsControl.GetPlayerCommands(player, ref warsContent, ref joystickState);
                if (com.left)
                {
                    ship.RotationAngle += 4 * elapsed;
                    //Console.WriteLine("RotationAngle: {0}", MathHelper.ToDegrees(ship.RotationAngle));
                }
                if (com.right)
                {
                    ship.RotationAngle -= 4 * elapsed;
                    //Console.WriteLine("RotationAngle: {0}", MathHelper.ToDegrees(ship.RotationAngle));
                }
                if (com.throttle)
                {
                    // Sound
                    if (player == 1)
                    {
                        warsContent.PlayEngineSound(player);
                    }
                    ship.Throttle = elapsed * ship.ThrottlePower; // NOTE NOTE NOTE TODO: Make dependent on time (elapsed)
                    //Console.WriteLine("Ship speed: {0}", ship.Speed.ToString());
                    warsContent.AddFume(player);
                    if (player > 1)
                    {
                        playerAbove1throttle = true;
                    }
                }
                else
                {
                    if (player == 1)
                    {
                        warsContent.PauseEngineSound(player);
                    }
                    ship.Throttle = 0;
                }

                ship.RedShotSpacingCounter -= elapsed;
                if (com.fire == 1)
                {
                    if (ship.RedShotSpacingCounter <= 0)
                    {
                        ship.RedShotSpacingCounter = ship.RedShotSpacing;
                        //Console.WriteLine("Shot!");
                        warsContent.AddShot(player);
                    }
                    if (player == 1)
                    {
                        foreach (Cat cat in warsContent.cats)
                        {
                            if (cat.RedShotSpacingCounter <= 0)
                            {
                                cat.RedShotSpacingCounter = cat.RedShotSpacing;
                                warsContent.AddCatShot(player, cat);
                            }
                        }
                    }
                }
            }

            if (playerAbove1throttle)
            {
                warsContent.PlayEngineSound(2);
            }
            else
            {
                warsContent.PauseEngineSound(2);
            }

            //            if(Keyboard.GetState().IsKeyDown(Keys.S)) {
            //                ship.Throttle -= 4 * elapsed;
            //            }

            foreach (Cat cat in warsContent.cats)
            {
                cat.RotationAngle += 4 * elapsed;
                cat.PosRotationAngle -= 4 * elapsed;
                cat.Pos = warsContent.getPlayersShip(cat.Owner).Pos;
                //Vector2 goal = warsContent.getPlayersShip(cat.Owner).Pos + cat.PosRotationVector * cat.ShipDistance;
                cat.Pos += cat.PosRotationVector * cat.ShipDistance;
                //cat.Speed = goal;
                //cat.move();
            }

            foreach (Ship ship in warsContent.ships)
            {
                ship.updateSpeed();
            }
            for (int playerMinus1 = 0; playerMinus1 < warsContent.ships.Count; playerMinus1++)
            {
                if (playerMinus1 != 0)
                {
                    warsContent.getPlayersShip(playerMinus1 + 1).move();
                    warsContent.getPlayersShip(playerMinus1 + 1).Pos -= warsContent.getPlayersShip(1).Speed;
                }
            }

            foreach (WarsObject obj in warsContent.planetObjects)
            {
                obj.Speed = -warsContent.getPlayersShip(1).Speed;
                obj.move();
            }

            foreach (WarsObject obj in warsContent.bgObjects)
            {
                //obj.Speed = new Vector2(-ship.Speed.X, ship.Speed.Y);
                //obj.Speed = -((Ship)warsContent.ships[1]).Speed;
                obj.Speed = -warsContent.getPlayersShip(1).Speed;

                obj.move();
                Vector2 newPos = obj.Pos;
                newPos.X = newPos.X % Constants.RESOLUTION_WIDTH;
                newPos.Y = newPos.Y % Constants.RESOLUTION_HEIGHT;
                if (newPos.X < 0)
                {
                    newPos.X += Constants.RESOLUTION_WIDTH;
                }
                if (newPos.Y < 0)
                {
                    newPos.Y += Constants.RESOLUTION_HEIGHT;
                }
                obj.Pos = newPos;
            }

            foreach (WarsObject obj in warsContent.fumeObjects)
            {
                obj.move();
                //obj.Pos += new Vector2(-ship.Speed.X / Constants.FUME_SPEED_AFFECT_FACTOR, ship.Speed.Y / Constants.FUME_SPEED_AFFECT_FACTOR);
                //obj.Pos += new Vector2(-ship.Speed.X, ship.Speed.Y);
                //obj.Pos -= ((Ship)warsContent.ships[1]).Speed;
                obj.Pos -= warsContent.getPlayersShip(1).Speed;
            }
            for (int i = 0; i < warsContent.fumeObjects.Count; i++)
            {
                WarsObject obj = (WarsObject)warsContent.fumeObjects[i];
                //newPos.X = newPos.X % Constants.RESOLUTION_WIDTH;
                //newPos.Y = newPos.Y % Constants.RESOLUTION_HEIGHT;
                if (obj.updateTTL(elapsed))
                {
                    warsContent.fumeObjects.RemoveAt(i);
                }
                else
                {
                    if (obj.Pos.X < 0)
                    {
                        warsContent.fumeObjects.RemoveAt(i);
                    }
                    else if (obj.Pos.Y < 0)
                    {
                        warsContent.fumeObjects.RemoveAt(i);
                    }
                }
            }


            // Shots
            foreach (WarsObject obj in warsContent.shotObjects)
            {
                obj.move();
                //obj.Pos += new Vector2(-ship.Speed.X, ship.Speed.Y);
                //obj.Pos -= ((Ship)warsContent.ships[1]).Speed;
                obj.Pos -= warsContent.getPlayersShip(1).Speed;
            }
            for (int i = 0; i < warsContent.shotObjects.Count; i++)
            {
                WarsObject obj = (WarsObject)warsContent.shotObjects[i];
                if (obj.updateTTL(elapsed))
                {
                    warsContent.shotObjects.RemoveAt(i);
                }
            }
            // Collision detection (player 1 shoots others)
            for (int playerIndexMinus1 = 1; playerIndexMinus1 < warsContent.ships.Count; playerIndexMinus1++)
            {
                Ship ship = warsContent.getPlayersShip(playerIndexMinus1 + 1);
                BoundingSphere shipSphere = new BoundingSphere(new Vector3(ship.Pos, 0), (float)ship.Texture.Width / 2f);
                for (int i = 0; i < warsContent.shotObjects.Count; i++)
                {
                    WarsObject obj = (WarsObject)warsContent.shotObjects[i];
                    BoundingSphere shotSphere = new BoundingSphere(new Vector3(obj.Pos, 0), (float)warsContent.redShot.Height / 2f);
                    if (shotSphere.Intersects(shipSphere))
                    {
                        if (obj.Owner != playerIndexMinus1 + 1)
                        {
                            warsContent.AddExplosion(playerIndexMinus1 + 1);
                            warsContent.PlayExplosionSound();
                            warsContent.shotObjects.RemoveAt(i);
                            warsContent.ships.RemoveAt(playerIndexMinus1);
                            whiteFlash = true;
                            break;
                        }
                    }
                }
            }
            // Collision detection (player 1 gets shot)
            Ship ship1 = warsContent.getPlayersShip(1);
            BoundingSphere ship1Sphere = new BoundingSphere(new Vector3(ship1.Pos, 0), (float)ship1.Texture.Width / 2f);
            for (int i = 0; i < warsContent.shotObjects.Count; i++)
            {
                WarsObject obj = (WarsObject)warsContent.shotObjects[i];
                BoundingSphere shotSphere = new BoundingSphere(new Vector3(obj.Pos, 0), (float)warsContent.redShot.Height / 2f);
                if (shotSphere.Intersects(ship1Sphere))
                {
                    if (obj.Owner != 1)
                    {
                        //                        warsContent.AddExplosion(1);
                        //                        warsContent.PlayExplosionSound();
                        warsContent.shotObjects.RemoveAt(i);
                        //                        warsContent.ships.RemoveAt(playerIndexMinus1);
                        warsContent.PlayImHit();
                        break;
                    }
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (whiteFlash && Constants.DO_FLASH)
            {
                GraphicsDevice.Clear(Color.White);
                whiteFlash = false;
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);

                // TODO: Add your drawing code here
                warsContent.spriteBatch.Begin();
                //Vector2 pos = new Vector2((titleSafe.Right - titleSafe.Left) / 2, (titleSafe.Bottom - titleSafe.Top) / 2);

                foreach (WarsObject obj in warsContent.planetObjects)
                {
                    warsContent.spriteBatch.Draw(obj.Texture, obj.Pos, Color.White);
                }
                foreach (Ship ship in warsContent.ships)
                {
                    warsContent.spriteBatch.Draw(ship.Texture, ship.Pos, null, Color.White, ship.getDrawRotationAngle(), ship.Origin, 1.0f, SpriteEffects.None, 0f);
                }
                foreach (Cat cat in warsContent.cats)
                {
                    warsContent.spriteBatch.Draw(cat.Texture, cat.Pos, null, Color.White, cat.getDrawRotationAngle(), cat.Origin, 1.0f, SpriteEffects.None, 0f);
                }
                foreach (WarsObject obj in warsContent.bgObjects)
                {
                    warsContent.spriteBatch.Draw(obj.Texture, obj.Pos, Color.White);
                }
                foreach (WarsObject fume in warsContent.fumeObjects)
                {
                    warsContent.spriteBatch.Draw(fume.Texture, fume.Pos, Color.White);
                }
                foreach (WarsObject shot in warsContent.shotObjects)
                {
                    warsContent.spriteBatch.Draw(shot.Texture, shot.Pos, null, Color.White, shot.getDrawRotationAngle(), shot.Origin, 1.0f, SpriteEffects.None, 0f);
                }


                /*
                XNAShape speed = new XNAShape(GraphicsDevice);
                Vector2 speedStart = new Vector2(50, Constants.RESOLUTION_HEIGHT - 50);
                Vector2 speedEnd   = speedStart + warsContent.ship.Speed;
                speed.DrawLine(warsContent.spriteBatch, speedStart, speedEnd, Color.White);

                XNAShape shotSpeed = new XNAShape(GraphicsDevice);
                Vector2 shotSpeedStart = new Vector2(100, Constants.RESOLUTION_HEIGHT - 50);
                Vector2 shotSpeedEnd   = shotSpeedStart + warsContent.latestShotSpeedVector;
                speed.DrawLine(warsContent.spriteBatch, shotSpeedStart, shotSpeedEnd, Color.White);
                */
                /*
                XNAShape speed = new XNAShape(GraphicsDevice);
                //Ship ship1 = (Ship)warsContent.ships[1];
                Vector2 start = new Vector2(300, Constants.RESOLUTION_HEIGHT - 300);
                //Vector2 start = new Vector2(ship1.X, ship1.Y);
                Vector2 end = start + warsControl.toOtherVec;
                speed.DrawLine(warsContent.spriteBatch, start, end, Color.White);
                */
                warsContent.spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}







/*
protected Rectangle GetTitleSafeArea(float percent)
{
    Rectangle retval = new Rectangle(graphics.GraphicsDevice.Viewport.X,
                                     graphics.GraphicsDevice.Viewport.Y,
                                     graphics.GraphicsDevice.Viewport.Width,
                                     graphics.GraphicsDevice.Viewport.Height);
    return retval;
}
*/
