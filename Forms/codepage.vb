Imports System.IO
Imports System.Text     'Encoding用
Imports System.Text.RegularExpressions

Public Class codepage

    Private Sub ini(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = My.Settings.usercpsel
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim cp As New Regex("^\d+", RegexOptions.ECMAScript)
        Dim cpm As Match = cp.Match(ComboBox1.Text)
        If cpm.Success Then
            My.Settings.MSCODEPAGE = CInt(cpm.Value)
            My.Settings.usercp = CInt(cpm.Value)
            My.Settings.usercpsel = ComboBox1.SelectedIndex
            My.Settings.cpstr = ComboBox1.Text
        End If
        Me.Close()

    End Sub
End Class