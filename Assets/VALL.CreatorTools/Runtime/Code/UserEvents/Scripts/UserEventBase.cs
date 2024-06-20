using NaughtyAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Vall.Tools.Events {
    public abstract class UserEventBase : ScriptableObject {

        public static Action<UserEventBase> OnFire;

        [Dropdown("GetEvents"), OnValueChanged("Serialize")]
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
                    if (events == null) {
                        Debug.LogError("Problem deserializing events");
                        events = new EventDatabase();
                    }
                }
                return events;
            }
        }
        static EventDatabase events;

        protected abstract List<string> GetEvents();


        private void Serialize() {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this); 
#endif
        }

    }


}