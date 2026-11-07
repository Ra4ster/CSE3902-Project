using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Byte.Projectile;

namespace Byte.Sounds
{
    public class SoundManager
    {
        public Song currentSong { get; private set; }
        private Song dungeonTheme;
        private List<SoundEffectInstance> soundEffects = new List<SoundEffectInstance>();
        private SoundEffect SwordSlash;
        private SoundEffect Arrow_Boomerang;
        private SoundEffect Bomb_Blow;
        private SoundEffect Bomb_Drop;
        private SoundEffect Boss_Hit;
        private SoundEffect Boss_Scream1;
        private SoundEffect Boss_Scream2;
        private SoundEffect Boss_Scream3;
        private SoundEffect Candle;
        private SoundEffect Door_Unlock;
        private SoundEffect Enemy_Die;
        private SoundEffect Enemy_Hit;
        private SoundEffect Fanfare;
        private SoundEffect Get_Heart;
        private SoundEffect Get_Item;
        private SoundEffect Get_Rupee;
        private SoundEffect Key_Appear;
        private SoundEffect Link_Die;
        private SoundEffect Link_Hurt;
        private SoundEffect LowHealth;
        private SoundEffect MagicalRod;
        private SoundEffect Refill_Loop;
        private SoundEffect Secret;
        private SoundEffect Shield;
        private SoundEffect Shore;
        private SoundEffect Stairs;
        private SoundEffect Sword_Combined;
        private SoundEffect Sword_Shoot;
        private SoundEffect Text;
        private SoundEffect Text_Slow;
        private Dictionary<IProjectile, SoundEffectInstance> boomerangSounds
    = new Dictionary<IProjectile, SoundEffectInstance>();

