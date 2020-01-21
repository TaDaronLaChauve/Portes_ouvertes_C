/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Classe :         SpriteGeneric.cs
 * Description:     Classe parent dont les autres classes vont hériter
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
    class SpriteGeneric
    {
        protected Game _game;
        protected Texture2D _texture;
        public Vector2 _position;
        public Rectangle _hitBox;

        public int _height;
        public int _width;
        public SpriteGeneric(Game game)
        {
            _game = game;
        }
        public virtual void Initialize(Vector2 position)
        {
            _position = position;
        }
        
        public void LoadContent(Texture2D texture)
        {
            _texture = texture;
            _width = _texture.Width;
            _height = _texture.Height;
        }
        public void UploadContent()
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Défini les dimensions et la position du rectangle pour pouvoir interagir avec les autres objets
            _hitBox = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
