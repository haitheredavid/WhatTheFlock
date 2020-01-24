using System.Collections.Generic;
using UnityEngine;

namespace myScripts {



    public class SpaceObj : MonoBehaviour {

        public SpaceContainer container;

        public void Awake( )
            {
                container = new SpaceContainer( gameObject );
            }

    }
    
    public class SpaceList : Space {

        public void SetList( GameObject[ ] input )
            {
                Meshes = new List<MeshRenderer>( );

                foreach ( var i in input ) {
                    var tempChild = ObjHelper.FindMeshRenderer( i );
                    foreach ( var c in tempChild ) {
                        Meshes.Add( c );
                    }
                }
            }

    }

    public class SpaceContainer : Space {

        public SpaceContainer( GameObject input )
            {
                Container = input;
                Meshes = ObjHelper.FindMeshRenderer( Container );
            }

    }

    public abstract class Space {

        protected List<MeshRenderer> Meshes = new List<MeshRenderer>( );
        protected GameObject Container { get; set; }

    }
}