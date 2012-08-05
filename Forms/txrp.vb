Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.VisualBasic

Public Class txrp

    Dim ss As String()
    Dim path As String = Application.StartupPath & "\APP\seekrp.txt"

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Me.Text = "検索" Then
            TextBox2.ReadOnly = True
        Else
            RPTEST.Enabled = True
        End If
        TextBox1.Text = My.Settings.seekstr
        TextBox2.Text = My.Settings.rpstr
        If (My.Settings.seek_rp And 1) <> 0 Then
            CheckBox1.Checked = True
        End If
        If (My.Settings.seek_rp And 2) <> 0 Then
            CheckBox2.Checked = True
        End If
        If (My.Settings.seek_rp And 4) <> 0 Then
            CheckBox3.Checked = True
        End If
        lsread()
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(ss)

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles RUN.Click, RPTEST.Click
        Dim seekst As String = TextBox1.Text
        Dim rpst As String = TextBox2.Text
        Dim p As parser = parser
        p = CType(Me.Owner, parser)
        Dim pst As String = p.TX.Text
        Dim pos As Integer = p.TX.SelectionStart
        Dim lens As Integer = p.TX.SelectionLength
        Dim s1 As String = p.TX.Text.Substring(0, pos)
        Dim s2 As String = p.TX.Text.Remove(0, pos + lens)

        If CheckBox3.Checked Then
            pst = p.TX.Text.Substring(pos, lens)
        End If
        Dim y As RegexOptions = RegexOptions.None
        If CheckBox1.Checked Then
            y = RegexOptions.IgnoreCase
        End If
        Dim seekstrg As Regex = New Regex(TextBox1.Text, y)
        Dim sm As Match = seekstrg.Match(pst)

        If CheckBox2.Checked = False Then
            If Me.Text = "検索" Then
                Dim x As StringComparison = StringComparison.Ordinal
                If CheckBox1.Checked Then
                    x = StringComparison.OrdinalIgnoreCase
                End If
                Dim k As Integer = pst.IndexOf(seekst, x)
                If k > 0 Then
                    If CheckBox3.Checked Then
                        k += pos
                    End If
                    p.TX.SelectionStart = k
                    p.TX.SelectionLength = 0
                    p.TX.ScrollToCaret()
                End If
            Else
                If sender Is RUN Then
                    If CheckBox3.Checked Then
                        p.TX.Text = s1 & pst.Replace(seekst, rpst) & s2
                    Else
                        p.TX.Text = pst.Replace(seekst, rpst)
                    End If
                Else
                    If CheckBox3.Checked Then
                        TESTTX.Text = s1 & pst.Replace(seekst, rpst) & s2
                    Else
                        TESTTX.Text = pst.Replace(seekst, rpst)
                    End If
                End If
            End If
        Else
            If Me.Text = "検索" Then
                Dim k As Integer = sm.Index
                If k > 0 Then
                    If CheckBox3.Checked Then
                        k += pos
                    End If
                    p.TX.SelectionStart = k
                    p.TX.SelectionLength = 0
                    p.TX.ScrollToCaret()
                End If
            Else
                rpst = rpst.Replace("\n", vbLf)
                rpst = rpst.Replace("\r", vbCr)
                rpst = rpst.Replace("\t", vbTab)
                While sm.Success
                    pst = Regex.Replace(pst, seekst, rpst)
                    sm = sm.NextMatch
                End While

                If CheckBox3.Checked Then
                    pst = s1 & pst & s2
                End If
                If sender Is RUN Then
                    p.TX.Text = pst
                    p.TX.SelectionStart = pos
                    p.TX.SelectionLength = 0
                    p.TX.ScrollToCaret()
                Else
                    TESTTX.Focus()
                    TESTTX.Text = pst
                    TESTTX.SelectionStart = pos
                    TESTTX.SelectionLength = 0
                    TESTTX.ScrollToCaret()
                End If
            End If
        End If
        If sender Is RUN Then
            Dim k As Integer = 0
            If CheckBox1.Checked Then
                k += 1
            End If
            If CheckBox2.Checked Then
                k += 2
            End If
            If CheckBox3.Checked Then
                k += 4
            End If
            My.Settings.seek_rp = k
            p.TX.Focus()
            My.Settings.seekstr = TextBox1.Text
            My.Settings.rpstr = TextBox2.Text
            Me.Close()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sel As Integer = ComboBox1.SelectedIndex
        Dim st As String() = Regex.Split(ss(sel).Trim, (""","""))
        Array.Resize(st, 3)
        TextBox1.Text = st(1)
        TextBox2.Text = st(2).Remove(st(2).Length - 1, 1)
        ComboBox1.SelectionStart = 0
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles TBLEDIT.Click
        If (File.Exists(path)) Then
            Process.Start(path)
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles TBLLOAD.Click
        If (File.Exists(path)) Then
            Form1_Load(sender, e)
        End If
    End Sub

    Private Function lsread() As Boolean
        If (File.Exists(Path)) Then
            Dim sr As StreamReader = New StreamReader(path, Encoding.GetEncoding(65001))
            Dim s As String = sr.ReadToEnd
            sr.Close()
            ss = s.Split(CChar(vbLf))
        End If
        Return True
    End Function
End Class