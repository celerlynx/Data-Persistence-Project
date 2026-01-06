using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.linearVelocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;

        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            float delta = Random.Range(0.2f, 0.5f);
            velocity += velocity.y > 0 ? Vector3.up * delta : Vector3.down * delta;
        }
        if (Mathf.Abs(velocity.x) <= 0.1f)
        {
            m_Rigidbody.AddForce(new Vector3(-5f, 0, 0));
        }

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 2.5f;
        }

        m_Rigidbody.linearVelocity = velocity;
    }
}
