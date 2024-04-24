using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ItemSystem
{
	public class ItemController
	{
		private Dictionary<string, List<Item>> _items = new ();

		public void AddItemUI(string id, Item item)
		{
			if (_items.TryGetValue(id, out List<Item> items))
			{
				items.Add(item);
			}
			else
			{
				List<Item> newItems = new List<Item> { item };
				_items.Add(id, newItems);
			}
		}
		
		public RectTransform GetRectTransform(string id)
		{
			if (_items.TryGetValue(id, out List<Item> items))
			{
				foreach (var item in items)
				{
					if (item.RTr == null) continue;

					return item.RTr;
				}
				return null;
			}

			return null;
		}
		
		public GridLayoutGroup GetGridLayoutGroup(string id)
		{
			if (_items.TryGetValue(id, out List<Item> items))
			{
				foreach (var item in items)
				{
					if (item.GridL == null) continue;

					return item.GridL;
				}
				return null;
			}

			return null;
		}
		
		public void SetAction(string id, UnityAction func)
		{
			if (_items.TryGetValue(id, out List<Item> items))
			{
				foreach (var item in items)
				{
					if (item.Btn == null) continue;
					item.Btn.onClick.AddListener(func);
				}
			}
		}
		
		public void SetAction(string id, UnityAction<string> func)
		{
			if (_items.TryGetValue(id, out List<Item> items))
			{
				foreach (var item in items)
				{
					if (item.Btn == null) continue;
					item.Btn.onClick.AddListener(() => func(item.Parm));
				}
			}
		}

		public void SetText(string id, string text)
		{
			if (_items.TryGetValue(id, out List<Item> items))
			{
				foreach (var item in items)
				{
					if (item.TextTMP == null) continue;
					item.TextTMP.text = text;
				}
			}
		}
	}

	public class Item
	{
		public Button Btn;
		public RectTransform RTr;
		public TMP_Text TextTMP;
		public string Parm;
		public Animator Anim;
		public GridLayoutGroup GridL;

		public Item(Button btn)
		{
			Btn = btn;
		}

		public Item(TMP_Text text)
		{
			TextTMP = text;
		}
		
		public Item(RectTransform rtr)
		{
			RTr = rtr;
		}
		
		public Item(Animator anim)
		{
			Anim = anim;
		}
		
		public Item(GridLayoutGroup gridL)
		{
			GridL = gridL;
		}
		
		public Item(Button btn, string parm)
		{
			Btn = btn;
			Parm = parm;
		}
	}
}
