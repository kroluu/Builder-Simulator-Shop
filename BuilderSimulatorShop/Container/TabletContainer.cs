using System;
using System.Collections.Generic;
using Pattern;
using UnityEngine;

namespace UI.Game.ReworkTablet.Container
{
    /// <summary>
    /// Class keeps instances of class that were bind to container
    /// </summary>
    public class TabletContainer : Singleton<TabletContainer>
    {
        private readonly Dictionary<Type, object> container = new Dictionary<Type, object>();
        
        /// <summary>
        /// Bind instance of class to container
        /// </summary>
        /// <param name="_reference">Instance of class</param>
        /// <typeparam name="T">Class type</typeparam>
        public void Bind<T>(T _reference) where T : class
        {
            if (container.ContainsKey(typeof(T)))
            {
                Debug.LogError($"Tablet container already contains reference of {typeof(T)} type");
                return;
            }
            container.Add(typeof(T),_reference);
        }

        /// <summary>
        /// Resolves instance from container of specific type
        /// </summary>
        /// <typeparam name="T">Class type</typeparam>
        /// <returns>Class instance, null if type was not find</returns>
        public T Resolve<T>() where T : class
        {
            if (container.ContainsKey(typeof(T))) return container[typeof(T)] as T;
            
            Debug.LogError($"Tablet container does not contain reference of {typeof(T)} type");
            return null;

        }
    }
}