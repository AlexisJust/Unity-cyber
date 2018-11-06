using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [System.Serializable]
    public class WeaponManager
    {
        public bool isMain;
        public string mw_id;
        public string sw_id;
        
        RuntimeWeapon curWeapon;
        public RuntimeWeapon GetCurrent()
        {
            return curWeapon;
        }

        public void  SetCurrent(RuntimeWeapon rw)
        {
            curWeapon = rw;
        }

        public void Tick(float delta)
        {
            if (curWeapon != null)
                curWeapon.w_hook.Tick(delta);
        }

        public RuntimeWeapon m_weapon;
        public RuntimeWeapon s_weapon;
    }
}
