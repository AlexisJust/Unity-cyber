using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Controller / Camera Values")]
    public class CameraValues : ScriptableObject
    {
        public float turnSmooth = .1f;
        public float moveSpeed = 9;
        public float aimSpeed = 15;
        public float y_rotate_speed = 8;
        public float x_rotate_speed = 8;
        public float minAngle = -35;
        public float maxAngle = 35;
        public float normalZ = -3f;
        public float normalX;
        public float normalY;
        public float aimZ = -0.5f;
        public float aimX = 0;
        public float crouchY;
        public float adaptSpeed = 9;

    }
}
