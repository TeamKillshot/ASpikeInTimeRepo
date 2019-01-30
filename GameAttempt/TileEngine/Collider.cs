using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt
{
    public class Collider
    {
        public Rectangle collider;
        Texture2D collisonImage;
        bool visible = false;
        public Color collisionColor = Color.White;

        public Collider(Texture2D Image, Vector2 startPos, Vector2 size)
        {
            collisonImage = Image;
            collider = new Rectangle((int)startPos.X, (int)startPos.Y, (int)size.X, (int)size.Y);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if(visible)
			spriteBatch.Draw(collisonImage, collider, collisionColor);
		}
	}
}
