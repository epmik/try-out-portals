using UnityEngine;
using TMPro;

internal class FpsTextController : CommonBehaviour
{
    public TextMeshProUGUI Text;

    private float _pollingTime = 1f;
    private float _time;
    private int _frameCount;

    private void Update()
    {
        _time += Time.unscaledDeltaTime;
        _frameCount++;

        if (_time >= _pollingTime)
        {
            int frameRate = Mathf.RoundToInt(_frameCount / _time);

            Text.text = frameRate + " FPS";

            //Log.Trace("FpsComponent.Update(): " + FpsText.text);

            _time -= _pollingTime;
            _frameCount = 0;
        }
    }
}
