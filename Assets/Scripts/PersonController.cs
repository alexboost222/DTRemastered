using System.Collections;
using UnityEngine;

public class PersonController : MonoBehaviour
{
    [SerializeField] [Min(0)] private float velocity;
    [SerializeField] private Camera personCamera;
    [SerializeField] private Collider groundCollider;

    private Coroutine _movingCoroutine;

    private void Update()
    {
        if(personCamera == null || groundCollider == null)
        {
            Debug.LogWarning("Some serialized fields in PersonController are null. Check the editor", gameObject);
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;
        
        Ray cameraToMouseRay = personCamera.ScreenPointToRay(Input.mousePosition);
        
        if (!Physics.Raycast(cameraToMouseRay, out RaycastHit hit) || hit.collider != groundCollider) 
            return;
        
        if(_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);
                
        _movingCoroutine = StartCoroutine(MovingCoroutine(hit.point));
    }

    private IEnumerator MovingCoroutine(Vector3 point)
    {
        Transform personTransform = transform;
        while (true)
        {
            float distance = Vector3.Distance(personTransform.position, point);
            
            if(distance < velocity * Time.deltaTime)
            {
                personTransform.position = point;
                break;
            }
            
            Vector3 translation = (point - personTransform.position).normalized * velocity * Time.deltaTime;
            personTransform.Translate(translation, Space.World);
            personCamera.transform.LookAt(personTransform);
            yield return null;
        }

        _movingCoroutine = null;
    }
}