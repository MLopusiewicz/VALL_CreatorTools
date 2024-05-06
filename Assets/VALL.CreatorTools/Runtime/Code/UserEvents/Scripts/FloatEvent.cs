using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vall.Tools.Events {
	[CreateAssetMenu(fileName = "MyEvent", menuName = "VALL/Events/Float Event", order = 0)]
	public class FloatEvent : UserEventBase {
		public float value;

		public override object[] GetParams() {
			return new object[] { value };
		}
        protected override List<string> GetEvents() {
            return eventsData.floatEvents;
        }
    }
}
