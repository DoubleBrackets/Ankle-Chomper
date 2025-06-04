using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    public class ArduinoInput : MonoBehaviour
    {
        private enum ButtonState
        {
            Idle,
            PassedUpper
        }

        [SerializeField]
        private int baudRate;

        [SerializeField]
        private string portName;

        [SerializeField]
        private Vector2 intensityThresholds;

        [SerializeField]
        private int signalQueueSize;

        [SerializeField]
        private bool log;

        [SerializeField]
        private bool connectOnStart;

        public UnityEvent<float> OnIntensityChange;

        /// <summary>
        ///     Button press using the intensity thresholds as conditions (passed upper threshold after staying below lower
        ///     threshold).
        /// </summary>
        public UnityEvent OnButtonPressed;

        public UnityEvent<string> OnStatusChange;
        public static ArduinoInput Instance { get; private set; }

        private readonly Queue<float> signalQueue = new();

        private SerialPort serialPort;

        private ButtonState buttonState = ButtonState.Idle;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (connectOnStart)
            {
                Connect();
            }
        }

        private void Update()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            if (serialPort.BytesToRead <= 0)
            {
                return;
            }

            float signal = int.Parse(serialPort.ReadLine());

            signalQueue.Enqueue(signal);

            if (signalQueue.Count > signalQueueSize)
            {
                signalQueue.Dequeue();
            }

            // Use the highest signal as the accepted value
            float highestSignal = signalQueue.Max();

            if (buttonState == ButtonState.Idle)
            {
                if (highestSignal > intensityThresholds.y)
                {
                    buttonState = ButtonState.PassedUpper;
                    OnButtonPressed?.Invoke();
                    Debug.Log("CHOMP threshold passed");
                }
            }
            else
            {
                if (highestSignal < intensityThresholds.x)
                {
                    buttonState = ButtonState.Idle;
                }
            }

            OnIntensityChange?.Invoke(highestSignal);

            if (log)
            {
                Debug.Log($"Signal: {signal}, Buffer max: {highestSignal}");
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public void Disconnect()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public void Connect()
        {
            Disconnect();

            try
            {
                Debug.Log($"Using port: {portName}");
                serialPort = new SerialPort(portName, baudRate);
                serialPort.ReadTimeout = 100;
                serialPort.Open();
                OnStatusChange?.Invoke($"Connected to {portName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to open serial port: {e.Message}");
                OnStatusChange?.Invoke($"Error Connecting: {e.Message}");
            }
        }

        public void SetSerialPort(string newPortName)
        {
            portName = newPortName;
        }
    }
}