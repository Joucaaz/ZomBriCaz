using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScrollToSelection2 : MonoBehaviour
{
	#region MEMBERS

	[Header("[ References ]")]
	[SerializeField, Tooltip("View (boundaries/mask) rect transform. Used to check if automatic scroll to selection is required.")]
	private RectTransform viewRectTransform;
	[SerializeField, Tooltip("Scroll rect used to reach selected element.")]
	private ScrollRect targetScrollRect;

	[Header("[ Scrolling ]")]
	[SerializeField, Tooltip("Allow automatic scrolling only on these axes.")]
	private Axis scrollAxes = Axis.ANY;
	[SerializeField, Tooltip("MOVE_TOWARDS: stiff movement, LERP: smoothed out movement")]
	private ScrollMethod usedScrollMethod = ScrollMethod.MOVE_TOWARDS;
	[SerializeField]
	private float scrollSpeed = 50;

	[Space(5)]
	[SerializeField, Tooltip("Scroll speed used when element to select is out of \"JumpOffsetThreshold\" range")]
	private float endOfListJumpScrollSpeed = 150;
	[SerializeField, Range(0, 1), Tooltip("If next element to scroll to is located over this screen percentage, use \"EndOfListJumpScrollSpeed\" to reach this element faster.")]
	private float jumpOffsetThreshold = 1;

	[Header("[ Input ]")]
	[SerializeField]
	private MouseButton cancelScrollMouseButtons = MouseButton.ANY;
	[SerializeField]
	private KeyCode[] cancelScrollKeys = new KeyCode[0];

	// INTERNAL - MEMBERS ONLY
	private Vector3[] viewRectCorners = new Vector3[4];
	private Vector3[] selectedElementCorners = new Vector3[4];

	#endregion

	#region PROPERTIES

	// REFERENCES
	private RectTransform ViewRectTransform {
		get { return viewRectTransform; }
		set { viewRectTransform = value; }
	}
	private ScrollRect TargetScrollRect {
		get { return targetScrollRect; }
		set { targetScrollRect = value; }
	}

	// SCROLLING
	private Axis ScrollAxes {
		get { return scrollAxes; }
	}
	private ScrollMethod UsedScrollMethod {
		get { return usedScrollMethod; }
	}
	private float ScrollSpeed {
		get { return scrollSpeed; }
	}

	private float EndOfListJumpScrollSpeed {
		get { return endOfListJumpScrollSpeed; }
	}
	private float JumpOffsetThreshold {
		get { return jumpOffsetThreshold; }
	}

	// INPUT
	private MouseButton CancelScrollMouseButtons {
		get { return cancelScrollMouseButtons; }
	}
	private KeyCode[] CancelScrollKeys {
		get { return cancelScrollKeys; }
	}

	// VARIABLES
	private RectTransform ScrollRectContentTransform { get; set; }
	private GameObject LastCheckedSelection { get; set; }

	// COROUTINES
	private Coroutine AnimationCoroutine { get; set; }

	#endregion

	#region FUNCTIONS

	protected void Awake ()
	{
		ValidateReferences();
	}

	protected void LateUpdate ()
	{
		TryToScrollToSelection();
	}

	protected void Reset ()
	{
		TargetScrollRect = gameObject.GetComponentInParent<ScrollRect>() ?? gameObject.GetComponentInChildren<ScrollRect>();
		ViewRectTransform = gameObject.GetComponent<RectTransform>();
	}

	private void ValidateReferences ()
	{
		if (ViewRectTransform == null)
		{
			ViewRectTransform = TargetScrollRect.GetComponent<RectTransform>();
		}

		if (TargetScrollRect != null)
		{
			ScrollRectContentTransform = TargetScrollRect.content;
		}
	}

	private void TryToScrollToSelection ()
	{
		if (EventSystem.current == null)
		{
			Debug.LogError("[UIScrollToSelection] Unity UI EventSystem not found. It is required to check current selected object.");
			enabled = false;
			return;
		}

		// update references if selection changed
		GameObject selection = EventSystem.current.currentSelectedGameObject;

		if (selection == null || selection.activeInHierarchy == false || selection == LastCheckedSelection ||
			selection.transform.IsChildOf(transform) == false)
		{
			return;
		}

		RectTransform selectionRect = selection.GetComponent<RectTransform>();

		ViewRectTransform.GetWorldCorners(viewRectCorners);
		selectionRect.GetWorldCorners(selectedElementCorners);

		ScrollToSelection(selection);

		LastCheckedSelection = selection;
	}

	private void ScrollToSelection (GameObject selection)
	{
		// initial check if we can scroll at all
		if (selection == null)
		{
			return;
		}

		// this is just to make names shorter a bit
		Vector3[] corners = viewRectCorners;
		Vector3[] selectionCorners = selectedElementCorners;

		// calculate scroll offset
		Vector2 offsetToSelection = Vector2.zero;

		offsetToSelection.x =
			(selectionCorners[0].x < corners[0].x ? selectionCorners[0].x - corners[0].x : 0) +
			(selectionCorners[2].x > corners[2].x ? selectionCorners[2].x - corners[2].x : 0);
		offsetToSelection.y =
			(selectionCorners[0].y < corners[0].y ? selectionCorners[0].y - corners[0].y : 0) +
			(selectionCorners[1].y > corners[1].y ? selectionCorners[1].y - corners[1].y : 0);

		// calculate final scroll speed
		float finalScrollSpeed = ScrollSpeed;

		if (Math.Abs(offsetToSelection.x) / Screen.width >= JumpOffsetThreshold || Math.Abs(offsetToSelection.y) / Screen.height >= JumpOffsetThreshold)
		{
			finalScrollSpeed = EndOfListJumpScrollSpeed;
		}

		// adjust scroll speeds to screen DPI
		float horizontalSpeed = (Screen.width / Screen.dpi) * finalScrollSpeed;
		float verticalSpeed = (Screen.height / Screen.dpi) * finalScrollSpeed;

		// initiate animation coroutine
		Vector2 targetPosition = (Vector2)ScrollRectContentTransform.localPosition - offsetToSelection;

		if (AnimationCoroutine != null)
		{
			StopCoroutine(AnimationCoroutine);
		}

		AnimationCoroutine = StartCoroutine(ScrollToPosition(targetPosition, finalScrollSpeed));
	}

	private IEnumerator ScrollToPosition (Vector2 targetPosition, float speed)
	{
		Vector3 startPosition = ScrollRectContentTransform.localPosition;

		// cancel movement on axes not specified in ScrollAxes mask
		targetPosition.x = ((ScrollAxes | Axis.HORIZONTAL) == ScrollAxes) ? targetPosition.x : startPosition.x;
		targetPosition.y = ((ScrollAxes | Axis.VERTICAL) == ScrollAxes) ? targetPosition.y : startPosition.y;
		
		// move to target position
		Vector2 currentPosition2D = startPosition;
		float horizontalSpeed = (Screen.width / Screen.dpi) * speed;
		float verticalSpeed = (Screen.height / Screen.dpi) * speed;
		
		while (currentPosition2D != targetPosition && CheckIfScrollInterrupted() == false)
		{
			currentPosition2D.x = MoveTowardsValue(currentPosition2D.x, targetPosition.x, horizontalSpeed, UsedScrollMethod);
			currentPosition2D.y = MoveTowardsValue(currentPosition2D.y, targetPosition.y, verticalSpeed, UsedScrollMethod);

			ScrollRectContentTransform.localPosition = currentPosition2D;
			
			yield return null;
		}

		ScrollRectContentTransform.localPosition = currentPosition2D;
	}

	private bool CheckIfScrollInterrupted ()
	{
		bool mouseButtonClicked = false;
		
		// check mouse buttons
		switch (CancelScrollMouseButtons)
		{
			case MouseButton.LEFT:
				mouseButtonClicked |= Input.GetMouseButtonDown(0);
				break;
			case MouseButton.RIGHT:
				mouseButtonClicked |= Input.GetMouseButtonDown(1);
				break;
			case MouseButton.MIDDLE:
				mouseButtonClicked |= Input.GetMouseButtonDown(2);
				break;
		}

		if (mouseButtonClicked == true)
		{
			return true;
		}

		// check keyboard buttons
		for (int i = 0; i < CancelScrollKeys.Length; i++)
		{
			if (Input.GetKeyDown(CancelScrollKeys[i]) == true)
			{
				return true;
			}
		}

		return false;
	}

	private float MoveTowardsValue (float from, float to, float delta, ScrollMethod method)
	{
		switch (method)
		{
			case ScrollMethod.MOVE_TOWARDS:
				return Mathf.MoveTowards(from, to, delta * Time.unscaledDeltaTime);
			case ScrollMethod.LERP:
				return Mathf.Lerp(from, to, delta * Time.unscaledDeltaTime);
			default:
				return from;
		}
	}

	#endregion

	#region CLASS_ENUMS

	[Flags]
	public enum Axis
	{
		NONE = 0x00000000,
		HORIZONTAL = 0x00000001,
		VERTICAL = 0x00000010,
		ANY = 0x00000011
	}

	[Flags]
	public enum MouseButton
	{
		NONE = 0x00000000,
		LEFT = 0x00000001,
		RIGHT = 0x00000010,
		MIDDLE = 0x00000100,
		ANY = 0x00000111
	}

	public enum ScrollMethod
	{
		MOVE_TOWARDS,
		LERP
	}

	#endregion
}
