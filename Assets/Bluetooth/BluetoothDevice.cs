using UnityEngine;

namespace brab.bluetooth
{

public class BluetoothDevice
{
    AndroidJavaObject JavaObject;

    public BluetoothDevice(AndroidJavaObject obj)
    {
        JavaObject = obj;
    }

    public string getName()
    {
        return JavaObject.Call<string>("getName");
    }

    public string getAddress()
    {
        return JavaObject.Call<string>("getAddress");
    }

    public BluetoothSocket createRfcommSocketToServiceRecord(UUID uuid)
    {
        var sock = JavaObject.Call<AndroidJavaObject>(
            "createRfcommSocketToServiceRecord",
            uuid.AndroidJavaObject);

        return new BluetoothSocket(sock);
    }
}

}