using UnityEngine;
using UnityEngine.Events;

public class ThreeDButton : MonoBehaviour
{
    private Renderer cubeRenderer;
    private Color originalColor;
    private bool isHovered = false;

    public UnityEvent onClick;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isHovered)
                {
                    OnHoverEnter();
                    isHovered = true;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    OnClick();
                }
            }
            else if (isHovered)
            {
                OnHoverExit();
                isHovered = false;
            }
        }
        else if (isHovered)
        {
            OnHoverExit();
            isHovered = false;
        }
    }

    void OnHoverEnter()
    {
        onHoverEnter.Invoke();
    }

    void OnHoverExit()
    {
        onHoverExit.Invoke();
    }

    void OnClick()
    {
        onClick.Invoke();
    }
}
