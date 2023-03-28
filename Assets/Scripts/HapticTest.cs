using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticTest : MonoBehaviour
{
    XRBaseInteractable interactable;
    XRBaseControllerInteractor interactor;

    void Start()
    {
        if (TryGetComponent<XRBaseInteractable>(out interactable))
        {
            interactable.hoverEntered.AddListener(StartHover);
        }
        if (TryGetComponent<XRBaseControllerInteractor>(out interactor))
        {
            interactor.hoverEntered.AddListener(StartHover);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
            interactable.hoverEntered.RemoveListener(StartHover);

        if (interactor != null)
            interactor.hoverEntered.RemoveListener(StartHover);
    }

    public void StartHover(HoverEnterEventArgs args)
    {
        var interactor = args.interactorObject as XRBaseControllerInteractor;
        if (interactor != null)
        {
            Debug.Log("Sending Haptic Impulse...");
            interactor.SendHapticImpulse(1f, 0.08f);
        }
    }
}
