using UnityEngine;

public class sc_fieldOfView : MonoBehaviour
{
    [HeaderAttribute("FOV Stats")]
    [SerializeField] private float viewFov;
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewCircleSize;
    [SerializeField] private int rayCount;
    [SerializeField] private LayerMask allVisableMask;
    [SerializeField] private int fullBrightLayer;
    [SerializeField] private int fullShadowLayer;


    private Mesh viewMesh;
    private float angleBetweenRays;
    private float startAngle = 0;
    private Vector3 origin = Vector3.zero;

    void Start()
    {
        viewMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = viewMesh;



    }

    //Creates a mesh in runtime for the view cone. Raycasts used to block view when covered by objects. Variable rayCount is the number of raycasts used.

    void LateUpdate()
    {

        //Initiates structures that are used to create the mesh
        angleBetweenRays = viewFov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];
        vertices[0] = origin;

        float currAngle = startAngle;

        //Each raycast sets a vertex for the mesh, either at the max distance or at a collision point.
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hitData = Physics2D.Raycast(origin, vectorFromAngle(currAngle), viewDistance, allVisableMask);
            

            if (hitData.collider == null)
            {
                vertex = origin + vectorFromAngle(currAngle) * viewDistance;
            }
            else if (hitData.collider.gameObject.layer == fullBrightLayer)
            {

                /*If the object hit should still be lit, the vertex is adjusted.
                 * Another ray is cast backwards from the max distance until it collides with the same object.
                 * The vertex is placed on the other side of the object.
                 */
                RaycastHit2D[] hitData2 = Physics2D.LinecastAll(origin + vectorFromAngle(currAngle).normalized * viewDistance, hitData.point, allVisableMask);
                vertex = hitData2[hitData2.Length - 1].point;

            }
            else
            {
                vertex = hitData.point;
            }
            vertices[vertexIndex] = vertex;

            //Creates the mesh's triangles from the vertices
            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            currAngle -= angleBetweenRays;
        }


        //Creates the mesh and updates bounds so it is always visible
        viewMesh.vertices = vertices;
        viewMesh.uv = uvs;
        viewMesh.triangles = triangles;
        viewMesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }



    //Mathematical funtions to convert between vectors and angles.
    private Vector3 vectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    
    public void setFacingDirection(Vector3 newFacingDirection)
    {
        newFacingDirection = newFacingDirection.normalized;
        float newAngle = Mathf.Atan2(newFacingDirection.y, newFacingDirection.x) * Mathf.Rad2Deg;
        if (newAngle < 0) newAngle += 360;
        startAngle = newAngle + viewFov/2f;
    }

    //Getters and Setters

    public void setOrigin(Vector3 newOrigin)
    {
        origin = newOrigin;

    }




}
