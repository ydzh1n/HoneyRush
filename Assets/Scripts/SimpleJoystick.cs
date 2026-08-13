using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private float radius = 90f;

    public float AxisX { get; private set; }
    public float AxisY { get; private set; }

    private RectTransform self;
    private Vector2 startLocal;

    private void Awake()
    {
        self = GetComponent<RectTransform>();
        handle.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(self, e.position, e.pressEventCamera, out startLocal);
        handle.gameObject.SetActive(true);
        OnDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(self, e.position, e.pressEventCamera, out local);
        Vector2 delta = Vector2.ClampMagnitude(local - startLocal, radius);
        handle.anchoredPosition = startLocal + delta;
        AxisX = delta.x / radius;
        AxisY = delta.y / radius;
    }

    public void OnPointerUp(PointerEventData e)
    {
        handle.gameObject.SetActive(false);
        AxisX = 0f;
        AxisY = 0f;
    }
}