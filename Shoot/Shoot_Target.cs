using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Target : MonoBehaviour
{
    public GameObject explosionEffectPrefab;  // 폭발 이펙트 프리팹
    public float deactivationTime = 3.0f;      // 과녁 비활성화 시간

    private bool isDeactivated = false;
    private GameObject currentExplosionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDeactivated && collision.gameObject.CompareTag("Bullet"))
        {
            // 충돌한 게임 오브젝트가 총알이라면
            Destroy(collision.gameObject); // 총알 제거

            // 과녁 비활성화 및 재활성화 예약
            isDeactivated = true;
            gameObject.SetActive(false);
            Invoke("ReactivateTarget", deactivationTime);

            // 폭발 이펙트 생성 및 재생
            foreach (ContactPoint contact in collision.contacts)
            {
                currentExplosionEffect = Instantiate(explosionEffectPrefab, contact.point, Quaternion.identity);
                Destroy(currentExplosionEffect, 1.0f); // 폭발 이펙트 재생 시간 후에 제거
                break; // 한 번만 재생하도록
            }
        }
    }

    private void ReactivateTarget()
    {
        isDeactivated = false;
        gameObject.SetActive(true);
    }
}
