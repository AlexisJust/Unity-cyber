using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Characters / Mesh Container")]
    public class MeshContainer : ScriptableObject
    {
        public StringVariable id;

        public Mesh m_mesh;
        public Mesh f_mesh;
        public Material material;

        public int cost = 500;
    }
}
