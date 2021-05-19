using System.Collections;
using System.Collections.Generic;
using Helper.Updater;
using UnityEngine;
using System.Linq;

namespace Helper.Utility
{
    public class TouchWrapper
    {
        public static TouchWrapper Instance => s_singleton.Instance;
        private static Singleton<TouchWrapper> s_singleton = new Singleton<TouchWrapper>();

        #region Main
        /// <summary>
        /// Returns true if this is the first touch, or if MouseButton(0) was pressed this frame
        /// </summary>
        public bool MainInputPressed => TouchPressed || Mouse0Pressed;
        /// <summary>
        /// Returns true if touchCount > 0 or if MouseButton(0) is held down
        /// </summary>
        public bool MainInputDown => Input.touchCount > 0 || Input.GetMouseButton(0);

        public bool MainInputPresent { get; private set; } = false;

        public event System.Action OnMainInputPressed;
        public event System.Action OnMainInputUp;
        #endregion

        #region Touch
        public Touch MainTouch { get; private set; }

        public bool TouchPressed { get; private set; } = false;

        public int TouchCount => Input.touchCount;
        public int PrevTouchCount { get; private set; } = 0;

        public event System.Action<Touch> OnFirstGroupTouch;
        public event System.Action<Touch> OnFirstFrameTouch;

        public event System.Action<Touch> OnTouchBegan;
        public event System.Action<Touch> OnTouchEnded;
        #endregion

        public bool Mouse0Pressed { get; private set; } = false;
        public bool Mouse1Pressed { get; private set; } = false;

        public bool IsMobile => Application.isMobilePlatform;

        public Vector2 PreviousMainPosition { get; private set; } = Vector2.zero;
        public Vector2 CurrentMainPosition { get; private set; } = Vector2.zero;
        public Vector2 MovementMainDirection { get; private set; } = Vector2.zero;

        public TouchWrapper()
        {
            FunctionUpdater.CreateUpdater(() =>
            {
                Update();
            }, "TouchInputWrapper_U", true, UpdateCycle.NORMAL, int.MinValue);
            FunctionUpdater.CreateUpdater(() =>
            {
                LateUpdate();
            }, "TouchInputWrapper_LU", true, UpdateCycle.LATE, int.MaxValue);

            if (!IsMobile) return;

            OnTouchBegan += (Touch touch) => 
            {
                if (TouchCount == 0)
                {
                    SetMainTouch(touch);
                    OnFirstGroupTouch?.Invoke(touch);
                }
                if (IsFirstTouch()) OnFirstFrameTouch?.Invoke(touch);

                TouchPressed = true;
            };

            OnTouchEnded += (Touch touch) => { if (MainTouch.fingerId == touch.fingerId) ResetMainTouch(); };
        }

        public bool GetMainInputPosition(out Vector2 pos)
        {
            if (IsMobile) return GetTouchPosition(0, out pos);

            pos = Input.mousePosition;

            return true;
        }
        public List<Vector2> GetAllInputPositions()
        {
            List<Vector2> _positions = new List<Vector2>(GetAllTouchPositions());

            if (IsMobile == false) _positions.Add(Input.mousePosition);

            return _positions;
        }

        #region Touch_Specific
        public bool IsFirstTouch() => PrevTouchCount == 0 && Input.touchCount > 0;

        public void SetMainTouch(Touch touch) => MainTouch = touch;
        public void ResetMainTouch()
        {
            if (GetTouch(0, out Touch _mainTouch))
            {
                SetMainTouch(_mainTouch);
            }
        }

        public bool GetTouchByID(int fingerID, out Touch touch)
        {
            bool _valid = false;
            touch = Input.touches.FirstOrDefault((testTouch) =>
            {
                _valid = testTouch.fingerId == fingerID;
                return _valid;
            });

            return _valid;
        }

        /// <summary>
        /// Returns whether there is a valid amount of touches on the screen
        /// </summary>
        /// <param name="touchID">The amount of touches on the screen</param>
        /// <returns></returns>
        public bool ValidTouchPresent(int touchID = 0) => Util.Math.Within(touchID, 1, PrevTouchCount - 1);

        /// <summary>
        /// Get the position of a touch on the screen
        /// </summary>
        /// <param name="touchID">The touch to check</param>
        /// <param name="pos">Position of the touch</param>
        /// <returns></returns>
        public bool GetTouchPosition(int touchID, out Vector2 pos)
        {
            bool _validTouch = ValidTouchPresent(touchID);

            pos = _validTouch ? Input.GetTouch(touchID).position : Vector2.zero;

            return _validTouch;
        }

        public List<Vector2> GetAllTouchPositions()
        {
            if (IsMobile == false) return new List<Vector2>();

            List<Vector2> _positions = new List<Vector2>(PrevTouchCount);

            for (int _touchIndex = 0; _touchIndex < PrevTouchCount; _touchIndex++)
            {
                _positions.Add(Input.touches[_touchIndex].position);
            }

            return _positions;
        }

        /// <summary>
        /// Tries to return a valid Touch if there is one
        /// </summary>
        /// <param name="touchID">The touch on the screen to get</param>
        /// <param name="touch">The touch that was retrieved</param>
        /// <returns></returns>
        public bool GetTouch(int touchID, out Touch touch)
        {
            bool _validTouch = ValidTouchPresent(touchID);

            touch = _validTouch ? Input.GetTouch(touchID) : new Touch();

            return _validTouch;
        }
        #endregion

        #region Update
        private void Update()
        {
            bool _prevMainInputDown = MainInputDown;

            if (IsMobile) Mobile_Update();
            else PC_Update();

            if (MainInputPressed) OnMainInputPressed?.Invoke();
            if (_prevMainInputDown && MainInputDown == false) OnMainInputUp?.Invoke();

            UpdateMovementDirection();
        }

        private void UpdateMovementDirection()
        {
            PreviousMainPosition = CurrentMainPosition;
            bool _validPos = GetMainInputPosition(out Vector2 _currentPos);
            CurrentMainPosition = _validPos ? _currentPos : CurrentMainPosition;

            MovementMainDirection = Util.Math.Direction(PreviousMainPosition, CurrentMainPosition);
        }

        private void PC_Update()
        {
            Mouse0Pressed = Input.GetMouseButtonDown(0);
            Mouse1Pressed = Input.GetMouseButtonDown(1);

            MainInputPresent = Input.mousePresent;
        }
        private void Mobile_Update()
        {
            PrevTouchCount = Input.touchCount;

            MainInputPresent = PrevTouchCount > 0;

            for (int _touchIndex = 0; _touchIndex < PrevTouchCount; _touchIndex++)
            {
                Touch _touch = Input.touches[_touchIndex];

                if (_touch.phase == TouchPhase.Began) OnTouchBegan?.Invoke(_touch);
                if (_touch.phase == TouchPhase.Ended) OnTouchEnded?.Invoke(_touch);
            }
        }

        private void LateUpdate()
        {
            TouchPressed = false;
        }
        #endregion
    }
}