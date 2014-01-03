using System;
using System.Collections.Generic;
//using System.Timers;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace HumansVsZombies.cs
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Attributes

        protected KeyboardState _kbstate;
        protected KeyboardState _prevKbState;
        protected GamePadState _gpstate;
        protected SpriteFont _tFont; //test font, probably won't use later
        public SpriteFont printScore;

        public Boolean extraction;
        public Boolean extracted;
        //textures
        //Texture2D _bullet;
        Texture2D _titleBackGround;
        Texture2D _pauseBackground;
        Texture2D _gameOver;
        Texture2D _victory;
        Texture2D _title;
        Texture2D _play;
        Texture2D _quit;
        Texture2D _pause;
        Texture2D _resume;
        Texture2D _options;
        Texture2D _credits;
        Texture2D _howTo;
        Texture2D _barrier;
        Texture2D gunAmmo;
        Texture2D _bullet;
        Texture2D _bulletL;
        Texture2D _bulletD;
        public GamePiece _h;
        Texture2D _HUD;
        Texture2D _extraction;
        Texture2D _creditScreen;
        //bullet attributes
        List<Bullet> listBullet;
        Bullet _b;
        double bulletTimer;
        double scoreTimer;

        //static Timer _time;

        //Song _music;

        // Player attributes
        Player _p;
        double humanStep;

        // Zombie attributes
        double zombieStep;
        List<Zombie> _zs;
        int ammo;

        //Score.
        public int score;

        public double timer = 0.0;
        int gameWidth;
        int gameHeight;
        public GamePiece _Extraction;

        // Maptiles
        MapTile mt00;
        MapTile mt01;
        MapTile mt02;
        MapTile mt03;
        MapTile mt10;
        MapTile mt11;
        MapTile mt12;
        MapTile mt13;

        MapTile currentMaptile;

        Dictionary<MapTile, int> dictionaryofMaptiles;
        List<MapTile> listofMaptiles;
        List<DartCollectible> dcs;

        //gameState
        protected GameState gameState;
        protected Highlighted buttonState;
        protected Pause highlight;

        #endregion

        #region All of the States

        /// <summary>
        /// The state of the game. (note: GameManager is main game.)
        /// </summary>
        public enum GameState
        {
            GameManager,
            Menu,
            GameOver,
            Pause,
            Directions,
            Credits
        }

        public enum Highlighted
        {
            Play,
            Credits,
            Quit
        }
        public enum Pause
        {
            Resume,
            Quit
        }

        #endregion

        #region Properties

        public int GetSetGameWidth { get { return gameWidth; } set { gameWidth = value; } }

        public int GetSetGameHeight { get { return gameHeight; } set { gameHeight = value; } }

        #endregion

        #region Constructor

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GameVariables.screenWidth;
            graphics.PreferredBackBufferHeight = GameVariables.screenHeight;
            graphics.ApplyChanges();
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameState.Menu;

            // Set the previous keyboard state variable to what the current keyboard state is (which is nothing)
            _prevKbState = Keyboard.GetState();

            _Extraction = new GamePiece(400, 560, 256, 256);

            // Set the button state to hightlight the "Play Button"
            buttonState = Highlighted.Play;

            // Create height and width varibles that can be used throughout the class
            gameHeight = graphics.PreferredBackBufferHeight;
            gameWidth = graphics.PreferredBackBufferWidth;

            // Set all of the Human values:
            _p = new Player(GameVariables.playerStartX, GameVariables.playerStartY, GameVariables.spriteWidth, GameVariables.spriteHeight);    // The actual Player
            humanStep = GameVariables.stepSpeed;                                                           // The rate at which the player will take a step

            zombieStep = GameVariables.stepSpeed;
            //The HUD rectangle object
            _h = new GamePiece(-9, -1, 480, 110);
            scoreTimer = 0;
            // Create the map objects
            mt00 = new MapTile(3, 1, gameWidth, gameHeight, this.Content.Load<Texture2D>("TopLeft"));
            mt01 = new MapTile(1, 0, gameWidth, gameHeight, this.Content.Load<Texture2D>("TopRight"));
            mt02 = new MapTile(2, 1, gameWidth, gameHeight, this.Content.Load<Texture2D>("grass"));
            mt03 = new MapTile(5, 0, gameWidth, gameHeight, this.Content.Load<Texture2D>("grass"));
            mt10 = new MapTile(2, 1, gameWidth, gameHeight, this.Content.Load<Texture2D>("BotLeft"));
            mt11 = new MapTile(3, 0, gameWidth, gameHeight, this.Content.Load<Texture2D>("BotRight"));
            mt12 = new MapTile(0, 2, gameWidth, gameHeight, this.Content.Load<Texture2D>("road"));
            mt13 = new MapTile(4, 0, gameWidth, gameHeight, this.Content.Load<Texture2D>("road"));


            // Then add them to the list to reference by number
            listofMaptiles = new List<MapTile>();
            listofMaptiles.Add(mt00);
            listofMaptiles.Add(mt01);
            listofMaptiles.Add(mt02);
            listofMaptiles.Add(mt03);

            listofMaptiles.Add(mt10);
            listofMaptiles.Add(mt11);
            listofMaptiles.Add(mt12);
            listofMaptiles.Add(mt13);

            // And also to a dictionary for reference by name for their number that references which
            // Maptile its connected to
            dictionaryofMaptiles = new Dictionary<MapTile, int>();
            for (int i = 0; i < listofMaptiles.Count; i++)
            {
                dictionaryofMaptiles.Add(listofMaptiles[i], i);
            }

            // Set the current maptile
            currentMaptile = listofMaptiles[0];

            // Method where you can connect all of the barriers to a map tile
            SetUpBarrierConnections();

            //making the lsit of zombies
            _zs = new List<Zombie>();
            foreach (Zombie z in currentMaptile.GetSetZombieList)
            {
                _zs.Add(z);
            }

            //Bullet stuffs
            listBullet = new List<Bullet>();
            _b = new Bullet();
            bulletTimer = GameVariables.bTimer;

            //changing how much ammo
            ammo = 20;

            dcs = new List<DartCollectible>();
            foreach (DartCollectible dc in currentMaptile.GetSetDartCollectibles)
            {
                dcs.Add(dc);
            }

            //making the start score 0
            score = 0;
            //CheckExternal();
            base.Initialize();
        }

        //private void CheckExternal()
        //{
        //    Stream s = File.OpenRead("ExternalTool/ExternalTool/bin/Debug/zombie.dat");
        //    Stream s2 = File.OpenRead("ExternalTool/ExternalTool/bin/Debug/ammo.dat");
        //    Stream s3 = File.OpenRead("ExternalTool/ExternalTool/bin/Debug/pspeed.dat");
        //    BinaryReader input = new BinaryReader(s);
        //    BinaryReader input2 = new BinaryReader(s2);
        //    BinaryReader input3 = new BinaryReader(s3);
        //    int zSpeed = input.ReadInt32();
        //    int darts = input2.ReadInt32();
        //    int pSpeed = input3.ReadInt32();
        //    input.Close();
        //    for (int i = 0; i < _zs.Count; i++)
        //    {
        //        _zs.ElementAt(i).DefaultChaseSpeed = zSpeed;
        //    }
        //    ammo = darts;
        //    _p.moveSpeed = pSpeed;
        //}
        #endregion

        #region LoadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

           // _tFont = this.Content.Load<SpriteFont>("testfont");
            //printScore = this.Content.Load<SpriteFont>("printScore");

            foreach (Zombie z in _zs)
            {
                z.UpA = this.Content.Load<Texture2D>("Zombie0A");
                z.UpB = this.Content.Load<Texture2D>("Zombie0B");
                z.RightA = this.Content.Load<Texture2D>("Zombie1A");
                z.RightB = this.Content.Load<Texture2D>("Zombie1B");
                z.DownA = this.Content.Load<Texture2D>("Zombie2A");
                z.DownB = this.Content.Load<Texture2D>("Zombie2B");
                z.LeftA = this.Content.Load<Texture2D>("Zombie3A");
                z.LeftB = this.Content.Load<Texture2D>("Zombie3B");
                z.CurrentZombie = z.UpA;
                z.PreviousZombie = z.UpB;
            }

            //twinkies
            _p._twinkie1 = this.Content.Load<Texture2D>("1hp");
            _p._twinkie2 = this.Content.Load<Texture2D>("2hp");
            _p._twinkie3 = this.Content.Load<Texture2D>("3hp");
            _p._health = _p._twinkie3;

            //humans
            _p.UpA = this.Content.Load<Texture2D>("Human0A");
            _p.UpB = this.Content.Load<Texture2D>("Human0B");
            _p.RightA = this.Content.Load<Texture2D>("Human1A");
            _p.RightB = this.Content.Load<Texture2D>("Human1B");
            _p.DownA = this.Content.Load<Texture2D>("Human2A");
            _p.DownB = this.Content.Load<Texture2D>("Human2B");
            _p.LeftA = this.Content.Load<Texture2D>("Human3A");
            _p.LeftB = this.Content.Load<Texture2D>("Human3B");
            _p.CurrentPlayer = this.Content.Load<Texture2D>("Human0A");
            _p.PreviousPlayer = this.Content.Load<Texture2D>("Human0B");

            _p.CurrentPlayer = _p.UpA;
            _p.CurrentPlayer = _p.UpB;

            //load in the screens
            _titleBackGround = this.Content.Load<Texture2D>("rit");
            _title = this.Content.Load<Texture2D>("title");
            _play = this.Content.Load<Texture2D>("play");
            _quit = this.Content.Load<Texture2D>("quit");
            _pauseBackground = this.Content.Load<Texture2D>("Pause Menu");
            _resume = this.Content.Load<Texture2D>("resume");
            _gameOver = this.Content.Load<Texture2D>("gameover");
            _victory = this.Content.Load<Texture2D>("victory");
            _pause = this.Content.Load<Texture2D>("paused");
            _options = this.Content.Load<Texture2D>("options");
            _credits = this.Content.Load<Texture2D>("credits");
            _howTo = this.Content.Load<Texture2D>("howtoplay");
            _barrier = this.Content.Load<Texture2D>("barrier");
            _HUD = this.Content.Load<Texture2D>("HUD");
            //Music
            //_music = this.Content.Load<Song>("Menu");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(_music);

            //loads in bullets
            //this one is for the ammoy bar at the bottom
            gunAmmo = this.Content.Load<Texture2D>("Bullet u");
            _bullet = this.Content.Load<Texture2D>("Bullet r");
            _bulletD = this.Content.Load<Texture2D>("Bullet d");
            _bulletL = this.Content.Load<Texture2D>("Bullet l");

            //extraction point
            _extraction = this.Content.Load<Texture2D>("extraction");

            //Team photo
            _creditScreen = this.Content.Load<Texture2D>("Team Photo");
        }

        #endregion

        #region SingleKeyPress

        /// <summary>
        /// Allows for Checking of a single key press.
        /// </summary>
        /// <param name="k">The Key that is being checked.</param>
        /// <returns></returns>
        protected bool SingleKeyPress(Keys k)
        {
            if (_kbstate.IsKeyDown(k) == true && _prevKbState.IsKeyUp(k) == true)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ScreenWrap

        public void ScreenWall(GamePiece g)
        {
            if (g._Rectangle.X <= 0)
            {
                g.XValue = 1;
            }
            else if (g._Rectangle.X + g._Rectangle.Width >= GameVariables.screenWidth)
            {
                g.XValue = GameVariables.screenWidth - g._Rectangle.Width;
            }
            else if (g._Rectangle.Y <= 0)
            {
                g.YValue = 1;
            }
            else if (g._Rectangle.Y + g._Rectangle.Height >= GameVariables.screenHeight)
            {
                g.YValue = GameVariables.screenHeight - g._Rectangle.Height;
            }
            else if (_h._Rectangle.Contains(g._Rectangle.X,g._Rectangle.Y - 5) && g._Rectangle.Y <= _h._Rectangle.Height - 5)
            {
                g.YValue = _h._Rectangle.Height;
            }
            else if (_h._Rectangle.Contains(g._Rectangle.X- 5, g._Rectangle.Y) && g._Rectangle.X <= _h._Rectangle.Width - 5)
            {
                g.XValue = _h._Rectangle.Width;
            }
        }
        /// <summary>
        /// This will warp the game object from the top to the bottom of the screen and vice versa, from the left to the right of the screed and vice versa
        /// </summary>
        /// <param name="go">The Gampiece object you wish to have the screenwrap affect</param>
        public void ScreenWrap()
        {
            // If the object moves up and collides with the top barrier & has its own unique number that's not the default
            if (_p.CheckCollision(currentMaptile.GetSetBarrierUp0) == true && currentMaptile.GetSetBarrierUp0.GetSetMaptileNumber != 100)
            {
                // It moves to the bottom of the screen minus the height of the object and the barrier
                _p.YValue = gameHeight - (currentMaptile.GetSetBarrierDown2._Rectangle.Height + _p._Rectangle.Height);

                // Change the current maptile to this maptile's new maptile
                currentMaptile = listofMaptiles[currentMaptile.GetSetBarrierUp0.GetSetMaptileNumber];

                // Clear the current list of zombies and input the new maptile's list into it
                _zs.Clear();
                foreach (Zombie z in currentMaptile.GetSetZombieList)
                {
                    _zs.Add(z);
                }
            }

            // If the object moves right and collides with the right barrier
            if (_p.CheckCollision(currentMaptile.GetSetBarrierRight1) == true && currentMaptile.GetSetBarrierRight1.GetSetMaptileNumber != 100)
            {
                // It moves to the left of the screen plus the width of the barrier
                _p.XValue = currentMaptile.GetSetBarrierLeft3._Rectangle.Width;

                // Change the current maptile to this maptile's new maptile
                currentMaptile = listofMaptiles[currentMaptile.GetSetBarrierRight1.GetSetMaptileNumber];

                // Clear the current list of zombies and input the new maptile's list into it
                _zs.Clear();
                foreach (Zombie z in currentMaptile.GetSetZombieList)
                {
                    _zs.Add(z);
                }
            }

            // If the object moves down and collides with the bottom barrier
            if (_p.CheckCollision(currentMaptile.GetSetBarrierDown2) == true && currentMaptile.GetSetBarrierDown2.GetSetMaptileNumber != 100)
            {
                // It moves to the top of the screen plus the height of the barrier
                _p.YValue = currentMaptile.GetSetBarrierUp0._Rectangle.Height;

                // Change the current maptile to this maptile's new maptile
                currentMaptile = listofMaptiles[currentMaptile.GetSetBarrierDown2.GetSetMaptileNumber];

                // Clear the current list of zombies and input the new maptile's list into it
                _zs.Clear();
                foreach (Zombie z in currentMaptile.GetSetZombieList)
                {
                    _zs.Add(z);
                }
            }

            // If the object moves left and collides with the left barrier
            if (_p.CheckCollision(currentMaptile.GetSetBarrierLeft3) == true && currentMaptile.GetSetBarrierLeft3.GetSetMaptileNumber != 100)
            {
                // It moves to the right of the screen minus the width of the object and the barrier
                _p.XValue = gameWidth - (currentMaptile.GetSetBarrierRight1._Rectangle.Width + _p._Rectangle.Width);

                // Change the current maptile to this maptile's new maptile
                currentMaptile = listofMaptiles[currentMaptile.GetSetBarrierLeft3.GetSetMaptileNumber];

                // Clear the current list of zombies and input the new maptile's list into it
                _zs.Clear();
                foreach (Zombie z in currentMaptile.GetSetZombieList)
                {
                    _zs.Add(z);
                }
            }

            dcs.Clear();
            foreach (DartCollectible dc in currentMaptile.GetSetDartCollectibles)
            {
                dcs.Add(dc);
            }
        }

        #endregion

        #region SetUpBarrierConnections

        /// <summary>
        /// Set up which barriers lead to which maptiles
        /// </summary>
        public void SetUpBarrierConnections()
        {
            // Maptile 0's right barriers number is mt01's designated number
            // This is because each maptile has a designated number that references to it
            // The barrier then stores that number to pull when to reference that maptile
            mt00.GetSetBarrierRight1.GetSetMaptileNumber = dictionaryofMaptiles[mt01];
            mt00.GetSetBarrierDown2.GetSetMaptileNumber = dictionaryofMaptiles[mt10];

            mt01.GetSetBarrierLeft3.GetSetMaptileNumber = dictionaryofMaptiles[mt00];
            mt01.GetSetBarrierDown2.GetSetMaptileNumber = dictionaryofMaptiles[mt11];
            mt01.GetSetBarrierRight1.GetSetMaptileNumber = dictionaryofMaptiles[mt02];

            mt02.GetSetBarrierLeft3.GetSetMaptileNumber = dictionaryofMaptiles[mt01];
            mt02.GetSetBarrierRight1.GetSetMaptileNumber = dictionaryofMaptiles[mt03];
            mt02.GetSetBarrierDown2.GetSetMaptileNumber = dictionaryofMaptiles[mt12];

            mt03.GetSetBarrierLeft3.GetSetMaptileNumber = dictionaryofMaptiles[mt02];
            mt03.GetSetBarrierDown2.GetSetMaptileNumber = dictionaryofMaptiles[mt13];

            mt10.GetSetBarrierUp0.GetSetMaptileNumber = dictionaryofMaptiles[mt00];
            mt10.GetSetBarrierRight1.GetSetMaptileNumber = dictionaryofMaptiles[mt11];

            mt11.GetSetBarrierLeft3.GetSetMaptileNumber = dictionaryofMaptiles[mt10];
            mt11.GetSetBarrierUp0.GetSetMaptileNumber = dictionaryofMaptiles[mt01];
            mt11.GetSetBarrierRight1.GetSetMaptileNumber = dictionaryofMaptiles[mt12];

            mt12.GetSetBarrierLeft3.GetSetMaptileNumber = dictionaryofMaptiles[mt11];
            mt12.GetSetBarrierUp0.GetSetMaptileNumber = dictionaryofMaptiles[mt02];
            mt12.GetSetBarrierRight1.GetSetMaptileNumber = dictionaryofMaptiles[mt13];

            mt13.GetSetBarrierLeft3.GetSetMaptileNumber = dictionaryofMaptiles[mt12];
            mt13.GetSetBarrierUp0.GetSetMaptileNumber = dictionaryofMaptiles[mt03];
        }

        #endregion

        #region Reset

        /// <summary>
        /// Resets the values of the game to their start values.
        /// </summary>
        public void Reset()
        {
            //Get rid of the bullets
            // Update the Projectiles
            for (int j = listBullet.Count - 1; j >= 0; j--)
            {
                if (listBullet[j].Active == true)
                {
                    listBullet[j].Active = false;
                }
            }
            // Reset the player back to his default location
            _p.XValue = GameVariables.playerStartX;
            _p.YValue = GameVariables.playerStartY;
            ammo = 20;
            score = 0;
            //then reset the location of the zombies randomly
            
            //SpawnZombies(zombieHoard);

            //reset player health
            _p.health = GameVariables.humanHealth;

            //reset extraction
            extracted = false;
            foreach (MapTile m in listofMaptiles)
            {
                m.extraction = false;
                m.GetSetZombieList.Clear();
            }

            //load the content again
            Initialize();
            LoadContent();
        }

        #endregion

        #region Update
        public void updateScore()
        {
            scoreTimer += .5;
                if(scoreTimer >= 100)
                {
                    score+= 100;
                    scoreTimer = 0;
                }
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Puts whatever the current keyboard state is into this variable to be used later
            _kbstate = Keyboard.GetState();
            _gpstate = GamePad.GetState(PlayerIndex.One);
                    if (bulletTimer < GameVariables.bTimer)
                    {
                        bulletTimer += gameTime.ElapsedGameTime.TotalSeconds;
                        if (bulletTimer >= GameVariables.bTimer)
                        {
                            bulletTimer = GameVariables.bTimer;
                        }
                    }
            // Finite state machine for the menu, the game, the pause screen,
            // and the game over screen
            switch (gameState)
            {
                #region The Start Menu

                case GameState.Menu:
                    switch (buttonState)                // Press button to alter which item is highlighted to
                    {                                   // to change where player will go on the menu
                        // If the "Play" button is highlighted
                        case Highlighted.Play:
                            {
                                if (SingleKeyPress(Keys.D) || _gpstate.ThumbSticks.Left.X == 1 || _gpstate.DPad.Right == ButtonState.Pressed)             // Press "D" to move right to the Credits option
                                    if (bulletTimer == GameVariables.bTimer)
                                    {
                                        buttonState = Highlighted.Credits;
                                    }
                              
                                if (SingleKeyPress(Keys.Enter) || _gpstate.Buttons.Start == ButtonState.Pressed || _gpstate.Buttons.A == ButtonState.Pressed)         // Press "Enter" to start the game
                                    gameState = GameState.GameManager;
                                break;
                            }

                        // If the "Credits" button is highlighted
                        case Highlighted.Credits:
                            {
                                if (SingleKeyPress(Keys.D) || _gpstate.ThumbSticks.Left.X == 1 || _gpstate.DPad.Right == ButtonState.Pressed)             // Press "D" to move right to the Quit option
                                    buttonState = Highlighted.Quit;

                                else if (SingleKeyPress(Keys.A) || _gpstate.ThumbSticks.Left.X == -1 || _gpstate.DPad.Left == ButtonState.Pressed)        // Press "A" to move left to the Play option
                                    buttonState = Highlighted.Play;

                                if (SingleKeyPress(Keys.Enter) || _gpstate.Buttons.Start == ButtonState.Pressed || _gpstate.Buttons.A == ButtonState.Pressed)         // Press "Enter" to enter the credits menu
                                    gameState = GameState.Credits;
                                break;
                            }

                        // If the "Quit" button is highlighted
                        case Highlighted.Quit:
                            {
                                if (SingleKeyPress(Keys.A) || _gpstate.ThumbSticks.Left.X == -1 || _gpstate.DPad.Left == ButtonState.Pressed)             // Press "A" to move Left to the Credits option
                                    buttonState = Highlighted.Credits;

                                if (SingleKeyPress(Keys.Enter) || _gpstate.Buttons.Start == ButtonState.Pressed || _gpstate.Buttons.A == ButtonState.Pressed)         // Press "Enter" to quit the game
                                    this.Exit();
                                break;
                            }
                    }
                    break;

                #endregion

                 #region The Game

                case GameState.GameManager:
                    MediaPlayer.Pause();                                                        // Stop the music from playing from the menu
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;                             // Start the timer up for later use
                    if (SingleKeyPress(Keys.Escape) || SingleKeyPress(Keys.P) || _gpstate.Buttons.Start == ButtonState.Pressed)
                    {
                        // If "P" or "Escape" is press opent he pause menu
                        gameState = GameState.Pause;
                    }

                    // To move the player
                    if (_kbstate.IsKeyDown(Keys.W) || _gpstate.ThumbSticks.Left.Y > 0 || _gpstate.DPad.Up == ButtonState.Pressed)                 // Press "W" to move up
                    {
                        _p.YValue -= _p.moveSpeed;
                        _p.DState = GamePiece.Direction.Up;
                    }

                    if (_kbstate.IsKeyDown(Keys.A) || _gpstate.ThumbSticks.Left.X < 0 || _gpstate.DPad.Left == ButtonState.Pressed)                 // Press "A" to move left
                    {
                        _p.XValue -= _p.moveSpeed;
                        _p.DState = GamePiece.Direction.Left;
                    }

                    if (_kbstate.IsKeyDown(Keys.S) || _gpstate.ThumbSticks.Left.Y < 0 || _gpstate.DPad.Down == ButtonState.Pressed)                 // Press "S" to move down
                    {
                        _p.YValue += _p.moveSpeed;
                        _p.DState = GamePiece.Direction.Down;
                    }

                    if (_kbstate.IsKeyDown(Keys.D) || _gpstate.ThumbSticks.Left.X > 0 || _gpstate.DPad.Right == ButtonState.Pressed)                 // Press "D" to move right
                    {
                        _p.XValue += _p.moveSpeed;
                        _p.DState = GamePiece.Direction.Right;
                    }

                    if (bulletTimer == GameVariables.bTimer)
                    {
                        if (SingleKeyPress(Keys.Space) || _gpstate.Triggers.Right == 1)
                        {
                            if (ammo > 0)
                            {
                                _b.Active = true;
                                AddProjectile(new Vector2(_p.XValue, _p.YValue));
                                ammo--;
                            }
                        }
                        bulletTimer = 0;
                        //bulletTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    foreach (DartCollectible dc in dcs)
                    {
                        if (_p.CheckCollision(dc) == true && dc.GetSetCollected != true && ammo +5 <= 20 )
                        {
                            ammo += GameVariables.dartCollectibleWorth;
                            dc.GetSetCollected = true;
                        }
                    }
                    updateScore();
                    //troll code
                    //if (_kbstate.IsKeyDown(Keys.Space))
                    //{
                    //    AddProjectile(new Vector2(_p.XValue, _p.YValue));
                    //{

                    //makes sure bullets dont fly off the screen
                    UpdateProjectiles();

                    //checks if bullets collide with zombies
                    bulletCollision();

                    //Screen"wrap", more like a big wall
                    ScreenWrap();
                    ScreenWall(_p);
                    // For each zombie activate their AI's and make sure they don't run into one another
                    for (int i = 0; i < _zs.Count; i++)
                    {
                        _zs.ElementAt(i).AI(_p, gameTime.ElapsedGameTime.TotalSeconds);
                        ScreenWall(_zs.ElementAt(i));
                        for (int j = 0; j < _zs.Count; j++)
                        {
                            if (j != i)
                            {
                                _zs.ElementAt(i).HitOtherZomibe(_zs.ElementAt(j));
                            }
                        }
                    }

                    //***game over condition(s) goes here***
                    if (score >= 300 && extracted == false)
                    {
                        extraction = true;
                        currentMaptile.extraction = true;
                    }
                    if (extraction == true)
                    {
                        currentMaptile.GetSetZombieList.Clear();
                        currentMaptile.SpawnZombies(10);
                        
                        foreach (Zombie z in currentMaptile.GetSetZombieList)
                        {
                            _zs.Add(z);
                        }
                        extracted = true;
                        extraction = false;
                    }

                    #if DEBUG
                    if (SingleKeyPress(Keys.NumPad0) == true)
                    {
                        gameState = GameState.GameOver;
                    }
                    #endif
                    break;

                #endregion

                #region The Pause Menu

                case GameState.Pause:
                    switch (highlight)
                    {
                        // If the Resume button is highlighted
                        case Pause.Resume:
                            {
                                if (SingleKeyPress(Keys.D) || _gpstate.ThumbSticks.Left.X == 1 || _gpstate.DPad.Right == ButtonState.Pressed)                 // Press "D" to move right to the Quit option
                                    highlight = Pause.Quit;

                                if (SingleKeyPress(Keys.Enter) || _gpstate.Buttons.Start == ButtonState.Pressed || _gpstate.Buttons.A == ButtonState.Pressed)             // Press "Enter" to go back to the game
                                    gameState = GameState.GameManager;

                                break;
                            }

                        // If the Quit button is highlighted
                        case Pause.Quit:
                            {
                                if (SingleKeyPress(Keys.A) || _gpstate.ThumbSticks.Left.X == -1 || _gpstate.DPad.Left == ButtonState.Pressed)                 // Press "A" to move left to the Resume option
                                    highlight = Pause.Resume;

                                if (SingleKeyPress(Keys.Enter) || _gpstate.Buttons.Start == ButtonState.Pressed || _gpstate.Buttons.A == ButtonState.Pressed)             // Press "Enter" to exit the game
                                {
                                    Reset();                                // Reset the game before exiting
                                    gameState = GameState.Menu;
                                }
                                break;
                            }
                    }
                    break;

                #endregion

                //***pust to options conditions here***//
                #region The GameOver Screen

                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter) || _gpstate.Buttons.Start == ButtonState.Pressed || _gpstate.Buttons.A == ButtonState.Pressed)
                    {
                        //reset the values of the game
                        Reset();
                        gameState = GameState.Menu;
                    }
                    //***quick quit from game over conditons could go here if needed***//
                    break;

                #endregion

                #region The Credits Screen

                case GameState.Credits:
                    if (SingleKeyPress(Keys.Escape) || _gpstate.Buttons.B == ButtonState.Pressed || _gpstate.Buttons.Back == ButtonState.Pressed)        // Press "Escape" to exit the Credits screen
                    {
                        gameState = GameState.Menu;
                    }
                    break;

                #endregion
            }

            // Set the current keyboard state into the previous keyboard variable
            _prevKbState = _kbstate;

            base.Update(gameTime);
        }
        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (gameState)
            {
                // The Menu Screen
                case GameState.Menu:
                    spriteBatch.Draw(_titleBackGround, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(_title, new Rectangle(295, 15, 724, 198), Color.White);

                    // Finistate for the Menu Screen
                    switch (buttonState)
                    {
                        // Highlight the Play Button
                        case Highlighted.Play:
                            {
                                spriteBatch.Draw(_play, new Rectangle(115, 415, 250, 150), Color.Green);
                                spriteBatch.Draw(_credits, new Rectangle(500, 425, 295, 150), Color.White);
                                spriteBatch.Draw(_quit, new Rectangle(915, 425, 265, 150), Color.White);
                                break;
                            }

                        // Highlight the Credits Button
                        case Highlighted.Credits:
                            {
                                spriteBatch.Draw(_play, new Rectangle(115, 415, 250, 150), Color.White);
                                spriteBatch.Draw(_credits, new Rectangle(500, 425, 295, 150), Color.Green);
                                spriteBatch.Draw(_quit, new Rectangle(915, 425, 265, 150), Color.White);
                                break;
                            }

                        // Highlight the Quit Button
                        case Highlighted.Quit:
                            {
                                spriteBatch.Draw(_play, new Rectangle(115, 415, 250, 150), Color.White);
                                spriteBatch.Draw(_credits, new Rectangle(500, 425, 295, 150), Color.White);
                                spriteBatch.Draw(_quit, new Rectangle(915, 425, 265, 150), Color.Green);
                                break;
                            }
                    }
                    break;

                // The Credits Screen
                case GameState.Credits:
                    spriteBatch.Draw(_creditScreen, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(_credits, new Rectangle(10, 15, 424, 198), Color.White);
                    break;

                // The Directions Screen
                case GameState.Directions:
                    spriteBatch.Draw(_titleBackGround, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(_howTo, new Rectangle(425, 15, 424, 198), Color.White);
                    //debug instructions menu
                    //spriteBatch.Draw(_Instru, new Rectangle(300, 200, 200, 250), Color.White);
                    break;

                // The actual game
                case GameState.GameManager:
                    //draw the background first

                    //spriteBatch.Draw(_barrier, currentMaptile.GetSetBarrierUp0._Rectangle, Color.White);

                    spriteBatch.Draw(currentMaptile.GetSetBackgroundImage, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(_HUD, new Rectangle(-9, -1, 480, 120), Color.White);

                    // Don't draw the barrier if it has the default value
                    if (currentMaptile.GetSetBarrierUp0.GetSetMaptileNumber != GameVariables.defaultMapTileReferenced)
                        spriteBatch.Draw(_barrier, currentMaptile.GetSetBarrierUp0._Rectangle, Color.White);

                    if (currentMaptile.GetSetBarrierRight1.GetSetMaptileNumber != GameVariables.defaultMapTileReferenced)
                        spriteBatch.Draw(_barrier, currentMaptile.GetSetBarrierRight1._Rectangle, Color.White);

                    if (currentMaptile.GetSetBarrierDown2.GetSetMaptileNumber != GameVariables.defaultMapTileReferenced)
                        spriteBatch.Draw(_barrier, currentMaptile.GetSetBarrierDown2._Rectangle, Color.White);

                    if (currentMaptile.GetSetBarrierLeft3.GetSetMaptileNumber != GameVariables.defaultMapTileReferenced)
                        spriteBatch.Draw(_barrier, currentMaptile.GetSetBarrierLeft3._Rectangle, Color.White);

                    //spriteBatch.Draw(_topRight, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    //_m.DrawMap(gameTime, spriteBatch);
                    //spriteBatch.Draw(_titleBackGround, b._Rectangle, Color.Black);

                    // Human Drawing Information
                    // Human Animation
                    if (_kbstate.IsKeyDown(Keys.W) == true || _kbstate.IsKeyDown(Keys.A) == true || _kbstate.IsKeyDown(Keys.S) == true || _kbstate.IsKeyDown(Keys.D) == true)
                    {
                        humanStep -= gameTime.ElapsedGameTime.TotalSeconds;
                        if (humanStep < 0)
                        {
                            _p.CheckPlayerDirection();
                            humanStep = GameVariables.stepSpeed;
                        }
                    }

                    // Drawing the player
                    spriteBatch.Draw(_p.CurrentPlayer, _p._Rectangle, Color.White);
                    Rectangle healthZone = new Rectangle(7, 15, 115, 115);
                    spriteBatch.Draw(_p._health, healthZone, Color.White);

                    // Zombie Drawing Information
                    // Zombie Animation
                    zombieStep -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (zombieStep < 0)
                    {
                        for (int i = 0; i < _zs.Count; i++)
                        {
                            _zs[i].CheckZombieDirection();
                            zombieStep = GameVariables.stepSpeed;
                        }
                    }

                    //drawing the ammo bar in the HUD
                    for (int i = 0; i < ammo; i++)
                    {
                        spriteBatch.Draw(gunAmmo, new Vector2(118 + 10 * i, 80), Color.White);
                    }

                    //drawing bullets
                    for (int i = 0; i < listBullet.Count; i++)
                    {
                        listBullet[i].Draw(spriteBatch);
                    }

                    for (int i = 0; i < dcs.Count; i++)
                    {
                        if (dcs[i].GetSetCollected != true)
                        {
                            spriteBatch.Draw(gunAmmo, dcs[i]._Rectangle, Color.White);
                        }
                    }

                    //drawing the score.
                    //spriteBatch.DrawString(printScore, "Score:" + score.ToString(), new Vector2(337, 25), Color.Gray);

                    
                    if (currentMaptile.extraction)
                    {
                        spriteBatch.Draw(_extraction, new Rectangle(400, 560, 256, 256), Color.White);
                        if (_Extraction.CheckCollision(_p))
                        {
                            gameState = GameState.GameOver;
                        }
                    }

                    //Drawing the zombies and checking for collisions
                    for (int i = 0; i < _zs.Count; i++)
                    {
                        if (_zs[i].CurrentZombie == null)
                        {
                            foreach (Zombie z in _zs)
                            {
                                z.UpA = this.Content.Load<Texture2D>("Zombie0A");
                                z.UpB = this.Content.Load<Texture2D>("Zombie0B");
                                z.RightA = this.Content.Load<Texture2D>("Zombie1A");
                                z.RightB = this.Content.Load<Texture2D>("Zombie1B");
                                z.DownA = this.Content.Load<Texture2D>("Zombie2A");
                                z.DownB = this.Content.Load<Texture2D>("Zombie2B");
                                z.LeftA = this.Content.Load<Texture2D>("Zombie3A");
                                z.LeftB = this.Content.Load<Texture2D>("Zombie3B");
                                z.CurrentZombie = z.UpA;
                                z.PreviousZombie = z.UpB;
                            }
                        }
                        spriteBatch.Draw(_zs[i].CurrentZombie, _zs[i]._Rectangle, Color.White);

                        if (_zs[i].CheckCollision(_p) == true)
                        {
                            // If one of the zombies makes contact with the player then...
                            spriteBatch.Draw(_p.CurrentPlayer, _p._Rectangle, Color.Red);   // Change player color to red

                            // Takes 1 hit then has "X" amount of time before the player is hurt again if still in contact
                            if (timer <= 0.0)
                            {
                                _p.TakeHit();
                                timer = 1.0;
                            }

                            // Gameover if the player's health is 0
                            if (_p.health <= 0)
                            {
                                gameState = GameState.GameOver;
                            }
                        }
                    }
                    break;

                // The Pause Screen
                case GameState.Pause:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Draw(_pauseBackground, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    // Identical to the above Main Menu Finite State Machine
                    switch (highlight)
                    {
                        // If the Resume Button is highlighted
                        case Pause.Resume:
                            {
                                spriteBatch.Draw(_pause, new Rectangle(465, 100, 350, 200), Color.White);
                                spriteBatch.Draw(_resume, new Rectangle(275, 275, 250, 150), Color.White);
                                spriteBatch.Draw(_quit, new Rectangle(725, 275, 195, 150), Color.Green);
                                break;
                            }

                        // If the Quit Button is highlighted
                        case Pause.Quit:
                            {
                                spriteBatch.Draw(_pause, new Rectangle(465, 100, 350, 200), Color.White);
                                spriteBatch.Draw(_resume, new Rectangle(275, 275, 250, 150), Color.Green);
                                spriteBatch.Draw(_quit, new Rectangle(725, 275, 195, 150), Color.White);
                                break;
                            }
                    }
                    break;

                // The GameOver Screen
                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.Black);
                    if (_p.health <= 0)
                    {
                        spriteBatch.Draw(_gameOver, new Rectangle(0, 0, 1280, 720), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_victory, new Rectangle(0, 0, 1280, 720), Color.White);
                    }
                    //spriteBatch.DrawString(_tFont, "PRESS ENTER TO GO TO THE MAIN MENU", new Vector2(480, 360), Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        private void AddProjectile(Vector2 position)
        {
            Bullet projectile = new Bullet();
            projectile.Initialize(GraphicsDevice.Viewport, _bullet, position,_p.DState);
            listBullet.Add(projectile);
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = listBullet.Count - 1; i >= 0; i--)
            {
                listBullet[i].Update(_p);
                if (listBullet[i].D == GamePiece.Direction.Up)
                {
                    listBullet[i].Texture = gunAmmo;
                }
                else if (listBullet[i].D == GamePiece.Direction.Right)
                {
                    listBullet[i].Texture = _bullet;
                }
                else if (listBullet[i].D == GamePiece.Direction.Left)
                {
                    listBullet[i].Texture = _bulletL;
                }
                else if (listBullet[i].D == GamePiece.Direction.Down)
                {
                    listBullet[i].Texture = _bulletD;
                }
                if (listBullet[i].Active == false)
                {
                    listBullet.RemoveAt(i);
                }
            }
        }

        private void bulletCollision()
        {
            Rectangle r1;
            Rectangle r2;

            //X,Y and then the 50,50 is the pixel by pixel width and height. Replace with actual vaules.
            r1 = new Rectangle(_p.XValue, _p.YValue, 50, 50);


            for (int i = 0; i < listBullet.Count; i++)
            {
                for (int j = 0; j < _zs.Count; j++)
                {
                    r1 = new Rectangle((int)listBullet[i].Position.X - listBullet[i].Width / 2, (int)listBullet[i].Position.Y - listBullet[i].Height / 2, listBullet[i].Width, listBullet[i].Height);
                    r2 = new Rectangle(_zs[j].XValue, _zs[j].YValue, 50, 50);
                    if (r1.Intersects(r2))
                    {
                        listBullet[i].Active = false;
                        _zs.RemoveAt(j);
                        score += 100;

                    }
                }
            }
        }
        //static void Start()
        //{
        //    _time = new Timer(60000); //starts the time for 1 minutes
        //    _time.Elapsed += new ElapsedEventHandler(_time_Elapsed);
        //    _time.Enabled = true;
        //}

        //public static void _time_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    score += 50;
        //    //throw new NotImplementedException();
        //}
    }
}
