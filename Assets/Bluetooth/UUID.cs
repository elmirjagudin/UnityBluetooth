using UnityEngine;

namespace brab.bluetooth
{

public class UUID
{
    static AndroidJavaClass _JavaClass = null;
    static AndroidJavaClass JavaClass
    {
        get
        {
            if (_JavaClass == null)
            {
                _JavaClass = new AndroidJavaClass("java.util.UUID");
            }

            return _JavaClass;
        }
    }

    AndroidJavaObject _JavaObject;
    public AndroidJavaObject AndroidJavaObject
    {
        get
        {
            return _JavaObject;
        }
    }

    UUID(AndroidJavaObject obj)
    {
        _JavaObject = obj;
    }

    public static UUID fromString(string name)
    {
        var obj = JavaClass.CallStatic<AndroidJavaObject>("fromString", name);

        return new UUID(obj);
    }

}

}