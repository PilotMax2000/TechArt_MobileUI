using System;
using UnityEngine;
using UnityEngine.UI;

namespace TechArtProject
{
    public class BottomBarIconView : MonoBehaviour
    {
        public Action<int> OnSlotToggledOn;
        public Action<int> OnSlotToggledOff;

        public int IconSlotIndex => _iconSlotIndex;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _isLocked;
        [SerializeField] private bool _isToggled;
        [SerializeField] private int _iconSlotIndex;
        [SerializeField] private Button _button;
        [SerializeField] private string _isLockedParamName;
        [SerializeField] private string _toggleOnTriggerParamName;
        [SerializeField] private string _toggleOffTriggerParamName;
        
        private int _cachedIsLockedParam;
        private int _cachedToggleOnTriggerParam;
        private int _cachedToggleOffTriggerParam;
        
        private void Awake()
        {
            _cachedIsLockedParam = Animator.StringToHash(_isLockedParamName);
            _cachedToggleOnTriggerParam = Animator.StringToHash(_toggleOnTriggerParamName);
            _cachedToggleOffTriggerParam = Animator.StringToHash(_toggleOffTriggerParamName);
            
            _animator.SetBool(_cachedIsLockedParam, _isLocked);
            _button.onClick.AddListener(() =>
            {
                if (_isLocked) 
                    return;
                
                _isToggled = !_isToggled;
                if (_isToggled)
                {
                    _animator.SetTrigger(_cachedToggleOnTriggerParam);
                    OnSlotToggledOn?.Invoke(_iconSlotIndex);
                }
                else
                {
                    _animator.SetTrigger(_cachedToggleOffTriggerParam);
                    OnSlotToggledOff?.Invoke(_iconSlotIndex);
                }
            });
        }
        
        public void ToggleOff()
        {
            if (_isLocked || _isToggled == false) 
                return;
            
            _isToggled = false;
            _animator.SetTrigger(_cachedToggleOffTriggerParam);
            OnSlotToggledOff?.Invoke(_iconSlotIndex);
        }

        public void ToggleOn()
        {
            if (_isLocked || _isToggled)
                return;

            _isToggled = true;
            _animator.SetTrigger(_cachedToggleOnTriggerParam);
            OnSlotToggledOn?.Invoke(_iconSlotIndex);
        }
    }
}