/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Classe :         Canon.cs
 * Hérite de :      SpriteGeneric.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paratrooper
{
    class Canon : SpriteGeneric
    {
        public Vector2 spriteOriginCanon;
        float _rotation;
        int _vitesseSpawnBouletInitial = 200;
        //A quel intervale de temps les boulets peuvent être tirés
        TimeSpan _bouletSpawnTime;
        TimeSpan _previousBouletSpawnTime;
        List<Boulet> listeBoulets = new List<Boulet>();
        Boulet boulet;
        public int VitesseSpawnBouletInitial { get => _vitesseSpawnBouletInitial; set => _vitesseSpawnBouletInitial = value; }
        public TimeSpan BouletSpawnTime { get => _bouletSpawnTime; set => _bouletSpawnTime = value; }

        public float Rotation {
            get => _rotation;
            set {
                //On empêche la rotation complète du canon
                if (value >= -1.6f && value <= 1.6f)
                {
                    _rotation = value;
                }
            } }

        public List<Boulet> ListeBoulets { get => listeBoulets; set => listeBoulets = value; }

        public Canon(Game game) : base(game)
        {
        }
        public virtual void Initialize(Vector2 position, int vitesseSpawnBouletInitial, float rotation)
        {
            _position = position;
            VitesseSpawnBouletInitial = vitesseSpawnBouletInitial;
            BouletSpawnTime = TimeSpan.FromMilliseconds(VitesseSpawnBouletInitial);
            _previousBouletSpawnTime = TimeSpan.Zero;
            ListeBoulets = new List<Boulet>();
            Rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            //On détermine l'axe de rotation du canon
            spriteOriginCanon = new Vector2(_texture.Width / 2, _texture.Height / 2 + 10);

            //Mouvement du canon
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) Rotation += 0.04f;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) Rotation -= 0.04f;

            //Apparition boulet avec le bon angle et après un interval de temps prédéfini
            if (!Keyboard.GetState().IsKeyDown(Keys.Space) && gameTime.TotalGameTime - _previousBouletSpawnTime >= BouletSpawnTime)
            {
                _previousBouletSpawnTime = gameTime.TotalGameTime;
                //boulet direction;
                float rotation = Rotation + 1.6f;

                //Calcule pourcentage en fonction de la valeur de rotation
                float pourcentage = 100 - (rotation * 100 / 3.2f);
                //Correspondance entre le pourcentage et l'angle
                float angle = pourcentage * 180.0f / 100;

                //Calcule du x du vecteur à attribuer au boulet
                float x = (float)(Math.Sqrt(Math.Pow(5.0, 2.0) + Math.Pow(5.0, 2.0)) * Math.Cos(Math.PI * angle / 180.0));
                //Calcule du y du vecteur à attribuer au boulet
                float y = (float)-(Math.Sqrt(Math.Pow(5.0, 2.0) + Math.Pow(5.0, 2.0)) * Math.Sin(Math.PI * angle / 180.0));

                boulet = new Boulet(_game);
                ListeBoulets.Add(boulet);
                ListeBoulets[ListeBoulets.Count - 1].LoadContent(_game.Content.Load<Texture2D>("ballTrans"));
                //Initialisation de la position du boulet à partir de l'axe de rotation du canon
                //en le faisant avancer de deux fois dans la direction de l'angle calculé
                ListeBoulets[ListeBoulets.Count - 1].Initialize(new Vector2(_position.X - boulet._width/2 +  (2 * x),
                                                                            _position.Y - boulet._height/2 + (2 * y)));
                ListeBoulets[ListeBoulets.Count - 1]._direction = new Vector2(x, y);
            }
            //Pour tous les boulets qui sont à l'extérieure on les supprime de la liste
            ListeBoulets.RemoveAll(b => b._estDetruite == true);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            _hitBox = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
            //Dessine le canon avec l'axe de rotation existant
            spriteBatch.Draw(_texture, _position, null, Color.White, Rotation, spriteOriginCanon, 1f, SpriteEffects.None, 0);
        }
    }
}
