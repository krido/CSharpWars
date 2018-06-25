using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WarsUtils;

public class Commands
{
    public Commands()
    {
        this.throttle = false;
        this.left = false;
        this.right = false;
        this.fire = 0;
    }

    public bool throttle;
    public bool left;
    public bool right;
    public int fire; // 0 = no fire, 1-nn = fire 1-nn
}

public class WarsControl
{
    Random random = new Random();
    public Vector2 toOtherVec;
    float compensationAngle = 0;

    public Commands GetPlayerCommands(int playerIndex, ref WarsContent warsContent, ref Microsoft.DirectX.DirectInput.JoystickState joystickState)
    {
        Commands commands = new Commands();
        
        /*
        if (true)//joystickState.X != 0)
        {
            Console.WriteLine("joystickState.X = {0}", joystickState.X);
        }
        if (true)//joystickState.Y != 0)
        {
            Console.WriteLine("joystickState.Y = {0}", joystickState.Y);
        }
        */

        byte[] buttons = joystickState.GetButtons();
        /*
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] >= 128)
            {
                Console.WriteLine("Button {0} pressed!", i);
            }
        }
        */

        Ship me = warsContent.getPlayersShip(playerIndex);
        if (me.ShipType == Constants.SHIP_TYPE_MANUALLY_CONTROLLED)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) || joystickState.X == 0)
            {
                commands.left = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D) || joystickState.X == 65535)
            {
                commands.right = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) || buttons[4] >= 128 || buttons[5] >= 128 || buttons[6] >= 128 || buttons[7] >= 128)
            {
                commands.throttle = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.P) || buttons[0] >= 128 || buttons[1] >= 128 || buttons[2] >= 128 || buttons[3] >= 128)
            {
                commands.fire = 1;
            }
        }
        else if (me.ShipType == Constants.SHIP_TYPE_COMPUTER_CONTROLLED)
        {
            //int left = random.Next(2);
            //Ship me = warsContent.getPlayersShip(playerIndex);
            Ship other = warsContent.getPlayersShip(me.FightingPlayer);
            if (me.Counter <= 0) // find new ship to fight
            {
                me.Counter = 3;
                int newPlayerToFight = FindNearestShip(playerIndex, ref warsContent);
                if (newPlayerToFight != 0 && (other = warsContent.getPlayersShip(newPlayerToFight)) != null)
                {
                    me.FightingPlayer = newPlayerToFight;
                    Console.WriteLine("New player to fight: {0}", newPlayerToFight);
                }
                //else
                //{
                //    other = warsContent.getPlayersShip(1);
                //}
            }
            if (other == null)
            {
                other = warsContent.getPlayersShip(1);
            }
            //Ship other = warsContent.getPlayersShip(me.FightingPlayer);
            //Vector2 toOtherVec;
            toOtherVec = new Vector2();
            toOtherVec = other.Pos - me.Pos;

            Vector2 totSpeedVec = other.Speed - me.Speed;
            float toOtherAngle = GetVectorAngle(toOtherVec);
            Console.WriteLine("toOtherAngle: {0}", MathHelper.ToDegrees(toOtherAngle).ToString());
            float fromOtherAngle = MathHelper.WrapAngle(toOtherAngle - MathHelper.Pi);
            float speedAngle = GetVectorAngle(totSpeedVec);
            float diffAngle = MathHelper.WrapAngle(speedAngle - fromOtherAngle);
            if (diffAngle != 0)
            {
                float speedLength = totSpeedVec.Length();
                if (speedLength != 0)
                {
                    double sinDiffAngle = Math.Sin(diffAngle);
                    if (sinDiffAngle != 0)
                    {
                        double val = (double)(speedLength / Constants.REDSHOT_SPEED) * sinDiffAngle;
                        compensationAngle = -(float)Math.Asin(val);
                    }
                }
            }
            toOtherAngle += compensationAngle;
            //Console.WriteLine("compensationAngle: {0}", MathHelper.ToDegrees(compensationAngle));
            //Console.WriteLine("compensationAngle: {0}", MathHelper.ToDegrees(compensationAngle));
            //Console.WriteLine("GetVectorAngle: {0}, {1}", MathHelper.ToDegrees(toOtherAngle), MathHelper.ToDegrees(me.RotationAngle));
            if (me.RotationAngle - toOtherAngle > Constants.SMALLEST_ANGLE_DIFF_ACCEPT)
            {
                if (me.RotationAngle - toOtherAngle < MathHelper.Pi) // Handle wrap-around ("normal" case == < Pi ==> right)
                {
                    commands.right = true;
                }
                else
                {
                    commands.left = true;
                }
            }
            else if (toOtherAngle - me.RotationAngle > Constants.SMALLEST_ANGLE_DIFF_ACCEPT)
            {
                if (toOtherAngle - me.RotationAngle < MathHelper.Pi) // Handle wrap-around ("normal" case == < Pi ==> left)
                {
                    commands.left = true;
                }
                else
                {
                    commands.right = true;
                }
            }

            if (toOtherVec.Length() > 400 && (me.Speed.Length() <= other.Speed.Length() || Math.Abs(GetVectorAngle(me.Speed) - GetVectorAngle(toOtherVec)) > MathHelper.PiOver4))
            {
                commands.throttle = true;
            }

            if (random.Next(10) == 5)
            {
                commands.fire = 1;
            }
        }
        return commands;
    }


    public float GetVectorAngle(Vector2 vec)
    {
        Vector2 normVec = new Vector2(vec.X, vec.Y);
        normVec.Normalize();
        float sinPart = (float)Math.Asin((double)normVec.Y);
        if (normVec.X < 0)
        {
            sinPart = MathHelper.Pi - sinPart;
        }
        return MathHelper.WrapAngle(-sinPart);
        /*
        Vector2 zeroAngleUnitVector = new Vector2(1, 0);
        vec.X = (float)(Math.Cos(angle));
        vec.Y = -(float)(Math.Sin(angle));
        //    rotationVector.X = (float)(Math.Cos(angle) * zeroAngleUnitVector.X - Math.Sin(angle) * zeroAngleUnitVector.Y);
        //    rotationVector.Y = -(float)(Math.Cos(angle) * zeroAngleUnitVector.Y + Math.Sin(angle) * zeroAngleUnitVector.X);
        return vec;
        */
    }


    int FindNearestShip(int playerIndex, ref WarsContent warsContent)
    {
        Dictionary<int, float> dict = new Dictionary<int, float>();
        int i = 1;
        Ship me = warsContent.getPlayersShip(playerIndex);
        Ship other;
        while((other = warsContent.getPlayersShip(i)) != null)
        {
            if (i != playerIndex)
            {
                dict[i] = (me.Pos - other.Pos).Length();
            }
            i++;
        }
        if (dict.Count == 0)
        {
            return 0;
        }
        KeyValuePair<int, float> nearest = new KeyValuePair<int, float>(0, 0);
        foreach (KeyValuePair<int, float> kvp in dict)
        {
            if (nearest.Key == 0 || kvp.Value < nearest.Value)
            {
                nearest = kvp;
            }
        }
        return nearest.Key;
    }



    /*  
    public float GetCompAngle(Vector2 c, Vector2 a)
    {
        float cAngle = GetVectorAngle(c);
        float fromOtherAngle = MathHelper.WrapAngle(cAngle - MathHelper.Pi);
        float aAngle = GetVectorAngle(a);
        float diffAngle2 = MathHelper.WrapAngle(aAngle - fromOtherAngle);
        return -(float)Math.Asin((double)(a.Length() / Constants.REDSHOT_SPEED) * Math.Sin(diffAngle2));
    }
    */
}
