using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vall.Events {
	public class FloatRemapper : MonoBehaviour {

		public FloatEvent Remapped;

		public float min, max;
		public void Remap(float f) {
			Remapped?.Invoke(f * (max - min) + min);
		}
	}
}