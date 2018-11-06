using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Single Instances / Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public ResourcesManager r_manager;

        public int version = 0;

        public bool isConnected;
        public UISettings uiSettings;
        public PlayerProfile playerProfile;
        public PlayerProfile ui_profile;

        public bool resetValues;

        public void Init()
        {

            if (resetValues)
            {
                DefaultProfile();
            }
        }

        public void DefaultProfile()
        {
            playerProfile.bought_items.Clear();
            playerProfile.AddItem("none");
            playerProfile.AddItem("rifle");
            playerProfile.AddItem("pistol");
            playerProfile.AddItem("Punk");
            playerProfile.AddItem("none");

            playerProfile.mw_id.value = "rifle";
            playerProfile.sw_id.value = "pistol";

            playerProfile.outfitID.value = "Punk";
            playerProfile.mask_id.value = "none";
            playerProfile.money.value = 10000;
        }


        [System.Serializable]
        public class UISettings
        {
            public LevelSelection curLvl;
            public GameEvent onLevelChanged;
            public StringVariable lvlType;
            public StringVariable lvlDescription;
            public SpriteVariable lvlImg;
        }

        public void UpdateCurrentLevel(LevelSelection targetLevel)
        {
            uiSettings.curLvl = targetLevel;
            uiSettings.lvlDescription.value = targetLevel.levelDescription;
            uiSettings.lvlType.value = StaticFunctions.LevelTypeToString(targetLevel.type);
            uiSettings.lvlImg.sprite = targetLevel.levelImg;
            if (uiSettings.onLevelChanged != null)
                uiSettings.onLevelChanged.Raise();
        }

        public bool isEquiped(Object obj, MyObjectType t)
        {
            bool retVal = false;

            switch (t)
            {
                case MyObjectType.mw:
                case MyObjectType.sw:
                    Weapon w = (Weapon)obj;
                    Weapon w_actual = r_manager.GetWeapon((t == MyObjectType.mw)? playerProfile.mw_id.value : playerProfile.sw_id.value);
                    if (w == w_actual)
                        retVal = true;

                    break;
                case MyObjectType.mask:
                    Mask m = (Mask)obj;
                    Mask m_actual = r_manager.GetMask(playerProfile.mask_id.value);
                    if (m == m_actual)
                        retVal = true;

                    break;
                case MyObjectType.skill:
                    break;
                case MyObjectType.outfit:
                    MeshContainer mc = (MeshContainer)obj;
                    if (string.Equals(mc.id, playerProfile.outfitID.value))
                        retVal = true;

                    break;
                default:
                    break;

            }

            return retVal;
        }

        public void UpdateUIProfile()
        {
            ui_profile.mw_id.value = playerProfile.mw_id.value;
            ui_profile.sw_id.value = playerProfile.sw_id.value;
            ui_profile.outfitID.value = playerProfile.outfitID.value;
            ui_profile.mask_id.value = playerProfile.mask_id.value;
        }

        public void UpdatePlayerProfile()
        {
            playerProfile.mw_id.value = ui_profile.mw_id.value;
            playerProfile.sw_id.value = ui_profile.sw_id.value;
            playerProfile.outfitID.value = ui_profile.outfitID.value;
            playerProfile.mask_id.value = ui_profile.mask_id.value;
        }

    }
}