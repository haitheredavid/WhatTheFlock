using UnityEngine;
using Vuforia;

namespace myScripts {
    public class RoomTracker : MonoBehaviour, ITrackableEventHandler {

        public GameObject room;
        public Vector3 Pos { get; private set; }
        
        private GameObject _blueGoal;
        private GameObject _redGoal;
        
        public Vector3 blueStart { get; private set; }
        public Vector3 redStart { get; private set; }
        public Vector3 blueEnd { get; private set; }
        public Vector3 redEnd { get; private set; }
        private GameObject _instance;
        private bool _vuforiaCreated;
        private TrackableBehaviour _behaviour;
        private TrackableBehaviour.Status _previousStatus;
        private TrackableBehaviour.Status _newStatus;

        private void Awake( ) {
            if ( _blueGoal == null || _redGoal == null ) {
                SetGoals( );
            } else {
                redStart = _blueGoal.transform.position;
                blueStart = _redGoal.transform.position;
                redEnd = blueStart;
                blueEnd = redStart;
            }
        }

        private void SetGoals( ) {
            foreach ( Transform c1 in transform ) {
                if ( c1.CompareTag( "BlueGoal" ) ) {
                    _blueGoal = c1.gameObject;
                } else if ( c1.CompareTag( "RedGoal" ) ) {
                    _redGoal = c1.gameObject;
                }
            }
        }

        private void Update( ) {
            

            if ( !_vuforiaCreated ) {
                CheckCreation( );
            } else {
                if ( _newStatus == TrackableBehaviour.Status.TRACKED ) {
                    if ( _instance == null ) {
                        _instance = Instantiate( room, transform );
                        _instance.transform.position = transform.position;
                    }

                    if ( !RoomManager.Instance.Trackers.Contains( this ) ) {
                        RoomManager.Instance.Trackers.Add( this );
                    }
                }
            }
        }

        protected virtual void CheckCreation( ) {
            if ( !VuforiaManager.Instance.Initialized ) return;

            SetupTrackableBehavior( );
            _vuforiaCreated = true;
        }

        protected virtual void ToggleObjects( bool status ) {
            foreach ( Transform t in transform ) {
                t.gameObject.SetActive( status );
            }
        }

        protected virtual void OnTrackingFound( ) {
            Debug.Log( "trackingFound" );
            ToggleObjects( true );
        }

        protected virtual void OnExtendedTracking( ) { }

        protected virtual void OnLimitedTracking( ) {
            if ( RoomManager.Instance.Trackers.Contains( this ) ) {
                RoomManager.Instance.Trackers.Remove( this );
            }
            ToggleObjects( false );
        }

        protected virtual void OnTrackingLost( ) {
            if ( RoomManager.Instance.Trackers.Contains( this ) ) {
                RoomManager.Instance.Trackers.Remove( this );
            }
            ToggleObjects( false );
        }

        private void SetupTrackableBehavior( ) {
            _behaviour = GetComponent<TrackableBehaviour>( );

            if ( _behaviour ) {
                _behaviour.RegisterTrackableEventHandler( this );
            }
            _vuforiaCreated = true;
        }

        public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus ) {
            _previousStatus = previousStatus;
            _newStatus = newStatus;

            if ( _newStatus == TrackableBehaviour.Status.DETECTED )
                OnTrackingFound( );
            else if ( _newStatus == TrackableBehaviour.Status.TRACKED )
                OnTrackingFound( );
            else if ( _newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED )
                OnExtendedTracking( );
            else if ( _newStatus == TrackableBehaviour.Status.LIMITED )
                OnLimitedTracking( );
            else if ( _previousStatus == TrackableBehaviour.Status.TRACKED && _newStatus == TrackableBehaviour.Status.NO_POSE )
                OnTrackingLost( );
            else
                OnTrackingLost( );
        }

    }
}