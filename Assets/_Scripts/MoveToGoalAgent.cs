using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace _Scripts
{
    public class MoveToGoalAgent : Agent
    {
        [SerializeField] 
        private Transform targetTransform;
        
        [SerializeField] 
        private Material winMaterial;
        
        [SerializeField] 
        private Material loseMaterial;
        
        [SerializeField] 
        private MeshRenderer floorMeshRenderer;

        public float Speed = 1f;

        public override void OnEpisodeBegin()
        {
            // transform.localPosition = new Vector3(0, 0, 0);
            
            transform.localPosition = new Vector3(Random.Range(-4, 4), 0, Random.Range(-7, 0));
            targetTransform.localPosition = new Vector3(Random.Range(-4, 4), 0, Random.Range(0, 7));
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(targetTransform.localPosition);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float moveX = actions.ContinuousActions[0];
            float moveZ = actions.ContinuousActions[1];

            transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * Speed;
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = Input.GetAxisRaw("Vertical");
            continuousActions[1] = - Input.GetAxisRaw("Horizontal");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Goal"))
            {
                SetReward(1f);
                floorMeshRenderer.material = winMaterial;
                EndEpisode();
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                SetReward(-1f);
                floorMeshRenderer.material = loseMaterial;
                EndEpisode();
            }
        }
    }
}
