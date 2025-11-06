using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    // The name of the next scene to load
    public string nextSceneName;
    // Add this code to your player script
    public DisplayMessage displayMessage;

    public string showMessage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.gameObject.tag == "Player")
        {
            // display win message
            displayMessage.ShowMessage(showMessage);
            // Delay for visual effect (optional)
            Invoke("LoadNextLevel", 2f);
        }
    }

    // Function to load the next scene
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
