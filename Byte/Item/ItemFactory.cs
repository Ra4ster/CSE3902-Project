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
        private readonly Texture2D tileSheet;

        public ItemFactory(Texture2D itemsTexture)
        {
            this.tileSheet = itemsTexture;
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
                0.2f 
            );

            return new Rupee(rupeeSprite, position);
        }
        
        public IItem CreateHeart(Vector2 position)
        {
            Rectangle[] heartFrames = new Rectangle[]
            {
                new Rectangle(0, 0, 6, 7), //red frame
                new Rectangle(0, 8, 6, 7) //blue frame
            };


            AnimatedSprite HeartSprite = new AnimatedSprite(
                tileSheet,
                ref position,
                Color.White,
                heartFrames,
                0.2f
            );

            return new Heart(HeartSprite, position);
        }
        public IItem CreateBomb(Vector2 position)
        {
            Rectangle bombFrame = new Rectangle(136, 0, 8, 8);
            StaticSprite BombSprite = new StaticSprite(
                tileSheet,
                position,
                Color.White,
                bombFrame,
                0.2f
            );

            return new Bomb(BombSprite, position);
        }
        public IItem CreateBow(Vector2 position)
        {
            Rectangle bowFrame = new Rectangle(144, 0, 8, 15);
            StaticSprite BowSprite = new StaticSprite(
                tileSheet,
                position,
                Color.White,
                bowFrame,
                0.2f
            );
            return new Bomb(BowSprite, position);
        }

        public IItem CreateBoomerang(Vector2 position)
        {
            Rectangle[] boomerangFrames = new Rectangle[]
            {
                new Rectangle(129, 3, 3, 8), // brown frame
                new Rectangle(129, 19, 3, 8) //blue frame
            };

            AnimatedSprite boomerangSprite = new AnimatedSprite(
                tileSheet,
                ref position,
                Color.White,
                boomerangFrames,
                0.2f
            );

            return new Boomerang(boomerangSprite, position);
        }

    }
}

