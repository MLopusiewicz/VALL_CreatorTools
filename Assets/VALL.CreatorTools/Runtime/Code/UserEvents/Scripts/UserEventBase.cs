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

        public TimeSettings timeSettings = new TimeSettings(0);
        static CoroutineObject coroutineObject;

        public void Fire() {
            if (coroutineObject == null) {
                var go = new GameObject("coroutineObject");
                coroutineObject = go.AddComponent<CoroutineObject>();
            }
            coroutineObject.StartCoroutine(DelayCoroutine(timeSettings.delay));
        }

        public abstract object[] GetParams();

        IEnumerator DelayCoroutine(float time) {
            yield return new WaitForSecondsRealtime(time);
            OnFire?.Invoke(this);
        }

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

    public struct TimeSettings {
        public float delay;

        public TimeSettings(float delay) {
            this.delay = delay;

        }
    }

}