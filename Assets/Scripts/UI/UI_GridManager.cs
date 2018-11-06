using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.UI
{
    public class UI_GridManager : MonoBehaviour
    {
        public IntVariable typeVariable;
        public MyObjectType type;
        public GameSettings gameSettings;

        public List<GameObject> createdObjs = new List<GameObject>();
        public Transform gridParent;
        public GameObject slotTemplate;

        public UI_ChangeWidth ui_slot_grid_size;

        public void LoadObjects()
        {
            ClearCreatedObjs();

            type = (MyObjectType)typeVariable.value;

            List<Object> objs = new List<Object>();
            objs = gameSettings.r_manager.GetObjectListFromType(type);
            for(int i = 0; i < objs.Count; i++)
            {
                CreateObject(objs[i], type);
            }

            ui_slot_grid_size.ChangeSize(createdObjs.Count);
        }

        void CreateObject(Object obj, MyObjectType t)
        {
            GameObject go = Instantiate(slotTemplate) as GameObject;
            go.SetActive(true);
            go.transform.SetParent(gridParent);
            go.transform.localScale = Vector3.one;
            createdObjs.Add(go);
            UI.UI_Slot slot = go.GetComponent<UI_Slot>();

            string targetText = null;
            bool isEquiped = false;
            isEquiped = gameSettings.isEquiped(obj, t);

            switch (t)
            {
                case MyObjectType.mw:
                    Weapon mw = (Weapon)obj;
                    targetText = mw.id.value;
                    break;
                case MyObjectType.sw:
                    Weapon sw = (Weapon)obj;
                    targetText = sw.id.value;
                    break;
                case MyObjectType.mask:
                    Mask m = (Mask)obj;
                    targetText = m.obj.id.value;
                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    MeshContainer mc = (MeshContainer)obj;
                    targetText = mc.id.value;
                    break;
                default:
                    break;
            }

            slot.type = type;
            slot.obj = obj;
            slot.item.text = targetText;
            slot.tick.SetActive(isEquiped);

            bool isBought = gameSettings.playerProfile.IsBought(targetText);
            slot.item.color = (isBought) ? Color.white : Color.gray;
        }

        public void ChangeType(int i)
        {
            type = (MyObjectType)i;
        }

        void ClearCreatedObjs()
        {
            for(int i =0; i < createdObjs.Count; i++)
            {
                Destroy(createdObjs[i]);
            }

            createdObjs.Clear();
        }

    }
}