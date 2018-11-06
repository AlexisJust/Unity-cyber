using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Weapons / Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public StringVariable id;

        public int price = 1000;
        public IKPositions m_h_ik;
        public GameObject modelPrefab;
        public MyBones holsterBone = MyBones.holster_pistol;

        public WeaponStat stats;
        public W_AudioHolder audio;

        public bool onIdleDisableOh;
        public int weaponType;
        public WeaponType type;

        public AnimationCurve recoilY;
        public AnimationCurve recoilZ;
    }

    [System.Serializable]
    public class W_AudioHolder
    {
        public AudioHolder firing;
        public AudioHolder reload;
    }

    public enum WeaponType
    {
        main, sidearm, melee
    }
}