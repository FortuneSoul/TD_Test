using System.Collections.Generic;

public class Registry<TItem>
{
	protected List<TItem> _items = new List<TItem>();
	public IEnumerable<TItem> Items => _items;

	public void Add(TItem item)
	{
		_items.Add(item);	
	}

	public void Remove(TItem item)
	{
		_items.Remove(item);
	}
}