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

    //和田変更点
    public SpriteRenderer resultPanel;
    public Transform player;
    [Header("Y軸微調整")]
    public float heightFix = 0.1f;
    public float widthFix  = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        player = GameObject.FindWithTag("Player").transform;
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
        if (resultPanel == null || player == null) return;
        /*recttransform用ーーーーーーーーーー
        vector3[] corners = new vector3[4];
        resultpanel.getworldcorners(corners);
        vec = corners[0];
        vec1 = corners[3];
        */

        //その１ーーーーーーーーー
        Bounds bounds = resultPanel.bounds;

        

        vec = new Vector3(bounds.max.x - widthFix, bounds.max.y - heightFix, 0);
        vec1 = new Vector3(bounds.min.x - widthFix, bounds.max.y - heightFix, 0);
        //vec2 = player.position + new Vector3(0, 2f, 0);

        //その２
        //Vector3 size = resultPanel.sprite.bounds.size;
        //Vector3 pos = resultPanel.transform.position;
        //Vector3 scale = resultPanel.transform.lossyScale;
        //
        //float halfX = (size.x * scale.x) * 0.5f;
        //float halfY = (size.y * scale.y) * 0.5f;
        //
        //vec = new Vector3(pos.x - halfX, pos.y - halfY, 0);
        //vec1 = new Vector3(pos.x + halfX, pos.y - halfY, 0);
        //vec2 = player.position + new Vector3(0, 2f, 0);

        mesh.SetVertices(new Vector3[] {
             vec,
             vec1,
             vec2,
            }
        );
        mesh.triangles = new int[] { 0, 2, 1 };

        mesh.SetColors(colors);
    }
}
