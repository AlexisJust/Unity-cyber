using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Characters / Object")]
    public class CharObject : ScriptableObject
    {
        public StringVariable id;

        public MyBones parentBone;
        public GameObject m_prefab;
        public GameObject f_prefab;

    }

}