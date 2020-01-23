using System;
using System.Collections.Generic;
using UnityEngine;

namespace myScripts {
    // [ExecuteInEditMode]
    public class RoomImporter : MonoBehaviour {

        public bool run = false;
        public List<GameObject> rooms = new List<GameObject>( );
        private List<GameObject> _stored;

        private void Update( ) {
            if ( run && rooms.Count > 0 ) {
                foreach ( var r in rooms ) {
                    // _stored.Add( r.ImportRoom( ) );
                }
                rooms.Clear( );

                foreach ( var room in _stored ) {
                    
                }
            }
        }

    }
}