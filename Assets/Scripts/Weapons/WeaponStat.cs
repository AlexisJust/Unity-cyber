using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [System.Serializable]
    public class WeaponStat
    {
        public int GetMagAmmo()
        {
            return base_magAmmo + mod_magAmmo;
        }

        public int GetMaxCarrying()
        {
            return base_maxCarrying + mod_maxCarrying;
        }

        public float GetSpread()
        {
            return base_Spread + mod_Spread;
        }

        public float GetFireRate()
        {
            return base_fireRate + mod_fireRate;
        }

        public float GetDamage()
        {
            return base_damage + mod_damage;
        }

        public int bulletAmount = 1;
        public int base_magAmmo = 30;
        public int mod_magAmmo = 0;
        public int base_maxCarrying = 120;
        public int mod_maxCarrying;
        public float base_Spread;
        public float mod_Spread;
        public float base_fireRate = 0.15f;
        public float mod_fireRate;
        public int base_damage = 15;
        public int mod_damage = 0;
    }
}