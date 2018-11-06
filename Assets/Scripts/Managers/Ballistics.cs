using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public static class Ballistics
    {
        
        public static RaycastHit RaycastShoot(Vector3 o, Vector3 d, ref bool s, LayerMask lm)
        {
            
            Ray ray = new Ray(o, d);
            RaycastHit hit;

            if(Physics.Raycast(o,d,out hit, 100, lm)) //On bullet hit
            {
                s = true;
            }

            return hit;

        }

    }
}