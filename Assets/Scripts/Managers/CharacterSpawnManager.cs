using RubyGame.GamePlay;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGame.Managers
{

    public class CharacterSpawnManager : MonoBehaviour
    {
        #region Singleton

        public static CharacterSpawnManager instance;

        private void InitSingleton()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }

        #endregion

        #region Variables

        [SerializeField] private List<GameObject> characterList = new();
        [SerializeField] private List<GameObject> battleCharacterList = new();

        #endregion

        #region Unity Events 
        private void Awake()
        {
            InitSingleton();
        }

        #endregion

        #region Methods
        public void SpawnStackableCharacter(TypeOfCharacters typeOfCharacter, Vector3 spawnPosition, CollectableArea locatedArea, int index)
        {
            switch (typeOfCharacter)
            {
                case TypeOfCharacters.villager:
                    GameObject characterGO = Instantiate(characterList[0], spawnPosition, Quaternion.identity);

                    if (characterGO.TryGetComponent(out StackableCharacter stackableCharacter))
                    {
                        stackableCharacter.locatedArea = locatedArea;
                        locatedArea.stackableCharacters[index] = stackableCharacter;
                        stackableCharacter.index = index;
                        stackableCharacter.isStackable = true;
                    }
                    break;
                case TypeOfCharacters.upgradedVillager:
                    break;
                default:
                    break;
            }
        }

        public GameObject SpawnMergeableCharacter(TypeOfCharacters typeOfCharacter, Vector3 spawnPosition)
        {
            GameObject newMergeableObj = null;

            switch (typeOfCharacter)
            {
                case TypeOfCharacters.villager:
                    newMergeableObj = Instantiate(characterList[0], spawnPosition, Quaternion.identity);

                    break;
                case TypeOfCharacters.upgradedVillager:
                    newMergeableObj = Instantiate(characterList[1], spawnPosition, Quaternion.identity);

                    break;
                default:
                    break;
            }

            return newMergeableObj;
        }

        public GameObject SpawnBattleCharacter(TypeOfCharacters typeOfCharacter, Vector3 spawnPosition)
        {
            GameObject newBattleObj = null;

            switch (typeOfCharacter)
            {
                case TypeOfCharacters.villager:
                    newBattleObj = Instantiate(battleCharacterList[0], spawnPosition, Quaternion.identity);

                    break;
                case TypeOfCharacters.upgradedVillager:

                    newBattleObj = Instantiate(battleCharacterList[1], spawnPosition, Quaternion.identity);

                    break;
                default:
                    break;
            }

            return newBattleObj;
        }
        #endregion

    }

}

public enum TypeOfCharacters
{
    villager,
    upgradedVillager,
}