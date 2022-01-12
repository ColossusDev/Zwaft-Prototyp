
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if WINDOWS_UWP
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

#endif
#if WINDOWS_UWP
#endif
public class BluetoothScanner_New : MonoBehaviour
{
    public GameObject text;
    public TextMeshProUGUI debugText;

#if WINDOWS_UWP
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

#if WINDOWS_UWP

        BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher();
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Received += Watcher_Received;

            DeviceWatcher devWatcher = DeviceInformation.CreateWatcher();
            devWatcher.Added += DevWatcher_Added;
            devWatcher.Updated += DevWatcher_Updated;

            watcher.Start();
            devWatcher.Start();

            debugText.text = "läuft...";

            while (true)
            {
            }

#endif
    }
#if WINDOWS_UWP

    private static void DevWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        Console.Write("\nUpdated\n");
    }

    private static void DevWatcher_Added(DeviceWatcher sender, DeviceInformation args)
    {
        //      Console.Write("Added : "+args.Name+"\n");

    }

    static List<ulong> tried = new List<ulong>();

    private static async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
    {

        if (tried.Contains(args.BluetoothAddress))
        {
            return;
        }

        tried.Add(args.BluetoothAddress);


        Console.Write("Device found =================================================");
    //debugText.text = "Device found";
        Console.Write(args.Advertisement.LocalName);
    //debugText.text = "Device found : " + args.Advertisement.LocalName;

        Console.Write("\nDataSections: " + args.Advertisement.DataSections[0].Data);

        //BluetoothLEDevice device =  
        BluetoothLEDevice device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
        try
        {
            Console.Write("\n GattServices: " + device.GetGattService(Guid.Parse("e1fa36b4-9700-414b-a4e0-382a3e249e56")).Uuid);
        }
        catch (Exception e)
        {
            Console.Write(e.ToString());
        }
        if (device == null || device.DeviceInformation == null)
        {
            Console.Write("DeviceInformation null");
            return;
        }

        if (device.DeviceInformation.Pairing == null)
        {
            Console.Write("Pairing null");
            return;

        }

        var res = await device.DeviceInformation.Pairing.PairAsync(DevicePairingProtectionLevel.None);

        Console.Write("Pair complete (?) ========================================" + res.Status);

    }

#endif

    //#if WINDOWS_UWP

    //            private void OnBeaconFound(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
    //            {

    //                try
    //                {
    //                    UnityEngine.WSA.Application.InvokeOnAppThread (() =>
    //                    {
    //                        hitCount++;
    //                        InstructionText.text = "OnBeaconFound " + hitCount.ToString();

    //                        var foundBeacon = new FoundBeacon()
    //                        {
    //                            RSSI = args.RawSignalStrengthInDBm.ToString()
    //                        };

    //                        UUIDText.text = foundBeacon.RSSI;

    //                        foundBeacon.Major = args.AdvertisementType.ToString();

    //                        //check for manufacturer data section
    //                        var beaconDataString = string.Empty;

    //                        var beaconDataSections = args.Advertisement.ManufacturerData;
    //                        if (beaconDataSections.Count > 0)
    //                        {
    //                            //print first data section for now
    //                            var beaconData = beaconDataSections[0];
    //                            var beaconDataBytes = new byte[beaconData.Data.Length];
    //                            using (var reader = DataReader.FromBuffer(beaconData.Data))
    //                            {
    //                                reader.ReadBytes(beaconDataBytes);
    //                            }

    //                            beaconDataString = BitConverter.ToString(beaconDataBytes);

    //                            foundBeacon.TxPower = beaconDataString;
    //                        }

    //                   }, true );
    //                }
    //                catch (Exception ex)
    //                {
    //                    UUIDText.text = ex.Message;
    //                }
    //            }    
    //#endif
    // Update is called once per frame
    void Update()
    {

    }
}

