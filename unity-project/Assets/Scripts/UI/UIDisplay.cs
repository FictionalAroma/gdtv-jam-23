using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIDisplay : MonoBehaviour
    {
        public enum UIDisplayMode
        {
            SliderValue,
            State,
            OutOf,
        }
        [SerializeField] private TextMeshProUGUI _stateText;
        [SerializeField] private Slider HPslider;
        [SerializeField] private UIDisplayMode displayMode;


        [field:SerializeField] public float MaxValue { get; set; }

        public void Awake()
        {
			if (HPslider != null)
			{
				HPslider.maxValue = MaxValue;
			}
        }

        public void SetToMax()
		{
			if (HPslider != null)
			{
				HPslider.value = this.MaxValue;
			}

		}
        public void SetUIState(bool state)
        {
            HPslider.enabled = !state;
            _stateText.enabled = state;
            displayMode = HPslider.enabled == true ? UIDisplayMode.SliderValue : UIDisplayMode.State;
        }

        public void SetSliderValue(float amountChanged,float value)
        {
			HPslider.value = value;
		}
        public void SetTextValue(string state)
        {
            _stateText.text = state;
        }
        /*public void SetValue(float amountChanged, float newValue, string state)
		{
			if (_slider != null) _slider.value = newValue;

			switch (displayMode)
            {
                case TextDisplayMode.State:
                {
                    _sliderValueText.text = state;
                    break;
                }
                case TextDisplayMode.SliderValue:
                {
                    var percentageValue = (newValue / MaxValue) * 100f;
                    _slider.value = percentageValue ;
                    break;
                }
                case TextDisplayMode.OutOf:
                {
                    _sliderValueText.text = $"{newValue:#00} / {MaxValue:#00}";
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
		}*/
    }
}
