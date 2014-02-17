using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Helpers
{
    /// <summary>
    /// Helper class to abbreviate gamepad input
    /// </summary>
    class Input
    {
        /// <summary>
        /// Checks to see if the X button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool X(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.X == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the Y button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool Y(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.Y == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the A button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool A(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.A == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the B button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool B(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.B == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the Back button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool BACK(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.Back == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the Start button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool START(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.Start == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the Left Bumper button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool LB(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.LeftShoulder == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the Right Bumper button is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool RB(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.RightShoulder == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the DPad's left side is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the Dpad is pressed</returns>
        public static bool DLEFT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Left == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the DPad's up side is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool DUP(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Up == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the DPad's down side is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool DDOWN(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Down == ButtonState.Pressed;
        }
        /// <summary>
        /// Checks to see if the DPad's right side is pressed for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <returns>Whether the button is pressed</returns>
        public static bool DRIGHT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Right == ButtonState.Pressed;
        }
        /// <summary>
        /// Gets how much the given player has moved the left stick in the X direction
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <returns>how much the stick has been pulled (-1 = totally left, 0 = not at all, 1 = totally right)</returns>
        public static float LSTICKX(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.X;
        }
        /// <summary>
        /// Gets how much the given player has moved the left stick in the Y direction
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <returns>how much the stick has been pulled (-1 = totally down, 0 = not at all, 1 = totally up)</returns>
        public static float LSTICKY(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.Y;
        }
        /// <summary>
        /// Gets how much the given player has moved the right stick in the X direction
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <returns>how much the stick has been pulled (-1 = totally left, 0 = not at all, 1 = totally right)</returns>
        public static float RSTICKX(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.X;
        }
        /// <summary>
        /// Gets how much the given player has moved the right stick in the Y direction
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <returns>how much the stick has been pulled (-1 = totally down, 0 = not at all, 1 = totally up)</returns>
        public static float RSTICKY(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.Y;
        }
        /// <summary>
        /// Gets how much the given player has moved the left stick in the X direction beyond a given bound
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <param name="bound">how much the stick should have moved</param>
        /// <returns>whether or not the stick has been moved beyond the bound</returns>
        public static bool LSTICKX(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.X >= bound;
        }
        /// <summary>
        /// Gets how much the given player has moved the left stick in the Y direction beyond a given bound
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <param name="bound">how much the stick should have moved</param>
        /// <returns>whether or not the stick has been moved beyond the bound</returns>
        public static bool LSTICKY(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.Y >= bound;
        }
        /// <summary>
        /// Gets how much the given player has moved the right stick in the X direction beyond a given bound
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <param name="bound">how much the stick should have moved</param>
        /// <returns>whether or not the stick has been moved beyond the bound</returns>
        public static bool RSTICKX(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.X >= bound;
        }
        /// <summary>
        /// Gets how much the given player has moved the right stick in the Y direction beyond a given bound
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <param name="bound">how much the stick should have moved</param>
        /// <returns>whether or not the stick has been moved beyond the bound</returns>
        public static bool RSTICKY(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.Y >= bound;
        }
        /// <summary>
        /// Gets how much the given player has pulled the left trigger
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <returns>how much the trigger has been pulled</returns>
        public static float LT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Left;
        }
        /// <summary>
        /// Gets how much the given player has pulled the right trigger
        /// </summary>
        /// <param name="playerNumber">the index of which player's gamepad to check</param>
        /// <returns>how much the trigger has been pulled</returns>
        public static float RT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Right;
        }
        /// <summary>
        /// Checks to see if the left trigger is pulled beyond a given bound for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <param name="bound">How much the trigger is pulled (0 = not at all, 1 = completely)</param>
        /// <returns>Whether the trigger has been pulled sufficiently</returns>
        public static bool LT(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Left >= bound;
        }
        /// <summary>
        /// Checks to see if the right trigger is pulled beyond a given bound for the given player's gamepad
        /// </summary>
        /// <param name="playerNumber">The index of the which player's gamepad to check</param>
        /// <param name="bound">How much the trigger is pulled (0 = not at all, 1 = completely)</param>
        /// <returns>Whether the trigger has been pulled sufficiently</returns>
        public static bool RT(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Right >= bound;
        }
        /// <summary>
        /// Translates an int into a PlayerIndex. Just a switch statement...
        /// </summary>
        /// <param name="num">The index to translate</param>
        /// <returns>The actual PlayerIndex</returns>
        public static PlayerIndex translate(int num)
        {
            if (num == 1) return PlayerIndex.One;
            if (num == 2) return PlayerIndex.Two;
            if (num == 3) return PlayerIndex.Three;
            if (num == 4) return PlayerIndex.Four;
            return 0;
        }


    }
}
