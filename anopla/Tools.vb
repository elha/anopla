Module Tools
    Public Function BitSet(ByVal value As Integer, ByVal bit As Integer) As Boolean
        Return CBool(value And bit)
    End Function

    Public Function IIf(Of T)(ByVal cond As Boolean, ByVal tv As T, ByVal fv As T) As T
        If cond Then Return tv
        Return fv
    End Function
End Module
