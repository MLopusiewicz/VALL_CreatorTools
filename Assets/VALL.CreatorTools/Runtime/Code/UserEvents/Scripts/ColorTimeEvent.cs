using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vall.Tools.Events {
    [CreateAssetMenu(fileName = "MyEvent", menuName = "VALL/Events/ColorFloatEvent", order = 0)]
    public class ColorTimeEvent : UserEventBase {
        public float time;
        public Color color;

        public override object[] GetParams() {
            return new object[] { color, time };
        }
        protected override List<string> GetEvents() {
            return eventsData.colorTimeEvents;
        }
    }
}