        public void LoadSound(ContentManager Content)
        {
            SwordSlash  = Content.Load<SoundEffect>("Sounds/LOZ_Sword_Slash");
            Arrow_Boomerang = Content.Load<SoundEffect>("Sounds/LOZ_Arrow_Boomerang");
            Bomb_Blow = Content.Load<SoundEffect>("Sounds/LOZ_Bomb_Blow");
            Bomb_Drop = Content.Load<SoundEffect>("Sounds/LOZ_Bomb_Drop");
            Boss_Hit = Content.Load<SoundEffect>("Sounds/LOZ_Boss_Hit");
            Boss_Scream1 = Content.Load<SoundEffect>("Sounds/LOZ_Boss_Scream1");
            Boss_Scream2 = Content.Load<SoundEffect>("Sounds/LOZ_Boss_Scream2");
            Boss_Scream3 = Content.Load<SoundEffect>("Sounds/LOZ_Boss_Scream3");
            Candle = Content.Load<SoundEffect>("Sounds/LOZ_Candle");
            Door_Unlock = Content.Load<SoundEffect>("Sounds/LOZ_Door_Unlock");
            Enemy_Die = Content.Load<SoundEffect>("Sounds/LOZ_Enemy_Die");
            Enemy_Hit = Content.Load<SoundEffect>("Sounds/LOZ_Enemy_Hit");
            Fanfare = Content.Load<SoundEffect>("Sounds/LOZ_Fanfare");
            Get_Heart = Content.Load<SoundEffect>("Sounds/LOZ_Get_Heart");
            Get_Item = Content.Load<SoundEffect>("Sounds/LOZ_Get_Item");
            Get_Rupee = Content.Load<SoundEffect>("Sounds/LOZ_Get_Rupee");
            Key_Appear = Content.Load<SoundEffect>("Sounds/LOZ_Key_Appear");
            Link_Die = Content.Load<SoundEffect>("Sounds/LOZ_Link_Die");
            Link_Hurt = Content.Load<SoundEffect>("Sounds/LOZ_Link_Hurt");
            LowHealth = Content.Load<SoundEffect>("Sounds/LOZ_LowHealth");
            MagicalRod = Content.Load<SoundEffect>("Sounds/LOZ_MagicalRod");
            Refill_Loop = Content.Load<SoundEffect>("Sounds/LOZ_Refill_Loop");
            Secret = Content.Load < SoundEffect>("Sounds/LOZ_Secret");
            Shield = Content.Load<SoundEffect>("Sounds/LOZ_Shield");
            Shore = Content.Load<SoundEffect>("Sounds/LOZ_Shore");
            Stairs = Content.Load<SoundEffect>("Sounds/LOZ_Stairs");
            Sword_Combined = Content.Load<SoundEffect>("Sounds/LOZ_Sword_Combined");
            Sword_Shoot = Content.Load<SoundEffect>("Sounds/LOZ_Sword_Shoot");
            Text = Content.Load<SoundEffect>("Sounds/LOZ_Text");
            Text_Slow = Content.Load<SoundEffect>("Sounds/LOZ_Text_Slow");

            dungeonTheme = Content.Load<Song>("Sounds/Dungeon_Theme");

        }
        public void PlayMusic()
        {
            MediaPlayer.Play(dungeonTheme);
            currentSong = dungeonTheme;
            MediaPlayer.IsRepeating = true;
        }
        public void PauseMusic()
        {
            MediaPlayer.Pause();
        }
        public void PlaySound(string SoundName)
        {
            switch (SoundName)
            {
                case "SwordSlash":
                    SoundEffectInstance swordSlashInstance = SwordSlash.CreateInstance();
                    swordSlashInstance.Play();
                    soundEffects.Add(swordSlashInstance);
                    break;
                case "Arrow_Boomerang":
                    SoundEffectInstance arrowBoomerangInstance = Arrow_Boomerang.CreateInstance();
                    arrowBoomerangInstance.Play();
                    soundEffects.Add(arrowBoomerangInstance);
                    break;
                case "Link_Hurt":
                    SoundEffectInstance linkHurtInstance = Link_Hurt.CreateInstance();
                    linkHurtInstance.Play();
                    soundEffects.Add(linkHurtInstance);
                    break;
                case "Link_Die":
                    SoundEffectInstance linkDieInstance = Link_Die.CreateInstance();
                    linkDieInstance.Play();
                    soundEffects.Add(linkDieInstance);
                    break;
                case "Bomb_Blow":
                    SoundEffectInstance bombBlowInstance = Bomb_Blow.CreateInstance();
                    bombBlowInstance.Play();
                    soundEffects.Add(bombBlowInstance);
                    break;
                case "Bomb_Drop":
                    SoundEffectInstance bombDropInstance = Bomb_Drop.CreateInstance();
                    bombDropInstance.Play();
                    soundEffects.Add(bombDropInstance);
                    break;
                case "Boss_Hit":
                    SoundEffectInstance bossHitInstance = Boss_Hit.CreateInstance();
                    bossHitInstance.Play();
                    soundEffects.Add(bossHitInstance);
                    break;
                case "Boss_Scream1":
                    SoundEffectInstance bossScream1Instance = Boss_Scream1.CreateInstance();
                    bossScream1Instance.Play();
                    soundEffects.Add(bossScream1Instance);
                    break;
                case "Boss_Scream2":
                    SoundEffectInstance bossScream2Instance = Boss_Scream2.CreateInstance();
                    bossScream2Instance.Play();
                    soundEffects.Add(bossScream2Instance);
                    break;
                case "Boss_Scream3":
                    SoundEffectInstance bossScream3Instance = Boss_Scream3.CreateInstance();
                    bossScream3Instance.Play();
                    soundEffects.Add(bossScream3Instance);
                    break;
                case "Candle":
                    SoundEffectInstance candleInstance = Candle.CreateInstance();
                    candleInstance.Play();
                    soundEffects.Add(candleInstance);
                    break;
                case "Door_Unlock":
                    SoundEffectInstance doorUnlockInstance = Door_Unlock.CreateInstance();
                    doorUnlockInstance.Play();
                    soundEffects.Add(doorUnlockInstance);
                    break;
                case "Enemy_Die":
                    SoundEffectInstance enemyDieInstance = Enemy_Die.CreateInstance();
                    enemyDieInstance.Play();
                    soundEffects.Add(enemyDieInstance);
                    break;
                case "Enemy_Hit":
                    SoundEffectInstance enemyHitInstance = Enemy_Hit.CreateInstance();
                    enemyHitInstance.Play();
                    soundEffects.Add(enemyHitInstance);
                    break;
                case "Fanfare":
                    SoundEffectInstance fanfareInstance = Fanfare.CreateInstance();
                    fanfareInstance.Play();
                    soundEffects.Add(fanfareInstance);
                    break;
                case "Get_Heart":
                    SoundEffectInstance getHeartInstance = Get_Heart.CreateInstance();
                    getHeartInstance.Play();
                    soundEffects.Add(getHeartInstance);
                    break;
                case "Get_Item":
                    SoundEffectInstance getItemInstance = Get_Item.CreateInstance();
                    getItemInstance.Play();
                    soundEffects.Add(getItemInstance);
                    break;
                case "Get_Rupee":
                    SoundEffectInstance getRupeeInstance = Get_Rupee.CreateInstance();
                    getRupeeInstance.Play();
                    soundEffects.Add(getRupeeInstance);
                    break;
                case "Key_Appear":
                    SoundEffectInstance keyAppearInstance = Key_Appear.CreateInstance();
                    keyAppearInstance.Play();
                    soundEffects.Add(keyAppearInstance);
                    break;
                case "LowHealth":
                    SoundEffectInstance lowHealthInstance = LowHealth.CreateInstance();
                    lowHealthInstance.IsLooped = true;
                    lowHealthInstance.Play();
                    soundEffects.Add(lowHealthInstance);
                    break;
                case "MagicalRod":
                    SoundEffectInstance magicalRodInstance = MagicalRod.CreateInstance();
                    magicalRodInstance.Play();
                    soundEffects.Add(magicalRodInstance);
                    break;
                case "Refill_Loop":
                    SoundEffectInstance refillLoopInstance = Refill_Loop.CreateInstance();
                    refillLoopInstance.Play();
                    soundEffects.Add(refillLoopInstance);
                    break;
                case "Secret":
                    SoundEffectInstance secretInstance = Secret.CreateInstance();
                    secretInstance.Play();
                    soundEffects.Add(secretInstance);
                    break;
                case "Shield":
                    SoundEffectInstance shieldInstance = Shield.CreateInstance();
                    shieldInstance.Play();
                    soundEffects.Add(shieldInstance);
                    break;
                case "Shore":
                    SoundEffectInstance shoreInstance = Shore.CreateInstance();
                    shoreInstance.Play();
                    soundEffects.Add(shoreInstance);
                    break;
                case "Stairs":
                    SoundEffectInstance stairsInstance = Stairs.CreateInstance();
                    stairsInstance.Play();
                    soundEffects.Add(stairsInstance);
                    break;
                case "Sword_Combined":
                    SoundEffectInstance swordCombinedInstance = Sword_Combined.CreateInstance();
                    swordCombinedInstance.Play();
                    soundEffects.Add(swordCombinedInstance);
                    break;
                case "Sword_Shoot":
                    SoundEffectInstance swordShootInstance = Sword_Shoot.CreateInstance();
                    swordShootInstance.Play();
                    soundEffects.Add(swordShootInstance);
                    break;
                case "Text":
                    SoundEffectInstance textInstance = Text.CreateInstance();
                    textInstance.Play();
                    soundEffects.Add(textInstance);
                    break;
                case "Text_Slow":
                    SoundEffectInstance textSlowInstance = Text_Slow.CreateInstance();
                    textSlowInstance.Play();
                    soundEffects.Add(textSlowInstance);
                    break;


                default:
                    break;
            }

        }
        public void Update()
        {
            for (int i = soundEffects.Count - 1; i >= 0; i--)
            {
                SoundEffectInstance sound = soundEffects[i];

                if (sound == null || sound.State == SoundState.Stopped)
                {
                    soundEffects.RemoveAt(i);
                }
            }
        }
        public void PlayBoomerangSound(IProjectile boomerang)
        {
            if (boomerangSounds.ContainsKey(boomerang))
                return;

            SoundEffectInstance instance = Arrow_Boomerang.CreateInstance();
            instance.IsLooped = true;
            instance.Play();

            boomerangSounds.Add(boomerang, instance);
            soundEffects.Add(instance);
        }

        public void StopBoomerangSound(IProjectile boomerang)
        {
            if (boomerangSounds.TryGetValue(boomerang, out SoundEffectInstance instance))
            {
                instance.Stop();
                instance.Dispose();

                soundEffects.Remove(instance);
                boomerangSounds.Remove(boomerang);
            }
        }
    }
}
