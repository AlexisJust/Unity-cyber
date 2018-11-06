using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "Level")]
    public class LevelSelection : ScriptableObject
    {
        public string targetLevel;
        public LevelType type;
        public int maxPlayers = 10;

        public string levelDescription;
        public Sprite levelImg;

    }

    public enum LevelType
    {
        shootout, heist, battleRoyal
    }
}