using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace myScripts {
    public abstract class BaseAgent {

        public Vector3 StartPos;
        public Vector3 EndPos;

    }

    public class DotAgent : BaseAgent {

        

    }
    public class NavAgent : BaseAgent {

        public NavMeshAgent AgentData;
        public GameObject Obj;

    }
    public class AgentManager {

        public AgentManager( List<GameObject> agentType, List<Vector3> spawns, int agentCount ) {
            if ( agentType.Count != spawns.Count ) {
                Debug.LogError( "Spawns and Agents do not match " );
                return;
            }
            _agentCount = agentCount;

            for ( var i = 0; i < agentType.Count; i++ ) {
                int count = SetAgentCount( );
                if ( count == 0 ) return;

                NavAgent[ ] tempAgents = new NavAgent[ count ];

                for ( var index = 0; index < tempAgents.Length; index++ ) {
                    tempAgents[ index ] = new NavAgent {
                        StartPos = spawns[ i ],
                        Obj = agentType[ i ],
                        AgentData = agentType[ i ].GetComponent<NavMeshAgent>( )
                    };
                    tempAgents[ index ].AgentData.Warp( tempAgents[ index ].StartPos );
                }
                _currentAgents.Add( agentType[ i ].name, tempAgents );
            }
        }

        private Dictionary<string, NavAgent[ ]> _currentAgents = new Dictionary<string, NavAgent[ ]>( );
        private int _agentCount;

        private int SetAgentCount( ) {
            int input = Random.Range( 0, _agentCount );
            _agentCount -= input;

            if ( _agentCount < 0 ) {
                _agentCount = 0;
            }
            return _agentCount;
        }

        public void SetAgentEnd( NavAgent[ ] agents, Vector3 pos ) {
            foreach ( var agent in agents ) {
                agent.AgentData.SetDestination( pos );
            }
        }

        public void SetAgentStart( NavAgent[ ] agents, Vector3 pos ) {
            foreach ( var agent in agents ) {
                agent.AgentData.Warp( pos );
            }
        }

    }
}