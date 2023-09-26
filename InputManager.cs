using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeClimber
{
    internal class InputManager
    {
        // Stores return values for each possible input.
        public static bool UpPress = false;
        public static bool DownPress = false;
        public static bool LeftPress = false;
        public static bool RightPress = false;
        public static bool EnterPress = false;
        public static bool PlusPress = false;
        public static bool MinusPress = false;
        public static bool UpHold = false;
        public static bool DownHold = false;
        public static bool LeftHold = false;
        public static bool RightHold = false;
        public static bool EnterHold = false;
        public static bool PlusHold = false;
        public static bool MinusHold = false;
        // E and R inputs do not have press functions!
        public static bool EHold = false;
        public static bool RHold = false;

        // Holds the info of the previous frame, extremely useful for condensing "press" functions.
        static bool prev1 = false;
        static bool prev2 = false;
        static bool prev3 = false;
        static bool prev4 = false;
        static bool prev5 = false;
        static bool prev6 = false;
        static bool prev7 = false;

        // NOT USED May ever so slightly increases response speed.
        static bool StoresUpInput = false;
        static bool StoresDownInput = false;
        static bool StoresLeftInput = false;
        static bool StoresRightInput = false;
        static bool StoresEInput = false;
        static bool StoresRInput = false;

        public static void Update()
        {
            KeyboardState _K = Keyboard.GetState();
            GamePadState _G = GamePad.GetState(PlayerIndex.One);

            // UP INPUTS
            if (_K.IsKeyDown(Keys.Up) || _K.IsKeyDown(Keys.W) || _G.Buttons.X == ButtonState.Pressed)
            {
                UpHold = true;
                UpPress = !prev1;
            }
            else
            {
                UpHold = false;
                UpPress = false;
            }
            prev1 = _K.IsKeyDown(Keys.Up) || _K.IsKeyDown(Keys.W) || _G.Buttons.X == ButtonState.Pressed;

            // DOWN INPUTS
            if (_K.IsKeyDown(Keys.Down) || _K.IsKeyDown(Keys.S) || _G.Buttons.B == ButtonState.Pressed)
            {
                DownHold = true;
                DownPress = !prev2;
            }
            else
            {
                DownHold = false;
                DownPress = false;
            }
            prev2 = _K.IsKeyDown(Keys.Down) || _K.IsKeyDown(Keys.S) || _G.Buttons.B == ButtonState.Pressed;

            // LEFT INPUTS
            if (_K.IsKeyDown(Keys.Left) || _K.IsKeyDown(Keys.A) || _G.Buttons.Y == ButtonState.Pressed)
            {
                LeftHold = true;
                LeftPress = !prev3;
            }
            else
            {
                LeftHold = false;
                LeftPress = false;
            }
            prev3 = _K.IsKeyDown(Keys.Left) || _K.IsKeyDown(Keys.A) || _G.Buttons.Y == ButtonState.Pressed;

            // RIGHT INPUTS
            if (_K.IsKeyDown(Keys.Right) || _K.IsKeyDown(Keys.D) || _G.Buttons.A == ButtonState.Pressed)
            {
                RightHold = true;
                RightPress = !prev4;
            }
            else
            {
                RightHold = false;
                RightPress = false;
            }
            prev4 = _K.IsKeyDown(Keys.Right) || _K.IsKeyDown(Keys.D) || _G.Buttons.A == ButtonState.Pressed;

            // ENTER INPUTS
            if (_K.IsKeyDown(Keys.Enter))
            {
                EnterHold = true;
                EnterPress = !prev5;
            }
            else
            {
                EnterHold = false;
                EnterPress = false;
            }
            prev5 = _K.IsKeyDown(Keys.Enter);

            // PLUS KEY INPUTS
            if (_K.IsKeyDown(Keys.OemPlus))
            {
                PlusHold = true;
                PlusPress = !prev6;
            }
            else
            {
                PlusHold = false;
                PlusPress = false;
            }
            prev6 = _K.IsKeyDown(Keys.OemPlus);

            // MINUS KEY INPUTS
            if (_K.IsKeyDown(Keys.OemMinus))
            {
                MinusHold = true;
                MinusPress = !prev7;
            }
            else
            {
                MinusHold = false;
                MinusPress = false;
            }
            prev7 = _K.IsKeyDown(Keys.OemMinus);

            // E
            if (_K.IsKeyDown(Keys.E))
            {
                EHold = true;
            }
            else
            {
                EHold = false;
            }

            // R
            if (_K.IsKeyDown(Keys.R))
            {
                RHold = true;
            }
            else
            {
                RHold = false;
            }
        }
    }
}
