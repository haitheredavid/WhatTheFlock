using UnityEngine;
using Random = UnityEngine.Random;

namespace myScripts {
    public class SceneHandler : MonoBehaviour {

        public Material meshMaterial;
        public int xAmount;
        public int yAmount;
        public RectTransform selectRect;
        private MeshContainer _meshContainer;
        private string _tagName = "BlueGoal";

        public void Start( ) {
            GameObject temp = new GameObject( );
            _meshContainer = temp.AddComponent<MeshContainer>( );
            _meshContainer.MeshMaterial = meshMaterial;
            _meshContainer.Verts = new Vector2( xAmount, yAmount );
            _meshContainer.UpdateMeshContainer( _tagName );
        }

        public void Update( ) {
            if ( !Input.GetKeyDown( KeyCode.E ) ) return;

            GameObject instance = GameObject.CreatePrimitive( PrimitiveType.Cube );
            instance.tag = _tagName;
            instance.name = "instance";
            instance.transform.position = new Vector3( Random.Range( -10, 10 ), Random.Range( -10, 10 ), Random.Range( -10, 10 ) );
            _meshContainer.UpdateMeshContainer( _tagName );
        }

    }
}