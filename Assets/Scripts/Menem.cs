using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Menem : MonoBehaviour
{
    [Header("Idle settings")]
    [SerializeField] private float timeToBeginDancing = 5f;

    [Header("Animation blend settings")]
    [SerializeField] [Range(0, 1)]  private float  blendCurve = 0.8f;
    [SerializeField]                private int    animationCount = 7;
    [SerializeField] [Range(0, 3)]  private float  transitionDuration = 0.5f;

    private Animator animator;
    private int activeLayer = 0;
    private bool switchingLayers = false;
    private AnimatorStateInfo stateInfo;
    private bool shouldDance = false;

    #region Unity methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("blend-main", Random.Range(1, animationCount));
    }

    private void Start()
    {
        StartCoroutine(StartDancingTimer());
    }

    #endregion

    #region Idle animation to Blend Tree

    private void Update()
    {
        if (!shouldDance) return;

        stateInfo = animator.GetCurrentAnimatorStateInfo(activeLayer);

        if (stateInfo.normalizedTime >= blendCurve && !switchingLayers)
        {
            switchingLayers = true;

            SetRandomAnimation();

            StartCoroutine(SwitchBetweenLayers());
        }
    }

    #endregion

    #region Layer Random animation and switch

    IEnumerator SwitchBetweenLayers()
    {
        float targetWeight = activeLayer == 0 ? 1f : 0f;
        float currentWeight = animator.GetLayerWeight(1);
        float transitionStartTime = Time.time;

        activeLayer = activeLayer == 0 ? 1 : 0;

        while (Time.time < transitionStartTime + transitionDuration)
        {
            float transitionProgress = (Time.time - transitionStartTime) / transitionDuration;
            float newWeight = Mathf.Lerp(currentWeight, targetWeight, transitionProgress);

            animator.SetLayerWeight(1, newWeight);

            yield return null;
        }

        animator.SetLayerWeight(1, targetWeight);
        switchingLayers = false;

    }

    private void SetRandomAnimation()
    {
        string paramName = activeLayer == 0 ? "blend-main" : "blend-layer-2";
        animator.SetFloat(paramName, Random.Range(1, animationCount));
        animator.Play("Blend Tree", activeLayer == 0 ? 1 : 0, 0f);
    }

    #endregion

    #region Dance Triggers

    public void StopDancing()
    {
        shouldDance = false;
        animator.SetTrigger("StopDancing");
    }

    public void BeginDance()
    {
        shouldDance = true;
        animator.SetTrigger("BeginDance");
    }

    IEnumerator StartDancingTimer()
    {
        yield return new WaitForSeconds(timeToBeginDancing);
        BeginDance();
    }

    #endregion
}
