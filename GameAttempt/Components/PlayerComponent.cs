using Components;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt.Components
{
    public class PlayerComponent : DrawableGameComponent
    {
        //Properties
        AnimatedSprite Sprite { get; set; }
        public int ID { get; set; }

        //variables for collision handling
        Rectangle collisionRect;
        float distance;
        float playerRightSideDistance;
        float playerLeftSideDistance;

        //variables for player position and drawing
        public Vector2 previousPosition;
        public Vector2 Position;
        public Rectangle Bounds;

        //Sounds
        SoundEffect sndJump, sndWalk, sndWalk2;
        SoundEffectInstance sndJumpIns, sndWalkIns, sndWalkIns2;

        //other variables
        SpriteFont font;
        int speed;
        TRender tiles;
        PlayerIndex index;

        //Temp variables to check size and dimensions of the players bounds
        Texture2D TempText;

        //PlayerStates
        public enum PlayerState { STILL, WALK, JUMP, FALL }
        public PlayerState _current;

        public PlayerComponent(Game game) : base(game)
        {
            GamePad.GetState(index);
            game.Components.Add(this);
            tiles = new TRender(game);
            DrawOrder = 1;
        }

        public override void Initialize()
        {
            Position = new Vector2(200, 300);
            speed = 9;
            ID = (int)index;
            _current = PlayerState.FALL;

            //serviceManager = new ServiceManager(Game);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            TempText = Game.Content.Load<Texture2D>("Sprites/Collison");

            #region Load In Audio and Font

            //Audio Load
            sndJump = Game.Content.Load<SoundEffect>("Audio/jump_snd");
            sndJumpIns = sndJump.CreateInstance();
            sndJumpIns.Volume = 1.0f;

            sndWalk = Game.Content.Load<SoundEffect>("Audio/step_snd");
            sndWalkIns = sndWalk.CreateInstance();
            sndWalkIns.Volume = 1.0f;

            sndWalk2 = Game.Content.Load<SoundEffect>("Audio/step_snd");
            sndWalkIns2 = sndWalk2.CreateInstance();
            sndWalkIns2.Volume = 1.0f;

            font = Game.Content.Load<SpriteFont>("font");

            #endregion

            #region Load In Sprites and set Index

            switch (index)
            {
                default:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/TileSheet3"), Position, 15, 11, Bounds);
                    break;

                case PlayerIndex.One:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/TileSheet3"), Position, 15, 11, Bounds);
                    ID = 1;
                    break;

                case PlayerIndex.Two:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/TileSheet3"), Position, 15, 11, Bounds);
                    ID = 2;
                    break;

                case PlayerIndex.Three:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/TileSheet3"), Position, 15, 11, Bounds);
                    ID = 3;
                    break;

                case PlayerIndex.Four:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/TileSheet3"), Position, 15, 11, Bounds);
                    ID = 4;
                    break;
            }

            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            bool hasCollided = false;

            Camera camera = Game.Services.GetService<Camera>();
            if (_current == PlayerState.WALK || _current == PlayerState.STILL || _current == PlayerState.FALL)
            {
                camera.FollowCharacter(Sprite.position, GraphicsDevice.Viewport);
            }
            else if(!hasCollided)
            {
                camera.FollowCharacter(Sprite.position, GraphicsDevice.Viewport);
            }

            Bounds = new Rectangle((int)Sprite.position.X, (int)Sprite.position.Y, Sprite.SpriteWidth, Sprite.SpriteHeight);
            collisionRect = new Rectangle(Bounds.Location.X, Bounds.Location.Y, Bounds.Width, Bounds.Height + 5);
            GamePadState state = GamePad.GetState(index);

            previousPosition = Sprite.position;

            var newCollisions = tiles.collisons.Where(c => c.collider.Intersects(collisionRect)).ToList();
            //all tiles that are in collision with player bounds
            var collisionSet = tiles.collisons.Where(c => c.collider.Intersects(Bounds)).ToList();

            //Booleans to help the switch statements with decisions
            bool isJumping = false;
            bool isFalling = false;
            bool hasCollidedBottom = false;

            //Switch statement to check Players State within the game
            switch (_current)
            {
                case PlayerState.FALL:

                    //Move player down by 5 every frame to simulate falling & Check thumbstick position
                    Sprite.position.Y += 5;
                    Sprite.position.X += state.ThumbSticks.Left.X * speed;

                    //Check for collisions between the top and bottom of the rectangles
                    for (int i = 0; i < newCollisions.Count - 1; i++)
                    {
                        distance = collisionRect.Bottom - newCollisions[i].collider.Top;

                        if (distance > 0)
                        {
                            //move the player back horizontally to it's previous position
                            //set the boolean to true (to say the player has something to stand on)
                            //set the players state
                            Sprite.position.Y = previousPosition.Y;
                            hasCollidedBottom = true;
                            _current = PlayerState.STILL;
                        }
                        else hasCollidedBottom = false;
                    }

                    break;

                case PlayerState.STILL:
                    //stop the walk sound if it's playing
                    if (sndWalkIns.State == SoundState.Playing)
                    {
                        sndWalkIns.Stop();
                    }
                    if (state.ThumbSticks.Left.X != 0)
                    {
                        _current = PlayerState.WALK;
                    }
                    if (InputManager.IsButtonPressed(Buttons.A))
                    {
                        _current = PlayerState.JUMP;
                    }
                    break;

                case PlayerState.WALK:

                    Sprite.position.X += state.ThumbSticks.Left.X * speed;

                    if (newCollisions.Count <= 0)
                    {
                        //if it's not colliding with anything, make the player fall
                        _current = PlayerState.FALL;
                        break;
                    }

                    for (int i = 0; i < collisionSet.Count - 1; i++)
                    {
                        //Check the distance between the two rectangles ahead of time
                        playerRightSideDistance = Bounds.Left - collisionSet[i].collider.Left;
                        playerLeftSideDistance = Bounds.Left - collisionSet[i].collider.Right;

                        //colliding from left
                        if (playerRightSideDistance >= 1 && playerLeftSideDistance <= 191)
                        {
                            hasCollided = true;
                            //bounce the player backwards from the thing it's colliding with
                            Sprite.position.X = previousPosition.X + 25;
                            _current = PlayerState.STILL;
                            //Check to see if the Player has soemthing to stand on
                            if (hasCollidedBottom == false)
                            {
                                //need to change this so the fall state occurs but it doesnt change the draw
                                _current = PlayerState.FALL;
                            }
                            break;
                        }
                        else if (playerLeftSideDistance <= 0 && playerRightSideDistance <= 0)
                        {
                            hasCollided = true;
                            Sprite.position.X = previousPosition.X - 25;
                            _current = PlayerState.STILL;
                            //Check to see if the Player has something to stand on
                            if (hasCollidedBottom == false)
                            {
                                _current = PlayerState.FALL;
                            }

                            break;
                        }
                    }

                    if (sndWalkIns.State != SoundState.Playing)
                    {
                        sndWalkIns.Play();
                        //sndWalkIns.IsLooped = true;
                    }

                    if (state.ThumbSticks.Left.X == 0)
                    {
                        _current = PlayerState.STILL;
                    }
                    if (state.ThumbSticks.Left.X > 0)
                    {
                        tiles.effect = SpriteEffects.FlipHorizontally;
                    }
                    else tiles.effect = SpriteEffects.None;
                    if (InputManager.IsButtonPressed(Buttons.A) && !isJumping && !isFalling)
                    {
                        _current = PlayerState.JUMP;
                    }
                    break;

                case PlayerState.JUMP:

                    if (!isJumping)
                    {
                        Sprite.position.Y -= 120;
                        Sprite.position.X += state.ThumbSticks.Left.X * speed;
                        isJumping = true;
                        _current = PlayerState.FALL;
                    }

                    if (sndJumpIns.State != SoundState.Playing)
                    {
                        sndJumpIns.Play();
                        sndJump.Play();
                    }
                    else if (InputManager.IsButtonPressed(Buttons.A))
                    {
                        sndJumpIns.Stop();
                    }

                    //Sprite.position.Y -= 1;
                    //_current = PlayerState.FALL;

                    break;
            }

            playerLeftSideDistance += 0;
            playerRightSideDistance += 0;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            Camera Cam = Game.Services.GetService<Camera>();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);
            //spriteBatch.Draw(TempText, Bounds, Color.Black);
            switch (_current)
            {
                case PlayerState.STILL:
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.StillSource, Color.White, 0f, Vector2.Zero, tiles.effect, 0f);
                    break;
                case PlayerState.JUMP:
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.FallSource, Color.White, 0f, Vector2.Zero, tiles.effect, 0f);
                    break;
                case PlayerState.WALK:

                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.WalkSource, Color.White, 0f, Vector2.Zero, tiles.effect, 0f);
                    break;
                case PlayerState.FALL:
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.FallSource, Color.White, 0f, Vector2.Zero, tiles.effect, 0f);
                    break;
            }
            spriteBatch.DrawString(font, _current.ToString(), new Vector2(100, 200), Color.Black);
            spriteBatch.DrawString(font, Bounds.ToString(), new Vector2(100, 220), Color.Black);
            spriteBatch.DrawString(font, collisionRect.ToString(), new Vector2(400, 220), Color.Black);
            spriteBatch.DrawString(font, distance.ToString(), new Vector2(100, 240), Color.Black);
            spriteBatch.DrawString(font, playerRightSideDistance.ToString(), new Vector2(220, 240), Color.Black);
            spriteBatch.DrawString(font, playerLeftSideDistance.ToString(), new Vector2(340, 240), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
