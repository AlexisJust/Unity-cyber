using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SO
{
    public class UI_TextUpdateMode : MonoBehaviour
    {
        public Text modeType;
        public Text levelName;
        public Text currentUsers;
        public Text maxUsers;
        public LevelSelection targetLevel;

        private void Start()
        {
            LoadLevel(targetLevel);
        }

        public void LoadLevel(LevelSelection l)
        {
            targetLevel = l;
            modeType.text = StaticFunctions.LevelTypeToString(l.type);
            levelName.text = l.targetLevel;
        }

        public void UpdateLevelOnGameSettings()
        {
            GameSettings gs = Resources.Load("Game Settings") as GameSettings;
            gs.UpdateCurrentLevel(targetLevel);
        }

    }
}