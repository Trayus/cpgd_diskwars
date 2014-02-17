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
    class OnceInput : GameComponent
    {
        public Dictionary<String, bool> buttons;
        private static OnceInput instance;
        private const float sensitivity = 0.2f;

        public OnceInput(Game game) : base(game)
        {
            game.Components.Add(this);
            OnceInput.instance = this;

            buttons = new Dictionary<string, bool>();

            for (int i = 1; i <= 4; i++)
            {
                buttons.Add("A" + i, false);
                buttons.Add("B" + i, false);
                buttons.Add("X" + i, false);
                buttons.Add("Y" + i, false);
                buttons.Add("RB" + i, false);
                buttons.Add("RT" + i, false);
                buttons.Add("LB" + i, false);
                buttons.Add("LT" + i, false);
                buttons.Add("START" + i, false);
                buttons.Add("BACK" + i, false);
                buttons.Add("DUP" + i, false);
                buttons.Add("DDOWN" + i, false);
                buttons.Add("DLEFT" + i, false);
                buttons.Add("DRIGHT" + i, false);
                buttons.Add("LSTICKUP" + i, false);
                buttons.Add("LSTICKDOWN" + i, false);
                buttons.Add("LSTICKLEFT" + i, false);
                buttons.Add("LSTICKRIGHT" + i, false);
                buttons.Add("RSTICKUP" + i, false);
                buttons.Add("RSTICKDOWN" + i, false);
                buttons.Add("RSTICKLEFT" + i, false);
                buttons.Add("RSTICKRIGHT" + i, false);
            }
            buttons.Add("ENTER", false);
            buttons.Add("SPACE", false);
            buttons.Add("UP", false);
            buttons.Add("DOWN", false);
            buttons.Add("LEFT", false);
            buttons.Add("RIGHT", false);
            buttons.Add("W", false);
            buttons.Add("S", false);
            buttons.Add("A", false);
            buttons.Add("D", false);
            buttons.Add("Q", false);
            buttons.Add("X", false);
            buttons.Add("Z", false);
            buttons.Add("C", false);
            buttons.Add("V", false);
            buttons.Add("F", false);
            buttons.Add("R", false);
            buttons.Add("T", false);
            buttons.Add("E", false);
            buttons.Add("BACKSPACE", false);
            buttons.Add("ESCAPE", false);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 1; i <= 4; i++)
            {
                PlayerIndex tocheck = Input.translate(i);

                if (GamePad.GetState(tocheck).IsConnected)
                {
                    if (GamePad.GetState(tocheck).Buttons.A == ButtonState.Released) buttons["A" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.B == ButtonState.Released) buttons["B" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.X == ButtonState.Released) buttons["X" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.Y == ButtonState.Released) buttons["Y" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.Back == ButtonState.Released) buttons["BACK" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.Start == ButtonState.Released) buttons["START" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.RightShoulder == ButtonState.Released) buttons["RB" + i] = false;
                    if (GamePad.GetState(tocheck).Buttons.LeftShoulder == ButtonState.Released) buttons["LB" + i] = false;
                    if (GamePad.GetState(tocheck).Triggers.Right < sensitivity) buttons["RT" + i] = false;
                    if (GamePad.GetState(tocheck).Triggers.Left < sensitivity) buttons["LT" + i] = false;
                    if (GamePad.GetState(tocheck).DPad.Up == ButtonState.Released) buttons["DUP" + i] = false;
                    if (GamePad.GetState(tocheck).DPad.Down == ButtonState.Released) buttons["DDOWN" + i] = false;
                    if (GamePad.GetState(tocheck).DPad.Left == ButtonState.Released) buttons["DLEFT" + i] = false;
                    if (GamePad.GetState(tocheck).DPad.Right == ButtonState.Released) buttons["DRIGHT" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Left.X > -sensitivity) buttons["LSTICKLEFT" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Left.X < sensitivity) buttons["LSTICKRIGHT" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Left.Y > -sensitivity) buttons["LSTICKDOWN" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Left.Y < sensitivity) buttons["LSTICKUP" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Right.X > -sensitivity) buttons["RSTICKLEFT" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Right.X < sensitivity) buttons["RSTICKRIGHT" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Right.Y > -sensitivity) buttons["RSTICKDOWN" + i] = false;
                    if (GamePad.GetState(tocheck).ThumbSticks.Right.Y < sensitivity) buttons["RSTICKUP" + i] = false;
                }
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.Enter)) buttons["ENTER"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Space)) buttons["SPACE"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.W)) buttons["W"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.A)) buttons["A"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.S)) buttons["S"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.D)) buttons["D"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Q)) buttons["Q"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.C)) buttons["C"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.V)) buttons["V"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.X)) buttons["X"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.R)) buttons["R"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.T)) buttons["T"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.F)) buttons["F"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.E)) buttons["E"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Z)) buttons["Z"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Up)) buttons["UP"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Down)) buttons["DOWN"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Left)) buttons["LEFT"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Right)) buttons["RIGHT"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Back)) buttons["BACKSPACE"] = false;
            if (!Keyboard.GetState().IsKeyDown(Keys.Escape)) buttons["ESCAPE"] = false;

            base.Update(gameTime);
        }

        public static bool ENTER()
        {
            if (!instance.buttons["ENTER"] && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                instance.buttons["ENTER"] = true;
                return true;
            }
            return false;
        }
        public static bool SPACE()
        {
            if (!instance.buttons["SPACE"] && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                instance.buttons["SPACE"] = true;
                return true;
            }
            return false;
        }
        public static bool UP()
        {
            if (!instance.buttons["UP"] && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                instance.buttons["UP"] = true;
                return true;
            }
            return false;
        }
        public static bool DOWN()
        {
            if (!instance.buttons["DOWN"] && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                instance.buttons["DOWN"] = true;
                return true;
            }
            return false;
        }
        public static bool LEFT()
        {
            if (!instance.buttons["LEFT"] && Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                instance.buttons["LEFT"] = true;
                return true;
            }
            return false;
        }
        public static bool RIGHT()
        {
            if (!instance.buttons["RIGHT"] && Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                instance.buttons["RIGHT"] = true;
                return true;
            }
            return false;
        }
        public static bool W()
        {
            if (!instance.buttons["W"] && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                instance.buttons["W"] = true;
                return true;
            }
            return false;
        }
        public static bool A()
        {
            if (!instance.buttons["A"] && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                instance.buttons["A"] = true;
                return true;
            }
            return false;
        }
        public static bool S()
        {
            if (!instance.buttons["S"] && Keyboard.GetState().IsKeyDown(Keys.S))
            {
                instance.buttons["S"] = true;
                return true;
            }
            return false;
        }
        public static bool D()
        {
            if (!instance.buttons["D"] && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                instance.buttons["D"] = true;
                return true;
            }
            return false;
        }
        public static bool Q()
        {
            if (!instance.buttons["Q"] && Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                instance.buttons["Q"] = true;
                return true;
            }
            return false;
        }
        public static bool E()
        {
            if (!instance.buttons["E"] && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                instance.buttons["E"] = true;
                return true;
            }
            return false;
        }
        public static bool R()
        {
            if (!instance.buttons["R"] && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                instance.buttons["R"] = true;
                return true;
            }
            return false;
        }
        public static bool T()
        {
            if (!instance.buttons["T"] && Keyboard.GetState().IsKeyDown(Keys.T))
            {
                instance.buttons["T"] = true;
                return true;
            }
            return false;
        }
        public static bool F()
        {
            if (!instance.buttons["F"] && Keyboard.GetState().IsKeyDown(Keys.F))
            {
                instance.buttons["F"] = true;
                return true;
            }
            return false;
        }
        public static bool Z()
        {
            if (!instance.buttons["Z"] && Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                instance.buttons["Z"] = true;
                return true;
            }
            return false;
        }
        public static bool X()
        {
            if (!instance.buttons["X"] && Keyboard.GetState().IsKeyDown(Keys.X))
            {
                instance.buttons["X"] = true;
                return true;
            }
            return false;
        }
        public static bool C()
        {
            if (!instance.buttons["C"] && Keyboard.GetState().IsKeyDown(Keys.C))
            {
                instance.buttons["C"] = true;
                return true;
            }
            return false;
        }
        public static bool V()
        {
            if (!instance.buttons["V"] && Keyboard.GetState().IsKeyDown(Keys.V))
            {
                instance.buttons["V"] = true;
                return true;
            }
            return false;
        }
        public static bool ESCAPE()
        {
            if (!instance.buttons["ESCAPE"] && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                instance.buttons["ESCAPE"] = true;
                return true;
            }
            return false;
        }
        public static bool BACKSPACE()
        {
            if (!instance.buttons["BACKSPACE"] && Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                instance.buttons["BACKSPACE"] = true;
                return true;
            }
            return false;
        }


        public static bool A(int playernum)
        {
            if (!instance.buttons["A" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.A == ButtonState.Pressed)
            {
                instance.buttons["A" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool B(int playernum)
        {
            if (!instance.buttons["B" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.B == ButtonState.Pressed)
            {
                instance.buttons["B" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool X(int playernum)
        {
            if (!instance.buttons["X" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.X == ButtonState.Pressed)
            {
                instance.buttons["X" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool Y(int playernum)
        {
            if (!instance.buttons["Y" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.Y == ButtonState.Pressed)
            {
                instance.buttons["Y" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool RB(int playernum)
        {
            if (!instance.buttons["RB" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.RightShoulder == ButtonState.Pressed)
            {
                instance.buttons["RB" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool LB(int playernum)
        {
            if (!instance.buttons["LB" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                instance.buttons["LB" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool BACK(int playernum)
        {
            if (!instance.buttons["BACK" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.Back == ButtonState.Pressed)
            {
                instance.buttons["BACK" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool START(int playernum)
        {
            if (!instance.buttons["START" + playernum] && GamePad.GetState(Input.translate(playernum)).Buttons.Start == ButtonState.Pressed)
            {
                instance.buttons["START" + playernum] = true;
                return true;
            }
            return false;
        }

        public static bool RT(int playernum)
        {
            if (!instance.buttons["RT" + playernum] && GamePad.GetState(Input.translate(playernum)).Triggers.Right > sensitivity)
            {
                instance.buttons["RT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool LT(int playernum)
        {
            if (!instance.buttons["LT" + playernum] && GamePad.GetState(Input.translate(playernum)).Triggers.Left > sensitivity)
            {
                instance.buttons["LT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool DUP(int playernum)
        {
            if (!instance.buttons["DUP" + playernum] && GamePad.GetState(Input.translate(playernum)).DPad.Up == ButtonState.Pressed)
            {
                instance.buttons["DUP" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool DDOWN(int playernum)
        {
            if (!instance.buttons["DDOWN" + playernum] && GamePad.GetState(Input.translate(playernum)).DPad.Down == ButtonState.Pressed)
            {
                instance.buttons["DDOWN" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool DLEFT(int playernum)
        {
            if (!instance.buttons["DLEFT" + playernum] && GamePad.GetState(Input.translate(playernum)).DPad.Left == ButtonState.Pressed)
            {
                instance.buttons["DLEFT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool DRIGHT(int playernum)
        {
            if (!instance.buttons["DRIGHT" + playernum] && GamePad.GetState(Input.translate(playernum)).DPad.Right == ButtonState.Pressed)
            {
                instance.buttons["DRIGHT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool LSTICKUP(int playernum)
        {
            if (!instance.buttons["LSTICKUP" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Left.Y > sensitivity)
            {
                instance.buttons["LSTICKUP" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool LSTICKDOWN(int playernum)
        {
            if (!instance.buttons["LSTICKDOWN" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Left.Y < -sensitivity)
            {
                instance.buttons["LSTICKDOWN" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool LSTICKLEFT(int playernum)
        {
            if (!instance.buttons["LSTICKLEFT" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Left.X < -sensitivity)
            {
                instance.buttons["LSTICKLEFT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool LSTICKRIGHT(int playernum)
        {
            if (!instance.buttons["LSTICKRIGHT" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Left.X > sensitivity)
            {
                instance.buttons["LSTICKRIGHT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool RSTICKUP(int playernum)
        {
            if (!instance.buttons["RSTICKUP" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Right.Y > sensitivity)
            {
                instance.buttons["RSTICKUP" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool RSTICKDOWN(int playernum)
        {
            if (!instance.buttons["RSTICKDOWN" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Right.Y < -sensitivity)
            {
                instance.buttons["RSTICKDOWN" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool RSTICKLEFT(int playernum)
        {
            if (!instance.buttons["RSTICKLEFT" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Right.X < -sensitivity)
            {
                instance.buttons["RSTICKLEFT" + playernum] = true;
                return true;
            }
            return false;
        }
        public static bool RSTICKRIGHT(int playernum)
        {
            if (!instance.buttons["RSTICKRIGHT" + playernum] && GamePad.GetState(Input.translate(playernum)).ThumbSticks.Right.X > sensitivity)
            {
                instance.buttons["RSTICKRIGHT" + playernum] = true;
                return true;
            }
            return false;
        }

    }
}
