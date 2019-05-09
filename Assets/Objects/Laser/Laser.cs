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

namespace Game
{
	public class Laser : MonoBehaviour
	{
		[SerializeField]
        protected LayerMask mask = Physics.DefaultRaycastLayers;
        public LayerMask Mask { get { return mask; } }

        protected float range = 100f;

        [SerializeField]
        protected LineRenderer line;
        public LineRenderer Line { get { return line; } }

        [SerializeField]
        protected Transform effect;
        public Transform Effect { get { return effect; } }

        RaycastHit hit;

        void Update()
        {
            line.SetPosition(0, transform.position);

            if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask, QueryTriggerInteraction.Ignore))
            {
                line.SetPosition(1, hit.point + transform.forward * 0.02f);

                effect.gameObject.SetActive(true);

                effect.transform.position = hit.point;
                effect.transform.rotation = Quaternion.Inverse(Quaternion.LookRotation(hit.normal));
            }
            else
            {
                effect.gameObject.SetActive(false);

                line.SetPosition(1, transform.position + transform.forward * range);
            }
        }
    }
}