using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvivor3D
{
    public class EventManager<TEventArgs>
    {
        // Event Dictionary
        private static Dictionary<string, Action<TEventArgs>> eventDict =
                    new Dictionary<string, Action<TEventArgs>>();

        /// <summary>
        /// Subscribe to an event if it doesn't exist in the event dictionary, add it and subscribe to it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listener"></param>
        public static void Register(string name, Action<TEventArgs> listener)
        {
            Action<TEventArgs> eventInstance;
            if (eventDict.TryGetValue(name, out eventInstance))
            {
                eventInstance += listener;
                eventDict[name] = eventInstance;
            }
            else
            {
                eventInstance += listener;
                eventDict.Add(name, eventInstance);

                Debug.Log("New event added: " + eventDict[name]);
                Debug.Log("Event Dictionary Count = " + eventDict.Count);
            }
        }

        /// <summary>
        /// Unsubscribe from an event if it exist in the event dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listener"></param>
        public static void Unregister(string name, Action<TEventArgs> listener)
        {
            if (eventDict.ContainsKey(name))
                eventDict[name] -= listener;
        }

        /// <summary>
        /// Here, we are adding a "payload" of type 'TEventArgs' as an argument.
        /// This will help with passing parameters based on the desired type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventArgs"></param>
        public static void Raise(string name, TEventArgs eventArgs)
        {
            if (eventDict.ContainsKey(name))
                eventDict[name]?.Invoke(eventArgs);
        }


        public static void Init()
        {
            Debug.Log("Event Dictionary loaded: " + eventDict.Count);
        }

        public static void Clear()
        {
            eventDict.Clear();
        }



    }
}
