using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class InstantiateObjectOnEvent : MonoBehaviour
    {

        public GameSettings gameSettings;
        public Transform spawnPosition;
        public MyObjectType type;

        public GameObject createdObj;
        public StringVariable stringVariable;


        public void CreateObject()
        {
            if (spawnPosition == null)
                spawnPosition = this.transform;

            if (gameSettings == null)
                gameSettings = Resources.Load("Game Settings") as GameSettings;

            if (createdObj)
                Destroy(createdObj);

            string targetId = stringVariable.value;
            GameObject prefab = null;

            Vector3 targetScale = Vector3.one;
            switch (type)
            {
                case MyObjectType.mw:
                    prefab = gameSettings.r_manager.GetWeapon(targetId).modelPrefab;
                    targetScale *= 0.01f;
                    break;
                case MyObjectType.sw:
                    prefab = gameSettings.r_manager.GetWeapon(targetId).modelPrefab;
                    break;
                case MyObjectType.mask:
                    prefab = gameSettings.r_manager.GetMask(targetId).obj.m_prefab;
                    break;
                case MyObjectType.skill:
                    return;
                default:
                    return;

            }

            if (prefab == null)
                return;

            createdObj = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation) as GameObject;
            createdObj.transform.parent = spawnPosition;
            createdObj.transform.localScale = targetScale;
        }

    }

    public enum MyObjectType
    {
        mw, sw, mask, skill, outfit
    }
}
