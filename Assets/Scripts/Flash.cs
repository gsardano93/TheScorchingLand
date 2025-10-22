using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
     [SerializeField] Material flashMaterial;
    [SerializeField] float restoreDefaultMatTime = 0.2f;
    Material defaultMaterial;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }
    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMaterial;
    }
    public float GetRestoreDefaultMatTime(){
        return restoreDefaultMatTime;
    }
}
