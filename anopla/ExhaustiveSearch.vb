' AForge Image Processing Library
' AForge.NET framework
' http://www.aforgenet.com/framework/
'
' Copyright © Andrew Kirillov, 2005-2009
' andrew.kirillov@aforgenet.com
'
Imports AForge.Imaging
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Class BoyerMooreTemplateMatching
    Implements ITemplateMatching

    Public Sub New()
    End Sub


    ''' <summary>
    ''' Process image looking for matchings with specified template.
    ''' </summary>
    ''' 
    ''' <param name="image">Source image to process.</param>
    ''' <param name="template">Template image to search for.</param>
    ''' <param name="searchZone">Rectangle in source image to search template for.</param>
    ''' 
    ''' <returns>Returns array of found template matches. The array is sorted by similarity
    ''' of found matches in descending order.</returns>
    ''' 
    ''' <exception cref="UnsupportedImageFormatException">The source image has incorrect pixel format.</exception>
    ''' <exception cref="InvalidImagePropertiesException">Template image is bigger than source image.</exception>
    ''' 
    Public Function ProcessImage(ByVal image As System.Drawing.Bitmap, ByVal template As System.Drawing.Bitmap, ByVal searchZone As System.Drawing.Rectangle) As AForge.Imaging.TemplateMatch() Implements AForge.Imaging.ITemplateMatching.ProcessImage

        ' check image format
        If ((image.PixelFormat <> PixelFormat.Format8bppIndexed) AndAlso (image.PixelFormat <> PixelFormat.Format24bppRgb)) OrElse (image.PixelFormat <> template.PixelFormat) Then
            Throw New UnsupportedImageFormatException("Unsupported pixel format of the source or template image.")
        End If

        ' check template's size
        If (template.Width > image.Width) OrElse (template.Height > image.Height) Then
            Throw New InvalidImagePropertiesException("Template's size should be smaller or equal to source image's size.")
        End If

        ' lock source and template images
        Dim imageData As BitmapData = image.LockBits(New Rectangle(0, 0, image.Width, image.Height), ImageLockMode.[ReadOnly], image.PixelFormat)
        Dim templateData As BitmapData = template.LockBits(New Rectangle(0, 0, template.Width, template.Height), ImageLockMode.[ReadOnly], template.PixelFormat)

        Dim matchings As TemplateMatch()

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

    Public Function ProcessImage(ByVal image As AForge.Imaging.UnmanagedImage, ByVal template As AForge.Imaging.UnmanagedImage, ByVal searchZone As System.Drawing.Rectangle) As AForge.Imaging.TemplateMatch() Implements AForge.Imaging.ITemplateMatching.ProcessImage
        Return Nothing
    End Function

    Public Function ProcessImage(ByVal imageData As System.Drawing.Imaging.BitmapData, ByVal templateData As System.Drawing.Imaging.BitmapData, ByVal searchZone As System.Drawing.Rectangle) As AForge.Imaging.TemplateMatch() Implements AForge.Imaging.ITemplateMatching.ProcessImage
		Dim matchings As New List(Of TemplateMatch)

		'find middle line from template
		Dim nRow = templateData.Height \ 2

		Dim matcher = New BoyerMoore(GetRowFromImage(templateData, nRow))
		Dim search() As Byte = Nothing 'reuse buffer

		For y = searchZone.Top + nRow To searchZone.Bottom - nRow
			Dim m = matcher.HorspoolMatch(GetRowFromImage(imageData, y, search))
			If m = -1 Then Continue For

			'check a few additional lines
			Dim bGood As Boolean = True
			Dim nChecks As Integer() = {-nRow \ 2, -1, 1, nRow \ 2}
			For Each nCheck In nChecks
				Dim hCheck = New BoyerMoore(GetRowFromImage(templateData, nRow + nCheck))
				If hCheck.HorspoolMatch(GetRowFromImage(imageData, y + nCheck)) <> m Then
					bGood = False : Exit For
				End If
			Next

			If bGood Then matchings.Add(New TemplateMatch(New Rectangle(m \ GetPixelSize(imageData), y - nRow, templateData.Width, templateData.Height), 1))
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
	End Function
End Class

