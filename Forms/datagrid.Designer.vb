<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class datagrid
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
        Me.components = New System.ComponentModel.Container()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.アドレス = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.編集タイプ = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.入力値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CNVbikou = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.addline = New System.Windows.Forms.ToolStripMenuItem()
        Me.addmacro = New System.Windows.Forms.ToolStripMenuItem()
        Me.cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.CPADV = New System.Windows.Forms.ToolStripMenuItem()
        Me.paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.moveup = New System.Windows.Forms.ToolStripMenuItem()
        Me.movedown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.notetag = New System.Windows.Forms.ToolStripMenuItem()
        Me.appy = New System.Windows.Forms.ToolStripMenuItem()
        Me.APPLY = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridsave = New System.Windows.Forms.CheckBox()
        Me.g_address = New System.Windows.Forms.RadioButton()
        Me.g_value = New System.Windows.Forms.RadioButton()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.timer = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FLOAT計算ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RPN = New System.Windows.Forms.ToolStripMenuItem()
        Me.CVTRPN = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.LOOKSORDER = New System.Windows.Forms.ToolStripMenuItem()
        Me.STACKORDER = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CNVbikou.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.アドレス, Me.値, Me.編集タイプ, Me.入力値, Me.備考})
        Me.DataGridView1.ContextMenuStrip = Me.CNVbikou
        Me.DataGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DataGridView1.Location = New System.Drawing.Point(6, 91)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 21
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(441, 240)
        Me.DataGridView1.TabIndex = 0
        '
        'アドレス
        '
        Me.アドレス.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.アドレス.Frozen = True
        Me.アドレス.HeaderText = "アドレス"
        Me.アドレス.MaxInputLength = 10
        Me.アドレス.Name = "アドレス"
        Me.アドレス.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.アドレス.Width = 47
        '
        '値
        '
        Me.値.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.値.Frozen = True
        Me.値.HeaderText = "値"
        Me.値.MaxInputLength = 10
        Me.値.Name = "値"
        Me.値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.値.Width = 23
        '
        '編集タイプ
        '
        Me.編集タイプ.AutoComplete = False
        Me.編集タイプ.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.編集タイプ.Frozen = True
        Me.編集タイプ.HeaderText = "編集タイプ"
        Me.編集タイプ.Items.AddRange(New Object() {"DEC", "DEC16BIT", "BINARY32", "BIN32>>16", "BINARY16", "OR", "AND", "XOR", "ASM"})
        Me.編集タイプ.Name = "編集タイプ"
        Me.編集タイプ.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.編集タイプ.Width = 61
        '
        '入力値
        '
        Me.入力値.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.入力値.Frozen = True
        Me.入力値.HeaderText = "入力値　　　 "
        Me.入力値.MaxInputLength = 11
        Me.入力値.MinimumWidth = 88
        Me.入力値.Name = "入力値"
        Me.入力値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.入力値.Width = 88
        '
        '備考
        '
        Me.備考.FillWeight = 200.0!
        Me.備考.HeaderText = "備考　　　　"
        Me.備考.MaxInputLength = 64
        Me.備考.MinimumWidth = 80
        Me.備考.Name = "備考"
        Me.備考.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.備考.Width = 83
        '
        'CNVbikou
        '
        Me.CNVbikou.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.addline, Me.cut, Me.copy, Me.paste, Me.ToolStripSeparator1, Me.moveup, Me.movedown, Me.ToolStripSeparator2, Me.notetag, Me.appy})
        Me.CNVbikou.Name = "CNVbikou"
        Me.CNVbikou.Size = New System.Drawing.Size(156, 192)
        Me.CNVbikou.Text = "備考変換"
        '
        'addline
        '
        Me.addline.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.addmacro})
        Me.addline.Name = "addline"
        Me.addline.Size = New System.Drawing.Size(155, 22)
        Me.addline.Text = "1行コード追加"
        '
        'addmacro
        '
        Me.addmacro.Enabled = False
        Me.addmacro.Name = "addmacro"
        Me.addmacro.Size = New System.Drawing.Size(172, 22)
        Me.addmacro.Text = "コードマクロ挿入"
        '
        'cut
        '
        Me.cut.Name = "cut"
        Me.cut.Size = New System.Drawing.Size(155, 22)
        Me.cut.Text = "カット"
        '
        'copy
        '
        Me.copy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CPADV})
        Me.copy.Name = "copy"
        Me.copy.Size = New System.Drawing.Size(155, 22)
        Me.copy.Text = "コピー"
        '
        'CPADV
        '
        Me.CPADV.Name = "CPADV"
        Me.CPADV.Size = New System.Drawing.Size(182, 22)
        Me.CPADV.Text = "入力値+備考を含む"
        '
        'paste
        '
        Me.paste.Name = "paste"
        Me.paste.Size = New System.Drawing.Size(155, 22)
        Me.paste.Text = "貼付け"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(152, 6)
        '
        'moveup
        '
        Me.moveup.Name = "moveup"
        Me.moveup.Size = New System.Drawing.Size(155, 22)
        Me.moveup.Text = "上に移動"
        Me.moveup.ToolTipText = "コードを1行上に移動します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CTRL押しながらメニューを表示すると☆マークが付き" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "一番上まで一気に移動します"
        '
        'movedown
        '
        Me.movedown.Name = "movedown"
        Me.movedown.Size = New System.Drawing.Size(155, 22)
        Me.movedown.Text = "下に移動"
        Me.movedown.ToolTipText = "コードを1行下に移動します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CTRL押しながらメニューを表示すると☆マークが付き" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "一番下まで一気に移動します"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(152, 6)
        '
        'notetag
        '
        Me.notetag.Name = "notetag"
        Me.notetag.Size = New System.Drawing.Size(155, 22)
        Me.notetag.Text = "備考タグ変換"
        Me.notetag.ToolTipText = "CWCコード横の説明部分/FREECHEATの_N2、SCMをタグに変換します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "//CWC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "_L 0x... 0x....(説明)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "//FREECHEA" & _
    "T" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "_N2 (説明)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "_L 0x... 0x...." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "$SCM{" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "$(説明)$2 $ (0123...)"
        '
        'appy
        '
        Me.appy.Name = "appy"
        Me.appy.Size = New System.Drawing.Size(155, 22)
        Me.appy.Text = "適用"
        '
        'APPLY
        '
        Me.APPLY.Location = New System.Drawing.Point(327, 3)
        Me.APPLY.Name = "APPLY"
        Me.APPLY.Size = New System.Drawing.Size(56, 23)
        Me.APPLY.TabIndex = 2
        Me.APPLY.Text = "適用"
        Me.APPLY.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(9, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = " "
        '
        'gridsave
        '
        Me.gridsave.AutoSize = True
        Me.gridsave.Location = New System.Drawing.Point(217, 337)
        Me.gridsave.Name = "gridsave"
        Me.gridsave.Size = New System.Drawing.Size(113, 16)
        Me.gridsave.TabIndex = 4
        Me.gridsave.Text = "適用と同時に保存"
        Me.gridsave.UseVisualStyleBackColor = True
        '
        'g_address
        '
        Me.g_address.AutoSize = True
        Me.g_address.Location = New System.Drawing.Point(198, 7)
        Me.g_address.Name = "g_address"
        Me.g_address.Size = New System.Drawing.Size(63, 16)
        Me.g_address.TabIndex = 5
        Me.g_address.TabStop = True
        Me.g_address.Text = "address"
        Me.g_address.UseVisualStyleBackColor = True
        '
        'g_value
        '
        Me.g_value.AutoSize = True
        Me.g_value.Checked = True
        Me.g_value.Location = New System.Drawing.Point(263, 7)
        Me.g_value.Name = "g_value"
        Me.g_value.Size = New System.Drawing.Size(50, 16)
        Me.g_value.TabIndex = 6
        Me.g_value.TabStop = True
        Me.g_value.Text = "value"
        Me.g_value.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"DEC", "DEC16BIT", "BINARY32", "BIN32>>16", "BINARY16", "ASM"})
        Me.ComboBox1.Location = New System.Drawing.Point(304, 31)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(104, 20)
        Me.ComboBox1.TabIndex = 7
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.APPLY)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.g_value)
        Me.Panel1.Controls.Add(Me.g_address)
        Me.Panel1.Location = New System.Drawing.Point(12, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(430, 57)
        Me.Panel1.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(195, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 12)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "編集タイプ一括変更"
        '
        'timer
        '
        Me.timer.AutoSize = True
        Me.timer.Location = New System.Drawing.Point(389, 338)
        Me.timer.Name = "timer"
        Me.timer.Size = New System.Drawing.Size(38, 12)
        Me.timer.TabIndex = 9
        Me.timer.Text = "Label3"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FLOAT計算ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(459, 26)
        Me.MenuStrip1.TabIndex = 10
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FLOAT計算ToolStripMenuItem
        '
        Me.FLOAT計算ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RPN})
        Me.FLOAT計算ToolStripMenuItem.Name = "FLOAT計算ToolStripMenuItem"
        Me.FLOAT計算ToolStripMenuItem.Size = New System.Drawing.Size(82, 22)
        Me.FLOAT計算ToolStripMenuItem.Text = "FLOAT計算"
        '
        'RPN
        '
        Me.RPN.Checked = True
        Me.RPN.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RPN.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CVTRPN, Me.ToolStripSeparator3, Me.LOOKSORDER, Me.STACKORDER})
        Me.RPN.Name = "RPN"
        Me.RPN.Size = New System.Drawing.Size(152, 22)
        Me.RPN.Text = "RPNモード"
        Me.RPN.ToolTipText = "逆ポーランド記法で複数の式を処理して単精度浮動小数点数を出力します。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "通常モード時は単体式のみ対応" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "通常モード;tan(45度) " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RPN式;9,2,3,*" & _
    ",6,*,9,+,tan"
        '
        'CVTRPN
        '
        Me.CVTRPN.Checked = True
        Me.CVTRPN.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CVTRPN.Name = "CVTRPN"
        Me.CVTRPN.Size = New System.Drawing.Size(180, 22)
        Me.CVTRPN.Text = "数式をRPNに変換"
        Me.CVTRPN.ToolTipText = "数式をRPN式に変換して処理します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "数式;tan(9+(2*3)*6)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "↓" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RPN式;9,2,3,*,6,*,9,+,tan"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(177, 6)
        '
        'LOOKSORDER
        '
        Me.LOOKSORDER.Checked = True
        Me.LOOKSORDER.CheckState = System.Windows.Forms.CheckState.Checked
        Me.LOOKSORDER.Name = "LOOKSORDER"
        Me.LOOKSORDER.Size = New System.Drawing.Size(180, 22)
        Me.LOOKSORDER.Text = "①,② スタック降順"
        Me.LOOKSORDER.ToolTipText = "関数引数順番がスタック降順になります" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "pow(①,②)→①,②,pow" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logx(①,②)→①,②,logx" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logy(①,②)→①,②,logy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ata" & _
    "n2_(①,②)→①,②,atan2_" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "atan2ms_(①,②)→①,②,atan2ms_"
        '
        'STACKORDER
        '
        Me.STACKORDER.Name = "STACKORDER"
        Me.STACKORDER.Size = New System.Drawing.Size(180, 22)
        Me.STACKORDER.Text = "②,① スタック昇順"
        Me.STACKORDER.ToolTipText = "関数引数順番がスタック昇順になります;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "pow(①,②)→②,①,pow" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logx(①,②)→②,①,logx" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logy(①,②)→②,①,logy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "at" & _
    "an2_(①,②)→②,①,atan2_" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "atan2ms_(①,②)→②,①,atan2ms_" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'datagrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(459, 361)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.timer)
        Me.Controls.Add(Me.gridsave)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "datagrid"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "データグリッドエディター"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CNVbikou.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents APPLY As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gridsave As System.Windows.Forms.CheckBox
    Friend WithEvents g_address As System.Windows.Forms.RadioButton
    Friend WithEvents g_value As System.Windows.Forms.RadioButton
    Friend WithEvents CNVbikou As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents notetag As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents appy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents moveup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents movedown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents addline As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents paste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents addmacro As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents アドレス As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 編集タイプ As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents 入力値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 備考 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents timer As System.Windows.Forms.Label
    Friend WithEvents CPADV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FLOAT計算ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RPN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CVTRPN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents STACKORDER As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LOOKSORDER As System.Windows.Forms.ToolStripMenuItem
End Class
