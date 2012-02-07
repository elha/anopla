Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Class BoyerMooreTemplateMatching
	Public Sub New()
	End Sub

    Public Function ProcessImage(ByVal image As System.Drawing.Bitmap, ByVal template As System.Drawing.Bitmap, ByVal searchZone As System.Drawing.Rectangle) As Rectangle()

        ' check image format
        If ((image.PixelFormat <> PixelFormat.Format8bppIndexed) AndAlso (image.PixelFormat <> PixelFormat.Format24bppRgb)) OrElse (image.PixelFormat <> template.PixelFormat) Then
            Throw New Exception("Unsupported pixel format of the source or template image.")
        End If

        ' check template's size
        If (template.Width > image.Width) OrElse (template.Height > image.Height) Then
            Throw New Exception("Template's size should be smaller or equal to source image's size.")
        End If

        ' lock source and template images
        Dim imageData As BitmapData = image.LockBits(New Rectangle(0, 0, image.Width, image.Height), ImageLockMode.[ReadOnly], image.PixelFormat)
        Dim templateData As BitmapData = template.LockBits(New Rectangle(0, 0, template.Width, template.Height), ImageLockMode.[ReadOnly], template.PixelFormat)

        Dim matchings As Rectangle()

        Try
            ' process the image
            matchings = ProcessImage(imageData, templateData, searchZone)
        Finally
            ' unlock images
            image.UnlockBits(imageData)
            template.UnlockBits(templateData)
        End Try

        Return matchings
    End Function

    Public Function ProcessImage(ByVal imageData As System.Drawing.Imaging.BitmapData, ByVal templateData As System.Drawing.Imaging.BitmapData, ByVal searchZone As System.Drawing.Rectangle) As Rectangle()
        Dim matchings As New List(Of Rectangle)

        'find middle line from template
        Dim nRow = templateData.Height \ 2

        Dim matcher = New BoyerMoore(GetRowFromImage(templateData, nRow))
        Dim search() As Byte = Nothing 'reuse buffer

        For y = searchZone.Top + nRow To searchZone.Bottom - nRow
            Dim matchpos = BoyerMoore.NOTFOUND
            Do
                matchpos = matcher.HorspoolMatch(GetRowFromImage(imageData, y, search), matchpos + 1)
                If Not matchpos = BoyerMoore.NOTFOUND Then
                    'check a few additional lines
                    Dim bGood As Boolean = True
                    Dim nChecks As Integer() = {-nRow \ 2, -1, 1, nRow \ 2}
                    For Each nCheck In nChecks
                        Dim hCheck = New BoyerMoore(GetRowFromImage(templateData, nRow + nCheck))
                        If hCheck.HorspoolMatch(GetRowFromImage(imageData, y + nCheck)) <> matchpos Then
                            bGood = False : Exit For
                        End If
                    Next

                    If bGood Then
                        matchings.Add(New Rectangle(matchpos \ GetPixelSize(imageData), y - nRow, templateData.Width, templateData.Height))
                    End If
                End If
            Loop Until matchpos = BoyerMoore.NOTFOUND
        Next y

        Return matchings.ToArray
    End Function

	Private Function GetRowFromImage(img As BitmapData, nRow As Integer, Optional ByRef out As Byte() = Nothing) As Byte()
		Dim PixelSize As Integer = GetPixelSize(img)
		If out Is Nothing Then Array.Resize(out, img.Width * PixelSize)
		Marshal.Copy(img.Scan0 + nRow * img.Stride, out, 0, out.Length)
		Return out
	End Function

	Private Function GetPixelSize(img As BitmapData) As Integer
		Select Case img.PixelFormat
			Case PixelFormat.Format24bppRgb
				Return 3
			Case PixelFormat.Format8bppIndexed
				Return 1
		End Select
		Throw New Exception("PixelSize unknown")
	End Function
End Class

