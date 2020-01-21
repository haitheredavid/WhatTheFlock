using System;
using UnityEngine;

namespace myScript {
    public class CameraPosition : MonoBehaviour {

        public Transform Pos { get; private set; }
        public static CameraPosition Instance { get; private set; }

        public void Awake( ) {
            Instance = this;
        }

        public void Update( ) {
            Pos = transform;
        }

    }
}