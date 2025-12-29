using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : BasicBehaviour
{
    public Transform player;
   
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