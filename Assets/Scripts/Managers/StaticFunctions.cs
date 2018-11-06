using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public static class StaticFunctions
    {
        public static string LevelTypeToString(LevelType t)
        {
            switch (t)
            {
                case LevelType.shootout:
                    return "Fusillade";
                case LevelType.heist:
                    return "Braquage";
                case LevelType.battleRoyal:
                    return "Battle Royale";
                default:
                    return "";

            }
        }
        


    }
}