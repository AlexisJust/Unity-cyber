using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SO.UI
{
    public class UI_EquipOrBuy : MonoBehaviour
    {

        public UnityEvent isTrue;
        public UnityEvent isFalse;
        public GameEvent onProfileUpdate;
        public StringVariable curCostVariable;

        UI_Slot curSlot;
        int curCost;
        string storeId;
        MyObjectType storeType;

        public void EquipOrBuy(UI_Slot slot)
        {
            curSlot = slot;
            GameSettings s = Resources.Load("Game Settings") as GameSettings;

            string targetId = null;
            int targetCost = 0;

            switch (slot.type)
            {
                case MyObjectType.mw:
                case MyObjectType.sw:
                    Weapon w = (Weapon)slot.obj;
                    targetId = w.id.value;
                    targetCost = w.price;
                    break;
                case MyObjectType.mask:
                    Mask m = (Mask)slot.obj;
                    targetId = m.obj.id.value;
                    targetCost = m.cost;
                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    MeshContainer mc = (MeshContainer)slot.obj;
                    targetId = mc.id.value;
                    targetCost = mc.cost;
                    break;
                default:
                    break;
            }

            storeId = targetId;
            storeType = slot.type;

            VisualizeItem(s);

            bool isBought = s.playerProfile.IsBought(targetId);

            if (isBought)
            {
                isTrue.Invoke();
                EquipItem(s, slot.type);
            }
            else
            {
                if(curCost > s.playerProfile.money.value)
                {
                    //TODO: change color of buy button
                }

                curCost = targetCost;
                curCostVariable.value = targetCost.ToString();

                isFalse.Invoke();
            }

            onProfileUpdate.Raise();
        }

        public void VisualizeItem(GameSettings s)
        {

            if(curSlot == null)
            {
                return;
            }

            //GameSettings s = Resources.Load("Game Settings") as GameSettings;

            switch (curSlot.type)
            {
                case MyObjectType.mw:
                    Weapon mw = (Weapon)curSlot.obj;
                    s.ui_profile.mw_id.value = mw.id.value;
                    break;
                case MyObjectType.sw:
                    Weapon sw = (Weapon)curSlot.obj;
                    s.ui_profile.sw_id.value = sw.id.value;
                    break;
                case MyObjectType.mask:
                    Mask m = (Mask)curSlot.obj;
                    s.ui_profile.mask_id.value = m.obj.id.value;
                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    MeshContainer mc = (MeshContainer)curSlot.obj;
                    s.ui_profile.outfitID.value = mc.id.value;
                    break;
                default:
                    break;
            }
        }

        public void EquipItem(GameSettings gs, MyObjectType t)
        {
            switch (t)
            {
                case MyObjectType.mw:
                    gs.playerProfile.mw_id.value = gs.ui_profile.mw_id.value;
                    break;
                case MyObjectType.sw:
                    gs.playerProfile.sw_id.value = gs.ui_profile.sw_id.value;
                    break;
                case MyObjectType.mask:
                    gs.playerProfile.mask_id.value = gs.ui_profile.mask_id.value;
                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    gs.playerProfile.outfitID.value = gs.ui_profile.outfitID.value;
                    break;
                default:
                    break;
            }
        }

        public void Buy()
        {
            GameSettings s = Resources.Load("Game Settings") as GameSettings;

            if (curCost > s.playerProfile.money.value)
                return;

            s.playerProfile.money.value -= curCost;

            switch (storeType)
            {
                case MyObjectType.mw:
                    Weapon mw = s.r_manager.GetWeapon(storeId);
                    s.playerProfile.bought_items.Add(mw.id.value);
                    break;
                case MyObjectType.sw:
                    Weapon sw = s.r_manager.GetWeapon(storeId);
                    s.playerProfile.bought_items.Add(sw.id.value);
                    break;
                case MyObjectType.mask:
                    Mask m = s.r_manager.GetMask(storeId);
                    s.playerProfile.bought_items.Add(m.obj.id.value);
                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    MeshContainer mc = s.r_manager.GetMesh(storeId);
                    s.playerProfile.bought_items.Add(mc.id.value);
                    break;
                default:
                    break;
            }

            EquipItem(s, storeType);
            onProfileUpdate.Raise();
            isTrue.Invoke();
        }
    }
}