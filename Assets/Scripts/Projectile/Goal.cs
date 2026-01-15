using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Goal : BasicBehaviour
{
    public Transform player;
    [SerializeField]
    protected Scene scene;
   
    private bool isGoal = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isGoal = true;
            Debug.Log("Goal");
            SceneManager.LoadScene("GoalScene");
        }
    }

    public bool IsGoal()
    {
        return isGoal;
    }


}