using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// 74
public class CameraFollow : MonoBehaviour {

    /// <summary>
    /// 设置相机跟随的目标为角色
    /// </summary>
    private Transform target;

    /// <summary>
    /// 相机最大最小值
    /// </summary>
    private float xMax, xMin, yMin, yMax;

    /// <summary>
    /// 对TileMap的引用
    /// </summary>
    [SerializeField]
    private Tilemap tilemap;

    /// <summary>
    /// 对角色的引用
    /// </summary>
    private Player player;

	void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        player = target.GetComponent<Player>();

        // 计算最大、最小位置
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        SetLimits(minTile, maxTile);

        player.SetLimits(minTile, maxTile);

	}

    private void LateUpdate()
    {
        // 确保相机不会超出世界
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
    }

    /// <summary>
    /// 设置相机范围，确保不会超出世界
    /// </summary>
    /// <param name="minTile">最小位置</param>
    /// <param name="maxTile">最大位置</param>
    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;
    }
}
