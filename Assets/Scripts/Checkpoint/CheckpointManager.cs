using DG.Tweening;
using Ebac.Core.Sigleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointManager : Singleton<CheckpointManager>
{
    public int lastCheckpointKey = 0;

    public TextMeshProUGUI checkpointMessage;
    public Ease messageAnimation = Ease.OutBack;
    public float animationDuration = 1.5f;
    public float fadeDuration = 1f;


    public List<CheckpointBase> checkpoints;

    private Tween _currTween;

    public bool HasCheckpoint()
    {
        return lastCheckpointKey > 0;
    }

    public void SaveCheckpoint(int i)
    {
        if (i > lastCheckpointKey)
        {
            lastCheckpointKey = i;
            StartCoroutine(TurnOnCheckpointText());
        }
    }

    public Vector3 GetPositionFromLastCheckpoint()
    {
        var checkpoint = checkpoints.Find(i => i.key == lastCheckpointKey);

        return checkpoint.transform.position;
    }

    private IEnumerator TurnOnCheckpointText()
    {
        if (_currTween != null) _currTween.Kill();
        checkpointMessage.gameObject.SetActive(true);
        _currTween = checkpointMessage.DOFade(0f, animationDuration).From();

        yield return new WaitForSeconds(fadeDuration);

        _currTween = checkpointMessage.DOFade(0f, animationDuration);

        yield return new WaitForSeconds(fadeDuration);

        checkpointMessage.gameObject.SetActive(false);
        _currTween = checkpointMessage.DOFade(1f, animationDuration);
    }
}
