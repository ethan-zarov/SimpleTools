using UnityEngine;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Plays a sound from this audioSource with target volume (pitch always 1).
        /// </summary>
        /// <param name="aSrc">Audio Source to play from.</param>
        /// <param name="clip">Audio Clip to play.</param>
        /// <param name="volume">Volume to play clip at.</param>
        /// <param name="volumeVariability">Volume variability (both lower and higher) to randomize.</param>
        public static void PlaySound(this AudioSource aSrc, AudioClip clip, float volume, float volumeVariability)
        {
            aSrc.clip = clip;
            float volumeMult = 1;

            aSrc.volume = (volume + UnityEngine.Random.Range(-volumeVariability, volumeVariability)) * volumeMult;
            aSrc.pitch = 1;
            aSrc.Play();

        }
        /// <summary>
        /// Plays a sound from this audioSource with target volume and pitch.
        /// </summary>
        /// <param name="aSrc">Audio Source to play from.</param>
        /// <param name="clip">Audio Clip to play.</param>
        /// <param name="volume">Volume to play clip at.</param>
        /// <param name="volumeVariability">Volume variability (both lower and higher) to randomize.</param>
        /// <param name="pitch">Pitch to play clip at.</param>
        /// <param name="pitchVariability">Pitch variability (both lower and higher) to randomize.</param>
        public static void PlaySound(this AudioSource aSrc, AudioClip clip, float volume, float volumeVariability, float pitch, float pitchVariability)
        {
            aSrc.clip = clip;
            float volumeMult = 1;
            aSrc.volume = (volume + Random.Range(-volumeVariability, volumeVariability)) * volumeMult;
            aSrc.pitch = pitch + Random.Range(-pitchVariability, pitchVariability);
            aSrc.Play();

        }
    }

}