/* Nom, prénom :    Wohlers, Luca
 * Classe :         I.FA-P3A
 * Date :           08 / 10 / 2019
 * Pojet :          Paratrooper
 * Classe :         ParallaxingBackground.cs
 * Description :    Créer le mouvement des nuages.
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Paratrooper
{
    class ParallaxingBackground
    {
        //La texture de l'image du ParallaxingBackground
        Texture2D texture;
        //Un tableau de positions pour le ParallaxingBackground
        Vector2[] positions;
        //La vitesse à laquelle le ParallaxingBackground bouge
        int speed;
        int bgHeight;
        int bgWidth;
        public void Initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight, int speed)
        {
            bgHeight = screenHeight;
            bgWidth = screenWidth;

            //Charge la texture du ParallaxingBackground
            texture = content.Load<Texture2D>(texturePath);

            //Définit sa vitesse
            this.speed = speed;

            //Si on divise la largeur de l'écran avec avec la largeur du ParallaxingBackground on peut determiner
            //le nombre de "colonne de pixels" dont nous avons besoins, on ajoute 2 pour être sur en fonction de la
            //vitesse à laquel le ParallaxingBackground se déplace
            positions = new Vector2[screenWidth / texture.Width + 2];

            //Définit la position initial du ParallaxingBackground
            for (int i = 0; i < positions.Length; i++)
            {
                //On a besoins des bord de l'image d'être côte à côte pour pas que l'image soit coupée
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }
        public void Update(GameTime gametime)
        {
            //Met à jours la position du ParallaxingBackground
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].X += speed;
                //Si le ParallaxingBackground se déplace à gauche
                if (speed <= 0)
                {
                    //Si le ParallaxingBackground est hors de la fenêtre on le repositionne
                    if (positions[i].X <= -texture.Width * (positions.Length - 1))//(positions[i].X <= -texture.Width / 1.2f)
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }
                //Si le ParallaxingBackground se déplace à droite
                else
                {
                    //Si le ParallaxingBackground est hors de la fenêtre on le repositionne
                    if (positions[i].X >= texture.Width * (positions.Length - 1))
                    {
                        positions[i].X = -texture.Width;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y, bgWidth, bgHeight);
                spriteBatch.Draw(texture, rectBg, Color.White);
            }
        }
    }
}
