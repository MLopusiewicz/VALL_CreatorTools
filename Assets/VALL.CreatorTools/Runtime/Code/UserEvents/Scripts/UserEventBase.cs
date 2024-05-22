using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vall.Tools.Events {
    public abstract class UserEventBase : ScriptableObject {


        public static Action<UserEventBase> OnFire;

        [Dropdown("GetEvents")]
        public string key;



        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Fire() {
            OnFire?.Invoke(this);
        }

        public abstract object[] GetParams();


        protected EventDatabase eventsData {
            get {
                if (events == null) {
                    var t = Resources.Load<TextAsset>("EditorEvents").text;
                    events = JsonConvert.DeserializeObject<EventDatabase>(t);
                }
                return events;
            }
        }
        static EventDatabase events;

        protected abstract List<string> GetEvents();

    }


}