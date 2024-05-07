using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class FreeCameraMovement : CameraController
{
    [Header("Movement Settings")]
    public FixedJoystick moveJoystick;
    public float moveSpeed = 5f;
    public float upDownSpeed = 2f;
    public float rotationSpeed = 10f;
    public float upSpeed = 5f;
    public float downSpeed = 2f;

    [Header("UI Elements")]
    public TMP_Text textBox;
    public Image pointerImage;

    [Header("Materials")]
    public Material highlightMaterial;
    public Material defaultMaterial;

    private CharacterController controller;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private Transform _selection;
    private string selectableTag = "Selectable";
    private bool isFreeCameraActive = true;

    public Button ChangeCamera;
    public UIManager uiManager;
    public CameraController NextCameraController;
    private CameraScript cameraScriptRef;

    public override void Init(CameraController touchControlCameraScript)
    {
        Debug.Log("Entering FreeCamera Mode");
        controller = GetComponent<CharacterController>();
        NextCameraController = touchControlCameraScript;
        EnableFreeCamera();
        uiManager.SetUIElementsActive(true);
        ChangeCamera.gameObject.SetActive(false);
    }

    public override void CameraRoutine(CameraScript cameraScript)
    {
        cameraScriptRef = cameraScript;
        if (isFreeCameraActive)
        {
            Movement();
            Rotation();
            SelectionCheck(cameraScript);
        }
    }

    void DisableFreeCamera()
    {
        isFreeCameraActive = false;
    }

    void EnableFreeCamera()
    {
        isFreeCameraActive = true;
    }

    void Movement()
    {
        if(moveJoystick.Horizontal + moveJoystick.Vertical != 0) 
        {

            Vector3 moveDirection = transform.right * moveJoystick.Horizontal + transform.forward * moveJoystick.Vertical;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        }
        

        if (isMovingUp)
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);

        if (isMovingDown)
            transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
    }

    void Rotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x > Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationInputX = touch.deltaPosition.x;
                    float rotationInputY = touch.deltaPosition.y;
                    transform.Rotate(Vector3.up, rotationInputX * rotationSpeed * Time.deltaTime);
                    transform.Rotate(Vector3.left, rotationInputY * upDownSpeed * Time.deltaTime);
                    Vector3 eulerRotation = transform.rotation.eulerAngles;
                    transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
                }
            }
        }
    }

    void SelectionCheck(CameraScript cameraScript)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                   // Debug.Log("Touch is over UI");
                    /*uiManager.SetUIElementsActive(true);
                    textBox.text = "";
                    ResetMaterial();*/
                    return; 
                }

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;
                    if (selection.CompareTag(selectableTag))
                    {
                        ChangeMaterial(selection);
                        uiManager.SetUIElementsActive(false);
                        DisplayObjectProperties(selection.gameObject);

                        ChangeCamera.gameObject.SetActive(true);
                    }
                    else
                    {
                        ChangeCamera.gameObject.SetActive(false);
                        ResetMaterial();
                        uiManager.SetUIElementsActive(true);
                        textBox.text = "";
                    }
                }
                else
                {
                    ChangeCamera.gameObject.SetActive(false);
                    uiManager.SetUIElementsActive(true);
                    textBox.text = "";
                    ResetMaterial();
                }
            }
        }
    }

    public void ChangeScript()
    {

        Debug.Log("Button was pressed");
        if (NextCameraController != null && cameraScriptRef.CurrentController != null && _selection != null)
        {
            TouchControlCamera touchControlCamera = NextCameraController as TouchControlCamera;
            if (touchControlCamera != null)
            {
                touchControlCamera.targetObject = _selection;
            }

            cameraScriptRef.CurrentController = NextCameraController;
            cameraScriptRef.CurrentController.Init(this);
            DisableFreeCamera();
            ChangeCamera.gameObject.SetActive(false);

        }
    }

    void ChangeMaterial(Transform selection)
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = defaultMaterial;
            }
        }

        var newSelectionRenderer = selection.GetComponent<Renderer>();
        if (newSelectionRenderer != null)
        {
            newSelectionRenderer.material = highlightMaterial;
        }
        _selection = selection;

        ChangeCamera.gameObject.SetActive(true);
    }

    private string GetObjectDimensions(GameObject obj)
    {
        Bounds bounds = obj.GetComponent<Collider>().bounds;
        return $"Dimensions: {bounds.size.x:F2} x {bounds.size.y:F2} x {bounds.size.z:F2}";
    }

    void ResetMaterial()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = defaultMaterial;
                _selection = null;
            }
        }
    }

    public void MoveUp()
    {
        isMovingUp = !isMovingUp;
    }

    public void MoveDown()
    {
        isMovingDown = !isMovingDown;
    }

    void DisplayObjectProperties(GameObject obj)
    {
        string dimensions = GetObjectDimensions(obj);
        textBox.text = $"{dimensions}\nName: {obj.name}\nPosition: {obj.transform.position}\nRotation: {obj.transform.rotation}\nScale: {obj.transform.localScale}";
    }
}
