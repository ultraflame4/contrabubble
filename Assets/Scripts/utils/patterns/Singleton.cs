

using System;
using JetBrains.Annotations;

namespace Utils.Patterns {

    public interface Singleton<T> where T : Singleton<T>, new() {
        [CanBeNull]
        public static T _instance { get; protected set; }

        [NotNull]
        public static T Instance {
            get {
                if (_instance == null) {
                    throw new NullReferenceException($"Singleton: No instance of {typeof(T).FullName} found!");
                }
                return _instance;
            }
        }

    }

}