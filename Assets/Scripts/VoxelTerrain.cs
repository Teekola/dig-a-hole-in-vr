using UnityEngine;

public class VoxelTerrain : MonoBehaviour
{
    [SerializeField] private GameObject voxelPrefab; // The voxel cube prefab
    [SerializeField] private GameObject referencePlane; // The reference plane to define terrain size
    [SerializeField] private int initialHeight = 2;

    private int width, depth, height;
    private float voxelSize;

    void Start()
    {
        referencePlane.SetActive(false); // Deactivate the reference plane
        height = initialHeight;
        voxelSize = voxelPrefab.transform.localScale.x;
        InitializeTerrain(); // Initialize scalar field and voxel objects based on the reference plane
    }

    void InitializeTerrain()
    {
        Renderer planeRenderer = referencePlane.GetComponent<Renderer>();
        Vector3 planeSize = planeRenderer.bounds.size;
        Vector3 planeCenter = planeRenderer.bounds.center;

        width = Mathf.RoundToInt(planeSize.x / voxelSize);
        depth = Mathf.RoundToInt(planeSize.z / voxelSize);

        // Align top voxel's top face with plane's top surface
        float topY = planeCenter.y + planeSize.y / 2 - voxelSize / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Vector3 position = new Vector3(
                        planeCenter.x - planeSize.x / 2 + (x * voxelSize) + voxelSize / 2,
                        topY - (y * voxelSize),
                        planeCenter.z - planeSize.z / 2 + (z * voxelSize) + voxelSize / 2
                    );

                    Vector3Int gridCoord = ToGridCoord(position);
                    Vector3 gridPos = new Vector3(gridCoord.x, gridCoord.y, gridCoord.z) * voxelSize;

                    Instantiate(voxelPrefab, gridPos, Quaternion.identity, transform);
                }
            }
        }
    }


   
    Vector3Int ToGridCoord(Vector3 pos)
    {
        return Vector3Int.RoundToInt(pos / voxelSize);
    }
}
