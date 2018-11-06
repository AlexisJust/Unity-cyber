using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class Character : MonoBehaviour
    {

        public Animator anim;
        public string outfitId;

        public CharObject hairObj;
        public CharObject eyebrowsObj;
        public Mask maskObj;
        public CharObject other;

        GameObject hair;
        GameObject eyebrows;
        GameObject mask;
        

        public bool isFemale;
        public SkinnedMeshRenderer bodyRenderer;

        public Transform eyebrowsBone;
        public Transform pistolHolster;
        public Transform rifleHolster;

        List<GameObject> instancedObjs = new List<GameObject>();

        ResourcesManager r_manager;

        public void Init(StatesManager st)
        {
            r_manager = st.r_manager;
            anim = st.anim;
            LoadCharacter();
            hair = LoadCharacterObject(hairObj);
            eyebrows = LoadCharacterObject(eyebrowsObj);

            LoadMask(maskObj);
            LoadCharacterObject(other);
        }

        public void LoadCharacter()
        {
            if (r_manager == null)
                r_manager = Resources.Load("Resources Manager") as ResourcesManager;

            MeshContainer m = r_manager.GetMesh(outfitId);
            if (m == null)
                return;
            LoadMeshContainer(m);
        }

        public void LoadMeshContainer(MeshContainer m)
        {
            bodyRenderer.sharedMesh = (isFemale)? m.f_mesh : m.m_mesh;
            bodyRenderer.material = m.material;
        }

        public void LoadMask(Mask m)
        {
            if (mask)
                Destroy(mask);

            if (m == null)
                return;

            if (hair)
                Destroy(hair);

            if (eyebrows)
                Destroy(eyebrows);

            if (m.enableHair)
                hair = LoadCharacterObject(hairObj);

            if (m.enableEyebrows)
                eyebrows = LoadCharacterObject(eyebrowsObj);

            mask = LoadCharacterObject(m.obj);
            //return mask;
        }

        public GameObject LoadCharacterObject(CharObject o)
        {
            if (o == null)
                return null;

            Transform b = GetBone(o.parentBone);
            GameObject prefab = o.f_prefab;
            if (prefab == null || !isFemale)
                prefab = o.m_prefab;
            GameObject go = Instantiate(prefab);
            go.transform.parent = b;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            instancedObjs.Add(go);
            go.transform.localScale = Vector3.one * 100;

            return go;
            
        }

        public Transform GetBone(MyBones b)
        {
            switch (b)
            {
                case MyBones.head:
                    return anim.GetBoneTransform(HumanBodyBones.Head);
                case MyBones.chest:
                    return anim.GetBoneTransform(HumanBodyBones.Chest);
                case MyBones.eyebrows:
                    return eyebrowsBone;
                case MyBones.rightHand:
                    return anim.GetBoneTransform(HumanBodyBones.RightHand);
                case MyBones.leftHand:
                    return anim.GetBoneTransform(HumanBodyBones.LeftHand);
                case MyBones.rightUpperLeg:
                    return anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
                case MyBones.hips:
                    return anim.GetBoneTransform(HumanBodyBones.Hips);
                case MyBones.holster_pistol:
                    return pistolHolster;
                case MyBones.holster_rifle:
                    return rifleHolster;
                default:
                    return null;
            }
        }
    }
}
