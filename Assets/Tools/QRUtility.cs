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

using ZXing;
using ZXing.QrCode;

namespace Game
{
	public static class QRUtility
	{
        public static Texture2D Generate(string text, int size)
        {
            var encoded = new Texture2D(size, size);

            var color32 = Encode(text, size);

            encoded.SetPixels32(color32);

            encoded.Apply();

            return encoded;
        }

        public static Color32[] Encode(string text, int size)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = size,
                    Width = size,
                    Margin = 0,
                }
            };
            return writer.Write(text);
        }
    }
}