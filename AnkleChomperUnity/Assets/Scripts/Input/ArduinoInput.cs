using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    public class ArduinoInput : MonoBehaviour
    {
        [SerializeField]
        private int baudRate;

        [SerializeField]
        private string portName;

        [SerializeField]
        private bool connectOnStart;

        public UnityEvent<string> OnStatusChange;
        public static ArduinoInput Instance { get; private set; }

        private SerialPort serialPort;

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