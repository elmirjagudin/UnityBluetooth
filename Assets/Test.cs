using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

using brab.bluetooth;

public class Test : MonoBehaviour
{
    static Text LogText;
    static int LogLines = 0;

    StreamReader reader = null;
    BtStream istream;
    BtStream ostream;

    public static void Log(string format, params object[] args)
    {
        var msg = string.Format(format, args);
        LogText.text += msg + "\n";
        Debug.Log(msg);

        if ((LogLines += 1) > 32)
        {
            /* trim away first line */
            var log = LogText.text;
            LogText.text = log.Substring(log.IndexOf('\n')+1);

            LogLines -= 1;
        }

    }

    void Start()
    {
        LogText = gameObject.GetComponent<Text>();

        var ba = BluetoothAdapter.getDefaultAdapter();
        Log("ba.isEnabled() = {0}", ba.isEnabled());

        BluetoothDevice hiper = null;
        foreach (var c in ba.getBondedDevices())
        {

            Log("bond {0}:<{1}>", c.getName(), c.getAddress());
            if (c.getAddress() == "00:07:80:36:02:C6")
            {
                Log("found Hiper divice");
                hiper = c;
            }
        }

        if (hiper == null)
        {
            Log("No hiper device found");
            return;
        }

        try
        {
            var uuid = UUID.fromString("00001101-0000-1000-8000-00805f9b34fb");
            Log("uuid {0}", uuid);

            var sock = hiper.createRfcommSocketToServiceRecord(uuid);
            Log("sock {0}", sock);

            sock.connect();

            istream = sock.getInputStream();
            ostream = sock.getOutputStream();

            Log("in {0} out {1}", istream, ostream);

            reader = new StreamReader(istream);
            var writer = new StreamWriter(ostream);

            Log("reader {0} writer {1}", reader, writer);

            writer.Write("em,/cur/term,/msg/nmea/GGA:.05\n\r");
            writer.Flush();

            Log("wrote");
        }
        catch (System.Exception e)
        {
            Log("Start Exception: {0}", e);
        }
    }

    void Update()
    {
        if (reader != null)
        {
            try
            {
                Log("line:'{0}'", reader.ReadLine());
            }
            catch (System.Exception e)
            {

                Log("Update Exception: {0}", e);
            }
        }
    }
}

