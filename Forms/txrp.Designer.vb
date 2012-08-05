<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class txrp
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.RUN = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.RPTEST = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TBLEDIT = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TBLLOAD = New System.Windows.Forms.Button()
        Me.TESTTX = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "検索対象文字列"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 12)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "置換文字列"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(26, 28)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(216, 19)
        Me.TextBox1.TabIndex = 2
        '
        'RUN
        '
        Me.RUN.Location = New System.Drawing.Point(23, 195)
        Me.RUN.Name = "RUN"
        Me.RUN.Size = New System.Drawing.Size(75, 23)
        Me.RUN.TabIndex = 3
        Me.RUN.Text = "実行"
        Me.RUN.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(26, 65)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(216, 19)
        Me.TextBox2.TabIndex = 4
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(26, 91)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(93, 16)
        Me.CheckBox1.TabIndex = 5
        Me.CheckBox1.Text = "大小区別無し"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(26, 114)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(72, 16)
        Me.CheckBox2.TabIndex = 6
        Me.CheckBox2.Text = "正規表現"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(136, 90)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(93, 16)
        Me.CheckBox3.TabIndex = 7
        Me.CheckBox3.Text = "選択範囲のみ"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'RPTEST
        '
        Me.RPTEST.Enabled = False
        Me.RPTEST.Location = New System.Drawing.Point(132, 195)
        Me.RPTEST.Name = "RPTEST"
        Me.RPTEST.Size = New System.Drawing.Size(75, 23)
        Me.RPTEST.TabIndex = 8
        Me.RPTEST.Text = "TEST"
        Me.RPTEST.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(265, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 12)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "置換TEST"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(6, 18)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(72, 20)
        Me.ComboBox1.TabIndex = 11
        '
        'TBLEDIT
        '
        Me.TBLEDIT.Location = New System.Drawing.Point(84, 18)
        Me.TBLEDIT.Name = "TBLEDIT"
        Me.TBLEDIT.Size = New System.Drawing.Size(62, 23)
        Me.TBLEDIT.TabIndex = 12
        Me.TBLEDIT.Text = "編集"
        Me.TBLEDIT.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TBLLOAD)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.TBLEDIT)
        Me.GroupBox1.Location = New System.Drawing.Point(26, 136)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(216, 46)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "置換文字セット"
        '
        'TBLLOAD
        '
        Me.TBLLOAD.Location = New System.Drawing.Point(152, 18)
        Me.TBLLOAD.Name = "TBLLOAD"
        Me.TBLLOAD.Size = New System.Drawing.Size(57, 23)
        Me.TBLLOAD.TabIndex = 13
        Me.TBLLOAD.Text = "反映"
        Me.TBLLOAD.UseVisualStyleBackColor = True
        '
        'TESTTX
        '
        Me.TESTTX.Location = New System.Drawing.Point(267, 28)
        Me.TESTTX.MaxLength = 0
        Me.TESTTX.Multiline = True
        Me.TESTTX.Name = "TESTTX"
        Me.TESTTX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TESTTX.Size = New System.Drawing.Size(211, 190)
        Me.TESTTX.TabIndex = 14
        '
        'txrp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(490, 230)
        Me.Controls.Add(Me.TESTTX)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.RPTEST)
        Me.Controls.Add(Me.CheckBox3)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.RUN)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "txrp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents RUN As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents RPTEST As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents TBLEDIT As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TBLLOAD As System.Windows.Forms.Button
    Friend WithEvents TESTTX As System.Windows.Forms.TextBox
End Class
