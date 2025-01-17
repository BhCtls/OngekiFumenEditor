﻿using Caliburn.Micro;
using OngekiFumenEditor.Kernel.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OngekiFumenEditor.Kernel.Audio.DefaultCommonImpl.Sound.DefaultFumenSoundPlayer;

namespace OngekiFumenEditor.Modules.AudioPlayerToolViewer.Models
{
    public class SoundVolumeProxy : PropertyChangedBase
    {
        private readonly IFumenSoundPlayer soundPlayer;
        private readonly SoundControl sound;

        public string Name => sound.ToString();

        public float Volume
        {
            get => soundPlayer.GetVolume(sound) ?? 0;
            set
            {
                soundPlayer.SetVolume(sound, value);
                NotifyOfPropertyChange(() => Volume);
            }
        }

        public bool IsValid => soundPlayer.GetVolume(sound) is not null;

        public SoundVolumeProxy(IFumenSoundPlayer soundPlayer, SoundControl sound)
        {
            this.soundPlayer = soundPlayer;
            this.sound = sound;
        }
    }
}
