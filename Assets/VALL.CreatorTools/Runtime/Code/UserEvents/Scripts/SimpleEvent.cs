using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vall.Tools.Events {
    [CreateAssetMenu(fileName = "MyEvent", menuName = "VALL/Events/Simple Event", order = 0)]
    public class SimpleEvent : UserEventBase {

         
        public override object[] GetParams() {
            return new object[] { };
        }
        protected override List<string> GetEvents() {
            return eventsData.simpleEvents;
        }
    }
}