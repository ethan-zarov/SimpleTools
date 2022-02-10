using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace EthanZarov.SimpleTools
{
    [CreateAssetMenu(menuName = "Simple Tools/Sound Group")]
    public class SoundGroup : ScriptableObject
    {
        [SerializeField] private WeightedSoundSample[] sounds;
        private float _totalWeight;
        
        private void OnValidate()
        {
            CalculateTotalWeight();
        }


        public AudioClip GetRandomSound()
        {
            float r = UnityEngine.Random.Range(0, _totalWeight);
            float currentR = 0;
            for (int i = 0; i < sounds.Length; i++)
            {
                currentR += sounds[i].soundWeight;

                if (r < currentR)
                {
                    return sounds[i].sound;
                }
            }

            return GetRandomSoundUnweighted();
        }

        public AudioClip GetRandomSoundUnweighted()
        {
            if (sounds.Length == 0) return null;
            return sounds[UnityEngine.Random.Range(0, sounds.Length)].sound;
        }

        public AudioClip GetSoundAt(int index)
        {
            if (sounds.Length == 0) return null;
            WeightedSoundSample sample = sounds[index % sounds.Length];
            return sample.sound;
        }

        void CalculateTotalWeight()
        {
            _totalWeight = 0;
            for (int i = 0; i < sounds.Length; i++)
            {
                _totalWeight += sounds[i].soundWeight;
            }
        }


        [System.Serializable]
        public struct WeightedSoundSample
        {
            public AudioClip sound;
            [Tooltip("At -1, the sound will nearly never play, while at 1, the sound has a high likelihood of playing"), Range(-1, 1)]
            public float soundWeight;
        }
    }
}
