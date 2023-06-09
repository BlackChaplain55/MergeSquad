//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/Resources/Input/GenericInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GenericInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GenericInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GenericInput"",
    ""maps"": [
        {
            ""name"": ""GenericMap"",
            ""id"": ""f87fa3ad-ceca-4dab-bd3b-57c598c3096f"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""51e46ed7-33be-499b-8594-fa77d8138094"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""10501ee6-c7a7-495c-9424-6a38e0311889"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b095fa07-c79e-442c-a496-0dbf041af1d7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""132c8a24-a16b-4331-935f-ed661d4fbf53"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c10b0428-5d7e-49a9-ac98-e7db151e9651"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GenericMap
        m_GenericMap = asset.FindActionMap("GenericMap", throwIfNotFound: true);
        m_GenericMap_LeftClick = m_GenericMap.FindAction("LeftClick", throwIfNotFound: true);
        m_GenericMap_Escape = m_GenericMap.FindAction("Escape", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GenericMap
    private readonly InputActionMap m_GenericMap;
    private List<IGenericMapActions> m_GenericMapActionsCallbackInterfaces = new List<IGenericMapActions>();
    private readonly InputAction m_GenericMap_LeftClick;
    private readonly InputAction m_GenericMap_Escape;
    public struct GenericMapActions
    {
        private @GenericInput m_Wrapper;
        public GenericMapActions(@GenericInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_GenericMap_LeftClick;
        public InputAction @Escape => m_Wrapper.m_GenericMap_Escape;
        public InputActionMap Get() { return m_Wrapper.m_GenericMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GenericMapActions set) { return set.Get(); }
        public void AddCallbacks(IGenericMapActions instance)
        {
            if (instance == null || m_Wrapper.m_GenericMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GenericMapActionsCallbackInterfaces.Add(instance);
            @LeftClick.started += instance.OnLeftClick;
            @LeftClick.performed += instance.OnLeftClick;
            @LeftClick.canceled += instance.OnLeftClick;
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
        }

        private void UnregisterCallbacks(IGenericMapActions instance)
        {
            @LeftClick.started -= instance.OnLeftClick;
            @LeftClick.performed -= instance.OnLeftClick;
            @LeftClick.canceled -= instance.OnLeftClick;
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
        }

        public void RemoveCallbacks(IGenericMapActions instance)
        {
            if (m_Wrapper.m_GenericMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGenericMapActions instance)
        {
            foreach (var item in m_Wrapper.m_GenericMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GenericMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GenericMapActions @GenericMap => new GenericMapActions(this);
    public interface IGenericMapActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
    }
}
