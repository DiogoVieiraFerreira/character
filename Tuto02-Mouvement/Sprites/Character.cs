using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuto02_Mouvement.Models;

namespace Tuto02_Mouvement.Sprites
{
    public class Character : Sprite
    {
        public enum Direction { Right, Left, UpRight, UpLeft, DownRight, DownLeft } //what the direction of character
        #region private/protected attributes
        private int _currentHealth;
        protected bool _firstSquat;//when character start squating
        protected bool _firstGetUp;//when character start get uping
        #endregion private attributes

        #region Properties
        /// <summary>
        /// Define or get the direction where character look
        /// </summary>
        public Direction Look { get; protected set; }
        /// <summary>
        /// Everyone can look at health, but only those inherited from character can change the value
        /// </summary>
        public int MaxHealth { get; protected set; }
        /// <summary>
        /// Get or modify the current health
        /// </summary>
        public int CurrentHealth
        {
            get => this._currentHealth;
            set
            {
                if (value > this.MaxHealth)
                    this._currentHealth = this.MaxHealth;
                else if(value < 0)
                    this._currentHealth = 0;
                else
                    this._currentHealth = value;
            }
        }
        /// <summary>
        /// Modify or get the physical damages of your character
        /// </summary>
        public int Damages { get; set; }

        /// <summary>
        /// Modify or get the phyysical defence of your character
        /// </summary>
        public int Defense { get; set; }
        /// <summary>
        /// Modify or get the magic resistance of your character
        /// </summary>
        public int Resistance { get; set; }
        /// <summary>
        /// Define or get if your character's squatting
        /// </summary>
        public bool Squat { get; set; }
        /// <summary>
        /// Define or get if your character eat
        /// </summary>
        public bool Eat { get; set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// A Character has a max health and not eat, jump or eat when he is created.
        /// Its default location is 0;0
        /// </summary>
        /// <param name="maxHealth">max health of character</param>
        /// <param name="speed">move speed of character</param>
        /// <param name="defense">total damages you can protect by physical attack</param>
        /// <param name="resistance">total damages you can protect by magic attack</param>
        /// <param name="damages">total physical damages</param>
        /// <param name="animations">Dictionnary with a specific word for specific animation</param>
        public Character(int maxHealth, int speed, int defense, int resistance, int damages, Dictionary<string, Animation> animations) : base(animations)
        {
            this.JumpSpeed = 6f;
            base.Speed = speed;
            this.MaxHealth = maxHealth;
            this.CurrentHealth = maxHealth;
            this.Squat = false;
            this.Jump = false;
            this.Eat = false;
            this.Defense = defense;
            this.Resistance = resistance;
            this.Damages = damages;
        }
        #endregion Constructor

        #region protected methods
        /// <summary>
        /// responds according to keyboard input
        /// to define the action of the character
        /// </summary>
        protected override void Move()
        {
            if (!this.Eat)
            {
                if (!Keyboard.GetState().IsKeyDown(Input.Eat))
                {
                    if (Keyboard.GetState().IsKeyDown(Input.Jump))
                    {
                        LastPosition = Position;
                            this.Jump = true;
                            this.JumpOk = false;
                        this.Squat = false;
                    }

                    if (Keyboard.GetState().IsKeyDown(Input.Up))
                    {

                        if (!Jump)
                        {
                            if (this.Squat)
                                this._firstGetUp = true;

                            this.Squat = false;
                        }
                        //Velocity.Y = -Speed;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Input.Down))
                    {
                        if (!Jump)
                        {
                            this.Jump = false;

                            if (!this.Squat)
                                this._firstSquat = true;

                            this.Squat = true;
                            //Velocity.Y = Speed;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Input.Left))
                        Velocity.X -= Speed;
                    else if (Keyboard.GetState().IsKeyDown(Input.Right))
                        Velocity.X += Speed;

                }
                else
                {
                    this.Eat = true;
                }
            }
        }
        /// <summary>
        /// Animations according to move of player
        /// </summary>
        protected override void SetAnimations()
        {
            if (this.Eat)
            {
                if (this.Look == Direction.Right)
                {
                    //animation to squat right
                }
                else if (this.Look == Direction.Left)
                {
                    //animation to squat left
                }
                this.Eat = false;
            }
            else if (this._firstSquat) //animation to squat
            {
                if (this.Look == Direction.Right)
                {
                    //animation to squat right
                    this._animationManager.Play(_animations["SquatRight"]);
                }
                else if (this.Look == Direction.Left)
                {
                    //animation to squat left
                    this._animationManager.Play(_animations["SquatLeft"]);
                }
                this._firstSquat = false;
            }
            else if (this._firstGetUp) //animation to get up 
            {
                if (this.Look == Direction.Right)
                {
                    //animation to get up right
                    this._animationManager.Play(_animations["WalkRight"]);
                }
                else if (this.Look == Direction.Left)
                {
                    //animation to get up left
                    this._animationManager.Play(_animations["WalkLeft"]);
                }
                this._firstGetUp = false;
            }


            if (this.Squat && Velocity.X > 0) //walk to the right while squating
            {
                this.Look = Direction.Right;
                this._animationManager.Play(_animations["SquatRight"]);
            }
            else if (this.Squat && Velocity.X < 0) //walk to the left while squating
            {
                this.Look = Direction.Left;
                this._animationManager.Play(_animations["SquatLeft"]);
            }
            else if (Velocity.X > 0) //walk to the right while stading
            {
                this.Look = Direction.Right;
                this._animationManager.Play(_animations["WalkRight"]);
            }
            else if (Velocity.X < 0) //walk to the left while stading
            {
                this.Look = Direction.Left;
                this._animationManager.Play(_animations["WalkLeft"]);
            }
            else if (Velocity.Y > 0) //fallen animation
            {
                if (this.Look == Direction.Right)
                {
                    this.Look = Direction.DownRight;
                    //animation to fallen right
                }
                else if (this.Look == Direction.Left)
                {
                    this.Look = Direction.DownLeft;
                    //animation to fallen left
                }
                //this._animationManager.Play(_animations["WalkDown"]);
            }
            else if (Velocity.Y < 0) //jump animation
            {
                if (this.Look == Direction.Right)
                {
                    this.Look = Direction.UpRight;
                    //animation to jump right
                }
                else if (this.Look == Direction.Left)
                {
                    this.Look = Direction.UpLeft;
                    //animation to jump left
                }
                //this._animationManager.Play(_animations["WalkUp"]);
                this.Jump = false;
            }
            else
                this._animationManager.Stop();
        }
        #endregion protected methods
    }
}
