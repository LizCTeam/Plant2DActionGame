using UnityEngine;

public class WaitForAnimation : CustomYieldInstruction
{
    private Animator _animator;
    private int _stateHash = 0;
    private int _layerNo = 0;
    
    public WaitForAnimation(Animator animator,int layerNo,string stateName)
    {
        this._animator = animator;
        this._layerNo = layerNo;
        _stateHash = Animator.StringToHash(stateName);
    }
    
    public override bool keepWaiting
    {
        get
        {
            var currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(_layerNo);
            return (currentAnimatorState.shortNameHash == _stateHash && (currentAnimatorState.normalizedTime < 1f));
        }
    }
}
