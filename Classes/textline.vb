Public Class textline

    Public Function linec(ByVal s As String, ByVal calet As Integer) As Integer
        Dim k As Integer = 1
        Dim t As Integer = 0
        Dim st As String() = s.Split(CChar(vbLf))
        For Each ss In st
            t += ss.Length + 1
            If t > calet Then
                Exit For
            End If
            k += 1
        Next
        Return k
    End Function

End Class
