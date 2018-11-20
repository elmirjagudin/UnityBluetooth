using UnityEngine;

namespace brab.bluetooth
{

public class BluetoothSocket
{
    AndroidJavaObject JavaObject;

    public BluetoothSocket(AndroidJavaObject obj)
    {
        JavaObject = obj;
    }

    public void connect()
    {
        JavaObject.Call("connect");
    }

    public BtStream getInputStream()
    {
        var istream = JavaObject.Call<AndroidJavaObject>("getInputStream");
        return new BtStream(istream, true);
    }

    public BtStream getOutputStream()
    {
        var ostream = JavaObject.Call<AndroidJavaObject>("getOutputStream");
        return new BtStream(ostream, false);
    }

}

}