﻿Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports anopla.API

Public Module Startup
	Public Sub Main()
		HookKeyboard()

		Dim o As New Main
		o.ShowDialog()

		UnhookKeyboard()
	End Sub

	Public KeyboardHandle As Integer = 0
	<MarshalAs(UnmanagedType.FunctionPtr)> Private KeyboardDelegate As KeyMouseHookDelegate

	Public MouseHandle As Integer = 0
	<MarshalAs(UnmanagedType.FunctionPtr)> Private MouseDelegate As KeyMouseHookDelegate

	Public Function KeyboardCallback(ByVal Code As Integer, ByVal wParam As Integer, ByRef Hookstruct As KBDLLHOOKSTRUCT) As Integer
		If (Code = HC_ACTION) Then
			If (Hookstruct.vkCode = VK_ESCAPE) And CBool(GetAsyncKeyState(VK_CONTROL) And &H8000) Then
				Return 1
			End If

			If (Hookstruct.vkCode = VK_TAB) And CBool(Hookstruct.flags And LLKHF_ALTDOWN) Then
				Return 1
			End If

			If (Hookstruct.vkCode = VK_ESCAPE) And CBool(Hookstruct.flags And LLKHF_ALTDOWN) Then
				Return 1
			End If
		End If

		Return CallNextHookEx(KeyboardHandle, Code, wParam, Hookstruct)
	End Function

	Public Function MouseCallback(ByVal Code As Integer, ByVal wParam As Integer, ByRef Hookstruct As MOUSEHOOKSTRUCT) As Integer
		If (Code = HC_ACTION) Then
			
		End If

		Return CallNextHookEx(KeyboardHandle, Code, wParam, Hookstruct)
	End Function

	Public Sub HookKeyboard()
		KeyboardDelegate = New KeyMouseHookDelegate(AddressOf KeyboardCallback)
		KeyboardHandle = SetWindowsHookExA(WH_KEYBOARD_LL, KeyboardDelegate, Marshal.GetHINSTANCE([Assembly].GetExecutingAssembly.GetModules()(0)).ToInt32, 0)

		MouseDelegate = New KeyMouseHookDelegate(AddressOf MouseCallback)
		MouseHandle = SetWindowsHookExA(WH_KEYBOARD_LL, MouseHandle, Marshal.GetHINSTANCE([Assembly].GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
	End Sub

	Public Sub UnhookKeyboard()
		If KeyboardHandle = 0 Then Return
		Call UnhookWindowsHookEx(KeyboardHandle)
	End Sub
End Module