using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonTools.ECS;
using Moonlight.Components;
using System;

namespace Moonlight.Renderers
{
    public class SpriteRenderer : Renderer
    {
        Filter SpriteFilter;
        SpriteBatch SpriteBatch;

        public SpriteRenderer(World world, SpriteBatch spriteBatch) : base(world)
        {
            SpriteBatch = spriteBatch;
            SpriteFilter = FilterBuilder
                .Include<Position>()
                .Include<Sprite>()
                .Build();
        }

        public void Draw()
        {
            SpriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise
            );
            foreach (var entity in SpriteFilter.Entities)
            {
                var position = Get<Position>(entity).Value;
                var rotation = Has<Rotation>(entity) ? Get<Rotation>(entity).Value : 0.0f;
                var scale = Has<Scale>(entity) ? Get<Scale>(entity).Value : Vector2.One;
                var sprite = Get<Sprite>(entity);
                SpriteBatch.Draw(
                    Content.Textures.Sprites,
                    new Vector2((int)position.X, (int)position.Y),
                    sprite.Rect,
                    Color.White,
                    rotation,
                    new Vector2(MathF.Floor(sprite.Rect.Width * 0.5f), MathF.Floor(sprite.Rect.Height * 0.5f)),
                    scale,
                    SpriteEffects.None,
                    sprite.Depth
                );
            }
            SpriteBatch.End();
        }
    }
}
