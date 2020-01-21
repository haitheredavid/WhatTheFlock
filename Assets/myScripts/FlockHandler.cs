using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Vuforia;

namespace myScript {
    public class FlockHandler : MonoBehaviour {

        public static FlockHandler Instance { get; set; }
        public bool ActiveTarget { private get; set; }
        public Vector3 SpawnPoint { private get; set; }

        // list of goals for points 
        public List<Vector3> RedGoals { private get; set; }
        public List<Vector3> BlueGoals { private get; set; }

        // spawner for agents 
        public GameObject RedSpawn { private get; set; }
        public GameObject BlueSpawn { private get; set; }

        // temp toggles for building scene 
        public bool createScene = false;
        public bool runSim = false;
        public int agentCount;
        
        public GameObject spawnScene;
        public Vector2 ratio = new Vector2( 1, 2 );
        
        public GameObject blueObj;
        public GameObject redObj;
        
        private List<GameObject> _redAgents;
        private List<GameObject> _blueAgents;
        private NavMeshAgent _redAgent;
        private NavMeshAgent _blueAgent;
        private GameObject _blueContainer;
        private GameObject _redContainer;
        private GameObject _tempRoom;

        private void Awake( ) {
            BlueSpawn = new GameObject( "Blue Spawner" );
            RedSpawn = new GameObject( "Red Spawner" );
            Instance = this;
            _redAgent = redObj.GetComponent<NavMeshAgent>( );
            _blueAgent = blueObj.GetComponent<NavMeshAgent>( );
        }

        public void SpawnSceneSetup( RoomTracker targetAnchor ) {
            var trackerRoom = Instantiate( spawnScene );
            trackerRoom.transform.position = targetAnchor.transform.position;
            BlueSpawn.transform.position = targetAnchor.blueStart;
            RedSpawn.transform.position = targetAnchor.blueStart;
        }

        private void Update( ) {
            Debug.Log( "Camera pos is " + CameraPosition.Instance.Pos );

            if ( !ActiveTarget ) {
                Debug.Log( "target is not active" );
                runSim = false;
                createScene = false;
                return;
            }

            //build the scene nw that target is tracking
            if ( ActiveTarget && !createScene ) {
                _tempRoom = SetUpScene( SpawnPoint, spawnScene );
                createScene = true;
            }

            if ( NavMeshWatcher.Instance.NavMeshReady ) {
                if ( _blueAgents != null ) {
                    DestroyAgents( );
                }
                SetUpAgents( );
                runSim = false;
            }
        }

        private GameObject SetUpScene( Vector3 loc, GameObject room ) {
            var scene = Instantiate( room );
            scene.transform.position = loc;
            scene.name = "Scene Room";
            return scene;
        }

        private void SetUpAgents( ) {
            // create containers 
            _blueContainer = new GameObject( "blue container" );
            _redContainer = new GameObject( "red container" );

            // get count 
            int blueAmount = (int) math.floor( agentCount / ratio.x );
            int redAmount = (int) math.floor( agentCount / ratio.y );

            // build list of agents 
            _blueAgents = BuildAgents( blueObj, _blueContainer, BlueSpawn, blueAmount, _blueAgent );
            _redAgents = BuildAgents( redObj, _redContainer, RedSpawn, redAmount, _redAgent );

            if ( BlueGoals != null && BlueGoals.Count > 0 && RedGoals != null && RedGoals.Count > 0 ) {
                // update agent list 
                SetAgentList( _blueAgents, BlueGoals );
                SetAgentList( _redAgents, RedGoals );
            } else {
                // Debug.Log( BlueGoals + " : " + BlueGoals.Count + RedGoals + " : " + RedGoals.Count );
            }
        }

        private static void SetAgentList( List<GameObject> agents, List<Vector3> goals ) {
            foreach ( var agent in agents ) {
                var data = agent.GetComponent<NavMeshAgent>( );

                if ( data != null ) {
                    data.SetDestination( goals[ 0 ] );
                }
            }
        }

        private static List<GameObject> BuildAgents( GameObject obj, GameObject container, GameObject start, int amount, NavMeshAgent agentData ) {
            List<GameObject> temp = new List<GameObject>( );

            for ( int i = 0; i < amount; i++ ) {
                var instance = Instantiate( obj );
                instance.name = obj.name + " " + i;
                instance.transform.position = start.transform.position;

                if ( instance.GetComponent<NavMeshAgent>( ) == null ) {
                    instance.AddComponent<NavMeshAgent>( );
                }
                temp.Add( instance );
            }
            return temp;
        }

        public void DestroyAgents( ) {
            _redAgents?.Clear( );
            _blueAgents?.Clear( );

            if ( _redContainer != null ) {
                foreach ( Transform agent in _redContainer.transform ) {
                    Destroy( agent.gameObject );
                }
                Destroy( _redContainer );
            }

            if ( _blueContainer != null ) {
                foreach ( Transform agent in _blueContainer.transform ) {
                    Destroy( agent.gameObject );
                }
                Destroy( _blueContainer );
            }
            Debug.Log( "agents cleared " );
        }

    }
}