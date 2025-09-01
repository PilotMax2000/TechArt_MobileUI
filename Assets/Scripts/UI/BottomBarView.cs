using UnityEngine;
using UnityEngine.Events;

namespace TechArtProject
{
    public class BottomBarView : MonoBehaviour
    {
        public UnityEvent Closed;
        public UnityEvent<int> ContentActivated;

        private int _lastToggledIndex = -1;
        private bool _togglingInProcess = false;
        
        [SerializeField] private BottomBarIconView[] _barSlots;
        
        private void Awake()
        {
            foreach (var icon in _barSlots)
            {
                icon.OnSlotToggledOn += OnIconToggledOn;
                icon.OnSlotToggledOff += OnIconToggledOff;
            }
        }

        private void OnIconToggledOff(int iconSlotIndex)
        {
            if (_togglingInProcess)
                return;

            if (_lastToggledIndex == iconSlotIndex)
            {
                Debug.Log($"BottomBarView: Icon index {_lastToggledIndex} toggled off, no slot is selected, shooting Closed event");
                _lastToggledIndex = -1;
                Closed?.Invoke();
            }
        }

        private void OnIconToggledOn(int iconSlotIndex)
        {
            if (_lastToggledIndex == -1)
            {
                _lastToggledIndex = iconSlotIndex;
            }
            else
            {
                if (IsIconSlotIndexNotInRange(iconSlotIndex)) 
                    return;
                _togglingInProcess = true;
                _barSlots[_lastToggledIndex].ToggleOff();
                _lastToggledIndex = iconSlotIndex;
                _togglingInProcess = false;
            }
            
            ContentActivated?.Invoke(_lastToggledIndex);
            Debug.Log($"BottomBarView: Icon index {_lastToggledIndex} toggled on");
        }

        private bool IsIconSlotIndexNotInRange(int iconSlotIndex)
        {
            if (_barSlots.Length < iconSlotIndex + 1)
            {
                Debug.LogError($"BottomBarView: Icon index {iconSlotIndex} is out of range");
                return true;
            }

            return false;
        }
    }
}