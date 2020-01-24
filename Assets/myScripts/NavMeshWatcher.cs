using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace myScripts {
    public class NavMeshWatcher : MonoBehaviour {

        public float factor = .01f;
        public Vector3 size = new Vector3( 80, 20, 80 );
        private Vector3 _center;
        private NavMeshData _navMesh;
        private AsyncOperation _operation;
        private NavMeshDataInstance _navInstance;
        private List<NavMeshBuildSource> _sources = new List<NavMeshBuildSource>( );
        public static NavMeshWatcher Instance { get; set; }
        public bool NavMeshReady { get; set; }

        private void Awake( ) {
            Instance = this;
            
        }

        private void OnEnable( ) {
            UpdateNavMesh( false );
            NavMeshReady = true;
        }

        private void OnDisable( ) {
            _navInstance.Remove(  );
            FlockHandler.Instance.DestroyAgents( );
            NavMeshReady = false;
        }

        private void UpdateNavMesh( bool asyync = false ) {
            _navMesh = new NavMeshData( );
            _navInstance = NavMesh.AddNavMeshData( _navMesh );
            var settings = NavMesh.GetSettingsByID( 0 );
            NavMeshSourceTag.Collect( ref _sources );
            var bounds = BoundsHelper.QauntizeBounds( _center, size, factor );

            if ( asyync )
                _operation = NavMeshBuilder.UpdateNavMeshDataAsync( _navMesh, settings, _sources, bounds );
            else
                NavMeshBuilder.UpdateNavMeshData( _navMesh, settings, _sources, bounds );
        }

        private void Update( ) {
            _center = transform.position;
        }

        public IEnumerator Start( ) {
            while ( true ) {
                UpdateNavMesh( true );
                yield return _operation;
            }
        }

        private void OnDrawGizmosSelected( ) {
            if ( _navMesh ) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube( _navMesh.sourceBounds.center, _navMesh.sourceBounds.size );
            }
            Gizmos.color = Color.yellow;
            var bounds = GetBounds( );
            Gizmos.DrawWireCube( bounds.center, bounds.size );
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube( _center, size );
        }

        private Bounds GetBounds( ) {
            return BoundsHelper.QauntizeBounds( _center, size, factor );
        }

    }
}