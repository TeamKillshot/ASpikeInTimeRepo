using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt.Components
{
    public class CollisionDetection : GameComponent
    {
        PlayerComponent Player;
        TRender Tiles;
        List<Collider> tilesInCollision = new List<Collider>();

        public CollisionDetection(Game game) : base(game)
        {
            tilesInCollision = Tiles.collisons.Where(c => c.collider.Intersects(Player.Bounds)).ToList();
            Player = Game.Services.GetService<PlayerComponent>();
            Tiles = Game.Services.GetService<TRender>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Collider c in tilesInCollision)
            {
                c.collisionColor = Color.Red;

                if(!c.collider.Intersects(Player.Bounds))
                {
                    RemoveTileFromList(c);
                }
            }

            base.Update(gameTime);
        }

        public void RemoveTileFromList(Collider c)
        {
            tilesInCollision.Remove(c);
        }
    }
}
