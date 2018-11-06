using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.Utilities
{
    public class Fader : MonoBehaviour
    {
        public Image img;
        public float speed = 2f;
        bool isFadeIn;
        bool isWaitingForFade;
        float t;
        Color c = Color.black;

        public void FadeIn()
        {
            isFadeIn = true;
            StartCoroutine("Fade");
        }

        public void FadeOut()
        {
            isFadeIn = false;
            StartCoroutine("Fade");
        }

        IEnumerator Fade()
        {
            t = 0;
            img.enabled = true;

            while (t < 1)
            {
                t += Time.deltaTime * speed;
                if (isFadeIn)
                    c.a = Mathf.Lerp(1, 0, t);
                else
                    c.a = Mathf.Lerp(0, 1, t);

                img.color = c;

                yield return null;
            }

            if (isFadeIn)
                img.enabled = false;
        }

        public void FadeAfterTime(float v)
        {
            if (isWaitingForFade)
                return;

            isWaitingForFade = true;

            StartCoroutine(FadeInAfterTime(v));
        }

        IEnumerator FadeInAfterTime(float v)
        {
            yield return new WaitForSeconds(v);
            FadeIn();
            isWaitingForFade = false;
        }
    }
}