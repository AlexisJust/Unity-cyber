using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.UI
{
    public class UI_PassSlotToManager : MonoBehaviour
    {
        public UI_EquipOrBuy equipOrBuy;
        public UI_Slot slot;

        public void PassSlot()
        {
            equipOrBuy.EquipOrBuy(slot);
        }

    }
}