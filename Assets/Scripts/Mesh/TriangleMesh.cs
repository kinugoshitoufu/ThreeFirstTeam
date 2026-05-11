using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriangleMesh : MonoBehaviour
{
    public static TriangleMesh instance;
    public Vector3 vec = new Vector3(-5.92f, -0.17f, 0);
    public Vector3 vec1 = new Vector3(6.68f, -0.17f, 0);
    public Vector3 vec2 = new Vector3(0, 1f, 0);
    public Color[] colors;
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    public RectTransform resultPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        mesh = new Mesh();
        meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        // 頂点定義 (2D)
        Vector3[] vertices = new Vector3[3] {
             vec,
             vec1,
             vec2,
        };
        mesh.vertices = vertices;

        // 三角形定義
        mesh.triangles = new int[3] { 0, 1, 2 };

        // ★頂点カラーの設定 (各頂点に色を割り当て)
        mesh.SetColors(colors);
        meshRenderer.material = new Material(Shader.Find("Custom/Triangle"));
    }


    void Update()
    {
        //Bounds bounds = resultPanel.bounds;
        //vec = new Vector3(bounds.min.x, bounds.min.y,0);
        //vec2 = new Vector3(bounds.max.x,bounds.min.y,0);

        Vector3[] corners = new Vector3[4];
        resultPanel.GetWorldCorners(corners);

        vec = corners[0];
        vec1 = corners[3];
        mesh.SetVertices(new Vector3[] {
             vec,
             vec1,
             vec2,
            }
        );
        mesh.triangles = new int[] { 0, 1, 2 };

        mesh.SetColors(colors);
    }
}
