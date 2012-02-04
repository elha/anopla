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
        'suche nach der mittleren Zeile des templates
        Dim matchings As New List(Of TemplateMatch)

        Dim PixelSize = 3
        Dim nRow = templateData.Height \ 2
        Dim pattern(templateData.Width * PixelSize - 1) As Byte
        Marshal.Copy(templateData.Scan0 + (nRow * templateData.Stride), pattern, 0, pattern.Length)
        
        Dim h As New BoyerMoore(pattern)

        Dim search(imageData.Width * PixelSize - 1) As Byte
        For y = searchZone.Top + nRow To searchZone.Bottom - nRow
            Marshal.Copy(imageData.Scan0 + (y * imageData.Stride), search, 0, search.Length)
            For Each m In h.HorspoolMatch(search)
                matchings.Add(New TemplateMatch(New Rectangle(m \ PixelSize, y - nRow, templateData.Width, templateData.Height), 1))
            Next
        Next

        Return matchings.ToArray
    End Function
End Class

