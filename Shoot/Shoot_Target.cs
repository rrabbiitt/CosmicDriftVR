using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Target : MonoBehaviour
{
    public GameObject explosionEffectPrefab;  // ���� ����Ʈ ������
    public float deactivationTime = 3.0f;      // ���� ��Ȱ��ȭ �ð�

    private bool isDeactivated = false;
    private GameObject currentExplosionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDeactivated && collision.gameObject.CompareTag("Bullet"))
        {
            // �浹�� ���� ������Ʈ�� �Ѿ��̶��
            Destroy(collision.gameObject); // �Ѿ� ����

            // ���� ��Ȱ��ȭ �� ��Ȱ��ȭ ����
            isDeactivated = true;
            gameObject.SetActive(false);
            Invoke("ReactivateTarget", deactivationTime);

            // ���� ����Ʈ ���� �� ���
            foreach (ContactPoint contact in collision.contacts)
            {
                currentExplosionEffect = Instantiate(explosionEffectPrefab, contact.point, Quaternion.identity);
                Destroy(currentExplosionEffect, 1.0f); // ���� ����Ʈ ��� �ð� �Ŀ� ����
                break; // �� ���� ����ϵ���
            }
        }
    }

    private void ReactivateTarget()
    {
        isDeactivated = false;
        gameObject.SetActive(true);
    }
}
