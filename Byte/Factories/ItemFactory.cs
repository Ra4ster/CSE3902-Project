using Byte.Item;
using Byte.Rooms;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Factory
{
    public class ItemFactory : IFactory<ItemData, IItem>
    {

        private readonly Texture2D tileSheet = GameAssets.Instance.ItemSheet;

        public ItemFactory()
        {
        }

        public IItem Create(ItemData data) => data.Type switch
        {
            "Rupee" => CreateRupee(data.Position),
            "Heart" => CreateHeart(data.Position),
            "Bomb" => CreateBomb(data.Position),
            "Bow" => CreateBow(data.Position),
            "Boomerang" => CreateBoomerang(data.Position),
            _ => throw new InvalidDataException($"Could not find item data with type: {data.Type}")
        };

        public IItem CreateRupee(Vector2 position)
        {
            Rectangle[] rupeeFrames =
            {
                new Rectangle(72, 0, 8, 16),
                new Rectangle(72, 16, 8, 16)
            };

            AnimatedSprite rupeeSprite = new AnimatedSprite(
                tileSheet,
                ref position,
                Color.White,
                rupeeFrames,
                1f,
                5f
            );

            return new Rupee(rupeeSprite, position);
        }

        public IItem CreateHeart(Vector2 position)
        {
            Rectangle[] heartFrames =
            {
                new Rectangle(0, 0, 7, 7), //red frame
                new Rectangle(0, 8, 7, 7) //blue frame
            };


            AnimatedSprite HeartSprite = new AnimatedSprite(
                tileSheet,
                ref position,
                Color.White,
                heartFrames,
                1f,
                5f
            );

            return new Heart(HeartSprite, position);
        }
        public IItem CreateBomb(Vector2 position)
        {
            Rectangle bombFrame = new Rectangle(136, 0, 8, 16);
            StaticSprite BombSprite = new StaticSprite(
                tileSheet,
                position,
                Color.White,
                bombFrame,
                5f
            );

            return new Bomb(BombSprite, position);
        }
        public IItem CreateBow(Vector2 position)
        {
            Rectangle bowFrame = new Rectangle(144, 0, 8, 16);
            StaticSprite BowSprite = new StaticSprite(
                tileSheet,
                position,
                Color.White,
                bowFrame,
                5f
            );
            return new Bow(BowSprite, position);
        }

        public IItem CreateBoomerang(Vector2 position)
        {
            Rectangle[] boomerangFrames =
            {
                new Rectangle(129, 3, 5, 8), // brown frame
                new Rectangle(129, 19, 5, 8) //blue frame
            };

            AnimatedSprite boomerangSprite = new AnimatedSprite(
                tileSheet,
                ref position,
                Color.White,
                boomerangFrames,
                1f,
                5f
            );

            return new Boomerang(boomerangSprite, position);
        }

    }
}

