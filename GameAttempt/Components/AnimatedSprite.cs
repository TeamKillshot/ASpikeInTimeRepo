using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameAttempt.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine;

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
        int currentFrame = 0;
        int mililsecondsBetweenFrames = 100;
        float timer = 0f;

        //the width and height of our texture
        protected int spriteWidth = 0;
        public int SpriteWidth
        {
            get { return spriteWidth; }
            set { spriteWidth = value; }
        }


        protected int spriteHeight = 0;
        public int SpriteHeight
        {
            get { return spriteHeight; }
            set { spriteHeight = value; }
        }

        //the source of our image within the sprite sheet to draw
       
        public Rectangle WalkSource;
        public Rectangle JumpSource;
        public Rectangle StillSource;
        public Rectangle FallSource;
        public Rectangle BoundingRect;

        public SpriteEffects _effect;
        public List<Rectangle> animList = new List<Rectangle>();
        public List<Rectangle> WalkAnim = new List<Rectangle>();
       
        // Variable added to only all sound to play on initial collision
        // maintains the state of collision of the sprite over the course of updates
        // Set in collision detection function
        public bool InCollision = false;
        public AnimatedSprite(Game game, Texture2D texture, Vector2 userPosition, int tsHeight, int framecount, Rectangle bounds) : base(game)
        {
            game.Components.Add(this);
            spriteImage = texture;
            position = userPosition;
            numberOfFrames = framecount;
            spriteHeight = spriteImage.Height / tsHeight;
            spriteWidth = spriteImage.Width / framecount;

        }


        public override void Update(GameTime gametime)
        {
            for (int x = 0; x <= 11; x++)
                if (animList.Count() <= 12)
                    animList.Add(new Rectangle(x * spriteWidth, 8 * spriteHeight, spriteWidth, spriteHeight));

            int y = 0;
            //set the source to be the current frame in our animation
            foreach (Rectangle rect in animList)
            {
                if (y > 4)
                    WalkAnim.Add(rect);     // Splits Walk Anim from full tile sheet;               
                if(y == 0)
                    StillSource = rect;
                if(y == 2)
                    FallSource = rect;
                y++;
            }

            
            


            BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);
      
        }
        public bool collisionDetect(AnimatedSprite otherSprite)
        {
            BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);
            Rectangle otherBound = new Rectangle((int)otherSprite.position.X, (int)otherSprite.position.Y, otherSprite.spriteWidth, this.spriteHeight);
            if (BoundingRect.Intersects(otherBound))
            {
                InCollision = true;
                return true;
            }
            else
            {
                InCollision = false;
                return false;
            }
        }
       
    }
}
