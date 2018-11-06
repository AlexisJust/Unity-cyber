using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;

        ParticleSystem[] particles;

        public Transform slider;
        Vector3 startPos;
        public float sliderSpeed = 3;
        public float targetZ;
        Vector3 targetPos;
        bool isLerping;
        bool initLerp;
        float t;

        AudioSource audioSource;

        public void Init()
        {
            if (slider != null)
                startPos = slider.localPosition;

            GameObject go = new GameObject();
            go.name = "audio holder";
            go.transform.parent = this.transform;
            audioSource = go.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;

            
        }

        public void Tick(float delta)
        {
            if (isLerping)
            {
                if(slider == null)
                {
                    isLerping = false;
                    return;
                }

                if (!initLerp)
                {
                    initLerp = true;
                    t = 0;
                    targetPos = startPos;
                    targetPos.z = targetZ;
                }

                t += delta * sliderSpeed;
                if(t > 1)
                {
                    t = 1;
                    isLerping = false;
                    initLerp = false;
                }

                float e = Mathf.Sin(Mathf.PI * t);

                Vector3 tp = Vector3.Lerp(startPos, targetPos, e);
                slider.transform.localPosition = tp;
            }
        }

        private void OnEnable()
        {
            particles = transform.GetComponentsInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }

            isLerping = true;
        }

        public void PlayAudio(AudioHolder a)
        {
            audioSource.pitch = a.GetPitch();
            audioSource.PlayOneShot(a.GetClip());
        }

    }
}
