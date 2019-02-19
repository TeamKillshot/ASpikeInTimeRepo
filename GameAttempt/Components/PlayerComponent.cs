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
		SpriteEffects s;
        ServiceManager serviceManager;
        CollisionDetection collision;

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
        PlayerState _current;

        public PlayerComponent(Game game): base(game)
        {
            GamePad.GetState(index);
            game.Components.Add(this);
            tiles = new TRender(game);
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
                        Game.Content.Load<Texture2D>("Sprites/SpikeSprSheet"), Position, 11, Bounds);
                    break;

                case PlayerIndex.One:
                    Sprite = new AnimatedSprite(Game, 
                        Game.Content.Load<Texture2D>("Sprites/SpikeSprSheet"), Position, 11, Bounds);
                    ID = 1;
                    break;

                case PlayerIndex.Two:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/SprSheet"), Position, 11, Bounds);
                    ID = 2;
                    break;

                case PlayerIndex.Three:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/SprSheet"), Position, 11, Bounds);
                    ID = 3;
                    break;

                case PlayerIndex.Four:
                    Sprite = new AnimatedSprite(Game,
                        Game.Content.Load<Texture2D>("Sprites/SprSheet"), Position, 11, Bounds);
                    ID = 4;
                    break;
            }

            PlayerRect = Game.Content.Load<Texture2D>("Sprites/Collison");
        }

        public override void Update(GameTime gameTime)
        {
            Camera camera = Game.Services.GetService<Camera>();
            previousPosition = Position;

            camera.FollowCharacter(Sprite.position, GraphicsDevice.Viewport);
            previousPosition = Sprite.position;
            Bounds = new Rectangle((int)Sprite.position.X, (int)Sprite.position.Y, 128, 128);
            GamePadState state = GamePad.GetState(index);
            //CollisionDetection.CheckCollision();

            var collisionSet = tiles.collisons.Where(c => c.collider.Intersects(Bounds)).ToList();

            foreach (Collider c in collisionSet)
            {
                c.collisionColor = Color.Red;

                if(collisionSet.Count <= 0)
                {
                    _current = PlayerState.FALL;
                    break;
                }
            }

            bool isJumping = false;
            bool isFalling = false;
            bool isCollided = false;

            switch (_current)
            {
                case PlayerState.FALL:

                    if (tiles.Collision())
                    {
                        //Sprite.position.Y -= 1;
                        _current = PlayerState.STILL;
                        break;
                    }
                    else if (!tiles.Collision())
                    {
                        Sprite.position.Y += 5;
                        //isFalling = false;
                        Sprite.position.X += state.ThumbSticks.Left.X * speed;
                    }
                    break;

                case PlayerState.STILL:
                    if (!tiles.Collision())
                    {
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
                    }
                    else if(tiles.Collision())
                    {
                        _current = PlayerState.FALL;
                    }
                    break;

                case PlayerState.WALK:

                    if (tiles.Collision())
                    {
                        //Sprite.position.Y -= 1;
                        _current = PlayerState.STILL;
                        break;
                    }
                    else if (!tiles.Collision())
                    {
                        Sprite.position.X += state.ThumbSticks.Left.X * speed;

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
                    }
                    break;

                case PlayerState.JUMP:
                    if (!tiles.Collision())
                    {
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
                    }
                    else if (tiles.Collision())
                    {
                        //Sprite.position.Y -= 1;
                        _current = PlayerState.FALL;
                        break;
                    }

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
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.sourceRectangle, Color.White, 0f, Vector2.Zero, s, 0f);
                    break;
                case PlayerState.JUMP:
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.sourceRectangle, Color.White, 0f, Vector2.Zero, s, 0f);
                    break;
                case PlayerState.WALK:
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.sourceRectangle, Color.White, 0f, Vector2.Zero, s, 0f);
                    break;
                case PlayerState.FALL:
                    spriteBatch.Draw(Sprite.SpriteImage, Sprite.BoundingRect, Sprite.sourceRectangle, Color.White, 0f, Vector2.Zero, s, 0f);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
