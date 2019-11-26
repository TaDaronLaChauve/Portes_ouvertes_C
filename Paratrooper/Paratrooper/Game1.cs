/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Description :    Le but du jeu est de tirer sur les ennemies arrivant du ciel
 *                  avant qu'ils ne touchent le sol.
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Paratrooper
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Déclaration des variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        ParallaxingBackground bgGrosNuage;
        ParallaxingBackground bgPetitsNuages;
        Rectangle rectBackground;
        
        Canon canon;

        
        Bonus bonus;
        BonusCanon bonusCanon;
        List<Canon> listeCanons;
        List<Parachutiste> listeParachutistes;
        List<Parachutiste> listeParachutistesImmobile;
        Random rd = new Random();
        bool sortirCanon;
        bool sortirBoulet;
        bool sortirParachutiste;
        TimeSpan previousParachuteSpawnTime;
        TimeSpan parachuteSpawnTime;

        TimeSpan tempsEntreNiveaux;
        TimeSpan anciensTempsEntreNiveaux;

        TimeSpan tempsApparitionBonus;
        TimeSpan anciensTempsApparitionBonus;

        TimeSpan tempsApparitionBonusCanon;
        TimeSpan anciensTempsApparitionBonusCanon;

        TimeSpan DureeBonus;
        TimeSpan anciensDureeBonus;

        //Pour les boutons
        List<Component> _gameComponents;
        //Le score est le nombre d'ennemis tués
        SpriteFont font;

        int nbParachutistesMaxParVague;
        //Nombre d'ennemis au sol nécessaire pour mourire
        const int NB_ENNEMIS_IMMOBILES = 10;
        const int NB_PARACHUTISTES_MAX_DERNIERE_VAGUE = 21;
        int score;
        bool bonusVisible;
        bool menu = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            //Pour l'affichage en FullScreen

            //graphics.IsFullScreen = true;
            //graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.ApplyChanges();
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
            //Initialisation des variables
            listeCanons = new List<Canon>();
            canon = new Canon(this);
            listeCanons.Add(canon);
            listeCanons[listeCanons.Count - 1].Initialize(new Vector2(GraphicsDevice.Viewport.Width/2, 7 * (GraphicsDevice.Viewport.Height/8)), 300, 0.0f);

            bonus = new Bonus(this);
            bonusCanon = new BonusCanon(this);

            
            listeParachutistes = new List<Parachutiste>();
            listeParachutistesImmobile = new List<Parachutiste>();

            sortirCanon = false;
            sortirBoulet = false;
            sortirParachutiste = false;

            //A quel intervale de temps les ennemies tombes
            parachuteSpawnTime = TimeSpan.FromMilliseconds(1500);
            previousParachuteSpawnTime = TimeSpan.Zero;

            
            
            //ParallaxingBackground
            bgGrosNuage = new ParallaxingBackground();
            bgPetitsNuages = new ParallaxingBackground();

            //Le temps entre l'ajout de parachutiste maximum
            tempsEntreNiveaux = TimeSpan.FromMilliseconds(10000);//10000
            anciensTempsEntreNiveaux = TimeSpan.Zero;

            //le temps entre chaque apparition de bonus
            tempsApparitionBonus = TimeSpan.FromSeconds(rd.Next(13,20));
            anciensTempsApparitionBonus = TimeSpan.Zero;

            tempsApparitionBonusCanon = TimeSpan.FromSeconds(rd.Next(20, 30));
            anciensTempsApparitionBonusCanon = TimeSpan.Zero;

            //On commence avec 3 -1 parachutiste maximum vu que c'est utilisé pour un random
            nbParachutistesMaxParVague = 3;

            score = 0;

            bonusVisible = false;
            //Le temps que le bonus sera actif
            DureeBonus = TimeSpan.FromSeconds(5);
            anciensDureeBonus = TimeSpan.Zero;

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

            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("backgroundSolSocle");
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            bgGrosNuage.Initialize(Content, "GrosNuage", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);
            bgPetitsNuages.Initialize(Content, "PetitsNuages", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);

            listeCanons[listeCanons.Count - 1].LoadContent(Content.Load<Texture2D>("canonTrans"));

            

            //Déclaration des bouttons avec leur positions, texte, image
            font = Content.Load<SpriteFont>("Arial");
            var restartButton = new Button(Content.Load<Texture2D>("Images/Button"), font)
            {
                Position = new Vector2(GraphicsDevice.Viewport.Width / 2 - (Content.Load<Texture2D>("Images/Button").Width/2), GraphicsDevice.Viewport.Height/2 - (Content.Load<Texture2D>("Images/Button").Height)),
                Text = "Recommencer",
            };
            restartButton.Click += RestartButton_Click;
            var quitButton = new Button(Content.Load<Texture2D>("Images/Button"), font)
            {
                Position = new Vector2(GraphicsDevice.Viewport.Width / 2 - (Content.Load<Texture2D>("Images/Button").Width / 2), GraphicsDevice.Viewport.Height / 2 + 10),
                Text = "Quitter",
            };
            quitButton.Click += QuitButton_Click;
            _gameComponents = new List<Component>()
            {
                restartButton,
                quitButton,
            };


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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                //Exit();
                foreach (var component in _gameComponents)
                    component.Update(gameTime);
                menu = true;
            }
                

            UpdateCanon(gameTime);
            UpdateBonus(gameTime);
            UpdateBoulet(gameTime);
            UpdateParachutiste(gameTime);
            
            bgGrosNuage.Update(gameTime);
            bgPetitsNuages.Update(gameTime);
            //Si le nombre d'ennemis maximal à terre a été atteint alors on update nos boutons pour les afficher ensuite
            if (listeParachutistesImmobile.Count >= NB_ENNEMIS_IMMOBILES || menu == true)
            {
                foreach (var component in _gameComponents)
                    component.Update(gameTime);
                anciensTempsApparitionBonus = gameTime.TotalGameTime;
                anciensTempsApparitionBonusCanon = gameTime.TotalGameTime;
            }
            //Quand le bonus se termine
            if (gameTime.TotalGameTime - anciensDureeBonus >= DureeBonus && bonus.bonusVisible == false)
            {
                anciensDureeBonus = gameTime.TotalGameTime;
                foreach (Canon canon in listeCanons)
                {
                    canon.BouletSpawnTime = TimeSpan.FromMilliseconds(canon.VitesseSpawnBouletInitial);
                }
                
            }
            //Petit bonus fun caché pour ajouter énormément de canon :)
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                listeCanons.Add(new Canon(this));
                listeCanons[listeCanons.Count - 1].LoadContent(Content.Load<Texture2D>("canonTrans"));
                listeCanons[listeCanons.Count - 1].Initialize(new Vector2(rd.Next(1, 105) * (GraphicsDevice.Viewport.Width / 100), 7 * (GraphicsDevice.Viewport.Height / 8)), 400, listeCanons[0].Rotation);
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Met à jours chaque canon de la liste
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateCanon(GameTime gameTime)
        {
            foreach (Canon canon in listeCanons)
            {
                canon.Update(gameTime);
            }
            
        }
        /// <summary>
        /// On met à jours les boulets de canon et on détectes les collisions avec les bonus et les parachutistes
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateBoulet(GameTime gameTime)
        {

            //Mettre à jours chaque boulet pour les faire avancer
            // Et faire la collision avec parachutiste
            foreach (Canon canon in listeCanons)
            {
                foreach (Boulet b in canon.ListeBoulets)
                {
                    //Maj
                    b.Update(gameTime);

                    //Si collision avec bonus rafale
                    if (b._hitBox.Intersects(bonus._hitBox) && bonus.bonusVisible == true)
                    {
                        anciensDureeBonus = gameTime.TotalGameTime;
                        bonus.bonusVisible = false;
                        canon.BouletSpawnTime = TimeSpan.FromMilliseconds(20);
                    }
                    //Si collision avec bonus canon supplémentaire
                    if (b._hitBox.Intersects(bonusCanon._hitBox) && bonusCanon.bonusVisible == true)
                    {
                        bonusCanon.bonusVisible = false;
                        listeCanons.Add(new Canon(this));
                        listeCanons[listeCanons.Count - 1].LoadContent(Content.Load<Texture2D>("canonTrans"));
                        //S'il on a deux canon avec le bonus on le positionne à 1 huitième de l'écrans à gauche du canon du milieu
                        if (listeCanons.Count == 2)
                        {
                            listeCanons[listeCanons.Count - 1].Initialize(new Vector2(GraphicsDevice.Viewport.Width / 8 * 3 - (canon._width/2), 7 * (GraphicsDevice.Viewport.Height / 8)), 400, listeCanons[0].Rotation);
                        }
                        //S'il on a trois canon avec le bonus on le positionne à 1 huitième de l'écrans à droite du canon du milieu
                        else if (listeCanons.Count == 3)
                        {
                            listeCanons[listeCanons.Count - 1].Initialize(new Vector2(GraphicsDevice.Viewport.Width / 8 * 5 + (canon._width / 2), 7 * (GraphicsDevice.Viewport.Height / 8)), 400, listeCanons[0].Rotation);
                        }
                        sortirCanon = true;
                    }
                    //Collision avec parachutistes
                    foreach (Parachutiste p in listeParachutistes)
                    {
                        if (b._hitBox.Intersects(p._hitBox))
                        {
                            score++;
                            listeParachutistes.Remove(p);
                            canon.ListeBoulets.Remove(b);
                            sortirBoulet = true;
                        }
                        if (sortirBoulet)
                            break;
                    }
                    if (sortirBoulet)
                        break;
                }
                if (sortirCanon)
                    break;
            }
            sortirCanon = false;
            sortirBoulet = false;
        }
        /// <summary>
        /// On met à jours les ennemis pour les faire apparaître et détecter s'ils arrivent au niveau du sol
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateParachutiste(GameTime gameTime)
        {
            //Si l'intervalle de temps entre le passage de niveau est atteint
            //alors on rajoute un ennemi au nombre maximal d'ennemis possible
            if (gameTime.TotalGameTime - anciensTempsEntreNiveaux >= tempsEntreNiveaux && nbParachutistesMaxParVague < NB_PARACHUTISTES_MAX_DERNIERE_VAGUE)
            {
                anciensTempsEntreNiveaux = gameTime.TotalGameTime;
                nbParachutistesMaxParVague++;
            }

            //Pour faire apparaître les ennemis à un interval de temps précis
            if (gameTime.TotalGameTime - previousParachuteSpawnTime >= parachuteSpawnTime)
            {
                previousParachuteSpawnTime = gameTime.TotalGameTime;
                //Random du nombre de parachutistes à faire apparaître
                int nbParachutistes = rd.Next(1, nbParachutistesMaxParVague);
                for (int i = 0; i < nbParachutistes; i++)
                {
                    listeParachutistes.Add(new Parachutiste(this));
                    listeParachutistes[listeParachutistes.Count - 1].LoadContent(Content.Load<Texture2D>("skyDiverTrans"));
                    listeParachutistes[listeParachutistes.Count - 1].Initialize(new Vector2(rd.Next(0, GraphicsDevice.Viewport.Width - listeParachutistes[listeParachutistes.Count - 1]._width), 0));
                }
            }
            //Parcours la liste pour les actualiser et voir s'ils arrivent sur le sol
            foreach (Parachutiste p in listeParachutistes)
            {
                p.Update(gameTime);
                //Arrive en bas
                if (p._hitBox.Bottom >= GraphicsDevice.Viewport.Height - (GraphicsDevice.Viewport.Height / 9.5f) - (listeCanons[listeCanons.Count-1]._height / 2) && p._hitBox.Center.X > listeCanons[listeCanons.Count - 1]._position.X - listeCanons[listeCanons.Count - 1]._width/2 && p._hitBox.Center.X < listeCanons[listeCanons.Count - 1]._position.X + listeCanons[listeCanons.Count - 1]._width /2)
                    ArretParachutiste(p);
                else if (p._hitBox.Bottom >= GraphicsDevice.Viewport.Height - (GraphicsDevice.Viewport.Height / 9.5f))
                    ArretParachutiste(p);

                if (sortirParachutiste)
                    break;
            }
            sortirParachutiste = false;
        }
        private void UpdateBonus(GameTime gameTime)
        {
            //Pour faire apparaître le bonus rafale à un intervale de temps précis
            if (gameTime.TotalGameTime - anciensTempsApparitionBonus >= tempsApparitionBonus)
            {
                anciensTempsApparitionBonus = gameTime.TotalGameTime;
                tempsApparitionBonus = TimeSpan.FromSeconds(rd.Next(13, 20));

                bonus.Apparition("Bonus");
                //bonus.LoadContent(Content.Load<Texture2D>("Bonus"));
                //bonus.Initialize(new Vector2(rd.Next(0, GraphicsDevice.Viewport.Width - bonus._width), 0));
                //bonusVisible = true;
            }
            //Pour faire apparaître le bonus d'un canon supplémentaire à un intervale de temps précis
            if (gameTime.TotalGameTime - anciensTempsApparitionBonusCanon >= tempsApparitionBonusCanon)
            {
                anciensTempsApparitionBonusCanon = gameTime.TotalGameTime;
                tempsApparitionBonusCanon = TimeSpan.FromSeconds(rd.Next(20, 30));
                if(listeCanons.Count < 3)
                bonusCanon.Apparition("canonTrans");
            }
            if (bonus.bonusVisible)
            {
                bonus.Update(gameTime);
            }
            if (bonusCanon.bonusVisible)
            {
                bonusCanon.Update(gameTime);
            }
        }
        /// <summary>
        /// On supprime l'ennemi qui à touché le sol de la liste des parrachutistes et on l'ajoute 
        /// dans la liste de ceux qui sont immobile.
        /// </summary>
        /// <param name="p"></param>
        private void ArretParachutiste(Parachutiste p)
        {
            p.LoadContent(Content.Load<Texture2D>("persoTrans"));
            listeParachutistes.Remove(p);
            listeParachutistesImmobile.Add(p);
            sortirParachutiste = true;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            spriteBatch.Draw(background, rectBackground, Color.White);
            bgPetitsNuages.Draw(spriteBatch);
            bgGrosNuage.Draw(spriteBatch);

            //Si ce n'est pas fini on dessine les parachutistes et les boulets
            if (listeParachutistesImmobile.Count < NB_ENNEMIS_IMMOBILES && menu == false)
            {
                foreach (Canon canon in listeCanons)
                {
                    foreach (Boulet b in canon.ListeBoulets)
                        b.Draw(spriteBatch);
                }
                

                foreach (Parachutiste p in listeParachutistes)
                    p.Draw(spriteBatch);

                if (bonus.bonusVisible)
                    bonus.Draw(spriteBatch);
                if (bonusCanon.bonusVisible)
                    bonusCanon.Draw(spriteBatch);
            }
            else//Autrement on arrête de les dessiner et on dessine les boutons et le curseur
            {
                IsMouseVisible = true;
                foreach (var component in _gameComponents)
                    component.Draw(gameTime, spriteBatch);
            }

            //Dessine chacun des parachutistes qui se trouvent sur le sol
            if (listeParachutistesImmobile.Count >= 1)
            {
                foreach (Parachutiste p in listeParachutistesImmobile)
                    p.Draw(spriteBatch);
            }
            //Affiche le score et le nombre d'ennemis au sol
            spriteBatch.DrawString(font, "Score: " + score, new Vector2((GraphicsDevice.Viewport.Width / 2) - (2 *(GraphicsDevice.Viewport.Width / 5)), GraphicsDevice.Viewport.Height - ( GraphicsDevice.Viewport.Height / 13.4f)), Color.Black);
            spriteBatch.DrawString(font, "Ennemis au sol: " + listeParachutistesImmobile.Count + " / " + NB_ENNEMIS_IMMOBILES, new Vector2((GraphicsDevice.Viewport.Width / 2) + (GraphicsDevice.Viewport.Width / 5), GraphicsDevice.Viewport.Height - (GraphicsDevice.Viewport.Height / 13.4f)), Color.Black);
            foreach (Canon canon in listeCanons)
            {
                canon.Draw(spriteBatch);
            }
            

            spriteBatch.End();
            base.Draw(gameTime);
        }
        private void RestartButton_Click(object sender, System.EventArgs e)
        {
            Initialize();
            LoadContent();
            IsMouseVisible = false;
            menu = false;
            
        }
        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

    }
}
