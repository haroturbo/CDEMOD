Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports System.Net.Sockets
Imports System.Net

Public Class ftp


    Private Sub form4load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Settings.ftpdaemon = True Then
            daemonfinder.Checked = True
        Else
            daemonfinder.Checked = False
        End If
        If My.Settings.customwait = True Then
            WAIT.Checked = True
        Else
            WAIT.Checked = False
        End If

        If My.Settings.fixedform = True Then
            Me.AutoSize = True
        End If


        Dim sta As String() = My.Settings.dhcpstart.Split("."c)
        Dim en As String() = My.Settings.dhcpend.Split("."c)
        Dim k As Integer = 0
        For Each s In sta
            sta(k) = s.ToString.Replace(".", "")
            k += 1
        Next
        k = 0
        For Each s In en
            en(k) = s.ToString.Replace(".", "")
            k += 1
        Next
        IP1.Text = sta(0)
        IP2.Text = sta(1)
        IP3.Text = sta(2)
        IP4.Text = sta(3)
        IP5.Text = en(0)
        IP6.Text = en(1)
        IP7.Text = en(2)
        IP8.Text = en(3)
        IPbox.Text = My.Settings.staticip
        FTPdir.Text = My.Settings.ftpdir
        SECOND.Text = My.Settings.customsecond.ToString

    End Sub

    Public Declare Function InternetOpen Lib "Wininet.DLL" Alias "InternetOpenA" _
(ByVal lpszAgent As String, ByVal dwAccessType As Long, ByVal lpszProxyName As String, _
ByVal lpszProxyBypass As String, ByVal dwFlags As Long) As Long


    Public Declare Function InternetConnect Lib "Wininet.DLL" Alias "InternetConnectA" _
