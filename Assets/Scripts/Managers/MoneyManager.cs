using RubyGame.GameData;
using TMPro;
using UnityEngine;

namespace RubyGame.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        #region Singleton

        public static MoneyManager Instance;

        private void InitSingleton()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }

        #endregion

        [SerializeField] private int currentMoney;
        [SerializeField] private GameObject moneyUIPanel;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private GameObject MoneyTextObj;

        private void Awake()
        {
            InitSingleton();
        }

        void Start()
        {
            currentMoney = GetCurrentMoney();
            UpdateCurrentMoney();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoneyCheat();
            }
        }

        public void MoneyCheat()
        {
            IncreaseMoney(100);
            UpdateCurrentMoney();
        }

        public void IncreaseMoney(int value)
        {
            currentMoney += value;
            SetCurrentMoney(currentMoney);
            UpdateCurrentMoney();
        }

        public void DecreaseMoney(int value)
        {
            currentMoney -= value;
            SetCurrentMoney(currentMoney);
            UpdateCurrentMoney();

        }

        public int GetCurrentMoney()
        {
            return SaveData.MoneyValue;
        }

        public void SetCurrentMoney(int value)
        {
            SaveData.MoneyValue = value;
        }

        public void UpdateCurrentMoney()
        {
            var currentBalance = GetCurrentMoney();
            moneyText.text = currentBalance.ToString() + "$";
        }
    }
}
