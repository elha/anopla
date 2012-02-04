Imports System.Collections.Generic
Imports System.Text


''' <summary>
''' Class that implements Boyer-Moore and related exact string-matching algorithms
''' </summary>
''' <remarks>
''' From "Handbook of exact string-matching algorithms"
'''   by Christian Charras and Thierry Lecroq
'''   chapter 15
''' http://www-igm.univ-mlv.fr/~lecroq/string/node15.html#SECTION00150
''' </remarks>
Public Class BoyerMoore
	Private m_badCharacterShift As Integer()
	Private m_goodSuffixShift As Integer()
	Private m_suffixes As Integer()
	Private m_pattern As Byte()

	''' <summary>
	''' Constructor
	''' </summary>
	''' <param name="pattern">Pattern for search</param>
	Public Sub New(pattern As Byte())
		' Preprocessing 

		m_pattern = pattern
		m_badCharacterShift = BuildBadCharacterShift(pattern)
		m_suffixes = FindSuffixes(pattern)
		m_goodSuffixShift = BuildGoodSuffixShift(pattern, m_suffixes)
	End Sub

	''' <summary>
	''' Build the bad character shift array.
	''' </summary>
	''' <param name="pattern">Pattern for search</param>
	''' <returns>bad character shift array</returns>
	Private Function BuildBadCharacterShift(pattern As Byte()) As Integer()
		Dim badCharacterShift As Integer() = New Integer(255) {}

		For c As Integer = 0 To badCharacterShift.Length - 1
			badCharacterShift(c) = pattern.Length
		Next
		For i As Integer = 0 To pattern.Length - 2
			badCharacterShift(pattern(i)) = pattern.Length - i - 1
		Next

		Return badCharacterShift
	End Function

	''' <summary>
	''' Find suffixes in the pattern
	''' </summary>
	''' <param name="pattern">Pattern for search</param>
	''' <returns>Suffix array</returns>
	Private Function FindSuffixes(pattern As Byte()) As Integer()
		Dim f As Integer = 0, g As Integer

		Dim patternLength As Integer = pattern.Length
		Dim suffixes As Integer() = New Integer(pattern.Length) {}

		suffixes(patternLength - 1) = patternLength
		g = patternLength - 1
		For i As Integer = patternLength - 2 To 0 Step -1
			If i > g AndAlso suffixes(i + patternLength - 1 - f) < i - g Then
				suffixes(i) = suffixes(i + patternLength - 1 - f)
			Else
				If i < g Then
					g = i
				End If
				f = i
				While g >= 0 AndAlso (pattern(g) = pattern(g + patternLength - 1 - f))
					g -= 1
				End While
				suffixes(i) = f - g
			End If
		Next

		Return suffixes
	End Function

	''' <summary>
	''' Build the good suffix array.
	''' </summary>
	''' <param name="pattern">Pattern for search</param>
	''' <returns>Good suffix shift array</returns>
	Private Function BuildGoodSuffixShift(pattern As Byte(), suff As Integer()) As Integer()
		Dim patternLength As Integer = pattern.Length
		Dim goodSuffixShift As Integer() = New Integer(pattern.Length) {}

		For i As Integer = 0 To patternLength - 1
			goodSuffixShift(i) = patternLength
		Next
		Dim j As Integer = 0
		For i As Integer = patternLength - 1 To -1 Step -1
			If i = -1 OrElse suff(i) = i + 1 Then
				While j < patternLength - 1 - i
					If goodSuffixShift(j) = patternLength Then
						goodSuffixShift(j) = patternLength - 1 - i
					End If
					j += 1
				End While
			End If
		Next
		For i As Integer = 0 To patternLength - 2
			goodSuffixShift(patternLength - 1 - suff(i)) = patternLength - 1 - i
		Next

		Return goodSuffixShift
	End Function

	''' <summary>
	''' Return all matches of the pattern in specified text using the Horspool algorithm
	''' </summary>
	''' <param name="text">text to be searched</param>
	''' <param name="startingIndex">Index at which search begins</param>
	''' <returns>IEnumerable which returns the indexes of pattern matches</returns>
	Public Function HorspoolMatch(text As Byte(), startingIndex As Integer) As IEnumerable(Of Integer)
		Dim patternLength As Integer = m_pattern.Length
		Dim textLength As Integer = text.Length
		Dim out As New List(Of Integer)

		' Searching 
		Dim index As Integer = startingIndex
		While index <= textLength - patternLength
			Dim unmatched As Integer
			unmatched = patternLength - 1
			While unmatched >= 0 AndAlso m_pattern(unmatched) = text(unmatched + index)
				unmatched -= 1
			End While

			If unmatched < 0 Then
				out.Add(index)
				index += m_goodSuffixShift(0)
			Else
				index += Math.Max(m_goodSuffixShift(unmatched), m_badCharacterShift(text(unmatched + index)) - patternLength + 1 + unmatched)
			End If

		End While
		Return out
	End Function

	''' <summary>
	''' Return all matches of the pattern in specified text using the Horspool algorithm
	''' </summary>
	''' <param name="text">text to be searched</param>
	''' <returns>IEnumerable which returns the indexes of pattern matches</returns>
	Public Function HorspoolMatch(text As Byte()) As IEnumerable(Of Integer)
		Return HorspoolMatch(text, 0)
	End Function

End Class
