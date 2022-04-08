using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code based from Sebastian Lague
public class BallController : MonoBehaviour
{
    Rigidbody ballRb;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject splatter;
	Transform target;
    bool isTargetSet;
    bool isTargetLaunched;

	float h = 3;
	float gravity = -30;

	public bool debugPath;

	void Start() {
        ballRb = ball.GetComponent<Rigidbody>();
	}

	void Update() {
		if (isTargetSet && !isTargetLaunched) {
			Launch ();
            isTargetLaunched = true;
		}

		if (debugPath) {
			DrawPath ();
		}

        if (ball.GetComponent<BallTrigger>().groundEntered)
        {
            splatter.transform.position = ball.transform.position;

            ball.SetActive(false);
            splatter.SetActive(true);
            StartCoroutine(EvaporateSplatter());
        }
        else if (transform.position.y < -30)
        {
            Destroy(gameObject);
        }
	}

    IEnumerator EvaporateSplatter()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void SetTarget(Transform tf)
    {
        target = tf;
        isTargetSet = true;
    }

	void Launch() 
    {
		Physics.gravity = Vector3.up * gravity;
		ballRb.useGravity = true;
		ballRb.velocity = CalculateLaunchData ().initialVelocity;
	}

	LaunchData CalculateLaunchData() 
    {
		float displacementY = target.position.y - ballRb.position.y;
		Vector3 displacementXZ = new Vector3 (target.position.x - ballRb.position.x, 0, target.position.z - ballRb.position.z);
		float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}

	void DrawPath() 
    {
		LaunchData launchData = CalculateLaunchData ();
		Vector3 previousDrawPoint = ballRb.position;

		int resolution = 30;
		for (int i = 1; i <= resolution; i++) {
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up *gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = ballRb.position + displacement;
			Debug.DrawLine (previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
	}

	struct LaunchData 
    {
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData (Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}	
	}
}
