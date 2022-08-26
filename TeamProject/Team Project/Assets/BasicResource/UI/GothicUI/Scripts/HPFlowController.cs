using UnityEngine;
using UnityEngine.UI;

namespace CrusaderUI.Scripts
{
	public class HPFlowController : MonoBehaviour {
	
		private Material _material;

		private void Start ()
		{
			_material = GetComponent<Image>().material;
		}

		public void SetValue(float value)
		{
			_material.SetFloat("_FillLevel", value);
		}
	}
}
