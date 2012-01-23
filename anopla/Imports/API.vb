Imports System.Runtime.InteropServices

Public Class API
	Public Declare Function FindWindow Lib "user32" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
	Public Declare Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
	Public Declare Function SetWindowsHookExA Lib "user32" (ByVal idHook As Int32, ByVal lpfn As KeyMouseHookDelegate, ByVal hmod As Int32, ByVal dwThreadId As Int32) As Int32
	Public Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Int32) As Int32
	Public Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Int32, ByVal nCode As Int32, ByVal wParam As Int32, ByVal lParam As KBDLLHOOKSTRUCT) As Int32
	Public Declare Function keybd_event Lib "user32" (ByVal vKey As Int32, ByVal bScan As Byte, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32) As Boolean
	Public Declare Function GetMessageExtraInfo Lib "user32" () As Int32
	Public Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As POINTAPI) As Boolean
	Public Declare Function WindowFromPoint Lib "user32" (ByVal xyPoint As POINTAPI) As Int32
	Public Delegate Function KeyHookDelegate(ByVal Code As Integer, ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) As Int32
	Public Delegate Function MouseHookDelegate(ByVal Code As Integer, ByVal wParam As Integer, ByRef lParam As MSLLHOOKSTRUCT) As Int32

	Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Integer

	Public Const HC_ACTION As Int32 = 0
	Public Const HC_GETNEXT As Int32 = 1
	Public Const WM_LBUTTONDOWN As Int32 = 513
	Public Const WM_LBUTTONUP As Int32 = 514
	Public Const WM_RBUTTONDOWN As Int32 = 516
	Public Const WM_RBUTTONUP As Int32 = 517
	Public Const WM_MBUTTONDOWN As Int32 = 519
	Public Const WM_MBUTTONUP As Int32 = 520
	Public Const WM_MOUSEWHEEL As Int32 = 522
	Public Const WM_MOUSEMOVE As Int32 = 512
	Public Const WM_MOUSEACTIVATE As Int32 = &H21
	Public Const VK_CONTROL As Int32 = 17
	Public Const VK_MENU As Int32 = 18
	Public Const VK_ESCAPE As Int32 = 27
	Public Const VK_TAB As Int32 = &H9
	Public Const VK_DELETE As Int32 = &H2E
	Public Const KEYEVENTF_KEYUP As Int32 = 2

	' Low-Level Keyboard Constants
	Public Const LLKHF_EXTENDED As Integer = &H1
	Public Const LLKHF_INJECTED As Integer = &H10
	Public Const LLKHF_ALTDOWN As Integer = &H20
	Public Const LLKHF_UP As Integer = &H80

	Public Const WH_MOUSE_LL As Int32 = 14
	Public Const WH_KEYBOARD_LL As Integer = 13&

	Public Structure POINTAPI
		Public X, Y As Int32
	End Structure
	Public Structure MOUSEHOOKSTRUCT
		Public pt As POINTAPI
		Public hwnd, wHitTestCode, dwExtraInfo As Int32
	End Structure
	Public Structure MSLLHOOKSTRUCT
		Public pt As POINTAPI
		Public mouseData, flags, time, dwExtraInfo As Int32
	End Structure
	Public Structure KBDLLHOOKSTRUCT
		Public vkCode As Integer
		Public scanCode As Integer
		Public flags As Integer
		Public time As Integer
		Public dwExtraInfo As Integer
	End Structure

	Public Const WS_EX_NOACTIVATE As Int32 = &H8000000
	Public Const MA_NOACTIVATE As Int32 = &H3
End Class
