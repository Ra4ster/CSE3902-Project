using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    internal class ItemFactory
    {
        private readonly Texture2D tileSheet = GameAssets.Instance.ItemSheet;

        public ItemFactory()
        {
        }

        public IItem CreateRupee(Vector2 position)
        {
            Rectangle[] rupeeFrames = new Rectangle[]
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
            Rectangle[] heartFrames = new Rectangle[]
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
            Rectangle[] boomerangFrames = new Rectangle[]
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

