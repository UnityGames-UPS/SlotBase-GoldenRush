using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System;

public class ManageLineButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{

	[SerializeField]
	private SlotBehaviour slotManager;
	[SerializeField]
	private TMP_Text num;

	internal bool isActive = false;
	//[SerializeField]
	//private int number;



	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("run on pointer enter");
		if (isActive)
		{
			slotManager.GenerateStaticLine(num);
		}

	}



	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("run on pointer exit");
		if (isActive)
		{
			slotManager.DestroyStaticLine();
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
		{
			this.gameObject.GetComponent<Button>().Select();
			Debug.Log("run on pointer down");
			slotManager.GenerateStaticLine(num);
		}
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
		{
			Debug.Log("run on pointer up");
			slotManager.DestroyStaticLine();
			DOVirtual.DelayedCall(0.1f, () =>
			{
				this.gameObject.GetComponent<Button>().spriteState = default;
				EventSystem.current.SetSelectedGameObject(null);
			});
		}
	}

}
