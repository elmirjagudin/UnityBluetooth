using UnityEngine;
using System;

namespace brab.bluetooth
{

public class BtStream : System.IO.Stream
{
    AndroidJavaObject JavaObject;
    bool IsInputStream;

    override public bool CanRead { get { return IsInputStream; } }
    override public bool CanSeek { get { return false; } }
    override public bool CanWrite { get { return !IsInputStream; } }
    override public long Length { get { throw new NotSupportedException(); } }
    override public long Position
    {
        get { throw new NotSupportedException(); }
        set { throw new NotSupportedException(); }
    }

    public BtStream(AndroidJavaObject obj, bool IsInputStream)
    {
        JavaObject = obj;
        this.IsInputStream = IsInputStream;
    }

    override public void Flush()
    {
        /*
         * we don't do any buffering, so
         * nothing to do here
         */
    }

    override public int Read(byte[] buffer, int offset, int count)
    {
        if (!IsInputStream)
        {
            throw new NotSupportedException();
        }

        /*
         * TODO: investigate if it's possible to make the
         * byte array buffer managment more efficient
         *
         * Right now for each read call we:
         *  - create new temp JNI array
         *  - use that when calling Java's InputStream.read() method
         *  - convert that temp array to managed array
         *  - copy from the converted array to callers provided buffer array
         *
         * This involved a lot of allocations and memory copy operations,
         * perhaps there is a way to directly write at the right memory location
         * in the caller provided buffer array.
         */

        var jniBuffer = AndroidJNI.NewByteArray(count);
        jvalue[] args = new jvalue[3];
        args[0].l = jniBuffer;
        args[1].i = 0;
        args[2].i = count;

        IntPtr methodId = AndroidJNIHelper.GetMethodID(
            JavaObject.GetRawClass(),
            "read", "([BII)I");

        var r = AndroidJNI.CallIntMethod(
            JavaObject.GetRawObject(),
            methodId,
            args);

        var manBuff = AndroidJNI.FromByteArray(jniBuffer);

        Array.Copy(manBuff, 0, buffer, offset, count);
        return r;
    }

    override public void Write(byte[] buffer, int offset, int count)
    {
        if (IsInputStream)
        {
            throw new NotSupportedException();
        }

        var jniBuffer = AndroidJNIHelper.ConvertToJNIArray(buffer);
        jvalue[] args = new jvalue[3];
        args[0].l = jniBuffer;
        args[1].i = offset;
        args[2].i = count;

        IntPtr methodId = AndroidJNIHelper.GetMethodID(
            JavaObject.GetRawClass(),
            "write", "([BII)V");

        AndroidJNI.CallVoidMethod(
            JavaObject.GetRawObject(),
            methodId,
            args);
    }

    override public long Seek(long offset, System.IO.SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    override public void SetLength(long value)
    {
        throw new NotSupportedException();
    }
}

}
