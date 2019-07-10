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

        public static readonly List<NetworkInterfaceType> NetworkInterfaceTypes = new List<NetworkInterfaceType>()
        {
            NetworkInterfaceType.Wireless80211,
            NetworkInterfaceType.Ethernet,
        };

        public static IPAddress Get()
        {
            List<NetworkInterface> interfaces = new List<NetworkInterface>();

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus != OperationalStatus.Up) continue;

                if (NetworkInterfaceTypes.Contains(networkInterface.NetworkInterfaceType))
                    interfaces.Add(networkInterface);
            }

            for (int x = 0; x < NetworkInterfaceTypes.Count; x++)
            {
                for (int y = 0; y < interfaces.Count; y++)
                {
                    if (interfaces[y].NetworkInterfaceType == NetworkInterfaceTypes[x])
                        foreach (UnicastIPAddressInformation AddressInfo in interfaces[y].GetIPProperties().UnicastAddresses)
                            if (AddressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                                return AddressInfo.Address;
                }
            }

            return Default;
        }
    }
}