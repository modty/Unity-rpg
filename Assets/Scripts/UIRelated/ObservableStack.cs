using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//用于创建特殊事件的委托
public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{
    /// <summary>
    /// 当往栈中压入类
    /// </summary>
    public event UpdateStackEvent OnPush;

    /// <summary>
    /// 当从栈中弹出类
    /// </summary>
    public event UpdateStackEvent OnPop;

    /// <summary>
    /// 当栈为空
    /// </summary>
    public event UpdateStackEvent OnClear;

    public ObservableStack(ObservableStack<T> items) : base(items)
    {

    }

    public ObservableStack()
    {

    }

    public new void Push(T item)
    {
        base.Push(item);

        if (OnPush != null) // 确保事件发生时不会空指针
        {
            OnPush(); // 触发事件
        }
    }

    public new T Pop()
    {
        T item = base.Pop();

        if (OnPop != null)
        {
            OnPop();
        }

        return item;
    }

    public new void Clear()
    {
        base.Clear();

        if (OnClear != null)
        {
            OnClear();
        }
    }
}
