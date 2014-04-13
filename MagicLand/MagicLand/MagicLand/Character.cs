using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MagicLand
{
    class Character : Sprite
    {
        const string character_asset_name = "hahmo";
        const int START_POSITION_X = 550;
        const int START_POSITION_Y = 540;
        const int CHARACTER_SPEED = 160;
        const int CHARACTER_JUMP_SPEED = 150;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int HIT_LEFT = 1;
        const int HIT_RIGHT = 2;
        const float TIME_PER_PICTURE = 250.0f;
        const float TIME_PER_PICTURE_SNEAKING = 500.0f;
        const int CHARACTER_JUMP_TOP = 70;

        enum State
        {
            Standing,
            Walking,
            Jumping
        }

        float elapsedTime;

        State currentCharacterState;

        public Vector2 direction = Vector2.Zero;
        Vector2 speed = Vector2.Zero;
        public Vector2 startingPosition;
        public Vector2 jumpStartPosition;

        KeyboardState previousKeyboardState;
        GamePadState previousButtonState;

        public Rectangle characterRectangle;
        public Rectangle farmRectangle;
        public Rectangle haisuliRectangleTop;
        public Rectangle haisuliRectangleBottom;

        public bool isOnBrick;
        public bool hitsBrickLeft;
        public bool hitsBrickRight;
        public bool canJump;
        bool walkPicture4;
        bool characterIsFacingLeft = false;

        //CharacterStatusBar = MagicGame.characterStatusBar;

        public Character()
        {
            startingPosition = new Vector2(START_POSITION_X, START_POSITION_Y);
            jumpStartPosition = Vector2.Zero;
            isOnBrick = false;
            hitsBrickLeft = false;
            hitsBrickRight = false;
            walkPicture4 = false;
            currentCharacterState = State.Standing;
            canJump = true;
        }

        public void LoadContent(ContentManager contentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);

            base.LoadContent(contentManager, character_asset_name);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            GamePadState currentButtonState = GamePad.GetState(PlayerIndex.One);

            UpdateJump(currentKeyboardState, currentButtonState);
            UpdateMovement(gameTime);
            UpdatePictureCount(gameTime);

            previousKeyboardState = currentKeyboardState;
            previousButtonState = currentButtonState;

            base.Update(gameTime, speed, direction);

            //Asetetaan Haisulin yläosaan, alaosaan ja muuhun ruumiiseen törmäyspisteet

            //farmiruutu (aika pieni (3x3pix kokonen),  ettei valita montaa farmiruutua kerralla)
            farmRectangle = new Rectangle((int)Position.X + Size.Width / 2, (int)Position.Y + Size.Width / 2,
                3, 3); 

            characterRectangle = new Rectangle((int)Position.X, (int)Position.Y,
                Size.Width, Size.Height - 1);
            haisuliRectangleTop = new Rectangle((int)Position.X, (int)Position.Y + 1,
                Size.Width, 3);
            haisuliRectangleBottom = new Rectangle((int)Position.X, (int)Position.Y + Size.Height - 3,
                Size.Width, 3);
        }

        //Päivitetään kulloinkin piirrettävä kuva
        private void UpdatePictureCount(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (currentCharacterState == State.Jumping)
            {
                if (direction.Y == MOVE_UP)
                    picture = 2;
                else if (direction.Y == MOVE_DOWN)
                    picture = 3;
            }

            if ((elapsedTime > TIME_PER_PICTURE) || (elapsedTime > TIME_PER_PICTURE_SNEAKING))
            {
                if (currentCharacterState == State.Standing)
                    picture = 0;
                else if (currentCharacterState == State.Walking)
                {
                    if (picture == 0 && walkPicture4 == false)
                    {
                        picture = 1;
                        walkPicture4 = true;
                    }
                    else if (picture == 0 && walkPicture4 == true)
                    {
                        picture = 2;
                        walkPicture4 = false;
                    }
                    else
                        picture = 0;
                }

                picture = picture % numberOfPictures;
                    elapsedTime -= TIME_PER_PICTURE;
            }
        }

        private void UpdateMovement(GameTime gameTime)
        {
            //Tutkitaan liikkuminen oikealle
            if ((Keyboard.GetState().IsKeyDown(Keys.Right) == true ||
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0) 
                && hitsBrickLeft == false) {
                MoveRight();
                characterIsFacingLeft = false;
            }

            //Tutkitaan liikkuminen vasemmalle
            else if ((Keyboard.GetState().IsKeyDown(Keys.Left) == true ||
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0) &&
                hitsBrickRight == false) {
                MoveLeft();
                characterIsFacingLeft = true;
            }
            //Tutkitaan liikkuminen ylös
            else if ((Keyboard.GetState().IsKeyDown(Keys.Up) == true || 
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0) &&
                hitsBrickRight == false) {
                MoveUp();
            }
            //Tutkitaan liikkuminen vasemmalle
            else  if ((Keyboard.GetState().IsKeyDown(Keys.Down) == true ||
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0) &&
                hitsBrickRight == false) {
                MoveDown();
            }
            //Jos ei liikuta, ukko seisoo
            else
                setStateStanding();

            if (currentCharacterState == State.Standing) {
                speed = Vector2.Zero;
                direction = Vector2.Zero;
            }
            else if (currentCharacterState == State.Walking || currentCharacterState == State.Jumping) {
                Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void MoveLeft()
        {
            if (direction.X == MOVE_LEFT)
            {
                goingLeft = true;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (currentCharacterState == State.Standing || currentCharacterState == State.Walking)
            {
                currentCharacterState = State.Walking;
                speed = new Vector2(CHARACTER_SPEED, CHARACTER_SPEED);
            }
            direction.X = MOVE_LEFT;
        }

        public void MoveRight()
        {
            goingLeft = false;

            spriteEffects = SpriteEffects.None;
            if (currentCharacterState == State.Standing || currentCharacterState == State.Walking)
            {
                currentCharacterState = State.Walking;
                
                speed = new Vector2(CHARACTER_SPEED, CHARACTER_SPEED);
            }
            direction.X = MOVE_RIGHT;
        }

        public void MoveDown()
        {
            if (direction.X == MOVE_LEFT || characterIsFacingLeft == true)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            if (currentCharacterState == State.Standing || currentCharacterState == State.Walking)
            {
                currentCharacterState = State.Walking;
                speed = new Vector2(CHARACTER_SPEED, CHARACTER_SPEED);
            }

            if (currentCharacterState != State.Jumping)
            {
                direction.Y = MOVE_DOWN;
            }
        }

        public void MoveUp()
        {
            if (direction.X == MOVE_LEFT || characterIsFacingLeft == true)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            if (currentCharacterState == State.Standing || currentCharacterState == State.Walking)
            {
                currentCharacterState = State.Walking;
                speed = new Vector2(CHARACTER_SPEED, CHARACTER_SPEED);
            }

            if (currentCharacterState != State.Jumping)
            {
                direction.Y = MOVE_UP;
            }
        }

        public void setStateStanding()
        {
            if (currentCharacterState != State.Jumping)
                currentCharacterState = State.Standing;
            else
                direction.X = 0;
        }

        public void setPaikallaan()
        {
            direction.X = 0;
            direction.Y = 0;
        }

        private void UpdateJump(KeyboardState currentKeyboardState, GamePadState currentButtonState)
        {
            if ((currentCharacterState == State.Walking || currentCharacterState == State.Standing) && currentCharacterState != State.Jumping )
            {

                if ((currentKeyboardState.IsKeyDown(Keys.Space) == true && previousKeyboardState.IsKeyDown(Keys.Space) == false) 
                    || 
                    (currentButtonState.IsButtonDown(Buttons.A) && previousButtonState.IsButtonDown(Buttons.A) == false))
                {
                    Jump();
                }
            }

            else if (currentCharacterState == State.Jumping)
            {
                float muutos = jumpStartPosition.Y - Position.Y;

                //jos hahmo on kerennyt jo siirtyä 70 pixeliä ylöspäin eli hyppy on suoritettu loppuun, asetetaan suunta alas
                if ( muutos > CHARACTER_JUMP_TOP && canJump == true)
                {
                    direction.Y = MOVE_DOWN;
                }
                else if (muutos < CHARACTER_JUMP_TOP && canJump == false)
                {
                    direction.Y = MOVE_DOWN;

                    System.Diagnostics.Debug.Write("muutos:"+ muutos);
                    System.Diagnostics.Debug.Write("jump start y:" + jumpStartPosition.Y);
                    System.Diagnostics.Debug.Write("y:" + Position.Y);
                }

                //laitetaan takas tilaan Standing
                if (Position.Y >= jumpStartPosition.Y)
                {
                    //Position.Y = startingPosition.Y;
                    currentCharacterState = State.Standing;
                    direction = Vector2.Zero;
                    canJump = true;   
                }

                //voi_hypata = false;
            }
        } //updateJump

        private void Jump()
        {
            if (currentCharacterState != State.Jumping && direction.Y == 0)
            {
                isOnBrick = false;
                currentCharacterState = State.Jumping;
                direction.Y = MOVE_UP;

                //otetaan talteen kohta mista hyppy on alkanut (x ja y)
                jumpStartPosition = Position;

                //nopeusvektori hyppya varten
                speed = new Vector2(CHARACTER_SPEED, CHARACTER_JUMP_SPEED);

                AudioManager.PlaySound("item", false, 0.3f);
                
            }
        }
    }
}