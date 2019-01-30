using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Managers
{
    public sealed class InputManager : GameComponent
    {
        private static GamePadState previousPadState;
        private static GamePadState currentPadState;

        private static GamePadState previousP2PadState;
        private static GamePadState currentP2PadState;

        private static KeyboardState previousKeyState;
        private static KeyboardState currentKeyState;

        //private static Vector2 previousMousePos;
        //private static Vector2 currentMousePos;

        private static MouseState previousMouseState;
        private static MouseState currentMouseState;

        public InputManager(Game _game)
            : base(_game)
        {
            currentPadState = GamePad.GetState(PlayerIndex.One);
            currentP2PadState = GamePad.GetState(PlayerIndex.Two);
            currentKeyState = Keyboard.GetState();

            _game.Components.Add(this);
        }

        public static void ClearState()
        {
            previousMouseState = Mouse.GetState();
            currentMouseState = Mouse.GetState();
            previousKeyState = Keyboard.GetState();
            currentKeyState = Keyboard.GetState();
        }

        public override void Update(GameTime gametime)
        {
            previousPadState = currentPadState;
            previousP2PadState = currentP2PadState;
            previousKeyState = currentKeyState;

            currentPadState = GamePad.GetState(PlayerIndex.One);
            currentP2PadState = GamePad.GetState(PlayerIndex.Two);
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            //currentMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            currentMouseState = Mouse.GetState();

            base.Update(gametime);
        }

        public static bool IsButtonPressed(Buttons buttonToCheck)
        {
            if (currentPadState.IsButtonDown(buttonToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static bool IsButtonReleased(Buttons buttonToCheck)
        {
            if (CurrentPadState.IsButtonUp(buttonToCheck)) return true;
            else return false;
        }

        public static bool IsButtonHeld(Buttons buttonToCheck)
        {
            if (currentPadState.IsButtonDown(buttonToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsKeyHeld(Keys buttonToCheck)
        {
            if (currentKeyState.IsKeyDown(buttonToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsKeyPressed(Keys keyToCheck)
        {
            if (currentKeyState.IsKeyUp(keyToCheck) && previousKeyState.IsKeyDown(keyToCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static GamePadState CurrentPadState
        {
            get { return currentPadState; }
            set { currentPadState = value; }
        }
        public static GamePadState CurrentP2PadState
        {
            get { return currentP2PadState; }
            set { currentP2PadState = value; }
        }
        public static KeyboardState CurrentKeyState
        {
            get { return currentKeyState; }
        }

        public static MouseState CurrentMouseState
        {
            get { return currentMouseState; }
        }

        public static MouseState PreviousMouseState
        {
            get { return previousMouseState; }
        }

        public static bool IsMouseLeftClick()
        {
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else 
                return false;
        }

        public static bool IsMouseRightClick()
        {
            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsMouseRightHeld()
        {
            if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public static bool IsMouseLeftHeld()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //public static Vector2 MousePosition
        //{
        //    get { return currentMousePos; }
        //}

    }
}
