/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Classe :         Bonus.cs
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
    class Bonus : SpriteGeneric
    {
        Random rd = new Random();
        public bool bonusVisible = false;
        public Bonus(Game game) : base(game)
        {

        }
        public virtual void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            //On fait avancer le bonus
            _position.Y += 2f;
        }
        public void Apparition(string img)
        {
            LoadContent(_game.Content.Load<Texture2D>(img));
            Initialize(new Vector2(rd.Next(0, _game.GraphicsDevice.Viewport.Width - _width), 0));
            bonusVisible = true;
        }
    }
}
