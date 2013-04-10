using System;
using Microsoft.SPOT;

namespace BlindPeople.Sound
{
    class WaveGenerator
    {
        Gadgeteer.Modules.GHIElectronics.Music music;
        public WaveGenerator(Gadgeteer.Modules.GHIElectronics.Music music)
        {
            this.music = music;
            music.musicFinished += new Gadgeteer.Modules.GHIElectronics.Music.MusicFinishedPlayingEventHandler(music_musicFinished);
        }

        void music_musicFinished(Gadgeteer.Modules.GHIElectronics.Music sender)
        {
            throw new NotImplementedException();
        }

        public void buzz()
        {
            byte[] b = generateWave();
            music.Play(b);
        }

        private byte[] generateWave()
        {
            byte[] res = new byte[440];
            //TODO: loop through values
            //TODO: parameters for different frequencies and lengths

            return res;
        }

    }
}
