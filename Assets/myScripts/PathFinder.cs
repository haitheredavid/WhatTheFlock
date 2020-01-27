using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace myScripts {
    public class PathFinder : MonoBehaviour {

        public Transform seeker;
        public Transform target;
        private GridObj _grid;

        private void Awake( ) {
            _grid = GetComponent<GridObj>( );
        }

        private void Update( ) {
            if ( !Input.GetKeyDown( KeyCode.K ) ) return;

            FindPath( seeker.position, target.position );
        }

        private void FindPath( Vector3 startPos, Vector3 targetPos ) {
            Stopwatch sw = new Stopwatch( );
            sw.Start( );
            Node startNode = _grid.NodeFromWorldPoint( startPos );
            Node targetNode = _grid.NodeFromWorldPoint( targetPos );
            Heap<Node> openSet = new Heap<Node>(_grid.MaxSize );
            HashSet<Node> closeSet = new HashSet<Node>( );
            openSet.Add( startNode );

            while ( openSet.Count > 0 ) {
                Node currentNode = openSet.RemoveFirst(  );

       
                closeSet.Add( currentNode );

                if ( currentNode == targetNode ) {
                    sw.Stop( );
                    Debug.Log( "path found " + sw.ElapsedMilliseconds + " ms" );
                    ReTracePath( startNode, targetNode );
                    return;
                }

                foreach ( Node neighbour in _grid.GetNeighbours( currentNode ) ) {
                    if ( !neighbour.Walkable || closeSet.Contains( neighbour ) ) continue;

                    int newMovementCostToNeighbour = currentNode.G_Cost + GetDistance( currentNode, neighbour );

                    if ( newMovementCostToNeighbour < neighbour.G_Cost || !openSet.Contains( neighbour ) ) {
                        neighbour.G_Cost = newMovementCostToNeighbour;
                        neighbour.H_Cost = GetDistance( neighbour, targetNode );
                        neighbour.Parent = currentNode;

                        if ( !openSet.Contains( neighbour ) )
                            openSet.Add( neighbour );
                    }
                }
            }
        }

        private void ReTracePath( Node startNode, Node endNode ) {
            List<Node> path = new List<Node>( );
            Node currentNode = endNode;

            while ( currentNode != startNode ) {
                path.Add( currentNode );
                currentNode = currentNode.Parent;
            }
            path.Reverse( );
            _grid.path = path;
        }

        private int GetDistance( Node nodeA, Node nodeB ) {
            int distX = Mathf.Abs( nodeA.GridX - nodeB.GridX );
            int distY = Mathf.Abs( nodeA.GridY - nodeB.GridY );

            if ( distX > distY )
                return 14 * distY + 10 * ( distX - distY );

            return 14 * distX + 10 * ( distY - distX );
        }

    }
}