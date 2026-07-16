using System;
using System.IO;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ConfigMenu : MonoBehaviour
    {
        private struct ConfigSaveData
        {
            public int lowerBound;
            public int upperBound;
        }

        [Header("Depends")]

        [SerializeField]
        private Slider _currentSignalSlider;

        [SerializeField]
        private Slider _minThresholdSlider;

        [SerializeField]
        private Slider _maxThresholdSlider;

        [SerializeField]
        private TMP_InputField _lowerBoundInputField;

        [SerializeField]
        private TMP_InputField _upperBoundInputField;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        private bool _visible;

        private string SaveFilePath => Application.persistentDataPath + "/config.json";

        private void Awake()
        {
            _lowerBoundInputField.onEndEdit.AddListener(UpdateLowerBound);
            _upperBoundInputField.onEndEdit.AddListener(UpdateUpperBound);
        }

        private void OnDestroy()
        {
            _lowerBoundInputField.onEndEdit.RemoveListener(UpdateLowerBound);
            _upperBoundInputField.onEndEdit.RemoveListener(UpdateUpperBound);
            ArduinoInput.Instance.OnIntensityChange.RemoveListener(HandleIntensityChanged);
        }

        private void Start()
        {
            ArduinoInput.Instance.OnIntensityChange.AddListener(HandleIntensityChanged);
            LoadValues();
            UpdateThresholdSliders();
        }

        private void UpdateUpperBound(string upperBound)
        {
            int val = int.Parse(upperBound);
            if (ArduinoInput.Instance)
            {
                ArduinoInput.Instance.SetUpperSignalBound(val);
                UpdateThresholdSliders();
                SaveValues();
            }
        }

        private void UpdateLowerBound(string lowerBound)
        {
            int val = int.Parse(lowerBound);
            if (ArduinoInput.Instance)
            {
                ArduinoInput.Instance.SetLowerSignalBound(val);
                UpdateThresholdSliders();
                SaveValues();
            }
        }

        public void ToggleVisibility()
        {
            _visible = !_visible;
            _canvasGroup.blocksRaycasts = _visible;
            _canvasGroup.interactable = _visible;
            _canvasGroup.alpha = _visible ? 1 : 0;
        }

        private void HandleIntensityChanged(float signal)
        {
            _currentSignalSlider.value = signal;
        }

        private void UpdateThresholdSliders()
        {
            var valueCap = 1000;
            float upperBound = ArduinoInput.Instance.IntensityThresholds.y;
            float lowerBound = ArduinoInput.Instance.IntensityThresholds.x;
            _currentSignalSlider.maxValue = valueCap;

            _minThresholdSlider.maxValue = valueCap;
            _minThresholdSlider.value = lowerBound;

            _maxThresholdSlider.maxValue = valueCap;
            _maxThresholdSlider.value = upperBound;

            _lowerBoundInputField.text = lowerBound.ToString();
            _upperBoundInputField.text = upperBound.ToString();
        }

        private void SaveValues()
        {
            float upperBound = ArduinoInput.Instance.IntensityThresholds.y;
            float lowerBound = ArduinoInput.Instance.IntensityThresholds.x;

            string json = JsonUtility.ToJson(new ConfigSaveData
            {
                upperBound = (int)upperBound,
                lowerBound = (int)lowerBound
            });

            try
            {
                File.WriteAllText(SaveFilePath, json);
                Debug.Log($"Saved config to {SaveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save config {e}");
            }
        }

        private void LoadValues()
        {
            try
            {
                string configJson = File.ReadAllText(SaveFilePath);
                var data = JsonUtility.FromJson<ConfigSaveData>(configJson);

                ArduinoInput.Instance.SetLowerSignalBound(data.lowerBound);
                ArduinoInput.Instance.SetUpperSignalBound(data.upperBound);

                Debug.Log($"Loaded config from {SaveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load config {e}");
            }
        }
    }
}