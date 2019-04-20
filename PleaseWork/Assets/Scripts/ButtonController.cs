using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public Button startButton, stopButton;
	public Timer timer;
	public CubeController cubes;
	// Use this for initialization
	void Start () {
		startButton.onClick.AddListener(StartButtonClicked);
		stopButton.onClick.AddListener(StopButtonClicked);
	}
	
	private void StartButtonClicked()
	{
		timer.StartTimer();
		cubes.StartFlickering();
	}

	private void StopButtonClicked()
	{
		timer.StopTimer();
		cubes.StopFlickering();
	}
}
