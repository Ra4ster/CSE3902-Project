using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Byte.Sounds
{
    public class SoundManager
    {
        public Song currentSong { get; private set; }
        private Song dungeonTheme;
        private List<SoundEffectInstance> soundEffects = new List<SoundEffectInstance>();
        private SoundEffect swordSlash;
        public void LoadSound(ContentManager Content)
        {
            swordSlash = Content.Load<SoundEffect>("Sounds/LOZ_Sword_Slash");
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
                    SoundEffectInstance swordSlashInstance = swordSlash.CreateInstance();
                    swordSlashInstance.Play();
                    soundEffects.Add(swordSlashInstance);
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

    }
}
