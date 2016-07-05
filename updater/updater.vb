Imports System.IO
Imports System.Text

Public Class updater

    Private Sub main_Load(ByVal sender As Object, _
        ByVal e As EventArgs) Handles MyBase.Load

        Dim uppath As String = Application.StartupPath & "\updater.txt"

        If File.Exists(uppath) Then
            Dim sr As New System.IO.StreamReader(uppath, _
        System.Text.Encoding.GetEncoding(932))
            '内容をすべて読み込む
            Dim s As String = sr.ReadToEnd()
            '閉じる
            sr.Close()

            'ダウンロードするファイル
            'Dim url As String = "http://unzu127xp.pa.land.to/mogura/writelog.php?dl=http://unzu127xp.pa.land.to/data/IJIRO/CDEMOD/bin/Release/CDE_CP932_FM4.exe"

            Dim url As String = "https://github.com/haroturbo/CDEMOD/raw/master/bin/Release/CDE_CP932_FM4.exe"

            '保存先のファイル名
            Dim fileName As String = s

            'WebRequestの作成
            Dim webreq As System.Net.HttpWebRequest = _
                CType(System.Net.WebRequest.Create(url),  _
                    System.Net.HttpWebRequest)

            'サーバーからの応答を受信するためのWebResponseを取得
            Dim webres As System.Net.HttpWebResponse = _
                CType(webreq.GetResponse(), System.Net.HttpWebResponse)

            '応答データを受信するためのStreamを取得
            Dim strm As System.IO.Stream = webres.GetResponseStream()

            'ファイルに書き込むためのFileStreamを作成
            Dim fs As New System.IO.FileStream(fileName, _
                System.IO.FileMode.Create, System.IO.FileAccess.Write)

            '応答データをファイルに書き込む
            Dim b As Integer
            While True
                b = strm.ReadByte()
                If b = -1 Then Exit While
                fs.WriteByte(Convert.ToByte(b))
            End While

            '閉じる
            fs.Close()
            strm.Close()

            File.Delete(uppath)

            Process.Start(s)
        Else
            MessageBox.Show("updater.txtが見つかりませんでした", "うｐ失敗")
        End If

        Me.Close()

    End Sub

End Class
