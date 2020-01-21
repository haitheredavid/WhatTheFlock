using UnityEngine;
using Vuforia;

namespace myScripts {
    public class Test : MonoBehaviour, ITrackableEventHandler {

        public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus ) {
            throw new System.NotImplementedException( );
        }

    }
}