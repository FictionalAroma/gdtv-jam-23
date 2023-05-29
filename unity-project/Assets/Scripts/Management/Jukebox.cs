using System.Collections.Generic;
using UnityEngine;

namespace Management
{
    public class Jukebox : GlobalMusicPlayer
	{
		private static Jukebox _instance = null;
		public static Jukebox Instance => _instance;

		private readonly List<AudioClip> _trackList = new List<AudioClip>();

        private int _trackIndex = -1;

		public void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
                DontDestroyOnLoad(this);
			}
			else
			{
				Destroy(this);
			}
		}


		public void SetTracks(IEnumerable<AudioClip> audioClips, bool playImmediate = false, bool fade = true)
        {
            _trackList.Clear();
            _trackList.AddRange(audioClips);
            _trackIndex = 0;
            if (playImmediate)
            {
                PlayNextTrack(fade);
            }
        }

        public void PlayNextTrack(bool fade = true)
        {
            _trackIndex++;

            if (_trackIndex >= _trackList.Count)
            {
                _trackIndex = 0;
            }

            PlayNewAudio(_trackList[_trackIndex], fade);
        }

        public void LateUpdate()
        {
            if (currentClip.length + (defaultFadeSeconds * 1.5f) < mainAudioSource.time)
            {
                PlayNextTrack();
            }
        }
    }
}
