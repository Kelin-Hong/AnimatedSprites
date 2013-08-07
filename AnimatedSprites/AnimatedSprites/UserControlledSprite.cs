using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Devices.Sensors;
namespace AnimatedSprites
{
    class UserControlledSprite : Sprite
    {
        // COMMENTED-OUT MOUSE SUPPORT
        // MouseState prevMouseState;
        Accelerometer accelerometer;
        Vector3 vector3;
        // Get direction of sprite based on player input and speed
        public override Vector2 direction
        {
            get
            {
               vector3 = accelerometer.CurrentValue.Acceleration;
                
                Vector2 inputDirection = Vector2.Zero;
                if (vector3.X > -0.3) inputDirection.Y = -1;
                if (vector3.X < -0.5) inputDirection.Y = 1;
                if (vector3.Y > 0.1) inputDirection.X = -1;
                if (vector3.Y < -0.1) inputDirection.X = 1;
                //if(vector3.X>-0.4&&Math.Abs(vector3.X+0.4)>Math.Abs(vector3.Y)+0.07)
                //    inputDirection.Y = -1;
                //if (vector3.X < -0.4 && Math.Abs(vector3.X +0.4) > Math.Abs(vector3.Y)+0.07)
                //    inputDirection.Y =1;
                //if(vector3.Y>0&&Math.Abs(vector3.Y)>Math.Abs(vector3.X +0.4)+0.07)
                //    inputDirection.X =-1;
                //if (vector3.Y < 0 && Math.Abs(vector3.Y) > Math.Abs(vector3.X +0.4)+0.07)
                //    inputDirection.X =1;
                // If player pressed arrow keys, move the sprite
                //if (Keyboard.GetState().IsKeyDown(Keys.Left))
                //    inputDirection.X -= 1;
                //if (Keyboard.GetState().IsKeyDown(Keys.Right))
                //    inputDirection.X += 1;
                //if (Keyboard.GetState().IsKeyDown(Keys.Up))
                //    inputDirection.Y -= 1;
                //if (Keyboard.GetState().IsKeyDown(Keys.Down))
                //    inputDirection.Y += 1;

                // If player pressed the gamepad thumbstick, move the sprite
                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                if (gamepadState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;

                return inputDirection * speed;
            }
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, null, 0)
        {
            accelerometer = new Accelerometer();
            accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
            accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            accelerometer.Start();
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, null, 0)
        {
            accelerometer = new Accelerometer();
            accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
            accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            accelerometer.Start();
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            vector3=e.SensorReading.Acceleration;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            position += direction;

            // COMMENTED-OUT MOUSE SUPPORT
            //
            //// If player moved the mouse, move the sprite
            //MouseState currMouseState = Mouse.GetState();
            //if (currMouseState.X != prevMouseState.X ||
            //    currMouseState.Y != prevMouseState.Y)
            //{
            //    position = new Vector2(currMouseState.X, currMouseState.Y);
            //}
            //prevMouseState = currMouseState;

            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X*0.4f)
                position.X = clientBounds.Width - frameSize.X*0.4f;
            if (position.Y > clientBounds.Height - frameSize.Y * 0.4f)
                position.Y = clientBounds.Height - frameSize.Y * 0.4f;

            base.Update(gameTime, clientBounds);
        }
    }
}
