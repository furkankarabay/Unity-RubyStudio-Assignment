using RubyGame.GamePlay;
using UnityEngine;

namespace RubyGame.Managers
{
    public class MergeManager : MonoBehaviour
    {
        #region Singleton

        public static MergeManager instance;

        private void InitSingleton()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }

        #endregion

        #region Variables

        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask mergeableLayer;

        private MergeableCharacter heldObj;
        private float offsetY;

        #endregion

        #region Unity Events
        private void Awake()
        {
            InitSingleton();
        }

        private void OnEnable()
        {
            EventsManager.OnMergeTriggered += MergeSoldiers;
        }

        private void OnDisable()
        {
            EventsManager.OnMergeTriggered -= MergeSoldiers;
        }


        // Update is called once per frame
        void Update()
        {
            if (heldObj)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    EventsManager.OnTouchedMergeableSoldier?.Invoke(false);
                    heldObj.ReleasedCharacter();
                    heldObj = null;
                    return;
                }

                Vector3 newPos = GetMouseWorldPos();
                heldObj.transform.position = new Vector3(newPos.x, offsetY, newPos.z);
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, 1000, mergeableLayer))
                    {
                        if (hitInfo.collider.TryGetComponent(out MergeableCharacter mergeableCharacter))
                        {
                            if (!mergeableCharacter.isMergeable || !mergeableCharacter.canBeHold)
                                return;

                            EventsManager.OnTouchedMergeableSoldier?.Invoke(true);
                            mergeableCharacter.HoldingForMerge();

                            heldObj = mergeableCharacter;
                            offsetY = heldObj.transform.position.y + 3;
                        }

                    }
                }
            }

        }
        #endregion

        #region Methods

        public void MergeSoldiers(GameObject mergeObj, GameObject mergeObj2, TypeOfCharacters type)
        {
            switch (type)
            {
                case TypeOfCharacters.villager:

                    CharacterSpawnManager.instance.SpawnMergeableCharacter(type + 1, mergeObj.transform.position);

                    BarrackManager.instance.DecreaseSoldier(2, type);

                    BarrackManager.instance.IncreaseSoldier(1, type + 1);


                    break;
            }

            Destroy(mergeObj);
            Destroy(mergeObj2);
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(heldObj.transform.position).z;
            return mainCamera.ScreenToWorldPoint(mousePos);
        }

        #endregion
    }
}
