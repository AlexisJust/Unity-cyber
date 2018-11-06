using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Audio Holder")]
    public class AudioHolder : ScriptableObject
    {
        public AudioClip[] clips;
        public float minPitch = 1;
        public float maxPitch = 1;
        
        public AudioClip GetClip()
        {
            int ran = Random.Range(0, clips.Length);
            return clips[ran];
        }

        public float GetPitch()
        {
            return Random.Range(minPitch, maxPitch);
        }
    }
}