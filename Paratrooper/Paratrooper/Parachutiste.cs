/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Classe :         Parachutiste.cs
 * Hérite de :      SpriteGeneric.cs
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paratrooper
{
    class Parachutiste : SpriteGeneric
    {
        public Parachutiste(Game game) : base(game)
        {

        }
        public virtual void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            //On fait avancer le parachutiste
            _position.Y += 1.9f;
        }
    }
}
