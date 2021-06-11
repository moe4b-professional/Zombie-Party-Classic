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

namespace Default
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
            var networkInterfaces = Query(NetworkInterfaceTypes);

            foreach (var networkInterface in networkInterfaces)
            {
                foreach (var info in networkInterface.GetIPProperties().UnicastAddresses)
                    if (info.Address.AddressFamily == AddressFamily.InterNetwork)
                        return info.Address;
            }

            return Default;
        }

        public static List<NetworkInterface> Query(IList<NetworkInterfaceType> types)
        {
            var all = NetworkInterface.GetAllNetworkInterfaces();

            var targets = new List<NetworkInterface>();

            foreach (var type in types)
            {
                foreach (var networkInterface in all)
                {
                    if (IgnoreInterface(networkInterface)) continue;

                    if (networkInterface.NetworkInterfaceType == type)
                        targets.Add(networkInterface);
                }
            }

            return targets;
        }

        public static bool IgnoreInterface(NetworkInterface networkInterface)
        {
            if (networkInterface.OperationalStatus != OperationalStatus.Up)
                return true;

            if (networkInterface.Description.ToLower().Contains("virtual"))
                return true;

            return false;
        }
    }
}