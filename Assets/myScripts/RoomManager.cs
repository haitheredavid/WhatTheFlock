using System.Collections.Generic;
using UnityEngine;

namespace myScripts {
    public enum Parts {

        Wall,
        Floor,
        Furniture,
        Poi

    }
    public static class RoomHelper {

        private static string[ ] _values = {"walls", "floors", "furniture"};

        public static GameObject ImportRoom( this GameObject room ) {
            GameObject output = Object.Instantiate( room );

            // search through first anchor 
            foreach ( Transform c1 in output.transform ) {
                // search through each  layer
                foreach ( Transform c2 in c1 ) {
                    // search through each object in that layer 
                    foreach ( Transform c3 in c2 ) {
                        foreach ( var v in _values ) {
                            // assign tag 
                            if ( !c3.name.ToLower( ).Contains( v.ToLower( ) ) ) continue;

                            c3.gameObject.tag = v;
                        }
                    }
                }
            }
            return output;
        }

    }
    
    public class RoomManager : MonoBehaviour {

        public List<RoomTracker> Trackers { get; set; }
        public static RoomManager Instance;

        private void Awake( ) {
            Instance = this;
            Trackers = new List<RoomTracker>( );
        }

        private bool _set = false;

        public void Update( ) {
            Debug.Log( "list count " + Trackers.Count );

            if ( Trackers.Count > 0 ) {
                if ( !_set ) {
                    var targetAnchor = Trackers[ 0 ];
                    FlockHandler.Instance.SpawnPoint = targetAnchor.Pos;
                    FlockHandler.Instance.SpawnSceneSetup( targetAnchor );
                    _set = true;
                }
            }

            if ( Trackers.Count == 0 ) {
                _set = false;
            }
            FlockHandler.Instance.ActiveTarget = Trackers.Count > 0;
        }

    }
}