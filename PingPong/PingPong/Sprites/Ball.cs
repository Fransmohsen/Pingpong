using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPong.Sprites
{
    public class Ball : Sprite
    {
        private float _timer = 0f; // Öka hastigheten beroande av tiden F(Hastigheten)=Tiden
        private Vector2? _startPosition = null;
        private float? _startSpeed;
        private bool _isPlaying;

        public bool isAlive = true;

        public Score Score;
        public int SpeedIncrementSpan = 3; // Hur ofta hastigheten kommer att ökas(Varje 5 sekunder)


        //bollens start hasighet
        public Ball(Texture2D texture)
          : base(texture)
        {
            Speed = 6f;
        }

        //Update() för att bollens poistion ska updaters

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (_startPosition == null)
            {
                _startPosition = Position;
                _startSpeed = Speed;

                Restart();
            }

            //ifall användaren trycker på space så startas spelet! (inaktiv medans bollen är igång)
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _isPlaying = true;

            if (!_isPlaying)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //bollens hasighet ökar med tiden
            if (_timer > SpeedIncrementSpan)
            {
                Speed++;
                _timer = 0;
            }

            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;

                if (this.Velocity.X > 0 && this.IsTouchingLeft(sprite))
                    this.Velocity.X = -this.Velocity.X;
                if (this.Velocity.X < 0 && this.IsTouchingRight(sprite))
                    this.Velocity.X = -this.Velocity.X;
                if (this.Velocity.Y > 0 && this.IsTouchingTop(sprite))
                    this.Velocity.Y = -this.Velocity.Y;
                if (this.Velocity.Y < 0 && this.IsTouchingBottom(sprite))
                    this.Velocity.Y = -this.Velocity.Y;
            }

            if (Position.Y <= 0 || Position.Y + _texture.Height >= Game1.ScreenHeight)
                Velocity.Y = -Velocity.Y;

            //ifall bollen går ut en av sidorna så får spelaren poäng (Vänster,Höger)
            if (Position.X <= 0)
            {
                Score.Score2++;
                Restart();
            }

            if (Position.X + _texture.Width >= Game1.ScreenWidth)
            {
                Score.Score1++;
                Restart();
            }
            //END

            Position += Velocity * Speed;
        }

        public void Restart()
        {
            var direction = Game1.Random.Next(0, 4);

            switch (direction)
            {
                case 0:
                    Velocity = new Vector2(1, 1);
                    break;
                case 1:
                    Velocity = new Vector2(1, -1);
                    break;
                case 2:
                    Velocity = new Vector2(-1, -1);
                    break;
                case 3:
                    Velocity = new Vector2(-1, 1);
                    break;
            }

            Position = (Vector2)_startPosition;
            Speed = (float)_startSpeed;
            _timer = 0;
            _isPlaying = false;
        }

        /////////////////////////////////////////////
        //public class Block
        //{
        //    bool Isalive = true;
        //    double TimetoDie; //hur länge blocken ska leva

        //    //Block() skapa objektet
        //    public Block(Texture2D texture, float X, float Y,
        //                             GameTime gameTime)
        //        : base(texture, X,Y,0,2f)
        //    {
        //        TimetoDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
        //    }
        //    //update() kontrollerar om blocken lever
        //    public void Update(GameTime gameTime)
        //    {
        //        //döda blocken om den är för gammal
        //        if (TimetoDie < gameTime.TotalGameTime.TotalMilliseconds)
        //             Isalive= false;
        //    }
        //}////////////////////////////////////////////////////////
    }

}