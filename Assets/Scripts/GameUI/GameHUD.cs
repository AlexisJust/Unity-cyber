using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SO.UI
{
    public class GameHUD : UIElements
    {

        public BoolVariable isAiming;
        public BoolVariable isLeftPivot;

        public Transform np_r;
        public Transform np_l;
        public Transform aim_p;

        public float moveSpeed = 4;
        public Transform hud;

        public override void Tick(float delta)
        {
            base.Tick(delta);

            Vector3 tp = (isLeftPivot.value)? np_l.localPosition : np_r.localPosition;
            Quaternion tr = (isLeftPivot.value)? np_l.localRotation : np_r.localRotation;
            
            if (isAiming.value)
            {
                tp = aim_p.localPosition;
                tr = aim_p.localRotation;
            }

            float t = delta * moveSpeed;

            Vector3 targetPosition = Vector3.Lerp(hud.localPosition, tp, t);
            Quaternion targetRotation = Quaternion.Slerp(hud.localRotation, tr, t);
            hud.localPosition = targetPosition;
            hud.localRotation = targetRotation;
        }
    }
}