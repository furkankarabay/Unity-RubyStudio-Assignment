using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGame.GameData
{
    public static class SaveData
    {
        #region Properties

        public static int MoneyValue
        {
            get => PlayerPrefs.GetInt("money", 100);
            set => PlayerPrefs.SetInt("money", value);
        }

        public static int FirstLevelSoldierNumberValue
        {
            get => PlayerPrefs.GetInt("firstLevelSoldier", 0);
            set => PlayerPrefs.SetInt("firstLevelSoldier", value);
        }

        public static int SecondLevelSoldierNumberValue
        {
            get => PlayerPrefs.GetInt("secondLevelSoldier", 0);
            set => PlayerPrefs.SetInt("secondLevelSoldier", value);
        }

        #endregion
    }
}
