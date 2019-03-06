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
        public SpriteEffects _effect;
        //the source of our image within the sprite sheet to draw
        public Rectangle WalkSource; // For Walk Anim
        public Rectangle FallSource; // For Jumping and Falling
        public Rectangle StillSource; // For idle
        
        public Rectangle BoundingRect;

        // Variable added to only all sound to play on initial collision
        // maintains the state of collision of the sprite over the course of updates
        // Set in collision detection function
        public List<Rectangle> WalkAnim = new List<Rectangle>();
        public List<Rectangle> AnimList = new List<Rectangle>();
        public bool InCollision = false;

        public AnimatedSprite(Game game, Texture2D texture, Vector2 userPosition, int tsRows, int framecount, Rectangle bounds) : base(game)
        {
            game.Components.Add(this);
            spriteImage = texture;
            position = userPosition;
            numberOfFrames = framecount;
            spriteHeight = spriteImage.Height / tsRows;
            spriteWidth = spriteImage.Width / framecount;
            _effect = SpriteEffects.None;
            BoundingRect = bounds;
            for (int i = 0; i <= 10; i++)
                AnimList.Add(new Rectangle(i * spriteWidth, 8 * spriteHeight, spriteWidth, spriteHeight));
            
            foreach (Rectangle rect in AnimList)
            {
                if (rect.X > 4 * spriteWidth)
                    WalkAnim.Add(rect);
            }
         
        }


        public override void Update(GameTime gametime)
        {
            

            StillSource = AnimList.Find(a => a.X == 0);
            FallSource = AnimList.Find(a => a.X == 2 * spriteWidth);

            foreach (Rectangle rect in WalkAnim)
            {
                WalkSource = rect;
            }

            BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);

        }

       
    }
}
