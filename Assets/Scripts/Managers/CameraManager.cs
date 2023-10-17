using Cinemachine;
using UnityEngine;

namespace RubyGame.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera mergeCamera;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private CinemachineVirtualCamera battleCamera;

        private void OnEnable()
        {
            EventsManager.OnLevelCreated += SwitchFollowCamera;
            EventsManager.OnInteractedMergeArea += SwitchMergeCamera;
            EventsManager.OnInteractedBattleArea += SwitchBattleCamera;

        }

        private void OnDisable()
        {
            EventsManager.OnLevelCreated -= SwitchFollowCamera;
            EventsManager.OnInteractedMergeArea -= SwitchMergeCamera;
            EventsManager.OnInteractedBattleArea -= SwitchBattleCamera;
        }

        public void SwitchMergeCamera(bool isMerging)
        {
            followCamera.gameObject.SetActive(!isMerging);
            mergeCamera.gameObject.SetActive(isMerging);
        }

        public void SwitchBattleCamera(bool isBattle)
        {
            followCamera.gameObject.SetActive(!isBattle);
            battleCamera.gameObject.SetActive(isBattle);
        }

        public void SwitchFollowCamera()
        {
            followCamera.gameObject.SetActive(true);
            battleCamera.gameObject.SetActive(false);
        }
    }
}
