using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tuto02_Mouvement.Models;
using Tuto02_Mouvement.Sprites;

namespace Tuto02_Mouvement
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Health display
        private Rectangle _bgHealthBar;
        private Texture2D _bgHealthBarTexture;
        private Rectangle _healthBar;
        private Texture2D _healthBarTexture;
        private SpriteFont _healthFont;
        private SpriteFont _healthFont2;

        Character character;
        //private List<Character> _characters;

        MouseState pastMouse;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;

            //Health display
            _healthFont = Content.Load<SpriteFont>("Health/Health");
            _healthFont2 = Content.Load<SpriteFont>("Health/Health2");
            _healthBarTexture = Content.Load<Texture2D>("Health/Life");
            _bgHealthBarTexture = Content.Load<Texture2D>("Health/bg");

            // TODO: use this.Content to load your game content here
            #region version standard
            //_characters = new List<Character>();
            var animations = new Dictionary<string, Animation>();
            animations.Add("WalkUp", new Animation(Content.Load<Texture2D>("Player/WalkUp"), 3));
            animations.Add("WalkDown", new Animation(Content.Load<Texture2D>("Player/WalkDown"), 3));
            animations.Add("WalkLeft", new Animation(Content.Load<Texture2D>("Player/WalkLeft"),3));
            animations.Add("WalkRight", new Animation(Content.Load<Texture2D>("Player/WalkRight"),3));
            animations.Add("SquatRight", new Animation(Content.Load<Texture2D>("Player/SquatRight"),4));
            animations.Add("SquatLeft", new Animation(Content.Load<Texture2D>("Player/SquatLeft"),4));

            //var character = new Character(100, 1, 50, 50, 10, animations);
            character = new Character(100, 1, 50, 50, 10, animations);
            character.Input = new Input();
            character.Input.Up = Keys.W;
            character.Input.Down = Keys.S;
            character.Input.Left = Keys.A;
            character.Input.Right = Keys.D;
            character.Input.Jump = Keys.Space;
            character.Input.Eat = Keys.E;
           // _characters.Add(character);

            #endregion version standard
            #region version opti
           /* _characters = new List<Character>()
            {
                new Character(100,1,50,50,10,new Dictionary<string, Animation>(){
                                { "WalkUp", new Animation(Content.Load<Texture2D>("Player/WalkUp"),3) },
                                { "WalkDown", new Animation(Content.Load<Texture2D>("Player/WalkDown"),3) },
                                { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/WalkLeft"),3) },
                                { "WalkRight", new Animation(Content.Load<Texture2D>("Player/WalkRight"),3) },
                                { "SquatRight", new Animation(Content.Load<Texture2D>("Player/SquatRight"),4) },
                                { "SquatLeft", new Animation(Content.Load<Texture2D>("Player/SquatLeft"),4) },
                })
                {
                    Input = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Jump = Keys.Space,
                        Eat = Keys.E
                    },
                }                
            };*/
            #endregion version opti  
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            
            _bgHealthBar = new Rectangle(10, 10, _bgHealthBarTexture.Width, _bgHealthBarTexture.Height/2);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //foreach (var character in _characters)
            //{
                _healthBar = new Rectangle(10, 10, _healthBarTexture.Width * (character.CurrentHealth*100/character.MaxHealth)/100, _healthBarTexture.Height/2);
                //health gestion
                if (mouseRectangle.Intersects(character.Rectangle) && mouse.LeftButton == ButtonState.Pressed && pastMouse.LeftButton == ButtonState.Released)
                    character.CurrentHealth -= 10;
                else if (mouseRectangle.Intersects(character.Rectangle) && mouse.RightButton == ButtonState.Pressed && pastMouse.LeftButton == ButtonState.Released)
                    character.CurrentHealth = character.MaxHealth;

                character.Update(gameTime);


                character.Position = new Vector2(Math.Min(Math.Max(0, character.Position.X), graphics.PreferredBackBufferWidth - character.AnimationManager.Animation.FrameWidth),
                                                 Math.Min(Math.Max(0, character.Position.Y), graphics.PreferredBackBufferHeight - character.AnimationManager.Animation.FrameHeight));
                
            //}

            pastMouse = mouse;


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(_bgHealthBarTexture,_bgHealthBar,Color.White);
            spriteBatch.Draw(_healthBarTexture, _healthBar,Color.White);
           // foreach (var character in _characters)
           //{
                string pv = character.CurrentHealth + " / " + character.MaxHealth;
                string info = "Cliquez sur moi!\n";
                spriteBatch.DrawString(_healthFont, pv , new Vector2(_bgHealthBar.Width / 2 - _healthFont.MeasureString(pv).X / 2, _bgHealthBar.Height / 2 ), Color.Black);
                spriteBatch.DrawString(_healthFont2, info , new Vector2(character.Position.X- character.Rectangle.Width - _healthFont.MeasureString(info).Y/2 + 10, character.Position.Y - _healthFont.MeasureString(info).Y), Color.Black);
                spriteBatch.DrawString(_healthFont2, pv, new Vector2(character.Position.X - character.Rectangle.Width, character.Position.Y - _healthFont.MeasureString(info).Y + _healthFont.MeasureString(info).Y/2), Color.Black);
                character.Draw(spriteBatch);
           // }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
