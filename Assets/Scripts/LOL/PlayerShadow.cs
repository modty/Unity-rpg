using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    public Transform playerTranform;//玩家(子物体)Transform
    public float shadowSizeFloat = 0.3f;//影子最小缩放比例(在原有基础上)

    private float heightDifference;//玩家跳跃高度差
    private Vector3 scale;//初始影子缩放大小
    private LOLPlayer playerMove;//声明玩家(父物体)移动脚本,主要是获取玩家设置的跳跃高度
    void Start()
    {
        playerMove = GetComponentInParent<LOLPlayer>();//得到玩家(父物体)移动脚本
        scale = transform.localScale;//将影子初始缩放赋值给scale
    }

    // Update is called once per frame
    void Update()
    {
        heightDifference = playerTranform.position.y - transform.position.y;//高度差计算:子物体y值-父物体y值
        //按照最大跳跃高度和高度差的比例来缩放影子大小，限制影子最小缩放
        //Mathf.Clamp()这里有三个参数，第一个参数是要限制的变量，第二个是最小值，第三个是最大值
        //用scale.x-(heightDifference/playerMove.jumpHeight)*scale.x,计算根据高度差与最大高度比例从0到初始值的变换
        //并且在最小值使用scale.x*shadowSizeFloat,来限制最小值，即使计算出来是0小于最小值，返回值也会是最小值
        //scale.y缩放同理
        transform.localScale = new Vector3(Mathf.Clamp(scale.x-(heightDifference/playerMove.jumpHeight)*scale.x, scale.x * shadowSizeFloat, scale.x), Mathf.Clamp(scale.y - (heightDifference / playerMove.jumpHeight) * scale.y, scale.y * shadowSizeFloat, scale.y), scale.z);
    }
}