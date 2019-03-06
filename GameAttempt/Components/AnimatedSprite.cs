using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameAttempt.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Components
{
    public class AnimatedSprite : DrawableGameComponent
    {
        //sprite texture and position
        Texture2D spriteImage;

        public Texture2D SpriteImage
        {
            get { return spriteImage; }
            set { spriteImage = value; }
        }
        public Vector2 position;

        //the number of frames in the sprite sheet
        //the current fram in the animation
        //the time between frames
        int numberOfFrames = 0;
        int currentFrame = 1;
        int tSLFrame = 0;
        int milliPerFrame = 60;

        //the width and height of our texture
        protected int spriteWidth = 0;

        public int SpriteWidth
        {
            get { return spriteWidth; }
            set { spriteWidth = value; }
        }
        int spriteHeight = 0;

        public int SpriteHeight
        {
            get { return spriteHeight; }
            set { spriteHeight = value; }
        }
       
        //the source of our image within the sprite sheet to draw
        public Rectangle WalkSource; // For Walk Anim
        public Rectangle FallSource; // For Jumping and Falling
        public Rectangle StillSource; // For idle        
        public Rectangle BoundingRect; // Storage For Bounds

        // Variable added to only all sound to play on initial collision
        // maintains the state of collision of the sprite over the course of updates
        // Set in collision detection function
        public List<Rectangle> WalkAnim = new List<Rectangle>(); // List For Walk Animation
        public List<Rectangle> AnimList = new List<Rectangle>(); // List For All Animations
        
        public AnimatedSprite(Game game, Texture2D texture, Vector2 userPosition, int tsRows, int framecount, Rectangle bounds) : base(game)
        {
            game.Components.Add(this);  // Add Animations to Components
            spriteImage = texture;      // Store Texture of Character
            position = userPosition;    // the Position of the Character for Bounds
            numberOfFrames = framecount;    // Frames of the SpriteSheet
            spriteHeight = spriteImage.Height / tsRows; // Gets the Height of the Sprite
            spriteWidth = spriteImage.Width / framecount;   // Gets the Width of the Sprite            
            BoundingRect = bounds;  // Bounds for Rect

            // Add All Frames into list
            for (int i = 0; i <= 10; i++) 
                AnimList.Add(new Rectangle(i * spriteWidth, 8 * spriteHeight, spriteWidth, spriteHeight));
            

            // Select the Frames From AnimList that are the Walk Animation
            foreach (Rectangle rect in AnimList)
            {
                if (rect.X > 4 * spriteWidth)
                    WalkAnim.Add(rect);
            }

        }


        public override void Update(GameTime gametime)
        {
            // Set The Time Since Last Frame to Game time 
            tSLFrame += gametime.ElapsedGameTime.Milliseconds;

            // if the time is greater than the time per frame update the frame counter
            if (tSLFrame > milliPerFrame)
            {
                tSLFrame -= milliPerFrame;
                currentFrame++;
            }
            
            // Set the Source for idle
            StillSource = AnimList.Find(a => a.X == 0);
            // Set the Source for Fall
            FallSource = AnimList.Find(a => a.X == 2 * spriteWidth);

            // Set the Source for Walking
            WalkSource = WalkAnim[currentFrame];
            
            // if the frame Counter is out of bounds reset
            if (currentFrame == WalkAnim.Count() - 1)
            {
                currentFrame = 1;
            }
            
            // Set the Bounds for the Animation
            BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight); 

        }

       
    }
}
