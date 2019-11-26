/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Classe :         Boulet.cs
 * Hérite de :      SpriteGeneric.cs
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paratrooper
{
    class Boulet : SpriteGeneric
    {
        public Vector2 _direction;
        public bool _estDetruite = false;
        public Boulet(Game game) : base(game)
        {

        }
        public void Initialize(Vector2 position, Vector2 direction)
        {

        }
        public override void Update(GameTime gameTime)
        {
            //On fait avancer le boulet avec les coordonées du vecteur de l'angle obtenu à partir de l'angle du canon
            _position.X += _direction.X * 2;
            _position.Y += _direction.Y * 2;
            if (_position.X + _texture.Width < 0 || _position.X > _game.GraphicsDevice.Viewport.Width)
            {
                _estDetruite = true;
            }
            if (_position.Y + _texture.Height < 0)
            {
                _estDetruite = true;
            }

        }
    }
}
