Public Class textline

    Public Function linec(ByVal s As String, ByVal calet As Integer) As Integer
        Dim k As Integer = 1
        Dim t As Integer = -1

        If s <> "" Then
            While True
                t = s.IndexOf(vbLf, t + 1)
                If t = -1 Then
                    Exit While
                ElseIf t >= calet Then
                    Exit While
                End If

                k += 1

            End While
        End If

        Return k
    End Function


End Class
