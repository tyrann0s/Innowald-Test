using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI resAmountText;
        
        [SerializeField] private TextMeshProUGUI droneAmountText, droneSpeedText;
        [SerializeField] private Slider droneAmountSlider, droneSpeedSlider;

        [SerializeField] private Toggle dronesShowPathToggle;
        public Base ParentBase { get; set; }

        public void UpdateResAmount(int amount)
        {
            resAmountText.text = amount.ToString();
        }

        public void ChangeDronesAmount()
        {
            ParentBase.DroneManager.PoolSize = (int)droneAmountSlider.value;
            droneAmountText.text = droneAmountSlider.value.ToString();
        }

        public void ChangeDronesSpeed()
        {
            ParentBase.DroneManager.ChangeDronesSpeed(droneSpeedSlider.value);
            droneSpeedText.text = droneSpeedSlider.value.ToString();
        }

        public void ToggleShowPath()
        {
            ParentBase.DroneManager.ChangeDronesTraceLine(dronesShowPathToggle.isOn);
        }
    }
}
