using Godot;
using System.Collections.Generic;

public class VfxPool<T> where T : Node3D, IVfxObject {
	private Stack<T> m_pool;
	private Node m_parent;
	private PackedScene m_prefab;
	private int m_maxSize;

	public VfxPool(Node parent, PackedScene prefab, int initSize = 10, int maxSize = 20) {
		m_parent = parent;
		m_maxSize = maxSize;
		m_prefab = prefab;

		m_pool = new(initSize);
		for(int i=0; i<initSize; ++i) {
			Create();
		}
	}

	public T Pop() {
		if(m_pool.Count == 0) {
			GD.PushWarning("VfxPool is empty, creating fallback object");
			Create();
		}

		return m_pool.Pop();
	}
	
	private void Create() {
		if(m_prefab == null) {
			GD.PushError("Prefab is not set");
			return;
		}

		T obj = m_prefab.Instantiate<T>();
		m_parent.AddChild(obj);
		obj.onFinish += () => Push(obj);

		m_pool.Push(obj);
	}

	private void Push(T obj) {
		if(m_pool.Count >= m_maxSize) {
			obj.QueueFree();
			return;
		}

		m_pool.Push(obj);
	}
}
