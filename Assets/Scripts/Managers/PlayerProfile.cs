using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [System.Serializable]
    public class PlayerProfile
    {
        public string userName;
        public IntVariable money;
        public StringVariable outfitID;
        public StringVariable mask_id;
        public StringVariable mw_id;
        public StringVariable sw_id;
        public BoolVariable isMale;

        public List<string> bought_items = new List<string>();

        public void AddItem(string id)
        {
            bought_items.Add(id);
        }

        public bool IsBought(string id)
        {
            return bought_items.Contains(id);
        }

    }
}
