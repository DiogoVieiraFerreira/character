using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuto02_Mouvement.Managers;
using Tuto02_Mouvement.Models;

namespace Tuto02_Mouvement.Sprites
{
    public class Sprite
    {
        #region Fields

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        protected Vector2 _position;

        protected Texture2D _texture;
        protected Vector2 LastPosition { get; set; }

        #endregion

        #region Properties
        public Rectangle Rectangle { get; set; }
        /// <summary>
        /// Define or get if your sprite jump
        /// </summary>
        public bool JumpOk { get; set; }
        public bool Jump { get; set; }
        public float JumpSpeed { get; set; }

        private bool _hasJumped;

        public Input Input;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 1f;

        public Vector2 Velocity;

        #endregion

        #region Methods

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("This ain't right..!");
        }

        protected virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Jump))
            {
                _position.Y -= 10f;
            }
        }

        protected virtual void SetAnimations()
        {
            if (Velocity.X > 0)
                this._animationManager.Play(_animations["WalkRight"]);
            else if (Velocity.X < 0)
                this._animationManager.Play(_animations["WalkLeft"]);
            else if (Velocity.Y > 0)
                this._animationManager.Play(_animations["WalkDown"]);
            else if (Velocity.Y < 0)
                this._animationManager.Play(_animations["WalkUp"]);
            else this._animationManager.Stop();
        }
        protected virtual void jumped()
        {
            //saute
            if (Jump && !_hasJumped)
            {
                _hasJumped = true;
                Jump = false;
            }
            //tombe
            if (!_hasJumped)
            {
                this.Velocity.Y += 0.15f * this.JumpSpeed;
            }
            //saute
            else
            {
                this.Velocity.Y -= 0.15f * this.JumpSpeed;
            }

            if (_hasJumped && this.Position.Y <= this.LastPosition.Y - this.AnimationManager.Animation.FrameHeight + 5 || this.Position.Y<=0)
            {
                _hasJumped = false;
            }
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }
        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, this.AnimationManager.Animation.FrameWidth, this.AnimationManager.Animation.FrameHeight);
            Move();
            SetAnimations();
            jumped();

            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        #endregion

        public AnimationManager AnimationManager
        {
            get => _animationManager;
        }
    }
}
