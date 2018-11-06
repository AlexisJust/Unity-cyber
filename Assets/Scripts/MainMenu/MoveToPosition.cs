using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class MoveToPosition : MonoBehaviour
    {

        public Transform[] positions;

        bool initLerp;
        bool isLerping;
        float t;
        Vector3 startPos;
        Vector3 targetPos;
        Quaternion startRot;
        Quaternion targetRot;
        public float speed = 3;
        public int index;
        float speedActual;

        private void Update()
        {
            if (!isLerping)
                return;

            if (!initLerp)
            {
                initLerp = true;
                startPos = transform.position;
                startRot = transform.rotation;
                targetPos = positions[index].position;
                targetRot = positions[index].rotation;
                speedActual = speed;
                t = 0;
            }

            t += Time.deltaTime * speedActual;
            if(t > 1)
            {
                t = 1;
                initLerp = false;
                isLerping = false;
            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
            Quaternion tr = Quaternion.Slerp(startRot, targetRot, t);
            transform.position = tp;
            transform.rotation = tr;
        }

        public void LerpToPos(int i)
        {
            index = i;
            isLerping = true;
            initLerp = false;
        }
    }
}