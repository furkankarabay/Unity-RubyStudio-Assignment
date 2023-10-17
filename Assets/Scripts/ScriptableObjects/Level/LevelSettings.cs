using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGame.ScriptableObjects.Level
{
    [CreateAssetMenu(fileName = "Level Asset", menuName = "Content/Ruby Game Sample Level", order = 1)]
    public class LevelSettings : ScriptableObject
    {
        public int levelIndex;
        public int prizeMoney;
        public int defeatMoney;
    }
}
