using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Game
{
	public static class LocalAddress
	{
        public static IPAddress Default { get { return IPAddress.Loopback; } }

        public static IPAddress Get()
        {
            var types = new NetworkInterfaceType[] { NetworkInterfaceType.Wireless80211, NetworkInterfaceType.Ethernet };

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus != OperationalStatus.Up) continue;

                if (types.Contains(networkInterface.NetworkInterfaceType))
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            return ip.Address;
            }

            return Default;
        }
    }
}