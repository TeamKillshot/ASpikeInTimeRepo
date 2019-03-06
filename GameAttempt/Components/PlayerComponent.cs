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
        //ServiceManager serviceManager;

        //variables
        int speed;
        TRender tiles;
        PlayerIndex index;
        Texture2D PlayerRect;
        public Vector2 previousPosition;
        public Vector2 Position;
        public Rectangle Bounds;
        //Camera camera;

        SoundEffect sndJump, sndWalk, sndWalk2;
        SoundEffectInstance sndJumpIns, sndWalkIns, sndWalkIns2;

        //PlayerStates
        public enum PlayerState { STILL, WALK, JUMP, FALL }
        public PlayerState _current;

        public PlayerComponent(Game game): base(game)
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

            PlayerRect = Game.Content.Load<Texture2D>("Sprites/Collison");
        }

        public override void Update(GameTime gameTime)
        {
            Camera camera = Game.Services.GetService<Camera>();

            camera.FollowCharacter(Sprite.position, GraphicsDevice.Viewport);
            Bounds = new Rectangle((int)Sprite.position.X, (int)Sprite.position.Y, 128, 128);
            GamePadState state = GamePad.GetState(index);

            previousPosition = Sprite.position - new Vector2(0,3f);
            //all tiles that are in collision with player bounds
            var collisionSet = tiles.collisons.Where(c => c.collider.Intersects(Bounds)).ToList();

            for (int i = 0; i < collisionSet.Count - 1; i++)
            {
                //change color of collider rectangle to red
                collisionSet[i].collisionColor = Color.Red;

                //distance between tiles collider top and player bounds bottom
                float distance = Bounds.Bottom - collisionSet[i].collider.Top;

                if (distance > 0)
                {
                    //move player back up
                    Sprite.position.Y = previousPosition.Y;
                    _current = PlayerState.STILL;
                    break;
                }
                else
                {
                    _current = PlayerState.FALL;
                }

                if (collisionSet[i].collider.Top <= Bounds.Bottom)
                {
                    Sprite.position = previousPosition;
                    _current = PlayerState.STILL;
                    break;
                }
                else
                {
                    collisionSet[i].collisionColor = Color.White;

                    _current = PlayerState.FALL;
                }

                if (collisionSet[i].collider.Right <= Bounds.Left || collisionSet[i].collider.Left >= Bounds.Right)
                {
                    Sprite.position.X = previousPosition.X;
                    _current = PlayerState.STILL;
                }
                break;
            }

            bool isJumping = false;
            bool isFalling = false;

            switch (_current)
            {
                case PlayerState.FALL:

                    Sprite.position.Y += 5;
                    Sprite.position.X += state.ThumbSticks.Left.X * speed;

                break;

                case PlayerState.STILL:

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
                    //_current = PlayerState.FALL;
                    break;

                case PlayerState.WALK:

                    Sprite.position.X += state.ThumbSticks.Left.X * speed;

                    //if(collisionSet)

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

            #region Uneeded?
            //        if (InputManager.IsKeyHeld(Keys.A))
            //        {
            //s = SpriteEffects.None;
            //            Position -= new Vector2(9, 0);
            //            _current = PlayerState.WALK;
            //        }
            //        if (InputManager.IsKeyHeld(Keys.D))
            //        {
            //s = SpriteEffects.FlipHorizontally;
            //            Position += new Vector2(9, 0);
            //            _current = PlayerState.WALK;
            //        }

            #endregion

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            Camera Cam = Game.Services.GetService<Camera>();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);
            switch(_current)
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
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
