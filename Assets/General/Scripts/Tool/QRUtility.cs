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
using ZXing.Common;

namespace Game
{
	public static class QRUtility
	{
        public static Texture2D Generate(string data, int size)
        {
            var writer = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,

                Options = new EncodingOptions()
                {
                    Height = size,
                    Width = size,
                    Margin = 0
                }
            };

            var matrix = writer.Encode(data);

            var colors = BitMatrixToColors(matrix);
            
            Texture2D tex = new Texture2D(matrix.Width, matrix.Height);

            tex.filterMode = FilterMode.Point;
            tex.SetPixels(colors);

            tex.Apply();

            return tex;
        }

        public static Color[] BitMatrixToColors(BitMatrix matrix)
        {
            Color[] colors = new Color[matrix.Width * matrix.Height];

            int pos = 0;
            for (int y = 0; y < matrix.Height; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    colors[pos++] = matrix[x, y] ? Color.black : Color.white;
                }
            }

            return colors;
        }
    }
}