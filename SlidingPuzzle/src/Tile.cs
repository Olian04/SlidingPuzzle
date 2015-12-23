using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlidingPuzzle.src
{
    class Tile
    {
        private static List<Texture2D> textures;
        private static Texture2D blank;

        public static void Init(ContentManager Content) {
            textures = new List<Texture2D>();
            blank = Content.Load<Texture2D>("pixel.png");
            textures.Add(blank);
            for (int i = 1; i < 9; i++) {
                textures.Add(Content.Load<Texture2D>("nr" + i + ".png"));
            }
        }

        //---------------------------------------------------//

        Texture2D texture;
        Rectangle rect;

        public Tile(int textureIndex) {
            texture = textures[textureIndex];
            rect = new Rectangle(0,0,10,10);
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (Game1.won)
                spriteBatch.Draw(texture, rect, Color.Green);
            else if (texture != blank)
                spriteBatch.Draw(texture, rect, Color.White);
            else
                spriteBatch.Draw(texture, rect, Color.Gray);
        }

        public void setPos(Point pos) {
            rect.Location = pos;
        }
        public void setSize(Point size) {
            rect.Size = size;
        }
        public Rectangle getRect() {
            return rect;
        }
        public bool isBlank() {
            return texture == blank;
        }
        
    }
}
