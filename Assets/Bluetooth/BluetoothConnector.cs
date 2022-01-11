using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;

public class BluetoothConnector
{
    public BluetoothConnector()
    {
    }

    void Connect()
    {
        string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

        DeviceWatcher deviceWatcher =
                    DeviceInformation.CreateWatcher(
                            BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                            requestedProperties,
                            DeviceInformationKind.AssociationEndpoint);

        // Register event handlers before starting the watcher.
        // Added, Updated and Removed are required to get all nearby devices
        deviceWatcher.Added += DeviceWatcher_Added;
        deviceWatcher.Updated += DeviceWatcher_Updated;
        deviceWatcher.Removed += DeviceWatcher_Removed;

        // EnumerationCompleted and Stopped are optional to implement.
        deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
        deviceWatcher.Stopped += DeviceWatcher_Stopped;

        // Start the watcher.
        deviceWatcher.Start();
    }
}
