// 属性修改类

using System.Collections.Generic;

public class AttributeChange
{
    /// <summary>
    /// 施加该属性变化的来源
    /// </summary>
    private long fromUid;

    /// <summary>
    /// 该属性施加的目标
    /// </summary>
    private long toUid;

    /// <summary>
    /// 修改的属性
    /// </summary>
    private Dictionary<string, int> attribute;

    public AttributeChange(long fromUid, long toUid, Dictionary<string, int> attribute)
    {
        this.fromUid = fromUid;
        this.toUid = toUid;
        this.attribute = attribute;
    }

    public long FromUid
    {
        get => fromUid;
        set => fromUid = value;
    }

    public long ToUid
    {
        get => toUid;
        set => toUid = value;
    }

    public Dictionary<string, int> Attribute
    {
        get => attribute;
        set => attribute = value;
    }
}
