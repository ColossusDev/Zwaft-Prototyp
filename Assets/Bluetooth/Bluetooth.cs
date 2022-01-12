using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_WSA_10_0 && !UNITY_EDITOR
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

#endif
#if UNITY_WSA_10_0 && !UNITY_EDITOR
#endif
public class Bluetooth : MonoBehaviour
{
    public static GameObject text;
    public static TextMeshProUGUI debugText;

#if UNITY_WSA_10_0 && !UNITY_EDITOR
    BluetoothLEAdvertisementWatcher scanner;
    int hitCount;
   // public TextMesh InstructionText;
   //public TextMesh UUIDText;

#endif
    // Start is called before the first frame update
    void Start()
    {
        debugText = text.GetComponent<TextMeshProUGUI>();
        debugText.text = "gestartet....";

#if UNITY_WSA_10_0 && !UNITY_EDITOR

        DeviceWatcher deviceWatcher = DeviceInformation.CreateWatcher(
                    BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                    requestedProperties,
                    DeviceInformationKind.AssociationEndpoint);
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;


            deviceWatcher.Start();

            debugText.text = "läuft...";

#endif
    }
#if UNITY_WSA_10_0 && !UNITY_EDITOR

    private static void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        debugText.text = "updated...";
    }

    private static void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
    {
        debugText.text = "added... " + args.Name;

    }
    private static void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        debugText.text = "removed... ";

    }

#endif

    void Update()
    {

    }
}
