using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f; //How close to interact with the object
    private bool isFocus = false;
    private Transform player;
    private bool hasInteracted = false;
    public Transform interactionTransform;
    public Camera _camera;

    public virtual void Interact() {
        // This method is meant to be overridden.
        Debug.Log( "[Interactable]: Interacting with " +transform );
    }

    private void LateUpdate() {
        //transform.forward = _camera.transform.forward;
    }

    private void Update() {
        if( isFocus && !hasInteracted )
        {
            float distance = Vector3.Distance( player.position, interactionTransform.position );
            if( distance <= radius ) {
                Interact();
                hasInteracted = true;
            }
        }
    }
    public void OnFocused( Transform playerTransform )
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused() {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    void OnDrawGizmosSelected() {
        if( interactionTransform == null )
            interactionTransform = transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere( interactionTransform.position, radius );
    }
}