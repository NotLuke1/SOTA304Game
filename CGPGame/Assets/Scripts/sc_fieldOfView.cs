using UnityEngine;

public class sc_fieldOfView : MonoBehaviour
{
    [HeaderAttribute("FOV Stats")]
    [SerializeField] private float viewFov;
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewCircleSize;
    [SerializeField] private int rayCount;
    [SerializeField] LayerMask layerMask;


    private Mesh viewMesh;
    private float angleBetweenRays;
    private float startAngle = 0;
    private Vector3 origin = Vector3.zero;

    void Start()
    {
        viewMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = viewMesh;



    }

    //Creates a mesh in runtime for the view cone. Raycasts used to block view when covered by objects

    void LateUpdate()
    {
        angleBetweenRays = viewFov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];
        vertices[0] = origin;

        float currAngle = startAngle;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hitData = Physics2D.Raycast(origin, vectorFromAngle(currAngle), viewDistance, layerMask);

            if (hitData.collider == null)
            {
                vertex = origin + vectorFromAngle(currAngle) * viewDistance;
            }
            else
            {
                vertex = hitData.point;

            }
            vertices[vertexIndex] = vertex;

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



        viewMesh.vertices = vertices;
        viewMesh.uv = uvs;
        viewMesh.triangles = triangles;
        viewMesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }



    private Vector3 vectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void setOrigin(Vector3 newOrigin)
    {
        origin = newOrigin;

    }

    public void setFacingDirection(Vector3 newFacingDirection)
    {
        newFacingDirection = newFacingDirection.normalized;
        float newAngle = Mathf.Atan2(newFacingDirection.y, newFacingDirection.x) * Mathf.Rad2Deg;
        if (newAngle < 0) newAngle += 360;
        startAngle = newAngle + viewFov/2f;
    }

    


}
