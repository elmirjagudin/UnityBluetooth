using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace brab.bluetooth
{

public class BluetoothAdapter
{
    static AndroidJavaClass _JavaClass = null;
    static AndroidJavaClass JavaClass
    {
        get
        {
            if (_JavaClass == null)
            {
                _JavaClass = new AndroidJavaClass("android.bluetooth.BluetoothAdapter");
            }

            return _JavaClass;
        }
    }

    AndroidJavaObject JavaObject;

    BluetoothAdapter(AndroidJavaObject obj)
    {
        JavaObject = obj;
    }

    public static BluetoothAdapter getDefaultAdapter()
    {
        var obj = JavaClass.CallStatic<AndroidJavaObject>("getDefaultAdapter");

        return new BluetoothAdapter(obj);
    }

    public bool isEnabled()
    {
        return JavaObject.Call<bool>("isEnabled");
    }

    public ICollection<BluetoothDevice> getBondedDevices()
    {
        var devices = new Collection<BluetoothDevice>();

        var DevsSet = JavaObject.Call<AndroidJavaObject>("getBondedDevices");
        var DevsIter = DevsSet.Call<AndroidJavaObject>("iterator");

        while (DevsIter.Call<bool>("hasNext"))
        {
            var dev = DevsIter.Call<AndroidJavaObject>("next");
            devices.Add(new BluetoothDevice(dev));
        }

        return devices;
    }
}

}
