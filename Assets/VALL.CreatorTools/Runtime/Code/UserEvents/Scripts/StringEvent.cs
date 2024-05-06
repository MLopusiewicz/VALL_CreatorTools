using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vall.Tools.Events {
    [CreateAssetMenu(fileName = "MyEvent", menuName = "VALL/Events/String Event", order = 0)]
    public class StringEvent : UserEventBase {
        public string text;

        public override object[] GetParams() {
            return new object[] { text };
        }
        protected override List<string> GetEvents() {
            return eventsData.stringEvents;
        }
    }
}