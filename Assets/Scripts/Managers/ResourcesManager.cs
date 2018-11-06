using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Single Instances / Resources")]
    public class ResourcesManager : ScriptableObject
    {
        public RuntimeReferences runtime;
        public List<Weapon> all_weapons = new List<Weapon>();
        Dictionary<string, int> w_dict = new Dictionary<string, int>();

        public List<MeshContainer> meshContainers = new List<MeshContainer>();
        Dictionary<string, int> m_dict = new Dictionary<string, int>();

        public List<CharObject> charObjects = new List<CharObject>();
        Dictionary<string, int> c_dict = new Dictionary<string, int>();

        public List<Mask> masks = new List<Mask>();
        Dictionary<string, int> mask_dict = new Dictionary<string, int>();

        public List<AudioHolder> audioHolders = new List<AudioHolder>();
        Dictionary<string, int> a_dict = new Dictionary<string, int>();

        public void Init()
        {
#if UNITY_EDITOR
            all_weapons = SO.Utilities.EditorUtilities.FindAssetsByType<Weapon>();
            meshContainers = SO.Utilities.EditorUtilities.FindAssetsByType<MeshContainer>();
            charObjects = SO.Utilities.EditorUtilities.FindAssetsByType<CharObject>();
            masks = SO.Utilities.EditorUtilities.FindAssetsByType<Mask>();
            audioHolders = SO.Utilities.EditorUtilities.FindAssetsByType<AudioHolder>();
#endif


            InitWeapons();
            InitMeshContainers();
            InitMaskDictionaries();
            InitCharObjects();
            InitAudioHolders();
        }


        void InitWeapons()
        {
            w_dict.Clear();
            for (int i = 0; i < all_weapons.Count; i++)
            {
                if (w_dict.ContainsKey(all_weapons[i].name))
                {

                }
                else
                {
                    w_dict.Add(all_weapons[i].name, i);
                }
            }
        }

        public Weapon GetWeapon(string id)
        {
            Weapon retVal = null;
            int index = -1;
            if (w_dict.TryGetValue(id, out index))
            {
                retVal = all_weapons[index];
            }

            return retVal;
        }

        public List<Weapon> GetAllWeaponsOfType(WeaponType type)
        {
            List<Weapon> r = new List<Weapon>();
            for (int i = 0; i < all_weapons.Count; i++)
            {
                if (all_weapons[i].type == type)
                {
                    r.Add(all_weapons[i]);
                }
            }

            return r;
        }

        public List<Object> GetObjectListFromType(MyObjectType t)
        {
            List<Object> retVal = new List<Object>();

            switch (t)
            {
                case MyObjectType.mw:
                    List<Weapon> mw = GetAllWeaponsOfType(WeaponType.main);
                    for (int i = 0; i < mw.Count; i++)
                    {
                        retVal.Add((Object)mw[i]);
                    }
                    break;
                case MyObjectType.sw:
                    List<Weapon> sw = GetAllWeaponsOfType(WeaponType.sidearm);
                    for (int i = 0; i < sw.Count; i++)
                    {
                        retVal.Add((Object)sw[i]);
                    }
                    break;
                case MyObjectType.mask:
                    for (int i = 0; i < masks.Count; i++)
                    {
                        retVal.Add((Object)masks[i]);
                    }
                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    for (int i = 0; i < meshContainers.Count; i++)
                    {
                        retVal.Add((Object)meshContainers[i]);
                    }
                    break;
                default:
                    break;
            }

            return retVal;
        }


        void InitMeshContainers()
        {
            m_dict.Clear();
            for (int i = 0; i < meshContainers.Count; i++)
            {
                if (m_dict.ContainsKey(meshContainers[i].name))
                {
                    
                }
                else
                {
                    m_dict.Add(meshContainers[i].name, i);
                }
            }
        }

        public MeshContainer GetMesh(string id)
        {
            MeshContainer retVal = null;
            int index = -1;
            if (m_dict.TryGetValue(id, out index))
            {
                retVal = meshContainers[index];
            }

            return retVal;
        }

        void InitMaskDictionaries()
        {
            mask_dict.Clear();
            for (int i = 0; i < masks.Count; i++)
            {
                if (mask_dict.ContainsKey(masks[i].obj.name))
                {
                    
                }
                else
                {
                    mask_dict.Add(masks[i].obj.name, i);
                }
            }
        }

        public Mask GetMask(string id)
        {
            Mask retVal = null;
            int index = -1;
            if (mask_dict.TryGetValue(id, out index))
            {
                retVal = masks[index];
            }

            return retVal;
        }

        void InitCharObjects()
        {
            c_dict.Clear();
            for (int i = 0; i < charObjects.Count; i++)
            {
                if (c_dict.ContainsKey(charObjects[i].name))
                {
                    
                }
                else
                {
                    c_dict.Add(charObjects[i].name, i);
                }
            }
        }

        void InitAudioHolders()
        {
            a_dict.Clear();

            for (int i = 0; i < audioHolders.Count; i++)
            {
                if (a_dict.ContainsKey(audioHolders[i].name))
                {

                }
                else
                {
                    a_dict.Add(audioHolders[i].name, i);
                }
            }
        }
    }


    public enum MyBones
    {
        head, chest, eyebrows, rightHand, leftHand, rightUpperLeg, hips, holster_pistol, holster_rifle
    }
}