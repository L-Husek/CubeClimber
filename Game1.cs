using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Formats.Asn1;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;

namespace CubeClimber
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        // TEXTURES ------------------------------------------------------------
        // Cube
        private Texture2D side1;
        private Texture2D side2;
        private Texture2D side3;
        private Texture2D side4;
        private Texture2D side5;
        private Texture2D side6;
        private Texture2D WhiteCube;
        private Texture2D PinkGlow;
        private Texture2D[] sides;
        // Panels
        private Texture2D hpanel;
        private Texture2D vpanel;
        private Texture2D cpanel;
        private Texture2D WPANEL;
        // Obstacles
        private Texture2D hpipe;
        private Texture2D vpipe;
        // Background Elements
        private Texture2D FactoryBackground;
        private Texture2D GreenMetal;
        private Texture2D MainMenu;
        private Texture2D AnimationElement1;
        // Testing
        private Texture2D MAN;
        // SOUNDS --------------------------------------------------------------
        private Song MovementSound;
        private Song HitMovementSound;
        private Song FallScraping;
        private Song WinJingle;
        private Song Dead;
        private Song Grabbed;
        // CONSTS --------------------------------------------------------------
        // Screen Dimensions
        public const int SCREEN_WIDTH = 414;
        public const int SCREEN_HEIGHT = 966;
        // Unit Size
        public const int UNIT = 80; // MUST BE DIVISIBLE BY 16
        // OTHER ---------------------------------------------------------------
        public int L = 1;
        public int[] PlayerLocation = {0, 0};
        public int[] CameraLocation = {0, 0};
        public bool Controls = true;
        public double zoom = 1.0;
        public SpriteFont R;
        public int ActiveAnim = 0;
        public double TIMER = 0;
        public double[] TempAnimCoords = { 0, 0 };
        public bool OneTimeMenu = true;
        public bool won;
        int WinSequence = 0;
        public bool DEAD = false;
        public bool isFalling = false;
        public int STUPIDLOCK = 0;
        public double AnimTimer1 = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // "_graphics" SETS WINDOW
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            // Summon all the levels using code from levelmanager
            LevelManager.Giveth();

            // This code will have to be moved into a function which activates it as needed.
            PlayerLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
            PlayerLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
            CameraLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
            CameraLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
            CubeManager.SideUp = LevelManager.Levels[L].PlayerStartSideUp;
            CubeManager.Rot = LevelManager.Levels[L].PlayerStartRot;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // BUNCHA BULL SHIP
            // Textures
            side1 = Content.Load<Texture2D>("RSide0");
            side2 = Content.Load<Texture2D>("RSide1");
            side3 = Content.Load<Texture2D>("RSide2");
            side4 = Content.Load<Texture2D>("RSide3");
            side5 = Content.Load<Texture2D>("RSide4");
            side6 = Content.Load<Texture2D>("RSide5");
            WhiteCube = Content.Load<Texture2D>("WhiteCube");
            PinkGlow = Content.Load<Texture2D>("PinkGlow");
            hpanel = Content.Load<Texture2D>("HorizontalPanels");
            vpanel = Content.Load<Texture2D>("VerticalPanels");
            cpanel = Content.Load<Texture2D>("CrossPanels");
            WPANEL = Content.Load<Texture2D>("WinPanel");
            hpipe = Content.Load<Texture2D>("HorizontalPipe");
            vpipe = Content.Load<Texture2D>("HorizontalPipe");
            FactoryBackground = Content.Load<Texture2D>("NiceFactory");
            GreenMetal = Content.Load<Texture2D>("GreenMetal");
            MainMenu = Content.Load<Texture2D>("MainMenu");
            AnimationElement1 = Content.Load<Texture2D>("AnimationElement1");
            MAN = Content.Load<Texture2D>("MAN");
            // Sounds
            MovementSound = Content.Load<Song>("MovementSound");
            HitMovementSound = Content.Load<Song>("HitMovementSound");
            FallScraping = Content.Load<Song>("FallScraping");
            WinJingle = Content.Load<Song>("WinJingle");
            Dead = Content.Load<Song>("Dead");
            Grabbed = Content.Load<Song>("Grabbed");
            // Fonts
            R = Content.Load<SpriteFont>("File");
            // Build Texture Array
            sides = new Texture2D[] { side1, side2, side3, side4, side5, side6, WhiteCube };
        }

        protected override void Update(GameTime gameTime)
        {
            // Quit Button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Tell InputManager to get most recent input information
            InputManager.Update();

            // Zoom Logic. Holding E decreases zoom multiplier (zooms in) and holding R increases zoom mutipler (zooms out)
            /*if (InputManager.EHold)
            {
                zoom *= 0.99;
            }
            else if (InputManager.RHold)
            {
                zoom *= 1.01;
            }*/

            if (InputManager.EnterPress)
            {
                OneTimeMenu = false;
            }

            AnimTimer1 += gameTime.ElapsedGameTime.TotalMilliseconds;
            AnimTimer1 %= 1200;
            
            // Control Logic. If two movement inputs happen on the same frame, they take priority of Up, Down, Left, Right.
            if (Controls) {
                if (InputManager.UpPress)
                {
                    if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1] - 1] != 'P') {
                        PlayerLocation[1] -= 1; // UP
                        CubeManager.Roll(2);
                        MediaPlayer.Play(MovementSound);
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W') {
                            WinSequence = 1;
                            Controls = false;
                        } else if (CubeManager.PanelDown == 'N' || (CubeManager.PanelDown == 'H' && Underneath != 'H' && Underneath != 'C') || (CubeManager.PanelDown == 'V' && Underneath != 'V' && Underneath != 'C')) {
                            isFalling = true;
                            MediaPlayer.Play(FallScraping);
                            TIMER = 0;
                            Controls = false;
                        }
                    } else {
                        MediaPlayer.Play(HitMovementSound);
                    }
                }
                else if (InputManager.DownPress)
                {
                    if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1] + 1] != 'P') {
                        PlayerLocation[1] += 1; // DOWN
                        CubeManager.Roll(0);
                        MediaPlayer.Play(MovementSound);
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W')
                        {
                            WinSequence = 1;
                            Controls = false;
                        }
                        else if (CubeManager.PanelDown == 'N' || (CubeManager.PanelDown == 'H' && Underneath != 'H' && Underneath != 'C') || (CubeManager.PanelDown == 'V' && Underneath != 'V' && Underneath != 'C'))
                        {
                            isFalling = true;
                            MediaPlayer.Play(FallScraping);
                            TIMER = 0;
                            Controls = false;
                        }
                    } else {
                        MediaPlayer.Play(HitMovementSound);
                    }
                }
                else if (InputManager.LeftPress)
                {
                    if (LevelManager.Levels[L].Layout[PlayerLocation[0] - 1, PlayerLocation[1]] != 'P') {
                        PlayerLocation[0] -= 1; // LEFT
                        CubeManager.Roll(1);
                        MediaPlayer.Play(MovementSound);
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W')
                        {
                            WinSequence = 1;
                            Controls = false;
                        }
                        else if (CubeManager.PanelDown == 'N' || (CubeManager.PanelDown == 'H' && Underneath != 'H' && Underneath != 'C') || (CubeManager.PanelDown == 'V' && Underneath != 'V' && Underneath != 'C'))
                        {
                            isFalling = true;
                            MediaPlayer.Play(FallScraping);
                            TIMER = 0;
                            Controls = false;
                        }
                    } else {
                        MediaPlayer.Play(HitMovementSound);
                    }
                }
                else if (InputManager.RightPress)
                {
                    if (LevelManager.Levels[L].Layout[PlayerLocation[0] + 1, PlayerLocation[1]] != 'P') {
                        PlayerLocation[0] += 1; // RIGHT
                        CubeManager.Roll(3);
                        MediaPlayer.Play(MovementSound);
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W')
                        {
                            WinSequence = 1;
                            Controls = false;
                        }
                        else if (CubeManager.PanelDown == 'N' || (CubeManager.PanelDown == 'H' && Underneath != 'H' && Underneath != 'C') || (CubeManager.PanelDown == 'V' && Underneath != 'V' && Underneath != 'C'))
                        {
                            isFalling = true;
                            MediaPlayer.Play(FallScraping);
                            TIMER = 0;
                            Controls = false;
                        }
                    } else {
                        MediaPlayer.Play(HitMovementSound);
                    }
                }
            }

            if (isFalling)
            {
                TIMER += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TIMER > 300) {
                    if (STUPIDLOCK == 0) {
                        STUPIDLOCK++;
                        PlayerLocation[1] += 1;
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W') {
                            WinSequence = 1;
                            Controls = false;
                            MediaPlayer.Stop();

                            isFalling = false;
                            TIMER = 0;
                            STUPIDLOCK = 0;
                        }
                        else if ((CubeManager.PanelDown == 'H' && (Underneath == 'H' || Underneath == 'C')) || (CubeManager.PanelDown == 'V' && (Underneath == 'V' || Underneath == 'C'))) {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(Grabbed);
                            Controls = true;

                            isFalling = false;
                            TIMER = 0;
                            STUPIDLOCK = 0;
                        }
                    }
                }
                if (TIMER > 600) {
                    if (STUPIDLOCK == 1) {
                        STUPIDLOCK++;
                        PlayerLocation[1] += 1;
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W') {
                            WinSequence = 1;
                            Controls = false;
                            MediaPlayer.Stop();

                            isFalling = false;
                            TIMER = 0;
                            STUPIDLOCK = 0;
                        }
                        else if ((CubeManager.PanelDown == 'H' && (Underneath == 'H' || Underneath == 'C')) || (CubeManager.PanelDown == 'V' && (Underneath == 'V' || Underneath == 'C'))) {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(Grabbed);
                            Controls = true;

                            isFalling = false;
                            TIMER = 0;
                            STUPIDLOCK = 0;
                        }
                    }
                }
                if (TIMER > 900) {
                    if (STUPIDLOCK == 2) {
                        STUPIDLOCK++;
                        PlayerLocation[1] += 1;
                        char Underneath = LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]];
                        if (LevelManager.Levels[L].Layout[PlayerLocation[0], PlayerLocation[1]] == 'W') {
                            WinSequence = 1;
                            Controls = false;
                            MediaPlayer.Stop();

                            isFalling = false;
                            TIMER = 0;
                            STUPIDLOCK = 0;
                        }
                        else if ((CubeManager.PanelDown == 'H' && (Underneath == 'H' || Underneath == 'C')) || (CubeManager.PanelDown == 'V' && (Underneath == 'V' || Underneath == 'C'))) {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(Grabbed);
                            Controls = true;

                            isFalling = false;
                            TIMER = 0;
                            STUPIDLOCK = 0;
                        }
                    }
                }
                if (TIMER > 1200)
                {
                    isFalling = false;
                    TIMER = 0;
                    STUPIDLOCK = 0;
                    DEAD = true;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(Dead);
                }
            }

            if (WinSequence > 0) {
                TIMER += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (WinSequence == 1) {
                    CubeManager.SideUp = 6;
                    MediaPlayer.Play(WinJingle);
                    WinSequence = 2;
                } else if (WinSequence == 2 && TIMER > 1000) {
                    L = (L + 1) % 4;
                    PlayerLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
                    PlayerLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
                    CameraLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
                    CameraLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
                    CubeManager.SideUp = LevelManager.Levels[L].PlayerStartSideUp;
                    CubeManager.Rot = LevelManager.Levels[L].PlayerStartRot;
                    Controls = true;
                    TIMER = 0;
                    WinSequence = 0;
                }
            }

            if (DEAD) {
                PlayerLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
                PlayerLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
                CameraLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
                CameraLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
                CubeManager.SideUp = LevelManager.Levels[L].PlayerStartSideUp;
                CubeManager.Rot = LevelManager.Levels[L].PlayerStartRot;
                DEAD = false;
                Controls = true;
            }

            // Update Camera Location
            if (CameraLocation[0] < PlayerLocation[0] - 1) {
                CameraLocation[0] += 1;
            }
            if (CameraLocation[0] > PlayerLocation[0] + 1) {
                CameraLocation[0] -= 1;
            }
            if (CameraLocation[1] < PlayerLocation[1] - 1) {
                CameraLocation[1] += 1;
            }
            if (CameraLocation[1] > PlayerLocation[1] + 1) {
                CameraLocation[1] -= 1;
            }

            // Debug/Cheats
            if (InputManager.PlusPress || InputManager.MinusPress)
            {
                if (InputManager.PlusPress){
                    L = (L + 1) % 4;
                } else
                {
                    L = (L + 3) % 4;
                }
                PlayerLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
                PlayerLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
                CameraLocation[0] = LevelManager.Levels[L].PlayerStartPlace[0];
                CameraLocation[1] = LevelManager.Levels[L].PlayerStartPlace[1];
                CubeManager.SideUp = LevelManager.Levels[L].PlayerStartSideUp;
                CubeManager.Rot = LevelManager.Levels[L].PlayerStartRot;
            }


            base.Update(gameTime); 
        }

        protected override void Draw(GameTime gameTime)
        {
            // Set default background color
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // START
            _spriteBatch.Begin();
            // BACKGROUND LAYER
            _spriteBatch.Draw(FactoryBackground, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.Gray);
            // LEVEL LAYER
            _spriteBatch.Draw(GreenMetal, new Rectangle(CamXFix(0), CamYFix(0), UNIT * LevelManager.Levels[L].Layout.GetLength(0), UNIT * LevelManager.Levels[L].Layout.GetLength(1)), Color.White);
            for (int i = 0; i < LevelManager.Levels[L].Layout.GetLength(0); i++)
            {
                for (int j = 0; j < LevelManager.Levels[L].Layout.GetLength(1); j++)
                {
                    if (LevelManager.Levels[L].Layout[i, j] == 'H') {
                        _spriteBatch.Draw(hpanel, new Rectangle(CamXFix(i * UNIT), CamYFix(j * UNIT), UNIT, UNIT), Color.White);
                    }
                    else if (LevelManager.Levels[L].Layout[i, j] == 'V') {
                        _spriteBatch.Draw(vpanel, new Rectangle(CamXFix(i * UNIT), CamYFix(j * UNIT), UNIT, UNIT), Color.White);
                    }
                    else if (LevelManager.Levels[L].Layout[i, j] == 'C') {
                        _spriteBatch.Draw(cpanel, new Rectangle(CamXFix(i * UNIT), CamYFix(j * UNIT), UNIT, UNIT), Color.White);
                    }
                    else if (LevelManager.Levels[L].Layout[i, j] == 'P') {
                        _spriteBatch.Draw(hpipe, new Rectangle(CamXFix(i * UNIT), CamYFix(j * UNIT), UNIT, UNIT), Color.White);
                    }
                    else if (LevelManager.Levels[L].Layout[i, j] == 'W') {
                        _spriteBatch.Draw(AnimationElement1, new Rectangle(CamXFix(i * UNIT + UNIT/2), CamYFix(j * UNIT + UNIT/2), UNIT, UNIT), new Rectangle?(), Color.White, (float)AnimTimer1/1200 * (float)Math.PI * 2, new Vector2(50f, 50f), SpriteEffects.None, 0.9f);
                        _spriteBatch.Draw(WPANEL, new Rectangle(CamXFix(i * UNIT), CamYFix(j * UNIT), UNIT, UNIT), Color.White);
                    }
                }
            }
            // CUBE SHADOW
            _spriteBatch.Draw(PinkGlow, new Rectangle(CamXFix(PlayerLocation[0] * UNIT - UNIT), CamYFix(PlayerLocation[1] * UNIT - UNIT), UNIT * 3, UNIT * 3), Color.White);
            _spriteBatch.Draw(sides[CubeManager.SideUp], new Rectangle(CamXFix(PlayerLocation[0] * UNIT + UNIT / 2), CamYFix(PlayerLocation[1] * UNIT + UNIT / 2), UNIT / 8 * 9, UNIT / 8 * 9), new Rectangle?(), Color.White, CubeManager.Rot * 90 * (float)Math.PI / 180, new Vector2(432, 432), SpriteEffects.None, 1f);
            // END
            /*
            _spriteBatch.DrawString(R, zoom + "", Vector2.Zero, Color.Red);
            _spriteBatch.DrawString(R, CubeManager.SideUp + "", new Vector2(0, 20), Color.Red);
            _spriteBatch.DrawString(R, CubeManager.Rot + "" , new Vector2(0, 40), Color.Red);
            _spriteBatch.DrawString(R, AnimTimer1/1200*(float)Math.PI*2 + "", new Vector2(0, 60), Color.Red);
            */
            // ONE-TIME MENU
            if (OneTimeMenu)
            {
                _spriteBatch.Draw(MainMenu, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        
        // CUSTOM FUNCTIONS

        // INPUT: X COORDINATE WITH CENTERED ORIGIN, OUTPUT: X COORDINATE WITH TOP LEFT ORIGIN
        public int Wfix(int i)
        {
            return ((SCREEN_WIDTH / 2) - i);
        }

        // INPUT: Y COORDINATE WITH CENTERED ORIGIN, OUTPUT: Y COORDINATE WITH TOP LEFT ORIGIN
        public int Hfix(int i)
        {
            return ((SCREEN_HEIGHT / 2) - i);
        }

        public int CamXFix(int i)
        {
            return i + SCREEN_WIDTH/2 - UNIT/2 - CameraLocation[0]*UNIT;
        }
        public int CamYFix(int i)
        {
            return i + SCREEN_HEIGHT/2 - UNIT/2 - CameraLocation[1]*UNIT;
        }
    }
}