Option Strict Off
Option Explicit On
Option Compare Text
Imports System.Collections

Module Tracer
	Public Interface ITraceListener
		Sub AddTrace(ByVal time As String, ByVal strFunction As String, ByVal msg As String)
	End Interface

	Public TraceToFile As Boolean = False
	Public Tracer As ITraceListener

	Public Sub Trace(ByVal strFunction As String, Optional ByVal msg As String = "")
		If Tracer IsNot Nothing Then Tracer.AddTrace(DateTime.Now.ToString("HH:mm:ssfff"), strFunction, msg)
		If TraceToFile Then LogErrorToFile(strFunction & vbTab & msg)
	End Sub

	Private Sub LogErrorToFile(strMessage As String)
		Try
			Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Anopla.log"
			Using logFile As IO.StreamWriter = IO.File.AppendText(path)
				logFile.Write(DateTime.Now.ToString("u") & ": " & strMessage & vbCrLf)
			End Using
		Catch ex As Exception
			'ignore
		End Try
	End Sub
End Module
