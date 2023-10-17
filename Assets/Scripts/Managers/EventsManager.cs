using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGame.Managers
{
    public class EventsManager
    {
        #region Deploy

        public static Action<Transform, GameObject> OnDeploySoldierToBarrack;

        #endregion

        #region Merge

        public static Action<bool> OnTouchedMergeableSoldier;
        public static Action<bool> OnInteractedMergeArea;
        public static Action<GameObject, GameObject, TypeOfCharacters> OnMergeTriggered;

        #endregion

        #region Battle

        public static Action<bool> OnInteractedBattleArea;
        public static Action OnPlacedSoldierInArena;


        #endregion

        #region Game Flow

        public static Action OnLevelStarted;
        public static Action OnLevelCreated;
        public static Action OnLevelCompleted;
        public static Action OnLevelFailed;

        #endregion

    }
}
