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
        Vector2 previousPosition { get; set; }
        AnimatedSprite Sprite { get; set; }
        public int ID { get; set; }
		SpriteEffects s;
        ServiceManager serviceManager;

        //variables
        int speed;
        TRender tiles;
        PlayerIndex index;
        Texture2D PlayerRect;
        Vector2 Position;
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

            serviceManager = new ServiceManager(Game);

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
                        Game.Content.Load<Texture2D>("Sprites/CharacterSpriteSheet"), Position, 11, Bounds);
                    break;

                case PlayerIndex.One:
                    Sprite = new AnimatedSprite(Game, 
                        Game.Content.Load<Texture2D>("Sprites/SprSheet"), Position, 11, Bounds);
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
            Camera camera = serviceManager.GetCameraService();

            camera.FollowCharacter(Sprite.position, GraphicsDevice.Viewport);
            previousPosition = Sprite.position;
            Bounds = new Rectangle((int)Sprite.position.X, (int)Sprite.position.Y, 128, 128);
            GamePadState state = GamePad.GetState(index);

            bool isJumping = false;
            bool isFalling = false;
            bool isColliding = false;

            switch (_current)
            {
                case PlayerState.FALL:

                    if (!isFalling)
                    {
                        Sprite.position.Y += 5;
                        isFalling = true;
                        Sprite.position.X += state.ThumbSticks.Left.X * speed;
                    }

                    if (isFalling)
                    {
                        foreach (Collider c in tiles.collisons)
                        {
                            Rectangle FloorRec = c.collider;

                            if (Bounds.Intersects(FloorRec))
                            {
                                Sprite.position = previousPosition;
                                isColliding = true;
                                _current = PlayerState.STILL;
                            }
                            else
                            {
                                c.collisionColor = Color.White;
                            }

                            if (_current != PlayerState.FALL) break;
                        }
                    }

                    break;

                case PlayerState.STILL:
                    if(sndWalkIns.State == SoundState.Playing)
                    {
                        sndWalkIns.Stop();
                    }
                    if (state.ThumbSticks.Left.X != 0)
                    {
                        _current = PlayerState.WALK;
                    }
                    if(InputManager.IsButtonPressed(Buttons.A))
                    {
                        _current = PlayerState.JUMP;
                    }
                    break;

                case PlayerState.WALK:
                    Sprite.position.X += state.ThumbSticks.Left.X * speed;
                    if(sndWalkIns.State != SoundState.Playing)
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
                        s = SpriteEffects.FlipHorizontally;
                    }
                    else s = SpriteEffects.None;
                    if (InputManager.IsButtonPressed(Buttons.A) && !isJumping && !isFalling)
                    {
                        _current = PlayerState.JUMP;
                    }
                    //if(!isColliding)
                    //{
                    //    _current = PlayerState.FALL;
                    //}
                    else if (isColliding)
                    {
                        _current = PlayerState.STILL;
                    }
                    break;

                case PlayerState.JUMP:
                    if(!isJumping)
                    {
                        Sprite.position.Y -= 100;
                        isJumping = true;

                        if(sndJumpIns.State != SoundState.Playing)
                        {
                            sndJumpIns.Play();
                            sndJump.Play();
                        }
                        else if(InputManager.IsButtonReleased(Buttons.A))
                        {
                            sndJumpIns.Stop();
                        }
                        _current = PlayerState.FALL;
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
