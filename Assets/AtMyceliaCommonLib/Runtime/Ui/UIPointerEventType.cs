using System;

namespace AtMycelia.Ui
{
    [Flags]
    public enum UIPointerEventType
    {
        Null,
        Click,
        Up,
        Down,
        Enter,
        Exit,
        BeginDrag,
        Drag,
        EndDrag,
        Drop
    }
}