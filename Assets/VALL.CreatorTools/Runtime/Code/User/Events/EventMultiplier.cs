using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vall.Events {
	public class EventMultiplier : MonoBehaviour {

		public FloatEvent Multiplied;
		public float multiplier;
		public void Multiply(float f) {
			Multiplied?.Invoke(f * multiplier);
		}
	}
}