(ByVal hInternet As Long, ByVal lpszServerName As String, ByVal nServerPort As Integer, _
ByVal lpszUsername As String, ByVal lpszPassword As String, ByVal dwService As Long, _
ByVal dwFlags As Long, ByVal dwContext As Long) As Long


    Public Sub SENDDB_PSPFTPD(ByVal filepath As String)

        If My.Computer.Network.IsAvailable Then
            Dim sendfile As String = filepath
            'サーバーのホスト名とポート番号
            Dim host As String = My.Settings.staticip
            Dim port As Integer = 21
            Dim tcp As New TcpClient
            Dim ns As NetworkStream = Nothing
            Dim ftpdir As String = My.Settings.ftpdir.Replace("\", "/")
            If sendfile.Contains(".CMF") Then
                ftpdir &= "/CMF"
            End If
            If sendfile.Contains(".dat") Then
                ftpdir = "/codeFreak/code"
            End If

            Dim ftpmessage As String = ""
            Dim data As Boolean = False
            Dim time As Integer = 100
            If My.Settings.customwait = True Then
                time = My.Settings.customsecond
            End If

            If My.Settings.ftpdaemon = False Then
                tcp.Connect(host, port)
                ns = tcp.GetStream
                'NetworkStreamを取得する
                '待ち時間
                Thread.Sleep(time)
                If ns.DataAvailable = True Then
                    data = True
                End If
            Else
                Dim shost As String = My.Settings.dhcpstart
                Dim sta As String() = My.Settings.dhcpstart.Split("."c)
                Dim en As String() = My.Settings.dhcpend.Split("."c)
                Dim k As Integer = 0
                For Each s In sta
                    sta(k) = s.ToString.Replace(".", "")
                    k += 1
                Next
                k = 0
                For Each s In en
                    en(k) = s.ToString.Replace(".", "")
                    k += 1
                Next
                shost = shost.Substring(0, shost.LastIndexOf("."))
                shost = shost.Substring(0, shost.LastIndexOf(".") + 1)



                Dim hostname As String = Dns.GetHostName()

                ' ホスト名からIPアドレスを取得する
                Dim adrList As IPAddress() = Dns.GetHostAddresses(hostname)
                For Each address As IPAddress In adrList
                    Console.WriteLine(address.ToString())
                Next


                Dim i As Integer = CInt(sta(2)) * 256 + CInt(sta(3))
                Dim z As Integer = CInt(en(2)) * 256 + CInt(en(3)) + 1

                While i < z

                    host = shost & (i \ 256).ToString & "." & (i And &HFF).ToString
                    Dim tcp_dhcp As New TcpClient
                    tcp_dhcp.Connect(host, port)
                    ns = tcp_dhcp.GetStream
                    'NetworkStreamを取得する
                    host = shost & (i \ 256).ToString & "." & (i And &HFF).ToString
                    IPbox.Text = host
                    '待ち時間
                    Thread.Sleep(time)

                    If ns.DataAvailable = True Then
                        data = True
                        Exit While
                    Else
                        tcp_dhcp.Close()
                    End If

                    i += 1
                End While
            End If

            If data = True Then

                ftpmessage &= ReceiveData(ns)
                SendData(ns, "PWD" & vbCrLf)
                ftpmessage &= ReceiveData(ns)
                SendData(ns, "USER " & "anonymous" & vbCrLf)
                ftpmessage &= ReceiveData(ns)
                SendData(ns, "PASS " & "anonymous" & vbCrLf)
                ftpmessage &= ReceiveData(ns)
                SendData(ns, "CWD " & ftpdir & vbCrLf)
                ftpmessage &= ReceiveData(ns)
                SendData(ns, "PWD" & vbCrLf)
                ftpmessage &= ReceiveData(ns)
                SendData(ns, "TYPE I" & vbCrLf)
                ftpmessage &= ReceiveData(ns)

                ftpmessage &= upload(ns, sendfile, host)

                SendData(ns, "QUIT" + vbCrLf)
                ns.Close()
                tcp.Close()

                MessageBox.Show(Me, My.Resources.ftps & ftpmessage, "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Else
                ns.Close()
                tcp.Close()
                MessageBox.Show(Me, My.Resources.ftpf, "FAIL", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub

    Function upload(ByVal ns As NetworkStream, ByVal tmpath As String, ByVal host As String) As String
        Dim sendp As String = ""
        Dim p As Integer = 0
        Dim q As Integer = 0
        Dim s As String = ""
        Dim inst As Integer = tmpath.LastIndexOf("\") + 1
        Dim prxname As String = tmpath.Substring(inst, tmpath.Length - inst)
        Dim UpLoadStream As NetworkStream
        Dim ftpm As String = ""

        SendData(ns, "PASV" & vbCrLf)
        'pasvのぽーとげと、あくてぃぶはPORTにかえるだけ
        'http://www.java2s.com/Tutorial/VB/0400__Socket-Network/FtpClientinVBnet.htm
        sendp = ReceiveData(ns).Replace(")", "")
        ftpm &= sendp.Replace("(", "")
        p = sendp.LastIndexOf(",") + 1
        s = sendp.Substring(p, sendp.Length - p)
        sendp = sendp.Remove(p - 1)
        p = CInt(s)
        q = sendp.LastIndexOf(",") + 1
        s = sendp.Substring(q, sendp.Length - q)
        q = CInt(s) * 256
        'ftpdのしようのためseplugins/tempar　は使えないっぽいのでるーと+TEMPARしかないっぽい
        Dim prx As New FileStream(tmpath, FileMode.Open, FileAccess.Read)
        Dim bs(CInt(prx.Length - 1)) As Byte
        prx.Read(bs, 0, bs.Length)
        prx.Close()
        Dim data As New TcpClient()
        data.Connect(host, p + q)
        UpLoadStream = data.GetStream
        SendData(ns, "STOR " & prxname & vbCrLf)
        ftpm &= ReceiveData(ns)
        UpLoadStream.Write(bs, 0, bs.Length)
        UpLoadStream.Close()
        data.Close()
        ftpm &= ReceiveData(ns)

        Return ftpm
    End Function

    Private Overloads Shared Function ReceiveData( _
        ByVal stream As NetworkStream, _
        ByVal multiLines As Boolean, _
        ByVal bufferSize As Integer, _
        ByVal enc As Encoding) As String
        Dim data(bufferSize - 1) As Byte
        Dim len As Integer = 0
        Dim msg As String = ""
        Dim ms As New System.IO.MemoryStream

        'すべて受信する
        '(無限ループに陥る恐れあり)
        Do
            '受信
            len = stream.Read(data, 0, data.Length)
            ms.Write(data, 0, len)
            '文字列に変換する
            msg = enc.GetString(ms.ToArray())
        Loop While stream.DataAvailable OrElse _
            ((Not multiLines OrElse msg.StartsWith("-ERR")) AndAlso _
                Not msg.EndsWith(vbCrLf)) OrElse _
            (multiLines AndAlso Not msg.EndsWith(vbCrLf + "." + vbCrLf))

        ms.Close()

        '"-ERR"を受け取った時は例外をスロー
        If msg.StartsWith("-ERR") Then
            Throw New ApplicationException("Received Error")
        End If

        Return msg
    End Function
    Private Overloads Shared Function ReceiveData( _
            ByVal stream As NetworkStream, _
            ByVal multiLines As Boolean, _
            ByVal bufferSize As Integer) As String
        Return ReceiveData(stream, multiLines, bufferSize, _
            Encoding.GetEncoding(0))
    End Function
    Private Overloads Shared Function ReceiveData( _
            ByVal stream As NetworkStream, _
            ByVal multiLines As Boolean) As String
        Return ReceiveData(stream, multiLines, 256)
    End Function

    Private Overloads Shared Function ReceiveData( _
            ByVal stream As NetworkStream) As String
        Return ReceiveData(stream, False)
    End Function

    Private Overloads Shared Sub SendData( _
        ByVal stream As NetworkStream, _
        ByVal msg As String, _
        ByVal enc As Encoding)
        'byte型配列に変換
        Dim data As Byte() = enc.GetBytes(msg)
        '送信
        stream.Write(data, 0, data.Length)
    End Sub

    Private Overloads Shared Sub SendData( _
            ByVal stream As NetworkStream, _
            ByVal msg As String)
        SendData(stream, msg, Encoding.ASCII)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If IP1.Text = "" Then
            IP1.Text = "0"
        End If
        If IP2.Text = "" Then
            IP2.Text = "0"
        End If
        If IP3.Text = "" Then
            IP3.Text = "0"
        End If
        If IP4.Text = "" Then
            IP4.Text = "0"
        End If
        If IP5.Text = "" Then
            IP5.Text = "0"
        End If
        If IP6.Text = "" Then
            IP6.Text = "0"
        End If
        If IP7.Text = "" Then
            IP7.Text = "0"
        End If
        If IP8.Text = "" Then
            IP8.Text = "0"
        End If

        Dim IPval1 As Integer = CInt(IP1.Text)
        Dim IPval2 As Integer = CInt(IP2.Text)
        Dim IPval3 As Integer = CInt(IP3.Text)
        Dim IPval4 As Integer = CInt(IP4.Text)
        Dim IPval5 As Integer = CInt(IP5.Text)
        Dim IPval6 As Integer = CInt(IP6.Text)
        Dim IPval7 As Integer = CInt(IP7.Text)
        Dim IPval8 As Integer = CInt(IP8.Text)
        Dim statotal As Integer = (IPval2 << 16) + (IPval3 << 8) + IPval4
        Dim endtotal As Integer = (IPval6 << 16) + (IPval7 << 8) + IPval8
        Dim r As New System.Text.RegularExpressions.Regex( _
    "^1(0|92|72)\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Dim m As System.Text.RegularExpressions.Match = r.Match(IPbox.Text)

        If ((IPval1.ToString = "192" AndAlso IPval2.ToString = "168") Or _
            (IPval1.ToString = "172" AndAlso IPval2 > 15 AndAlso IPval2 < 32 AndAlso IPval6 > 15 AndAlso IPval6 < 32) _
            Or IPval1.ToString = "10") AndAlso IP1.Text = IP5.Text AndAlso statotal <= endtotal _
            AndAlso IPval2 < 256 AndAlso IPval3 < 256 _
            AndAlso IPval4 < 256 AndAlso IPval6 < 256 _
            AndAlso IPval7 < 256 AndAlso IPval8 < 256 _
            AndAlso IPbox.Text.Length = m.Value.Length Then
            My.Settings.staticip = IPbox.Text
            My.Settings.ftpdir = FTPdir.Text
            My.Settings.customsecond = CInt(SECOND.Text)
            My.Settings.dhcpstart = IPval1.ToString & "." & IPval2.ToString & "." & IPval3.ToString & "." & IPval4.ToString
            My.Settings.dhcpend = IPval5.ToString & "." & IPval6.ToString & "." & IPval7.ToString & "." & IPval8.ToString
            Me.Close()
        ElseIf statotal > endtotal Then
            MessageBox.Show(Me, My.Resources.ip1, "INVAIDIP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            MessageBox.Show(Me, My.Resources.ip2, "INVAIDIP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles daemonfinder.CheckedChanged
        If daemonfinder.Checked = False Then
            My.Settings.ftpdaemon = False
        Else
            My.Settings.ftpdaemon = True
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
      Handles IP1.KeyPress, IP2.KeyPress, IP3.KeyPress, IP4.KeyPress, IP5.KeyPress, IP6.KeyPress, IP7.KeyPress, IP8.KeyPress, SECOND.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub ipbox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles IPbox.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub IP1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IP1.TextChanged
        IP5.Text = IP1.Text
        If IP1.Text = "192" Then
            IP2.Text = "168"
            IP6.Text = "168"
        End If
    End Sub

    Private Sub IP5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IP5.TextChanged
        IP1.Text = IP5.Text
        If IP5.Text = "192" Then
            IP2.Text = "168"
            IP6.Text = "168"
        End If
    End Sub

    Private Sub WAIT_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WAIT.CheckedChanged
        If WAIT.Checked = False Then
            My.Settings.customwait = False
        Else
            My.Settings.customwait = True
        End If
    End Sub
End Class