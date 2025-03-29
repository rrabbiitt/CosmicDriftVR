using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnterLobby : MonoBehaviour
{
    public XRGrabInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        interactable.onSelectEntered.AddListener(OnSelectEnter);
    }

    private void OnSelectEnter(XRBaseInteractor interactor)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
