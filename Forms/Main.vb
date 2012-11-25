Imports System.IO       'Stream、StreamWriter、StreamReader、FileStream用
Imports System.Text     'Encoding用
Imports System.Diagnostics
Imports System.Collections
Imports System.Linq
Imports System.Net
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices

Public Class MERGE
    Friend database As String = Nothing
    Friend loaded As Boolean = False
    Friend PSX As Boolean = False
    Friend CODEFREAK As Boolean = False
    Friend DATEL As Boolean = False
    Dim enc1 As Integer = My.Settings.MSCODEPAGE
    Friend maintop As Boolean = My.Settings.TOP
    Friend showerror As Boolean = My.Settings.ERR
    Friend browser As String = My.Settings.browser
    Dim listmax As Integer = 30
    Friend url(listmax) As String
    Friend app(listmax) As String

#Region "ini"

    Private Sub main_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        UPDATE_URLAPPS(False, My.Settings.urls, 4)
        UPDATE_URLAPPS(True, My.Settings.apps, 5)

        If My.Settings.fixedform = True Then
            Me.AutoSize = True
            fixedform.Checked = True
        Else
            Me.Width = My.Settings.mainyoko
            Me.Height = My.Settings.maintate
        End If

        cpstring.Checked = My.Settings.checkcpstr
        GBKOP.Checked = My.Settings.GBKOP
        CFEDIT.Checked = My.Settings.cfid
        DBENCODE.Checked = My.Settings.saveencode
        update_save_filepass.Checked = My.Settings.codepathwhensave
        PBPHBHASH.Checked = My.Settings.hbhash
        ARBINhanzen.Checked = My.Settings.arbinhanzen
        ARCUT.Checked = My.Settings.arbincut

        CT_tb.Font = My.Settings.CT_tb
        GID_tb.Font = My.Settings.GID_tb
        GT_tb.Font = My.Settings.CT_tb
        cmt_tb.Font = My.Settings.cmt_tb
        cl_tb.Font = My.Settings.cl_tb
        codetree.Font = My.Settings.codetree

        GITHUB.Checked = My.Settings.updatesever
        GOOGLESVN.Checked = Not GITHUB.Checked


        If My.Settings.savetype = True Then
            CPENC.Checked = My.Settings.savetype
        Else
            ENCTRING.Checked = True
        End If


        If My.Settings.updater = True Then
            Dim check As New checkupdate
            check.CDEupater("start")
            autoupdater.Checked = True
        Else
            autoupdater.Checked = False
        End If


        If My.Settings.updatemode = False Then
            releasedate.Checked = True
        Else
            samename.Checked = True
        End If

        If showerror = True Then
            error_window.Show()
            options_error.Checked = True
            options_error.Text = "エラー画面を隠す"

            If maintop = True Then
                error_window.TopMost = True
            End If
        Else
            error_window.Hide()
            options_error.Checked = False
            options_error.Text = "エラー画面を表示"
        End If

        If maintop = True Then
            Me.TopMost = True
            error_window.TopMost = True
            options_ontop.Checked = True
        Else
            error_window.TopMost = False
            options_ontop.Checked = False
        End If

        If My.Settings.gridvalueedit = True Then
            grided_use.Checked = True
            DATAGRID.Visible = True
            dgedit.Visible = True
        End If

        If System.IO.File.Exists(browser) Then
        Else
            browser = "IExplore.exe"
        End If


        For Each cmd As String In My.Application.CommandLineArgs
            My.Settings.lastcodepath = cmd
        Next

        If System.IO.File.Exists(My.Settings.lastcodepath) = True Then

            DBLOAD(My.Settings.lastcodepath)

        Else
            codetree.Nodes.Add("NEW_DB").ImageIndex = 0
        End If

        reset_codepage()

        'http://dobon.net/vb/dotnet/control/tvdraganddrop.html
        'TreeView1へのドラッグを受け入れる
        codetree.AllowDrop = True
        'イベントハンドラを追加する
        AddHandler codetree.ItemDrag, AddressOf codetree_ItemDrag
        AddHandler codetree.DragOver, AddressOf codetree_DragOver
        AddHandler codetree.DragDrop, AddressOf codetree_DragDrop


        If My.Settings.updatecomp = True Then
            MessageBox.Show(Me, "アップデートが完了しました", "アップデート完了")
            My.Settings.updatecomp = False
        End If

    End Sub

    'BeforeLabelEditイベントハンドラ
    'ツリーノードのラベルの編集が開始された時
    Private Sub TreeView1_BeforeLabelEdit(ByVal sender As Object, _
                                          ByVal e As NodeLabelEditEventArgs) Handles codetree.BeforeLabelEdit
        'ルートのコードは編集できないようにする
        If e.Node.Parent Is Nothing Then
            e.CancelEdit = True
        End If

    End Sub

    'AfterLabelEditイベントハンドラ
    'ツリーノードのラベルの編集された時
    Private Sub TreeView1_AfterLabelEdit(ByVal sender As Object, _
                                         ByVal e As NodeLabelEditEventArgs) Handles codetree.AfterLabelEdit
        'ラベルが変更されたか調べる
        'e.LabelがNothingならば、変更されていない
        Dim treenode As TreeNode = codetree.SelectedNode

        If (e.Label = "") Then
            e.CancelEdit = True
        ElseIf (e.Label.Trim = "") Then
            e.CancelEdit = True
            'ElseIf (e.Label.Length > 74) Then
            '    treenode.Text = e.Label.Substring(0, 74)
        End If

    End Sub

#End Region

#Region "Menubar procedures"

#Region "Open Database/Save Database"

    Private Sub new_psp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles new_psp.Click

        resets_level1()
        Dim ok As Boolean = False

        If loaded = False Then
            codetree.BeginUpdate()
            reset_PSP()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
            loaded = True
            ok = True
        ElseIf MessageBox.Show("新規データベースを作成すると現在のデータベースが消えてしまいます。このまま新規データベースを作成してもよろしいですか？", "データベース保存の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then
            codetree.BeginUpdate()
            reset_PSP()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
            ok = True
        End If
        If ok = True Then
            resets_level1()
            UTF16BE.Enabled = False
            saveas_codefreak.Enabled = False
            PSX = False
            saveas_cwcheat.Enabled = True
            saveas_psx.Enabled = False
            file_saveas.Enabled = True
            overwrite_db.Enabled = True
        End If
    End Sub

    Private Sub new_psx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles new_psx.Click

        resets_level1()
        Dim ok As Boolean = False

        If loaded = False Then
            codetree.BeginUpdate()
            reset_PSX()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
            loaded = True
            ok = True
        ElseIf MessageBox.Show("新規データベースを作成すると現在のデータベースが消えてしまいます。このまま新規データベースを作成してもよろしいですか？", "データベース保存の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then
            codetree.BeginUpdate()
            reset_PSX()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
            ok = True
        End If
        If ok = True Then
            resets_level1()
            file_saveas.Enabled = True
            UTF16BE.Enabled = False
            saveas_codefreak.Enabled = False
            PSX = True
            saveas_cwcheat.Enabled = False
            saveas_psx.Enabled = True
            overwrite_db.Enabled = True
        End If
    End Sub

    Private Sub file_open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles file_open.Click

        Dim open As New load_db

        Me.open_file.Filter = "対応ファイル(*.db;*ar;*.cmf;*.txt;*.dat;*.bin)|*.db;*.ar;*.cmf;*.txt;*.dat;*.bin|CWcheat (*.db)|*.d" & _
            "b|ACTIONREPLAY(*.ar;*.bin)|*.ar;*.bin|CMFUSION (*.cmf)|*.cmf|FreeCheat (*.txt)|*.txt|CodeFre" & _
            "ak (*.dat)|*.dat|全てのファイル (*.*)|*.*"
        If My.Settings.lastcodepath <> "" Then
            Dim z As Integer = My.Settings.lastcodepath.LastIndexOf("\")
            open_file.InitialDirectory = My.Settings.lastcodepath.Substring(0, z)
        End If

        If open_file.ShowDialog = Windows.Forms.DialogResult.OK And open_file.FileName <> Nothing Then


            DBLOAD(open_file.FileName)

            'database = open_file.FileName
            'error_window.list_save_error.Items.Clear() 'Clear any save errors from a previous database
            'PSX = open.check_db(database, 932) ' Check the file's format
            'CODEFREAK = open.check2_db(database, 1201)
            'DATEL = open.check3_db(database, 932)
            'codetree.Nodes.Clear()
            'codetree.BeginUpdate()
            'error_window.list_load_error.BeginUpdate()

            'If CODEFREAK = True Then
            '    reset_PSP()
            '    Application.DoEvents()
            '    enc1 = 1201
            '    If CFEDIT.Checked = False Then
            '        open.read_cf(database, 1201)
            '    Else
            '        open.read_cfcp1201(database, 1201)
            '    End If
            'ElseIf DATEL = True Then
            '    reset_PSP()
            '    Application.DoEvents()
            '    enc1 = 932
            '    open.read_ar(database, 932)
            '    If ARBINhanzen.Checked = True Then
            '        半角カナ全角ToolStripMenuItem_Click(sender, e)
            '    End If

            '    ElseIf PSX = True Then
            '        enc1 = open.check_enc(database)
            '        reset_PSX()
            '        Application.DoEvents()
            '        open.read_PSX(database, enc1)
            '    ElseIf open.no_db(database, enc1) = False Then
            '        enc1 = open.check_enc(database)
            '        reset_PSP()
            '        Application.DoEvents()
            '        open.read_PSP(database, enc1)
            '    End If
            '    If codetree.Nodes.Count >= 1 Then
            '        codetree.Nodes(0).Expand()
            '    End If
            '    resets_level1()
            '    codetree.EndUpdate()
            '    reset_codepage()
            '    error_window.list_load_error.EndUpdate()
            '    loaded = True
            '    file_saveas.Enabled = True
            '    overwrite_db.Enabled = True
            '    My.Settings.lastcodepath = database
            '    overwrite_db.ToolTipText = "対象;" & database

        End If

    End Sub

    Private Function DBLOAD(ByVal dbfile As String) As Boolean
        Dim open As New load_db

        database = dbfile

        error_window.list_save_error.Items.Clear() 'Clear any save errors from a previous database
        PSX = open.check_db(database, 932) ' Check the file's format
        CODEFREAK = open.check2_db(database, 1201)
        DATEL = open.check3_db(database, 932)
        codetree.Nodes.Clear()
        codetree.BeginUpdate()
        error_window.list_load_error.BeginUpdate()

        If CODEFREAK = True Then
            reset_PSP()
            Application.DoEvents()
            enc1 = 1201
            If CFEDIT.Checked = False Then
                open.read_cf(database, 1201)
            Else
                open.read_cfcp1201(database, 1201)
            End If
        ElseIf DATEL = True Then
            reset_PSP()
            Application.DoEvents()
            enc1 = 932
            open.read_ar(database, 932)
            If ARBINhanzen.Checked = True Then
                半角カナ全角ToolStripMenuItem_Click(Nothing, Nothing)
            End If

        ElseIf PSX = True Then
            enc1 = open.check_enc(database)
            reset_PSX()
            Application.DoEvents()
            open.read_PSX(database, enc1)
        ElseIf open.no_db(database, enc1) = False Then
            enc1 = open.check_enc(database)
            reset_PSP()
            Application.DoEvents()
            open.read_PSP(database, enc1)
        End If
        If codetree.Nodes.Count >= 1 Then
            codetree.Nodes(0).Expand()
        End If
        resets_level1()
        codetree.EndUpdate()
        reset_codepage()
        error_window.list_load_error.EndUpdate()
        loaded = True
        file_saveas.Enabled = True
        overwrite_db.Enabled = True
        overwrite_db.ToolTipText = "対象;" & database

        My.Settings.lastcodepath = database

        Return True

    End Function

    Private Sub overwrite_db_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles overwrite_db.Click
        Dim s As New save_db
        If My.Settings.lastcodepath <> "" Then

            If CODEFREAK = True Then
                s.save_cf(database, 1201)
            ElseIf DATEL = True Then
                If ARBINhanzen.Checked = True Then
                    codetree.BeginUpdate()
                    Dim n As TreeNode = CType(codetree.Nodes(0).Clone(), TreeNode)
                    全角カナ半角カナToolStripMenuItem_Click(sender, e)
                    s.save_ar(database, 932)
                    codetree.Nodes.Clear()
                    codetree.Nodes.Add(n)
                    If codetree.Nodes.Count >= 1 Then
                        codetree.Nodes(0).Expand()
                    End If
                    codetree.EndUpdate()
                Else
                    s.save_ar(database, 932)
                End If

            ElseIf PSX = True Then
                s.save_psx(database, enc1)
            Else
                s.save_cwcheat(database, enc1)
            End If

            codetree.Nodes(0).Text = Path.GetFileNameWithoutExtension(database)
            If My.Settings.codepathwhensave = True Then
                My.Settings.lastcodepath = database
            End If

        End If
    End Sub

    Private Sub saveas_cwcheat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_cwcheat.Click
        Dim open As New load_db
        Dim s As New save_db

        Me.save_file.Filter = "CWcheat (*.db)|*.db|ACTIOPREPLAY (*.ar)|*.ar|CMFUSION (*.cmf)|*.cmf|FreeCheat (*." & _
            "txt)|*.txt"

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then

            database = save_file.FileName
            s.save_cwcheat(database, enc1)

            codetree.Nodes(0).Text = Path.GetFileNameWithoutExtension(database)
            overwrite_db.ToolTipText = "対象;" & database

            DATEL = False
            CODEFREAK = False
            If My.Settings.codepathwhensave = True Then
                My.Settings.lastcodepath = database
            End If

        End If
    End Sub

    Private Sub saveas_psx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_psx.Click
        Dim open As New load_db
        Dim s As New save_db

        Me.save_file.Filter = "CWcheat (*.db)|*.db"

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then

            database = save_file.FileName
            s.save_psx(database, enc1)
            codetree.Nodes(0).Text = Path.GetFileNameWithoutExtension(database)
            overwrite_db.ToolTipText = "対象;" & database


            PSX = True
            If My.Settings.codepathwhensave = True Then
                My.Settings.lastcodepath = database
            End If

        End If
    End Sub

    Private Sub saveas_codefreak_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_codefreak.Click
        Dim open As New load_db
        Dim s As New save_db

        Me.save_file.Filter = "CODEFREAK (*.dat)|*.dat"

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then

            database = save_file.FileName
            s.save_cf(database, 1201)

            codetree.Nodes(0).Text = Path.GetFileNameWithoutExtension(database)
            overwrite_db.ToolTipText = "対象;" & database

            DATEL = False
            CODEFREAK = True
            If My.Settings.codepathwhensave = True Then
                My.Settings.lastcodepath = database
            End If

        End If
    End Sub

    Private Sub saveas_actionreplay_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_actionreplay.Click
        Dim open As New load_db
        Dim s As New save_db

        Me.save_file.Filter = "ACTIONREPLAY (*.bin)|*.bin"

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then

            database = save_file.FileName
            If ARBINhanzen.Checked = True Then
                codetree.BeginUpdate()
                Dim n As TreeNode = CType(codetree.Nodes(0).Clone(), TreeNode)
                全角カナ半角カナToolStripMenuItem_Click(sender, e)
                s.save_ar(database, 932)
                codetree.Nodes.Clear()
                codetree.Nodes.Add(n)
                If codetree.Nodes.Count >= 1 Then
                    codetree.Nodes(0).Expand()
                End If
                codetree.EndUpdate()
            Else
                s.save_ar(database, 932)
            End If

            codetree.Nodes(0).Text = Path.GetFileNameWithoutExtension(database)
            overwrite_db.ToolTipText = "対象;" & database

            DATEL = True
            CODEFREAK = False
            If My.Settings.codepathwhensave = True Then
                My.Settings.lastcodepath = database
            End If
        End If
    End Sub

    Private Sub file_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles file_exit.Click
        Close()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Me.AutoSize = False Then
            My.Settings.mainyoko = Me.Width
            My.Settings.maintate = Me.Height
        End If

    End Sub

    '初期化
#Region "Control resets"

    Private Sub resets_level1()

        ' Disable editing of games and codes if the root node is selected
        GID_tb.Enabled = False
        GID_tb.Text = Nothing
        GT_tb.Enabled = False
        GT_tb.Text = Nothing
        cmt_tb.Enabled = False
        cmt_tb.Text = Nothing
        cl_tb.Enabled = False
        cl_tb.Text = Nothing
        CT_tb.Enabled = False
        CT_tb.Text = Nothing
        off_rd.Enabled = False
        on_rd.Enabled = False
        NodeConvert.Visible = False
        Panel1.Enabled = False
        DATAGRID.Enabled = False
        dgedit.Enabled = False
        PSF.Enabled = False

        USELIST.Enabled = False
        SHIFLIST.Enabled = False
        SELECTLIST.Enabled = False


        ftpcmf.Enabled = False
        ftpscm.Enabled = False
        ftptab.Enabled = False

        If PSX = False Then
            saveas_cwcheat.Enabled = True
            saveas_actionreplay.Enabled = True
            saveas_codefreak.Enabled = True
            saveas_psx.Enabled = False
            CMFexport.Enabled = True
            SCMexport.Enabled = True
            TABexport.Enabled = True
            FCTXT.Enabled = True
        ElseIf PSX = True Then
            saveas_cwcheat.Enabled = False
            saveas_actionreplay.Enabled = False
            saveas_codefreak.Enabled = False
            saveas_psx.Enabled = True
            CMFexport.Enabled = False
            SCMexport.Enabled = False
            TABexport.Enabled = False
            FCTXT.Enabled = False
        End If

        button_list.Enabled = False
        inverse_chk.Enabled = False
        inverse_chk.Checked = False

        For i = 0 To 19 ' Reset the checked list box state
            button_list.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub resets_level2()

        ' Disable editing of a code if one is not selected
        GID_tb.Enabled = True
        GT_tb.Enabled = True
        cmt_tb.Enabled = False
        cmt_tb.Text = Nothing
        cl_tb.Enabled = False
        cl_tb.Text = Nothing
        CT_tb.Enabled = False
        CT_tb.Text = Nothing
        off_rd.Enabled = False
        on_rd.Enabled = False
        Panel1.Enabled = False
        DATAGRID.Enabled = False
        dgedit.Enabled = False
        NodeConvert.Visible = True

        PSF.Enabled = True
        USELIST.Enabled = False
        SHIFLIST.Enabled = False
        SELECTLIST.Enabled = False


        If PSX = False Then
            saveas_cwcheat.Enabled = True
            saveas_actionreplay.Enabled = True
            saveas_codefreak.Enabled = True
            saveas_psx.Enabled = False
            CMFexport.Enabled = True
            SCMexport.Enabled = True
            TABexport.Enabled = True
            FCTXT.Enabled = True
            ftpcmf.Enabled = True
            ftpscm.Enabled = True
            ftptab.Enabled = True
        ElseIf PSX = True Then
            saveas_cwcheat.Enabled = False
            saveas_actionreplay.Enabled = False
            saveas_codefreak.Enabled = False
            saveas_psx.Enabled = True
            CMFexport.Enabled = False
            SCMexport.Enabled = False
            TABexport.Enabled = False
            ftpcmf.Enabled = False
            ftpscm.Enabled = False
            ftptab.Enabled = False
        End If


        button_list.Enabled = False
        inverse_chk.Enabled = False
        inverse_chk.Checked = False

        For i = 0 To 19 ' Reset the checked list box state
            button_list.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub resets_level3()

        ' Enable editing of all controls
        cmt_tb.Enabled = True
        cmt_tb.Text = Nothing
        cl_tb.Enabled = True
        cl_tb.Text = Nothing
        CT_tb.Enabled = True
        off_rd.Enabled = True
        on_rd.Enabled = True
        Panel1.Enabled = True
        USELIST.Enabled = True
        SHIFLIST.Enabled = True
        SELECTLIST.Enabled = True

        NodeConvert.Visible = True
        DATAGRID.Enabled = True
        dgedit.Enabled = True
        PSF.Enabled = True

        If PSX = False Then
            button_list.Enabled = True
            inverse_chk.Enabled = True
        End If

        For i = 0 To 19 ' Reset the checked list box state
            button_list.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub reset_PSX()

        codetree.ImageList = PSX_iconset
        With tool_menu
            add_game.Image = My.Resources.Resources.add_PSX_game
            rem_game.Image = My.Resources.Resources.remove_PSX_game
            save_gc.Image = My.Resources.Resources.save_PSX_game
        End With

    End Sub

    Private Sub reset_PSP()

        codetree.ImageList = iconset
        With tool_menu
            add_game.Image = My.Resources.Resources.add_game
            rem_game.Image = My.Resources.remove_game
            save_gc.Image = My.Resources.Resources.save_game
        End With

    End Sub

#End Region

#End Region

#Region "Sort procedures"
    Private Sub GID昇順(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sort_GID１.Click
        Dim s As New sort
        s.sort_game(0)
    End Sub

    Private Sub GID降順(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sort_GID2.Click
        Dim s As New sort
        s.sort_game(1)
    End Sub

    Private Sub 国別ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gid_country.Click
        Dim s As New sort
        s.sort_game(4)
    End Sub

    Private Sub 国別gnameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gname_country.Click
        Dim s As New sort
        s.sort_game(8)
    End Sub

    Private Sub Sort_GTitle1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sort_GTitle1.Click
        Dim s As New sort
        s.sort_game(2)
    End Sub

    Private Sub Sort_CTitle2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sort_GTitle2.Click
        Dim s As New sort
        s.sort_game(3)
    End Sub

#End Region

#Region "Options"

#Region "FONT"
    Private Sub ツリービューToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font_treeview.Click

        Dim fd As New FontDialog()

        fd.Font = codetree.Font
        fd.Color = codetree.ForeColor
        fd.MaxSize = 24
        fd.MinSize = 9
        fd.FontMustExist = True
        fd.ShowHelp = True
        fd.ShowApply = True
        If fd.ShowDialog() <> DialogResult.Cancel Then
            codetree.Font = fd.Font
            My.Settings.codetree = fd.Font
        End If
    End Sub

    Private Sub ゲームタイトルToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font_gtitle.Click
        Dim fd As New FontDialog()
        fd.Font = GT_tb.Font
        fd.Color = GT_tb.ForeColor
        fd.MaxSize = 12
        fd.MinSize = 9
        fd.FontMustExist = True
        fd.ShowHelp = True
        fd.ShowApply = True
        If fd.ShowDialog() <> DialogResult.Cancel Then
            GT_tb.Font = fd.Font
            My.Settings.GT_tb = fd.Font
        End If
    End Sub

    Private Sub ゲームIDToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font_gid.Click
        Dim fd As New FontDialog()
        fd.Font = GID_tb.Font
        fd.Color = GID_tb.ForeColor
        fd.MaxSize = 12
        fd.MinSize = 9
        fd.FontMustExist = True
        fd.ShowHelp = True
        fd.ShowApply = True
        fd.FixedPitchOnly = True
        If fd.ShowDialog() <> DialogResult.Cancel Then
            GID_tb.Font = fd.Font
            My.Settings.GID_tb = fd.Font
        End If
    End Sub

    Private Sub コード名ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font_codename.Click
        Dim fd As New FontDialog()
        fd.Font = CT_tb.Font
        fd.Color = CT_tb.ForeColor
        fd.MaxSize = 12
        fd.MinSize = 9
        fd.FontMustExist = True
        fd.ShowHelp = True
        fd.ShowApply = True
        If fd.ShowDialog() <> DialogResult.Cancel Then
            CT_tb.Font = fd.Font
            My.Settings.CT_tb = fd.Font
        End If
    End Sub

    Private Sub コード内容ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font_codetxt.Click

        Dim fd As New FontDialog()

        fd.Font = cl_tb.Font
        fd.Color = cl_tb.ForeColor
        fd.MaxSize = 12
        fd.MinSize = 9
        fd.FontMustExist = True
        fd.ShowHelp = True
        fd.ShowApply = True
        fd.FixedPitchOnly = True
        If fd.ShowDialog() <> DialogResult.Cancel Then
            cl_tb.Font = fd.Font
            My.Settings.cl_tb = fd.Font
        End If
    End Sub

    Private Sub コメントToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font_cmt.Click
        Dim fd As New FontDialog()

        fd.Font = cmt_tb.Font
        fd.Color = cmt_tb.ForeColor
        fd.MaxSize = 24
        fd.MinSize = 9
        fd.FontMustExist = True
        fd.ShowHelp = True
        fd.ShowApply = True
        If fd.ShowDialog() <> DialogResult.Cancel Then
            cmt_tb.Font = fd.Font
            My.Settings.cmt_tb = fd.Font
        End If
    End Sub
#End Region

    Private Sub options_error_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles options_error.Click

        If options_error.Checked = False Then
            error_window.Show()
            options_error.Checked = True
            options_error.Text = "エラー画面を隠す"
            My.Settings.ERR = True
            Me.Focus()

            If options_ontop.Checked = True Then
                Me.TopMost = True
                error_window.TopMost = True
            End If

        Else
            error_window.Hide()
            options_error.Checked = False
            options_error.Text = "エラー画面を表示"
            My.Settings.ERR = False
        End If

    End Sub

    Private Sub options_ontop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles options_ontop.Click

        If options_ontop.Checked = False Then
            Me.TopMost = True
            error_window.TopMost = True
            options_ontop.Checked = True
            My.Settings.TOP = True
        Else
            Me.TopMost = False
            error_window.TopMost = False
            options_ontop.Checked = False
            My.Settings.TOP = False
        End If

    End Sub

    Private Sub codesite(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ブラウザ変更.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = "C:\Program Files"
        ofd.Filter = _
    "EXEファイル(*.exe)|*.exe"
        ofd.Title = "ブラウザのEXEを選んでください"
        If ofd.ShowDialog() = DialogResult.OK Then
            'OKボタンがクリックされたとき
            '選択されたファイル名を表示する
            My.Settings.browser = ofd.FileName
            browser = ofd.FileName
        End If
    End Sub


    Function urltrim(ByVal url As String) As String
        Dim r As New Regex("^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$")
        Dim m As Match = r.Match(url)
        If m.Success Then
            Dim i As Integer = m.Value.IndexOf(":") + 3
            url = url.Substring(i, url.Length - i)
            i = 0
            i = url.IndexOf("/")
            If i > 0 Then
                url = url.Substring(0, i)
            End If
        End If
        Return url
    End Function


    Function exename(ByVal path As String) As String
        Dim root As Integer = path.LastIndexOf("\") + 1
        Dim str As String = path.Substring(root, path.Length - root)

        Return str.Replace(".exe", "")

    End Function

    Private Sub G有効ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grided_use.Click

        If DATAGRID.Visible = False Then
            DATAGRID.Visible = True
            dgedit.Visible = True
            grided_use.Checked = True
        Else
            DATAGRID.Visible = False
            dgedit.Visible = False
            grided_use.Checked = False
        End If
        My.Settings.gridvalueedit = DATAGRID.Visible

    End Sub

    Private Sub フォーム固定ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fixedform.Click

        If fixedform.Checked = True Then
            My.Settings.fixedform = False
            fixedform.Checked = False
            Me.AutoSize = False
        Else
            My.Settings.fixedform = True
            fixedform.Checked = True
            Me.AutoSize = True
        End If
    End Sub

    Private Sub 保存時最終コードパスを更新ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles update_save_filepass.Click

        If update_save_filepass.Checked = True Then
            My.Settings.codepathwhensave = False
            update_save_filepass.Checked = False
        Else
            My.Settings.codepathwhensave = True
            update_save_filepass.Checked = True
        End If
    End Sub

    Private Sub autoupdater_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles autoupdater.Click
        If autoupdater.Checked = True Then
            My.Settings.updater = False
            autoupdater.Checked = False
        Else
            My.Settings.updater = True
            autoupdater.Checked = True
        End If
    End Sub

#End Region

#Region "MSCODEPAGE"

    Function reset_codepage() As Integer

        GBK.Checked = False
        EUCJP.Checked = False
        SJIS.Checked = False
        BIG5.Checked = False
        UTF16BE.Checked = False
        UTF16BE.Enabled = False
        EUCJIS20004.Checked = False
        SHIFTJIS2004.Checked = False
        CCP.Checked = False
        UHC.Checked = False
        eucms.Checked = False

        If enc1 = 932 Then
            SJIS.Checked = True
        ElseIf enc1 = 1201 Then
            UTF16BE.Checked = True
            UTF16BE.Enabled = True
        ElseIf enc1 = 936 Then
            GBK.Checked = True
        ElseIf enc1 = 51932 Then
            EUCJP.Checked = True
        ElseIf enc1 = 951 Then
            BIG5.Checked = True
        ElseIf enc1 = 949 Then
            UHC.Checked = True
        ElseIf enc1 = 512132004 Then
            EUCJIS20004.Checked = True
        ElseIf enc1 = 21220932 Then
            eucms.Checked = True
        ElseIf enc1 = 2132004 Then
            SHIFTJIS2004.Checked = True
        ElseIf enc1 = My.Settings.usercp Then
            CCP.Checked = True
        End If

        CCP.ToolTipText = "コンボボックスから対応M$コードページを指定します" & vbCrLf & "指定コード:" & My.Settings.cpstr

        Return 0

    End Function

    Private Sub CP932ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SJIS.Click

        'エンコードを指定する場合
        My.Settings.MSCODEPAGE = 932
        enc1 = 932
        reset_codepage()
    End Sub

    Private Sub EUCJPCP51932ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EUCJP.Click

        My.Settings.MSCODEPAGE = 51932
        enc1 = 51932
        reset_codepage()

    End Sub

    Private Sub GBKToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GBK.Click

        'エンコードを指定する場合
        My.Settings.MSCODEPAGE = 936
        enc1 = 936
        reset_codepage()
    End Sub

    Private Sub BIG5CPToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BIG5.Click

        'エンコードを指定する場合
        My.Settings.MSCODEPAGE = 951
        enc1 = 951
        reset_codepage()

    End Sub

    Private Sub SHIFTJIS2004_Click(sender As System.Object, e As System.EventArgs) Handles SHIFTJIS2004.Click

        My.Settings.MSCODEPAGE = 2132004
        enc1 = 2132004
        reset_codepage()

    End Sub

    Private Sub EUCJIS20004_Click(sender As System.Object, e As System.EventArgs) Handles EUCJIS20004.Click

        My.Settings.MSCODEPAGE = 512132004
        enc1 = 512132004
        reset_codepage()

    End Sub

    Private Sub UTF16BECP1201ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UTF16BE.Click
        'エンコードを指定する場合
        enc1 = 1201
        reset_codepage()
    End Sub

    Private Sub CCP_Click(sender As System.Object, e As System.EventArgs) Handles CCP.Click

        Dim f As New codepage
        f.ShowDialog()
        f.Dispose()

        If My.Settings.MSCODEPAGE = My.Settings.usercp Then
            enc1 = My.Settings.usercp
            reset_codepage()
        End If

    End Sub

    Private Sub EUCKR_Click(sender As System.Object, e As System.EventArgs) Handles UHC.Click

        My.Settings.MSCODEPAGE = 949
        enc1 = 949
        reset_codepage()

    End Sub

    Private Sub ToolStripMenuItem2_Click_1(sender As System.Object, e As System.EventArgs) Handles eucms.Click

        My.Settings.MSCODEPAGE = 21220932
        enc1 = 21220932
        reset_codepage()
    End Sub
#End Region

#Region "codetree"

    'コードパーサー
    Private Sub paserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cntparser.Click, paserToolStripMenuItem.Click
        Dim backup As String = cmt_tb.Text
        Dim f As parser = parser
        Dim open As New load_db
        cmt_tb.Text = Nothing
        f.ShowDialog(Me)
        open.code_parser(cmt_tb.Text)
        f.Dispose()
        cmt_tb.Text = backup


    End Sub

    Private Sub すべて閉じるToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tree_collapse.Click, cntclose.Click
        codetree.CollapseAll()
        codetree.TopNode.Expand()
    End Sub

    Private Sub 全て展開するToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tree_expand.Click, cntexpand.Click
        codetree.ExpandAll()
    End Sub

    Private Sub 半角カナ全角ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles str_wide.Click, hankaku.Click

        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = 0
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        For Each n As TreeNode In codetree.Nodes(0).Nodes
            b1 = n.Text
            b1 = ConvANK(b1)
            codetree.Nodes(0).Nodes(i).Text = b1
            For Each m As TreeNode In n.Nodes
                b2 = m.Text
                b2 = ConvANK(b2)
                codetree.Nodes(0).Nodes(i).Nodes(z).Text = b2
                z += 1
            Next
            i += 1
            z = 0
        Next
        codetree.EndUpdate()

    End Sub

    Private Sub 全角カナ半角カナToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles str_narrow.Click

        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = 0
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        For Each n As TreeNode In codetree.Nodes(0).Nodes
            b1 = n.Text
            b1 = ConvANK2(b1)
            codetree.Nodes(0).Nodes(i).Text = b1
            For Each m As TreeNode In n.Nodes
                b2 = m.Text
                b2 = ConvANK2(b2)
                codetree.Nodes(0).Nodes(i).Nodes(z).Text = b2
                z += 1
            Next
            i += 1
            z = 0
        Next
        codetree.EndUpdate()
    End Sub

    Private Sub 簡体繁体ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles kan2han.Click

        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = 0
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        For Each n As TreeNode In codetree.Nodes(0).Nodes
            b1 = n.Text
            b1 = ConvANK3(b1)
            codetree.Nodes(0).Nodes(i).Text = b1
            For Each m As TreeNode In n.Nodes
                b2 = m.Text
                b2 = ConvANK3(b2)
                codetree.Nodes(0).Nodes(i).Nodes(z).Text = b2
                z += 1
            Next
            i += 1
            z = 0
        Next
        codetree.EndUpdate()


    End Sub

    Private Sub 繁体簡体ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles han2kan.Click

        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = 0
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        For Each n As TreeNode In codetree.Nodes(0).Nodes
            b1 = n.Text
            b1 = ConvANK4(b1)
            codetree.Nodes(0).Nodes(i).Text = b1
            For Each m As TreeNode In n.Nodes
                b2 = m.Text
                b2 = ConvANK4(b2)
                codetree.Nodes(0).Nodes(i).Nodes(z).Text = b2
                z += 1
            Next
            i += 1
            z = 0
        Next
        codetree.EndUpdate()

    End Sub

    Public Function ConvANK(ByVal moto As String) As String
        Dim re2 As Regex = New Regex("[\uFF61-\uFF9F]+")
        Dim output2 As String = re2.Replace(moto, AddressOf myReplacer2)
        Return output2
    End Function

    Public Function ConvANK2(ByVal moto As String) As String
        Dim re2 As Regex = New Regex("[\u3000-\u30FF]+")
        Dim output2 As String = re2.Replace(moto, AddressOf myReplacer1)
        Return output2
    End Function

    Public Function ConvANK3(ByVal moto As String) As String
        Dim re2 As Regex = New Regex("[\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]+")
        Dim output2 As String = re2.Replace(moto, AddressOf myReplacer4)
        Return output2
    End Function

    Public Function ConvANK4(ByVal moto As String) As String
        Dim re2 As Regex = New Regex("[[\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]+")
        Dim output2 As String = re2.Replace(moto, AddressOf myReplacer3)
        Return output2
    End Function

    Shared Function myReplacer1(ByVal m As Match) As String
        Dim s As String = m.Value
        s = Microsoft.VisualBasic.Strings.StrConv(s, Microsoft.VisualBasic.VbStrConv.Katakana, &H411)
        Return Strings.StrConv(s, Microsoft.VisualBasic.VbStrConv.Narrow, &H411)
    End Function

    Shared Function myReplacer2(ByVal m As Match) As String
        Return Strings.StrConv(m.Value, VbStrConv.Wide, &H411)
    End Function

    Shared Function myReplacer3(ByVal m As Match) As String
        Return Strings.StrConv(m.Value, VbStrConv.SimplifiedChinese)
    End Function

    Shared Function myReplacer4(ByVal m As Match) As String
        Return Strings.StrConv(m.Value, VbStrConv.TraditionalChinese)
    End Function

    Private Sub 中国語文字化け対策ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles str_gbk.Click, CNchar.Click

        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = 0
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        For Each n As TreeNode In codetree.Nodes(0).Nodes
            b1 = n.Text
            b1 = ConvCH(b1)
            codetree.Nodes(0).Nodes(i).Text = b1
            For Each m As TreeNode In n.Nodes
                b2 = m.Text
                b2 = ConvCH(b2)
                codetree.Nodes(0).Nodes(i).Nodes(z).Text = b2
                z += 1
            Next
            i += 1
            z = 0
        Next

        If GBKOP.Checked = True Then
            半角カナ全角ToolStripMenuItem_Click(sender, e)
        End If
        codetree.EndUpdate()
    End Sub

    Public Function ConvCH(ByVal moto As String) As String
        Dim st As String() = {"∋", "⊆", "⊇", "⊂", "⊃", "￢", "⇒", "⇔", "∃", "∂", "∇", "≪", "≫", "∬", "Å", "♯", "♭", "♪", "†", "‡", "¶", "⑪", "⑫", "⑬", "⑭", "⑮", "⑯", "⑰", "⑱", "⑲", "⑳", "㍉", "㌔", "㌢", "㍍", "㌘", "㌧", "㌃", "㌶", "㍑", "㍗", "㌍", "㌦", "㌣", "㌫", "㍊", "㌻", "㍻", "〝", "〟", "㏍", "㊤", "㊥", "㊦", "㊧", "㊨", "㍾", "㍽", "㍼"}
        Dim sr As String() = {" ", " ", " ", " ", " ", " ", "→", "←→", "ヨ", "", "", "<<", ">>", "ダブルインテグラル", "オングストローム", "シャープ", "フラット", "8分音符", "ダガー", "ダブルダガー", "パラグラフ", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "ミリ", "キロ", "センチ", "メートル", "グラム", "トン", "ア-ル", "ヘクタール", "リットル", "ワｯト", "カロリー", "ドル", "セント", "パ-セント", "ミリバール", "ページ", "平成", " ", " ", "KK", "上", "中", "下", "左", "右", "明治", "大正", "昭和"}
        Dim i As Integer = 0
        For i = 0 To 58
            If moto.Contains(st(i)) Then
                moto = moto.Replace(st(i), sr(i))
            End If
        Next
        Return moto
    End Function

    '<DllImport("libmecab.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Overloads Shared Function mecab_new2(ByVal arg As String) As IntPtr
    'End Function
    '<DllImport("libmecab.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Overloads Shared Function mecab_sparse_tostr(ByVal m As IntPtr, ByVal str As String) As IntPtr
    'End Function
    '<DllImport("libmecab.dll", CallingConvention:=CallingConvention.Cdecl)> _
    'Overloads Shared Function mecab_destroy(ByVal m As IntPtr) As Boolean
    'End Function

    'Private Sub MECAB半角カナToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MECAB半角カナToolStripMenuItem.Click

    '    codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

    '    Dim z As Integer = 0
    '    Dim i As Integer = 0
    '    Dim b1 As String = Nothing
    '    Dim b2 As String = Nothing
    '    Dim mecab As IntPtr
    '    Dim s As IntPtr
    '    For Each n As TreeNode In codetree.Nodes(0).Nodes
    '        b1 = n.Text

    '        mecab = mecab_new2("-Oyomi")
    '        s = mecab_sparse_tostr(mecab, b1)
    '        b1 = Marshal.PtrToStringAnsi(s)
    '        mecab_destroy(mecab)
    '        codetree.Nodes(0).Nodes(i).Text = b1
    '        For Each m As TreeNode In n.Nodes
    '            mecab = mecab_new2("-Oyomi")
    '            s = mecab_sparse_tostr(mecab, b2)
    '            b2 = Marshal.PtrToStringAnsi(s)
    '            mecab_destroy(mecab)
    '            codetree.Nodes(0).Nodes(i).Nodes(z).Text = b2
    '            z += 1
    '        Next
    '        i += 1
    '        z = 0
    '    Next
    '    codetree.EndUpdate()

    'End Sub


    'Private Sub MECABでローマ字ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MECABでローマ字ToolStripMenuItem.Click

    'End Sub

    Public Function hankaku_zenkana(ByVal c As Char) As Boolean
        Return (ChrW(&H20) <= c AndAlso c <= ChrW(&H7E)) OrElse _
        (ChrW(&H30A0) <= c AndAlso c <= ChrW(&H30FF)) OrElse _
        (ChrW(&H31F0) <= c AndAlso c <= ChrW(&H31FF)) OrElse _
        (ChrW(&H3099) <= c AndAlso c <= ChrW(&H309C))
    End Function

#End Region

#Region "BROWSER"

    Private Sub URL編集ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles URL編集ToolStripMenuItem.Click
        Dim f As New url_list
        f.Text = "URL編集"
        f.ShowDialog()
        f.Dispose()
        UPDATE_URLAPPS(False, My.Settings.urls, 4)
    End Sub

    Private Sub newitem_click(sender As Object, e As EventArgs)

        Dim menu_number As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Process.Start(browser, url(CInt(menu_number.Name)))

    End Sub

    Private Function UPDATE_URLAPPS(ByVal mode As Boolean, ByVal urlapp As String, ByVal pos As Integer) As Boolean


            treeopen.Items.RemoveAt(pos)

            Dim ss As String() = urlapp.Split(CChar(vbLf))
            Dim i As Integer = 0


        Dim fileitem As New ToolStripMenuItem()
        Dim fileitem2 As New ToolStripMenuItem()
            If mode = False Then
                fileitem.Text = "ブラウザ(&B)"
            Else
            fileitem.Text = "EXE(&E)"
            fileitem2.Text = "EXE起動"

            'コンテキストを動的に作成
            ContextMenus.Items.RemoveAt(9)

            'ContextMenus.Items.Insert(9, New ToolStripMenuItem("EXE起動"))
            End If

            For Each s In ss
                s = s.Trim
                If ss(i).Contains(vbTab) Then
                    Dim newitem As New ToolStripMenuItem()
                    newitem.Text = ss(i).Substring(0, ss(i).IndexOf(vbTab))
                    newitem.Name = i.ToString
                    fileitem.DropDownItems.Add(newitem)

                    If mode = False Then
                        AddHandler newitem.Click, AddressOf newitem_click
                        url(i) = s.Remove(0, s.IndexOf(vbTab) + 1)
                    Else
                    Dim newitem2 As New ToolStripMenuItem()
                    newitem2.Text = newitem.Text
                    newitem2.Name = i.ToString

                    fileitem2.DropDownItems.Add(newitem2)

                    'コンテキスト10番目に追加する
                    'CType(ContextMenus.Items(9), ToolStripMenuItem).DropDownItems.Add(newitem2)

                    'サブルーチンを指定
                    AddHandler newitem.Click, AddressOf newapp_click
                    AddHandler newitem2.Click, AddressOf newapp_click
                        app(i) = s.Remove(0, s.IndexOf(vbTab) + 1)
                    End If
                    i += 1
                End If
            Next

        If mode = True Then
            ContextMenus.Items.Insert(9, fileitem2)
        End If

        '-----の追加
        fileitem.DropDownItems.Add(New ToolStripSeparator())

        Dim edi As ToolStripMenuItem = New ToolStripMenuItem()

            If mode = False Then
                edi.Text = "URL編集"
                fileitem.DropDownItems.Add(edi)
                AddHandler edi.Click, AddressOf URL編集ToolStripMenuItem_Click
            ElseIf mode = True Then
                edi.Text = "EXE編集"
                fileitem.DropDownItems.Add(edi)
                AddHandler edi.Click, AddressOf ランチャー編集ToolStripMenuItem_Click
            End If


        treeopen.Items.Insert(pos, fileitem)


        Return True

    End Function


#End Region

#Region "EXECUTE"

    Private Sub newapp_click(sender As Object, e As EventArgs)

        Dim menu_number As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        boot(app(CInt(menu_number.Name)))
    End Sub


    Private Sub ランチャー編集ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ランチャー編集ToolStripMenuItem.Click
        Dim f As New url_list
        f.Text = "ランチャー編集"
        f.ShowDialog()
        f.Dispose()
        UPDATE_URLAPPS(True, My.Settings.apps, 5)

    End Sub

    Function boot(ByVal exe As String) As Boolean

        If exe = "" Then
            MessageBox.Show(Me, "アプリケーションが登録されてません。", "アプリ未登録")
            Return False
        ElseIf Not exe.Contains(":") AndAlso Not exe.Contains(Application.StartupPath) AndAlso exe.Contains("APP\") AndAlso exe.Contains(".bat") Then
            exe = Application.StartupPath & "\" & exe
        End If

        If File.Exists(exe) = True Then
            Process.Start(exe)
        Else
            MessageBox.Show(Me, "'" + exe + "'が見つかりませんでした。")
        End If

        Return True
    End Function

#End Region

#Region "HELP"

    Private Sub オンラインヘルプToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles オンラインヘルプToolStripMenuItem.Click
        System.Diagnostics.Process.Start(browser, "http://ijiro.daiwa-hotcom.com/data/CDE.html")
    End Sub

    Private Sub バージョン情報ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles バージョン情報ToolStripMenuItem.Click
        Dim f As New version
        f.ShowDialog(Me)
        f.Dispose()
    End Sub




#End Region


    Private Sub menu_option(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_options.Click
        move_up.Enabled = False
        move_down.Enabled = False
        merge_codes.Enabled = False
    End Sub

    Private Sub help(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ヘルプHToolStripMenuItem.Click
        move_up.Enabled = False
        move_down.Enabled = False
        merge_codes.Enabled = False
    End Sub

#End Region

#Region "Toolbar buttons procedures"

    Private Sub add_game_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles add_game.Click

        Try

            Dim newgame As New TreeNode
            Dim newname As String = "新規ゲーム"

            If DATEL = True Then
                newname = "ｼﾝｷｹﾞｰﾑ"
                If ARBINhanzen.Checked Then
                    newname = "シンキゲーム"
                End If
            End If

            With newgame
                .Name = newname
                .Text = newname
                .ImageIndex = 1
                .Tag = "0000-00000"
            End With
            codetree.Nodes(0).Nodes.Insert(0, newgame)
            codetree.SelectedNode = newgame
            GT_tb.Enabled = True
            GT_tb.Text = newname

            file_saveas.Enabled = True
            overwrite_db.Enabled = True

        Catch ex As Exception

        End Try


    End Sub

    Private Sub rem_game_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rem_game.Click

        Try
            Select Case codetree.SelectedNode.Level

                Case Is <> 0
                    If MessageBox.Show("選択しているゲームとコードをすべて削除しますか？", "削除の確認", _
                                      MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        Select Case codetree.SelectedNode.Level
                            Case Is = 1
                                codetree.SelectedNode.Remove()
                            Case Is = 2
                                codetree.SelectedNode.Parent.Remove()
                        End Select

                    End If

            End Select

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Add_cd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Add_cd.Click

        Try
            Dim newcode As New TreeNode
            Dim newname As String = "新規コード"

            If DATEL = True Then
                newname = "ｼﾝｷｺｰﾄﾞ"
                If ARBINhanzen.Checked Then
                    newname = "シンキコード"
                End If
            End If


            With newcode
                .ImageIndex = 2
                .SelectedImageIndex = 3
                .Name = newname
                .Text = newname
                If DATEL = True Then
                    .Tag = "2"
                Else
                    .Tag = "0"
                End If
            End With

            Select Case codetree.SelectedNode.Level

                Case Is = 1

                    off_rd.Checked = True
                    CT_tb.Enabled = True
                    CT_tb.Text = newname
                    cmt_tb.Enabled = True
                    cl_tb.Enabled = True
                    codetree.SelectedNode.Nodes.Insert(0, newcode)
                    codetree.SelectedNode = newcode
                Case Is = 2

                    off_rd.Checked = True
                    CT_tb.Enabled = True
                    CT_tb.Text = newname
                    cmt_tb.Enabled = True
                    cl_tb.Enabled = True
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index + 1, newcode)
                    codetree.SelectedNode = newcode

            End Select

        Catch ex As Exception

        End Try

    End Sub

    Private Sub rem_cd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rem_cd.Click

        Try
            If codetree.SelectedNode.Level = 2 Then

                If MessageBox.Show("選択しているコードを削除しますか?", "削除の確認", MessageBoxButtons.OKCancel, _
                   MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

                    codetree.SelectedNode.Remove()

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub save_gc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_gc.Click

        changed.Text = ""
        Try
            GID_tb.Text = Regex.Replace(GID_tb.Text, "[^\-0-9A-Za-z]", "0").ToUpper
            Dim tv As TreeNode = codetree.SelectedNode

            If tv.Level > 0 Then
                If tv.Level = 2 Then
                    tv = tv.Parent
                End If

                With tv
                    .Name = GT_tb.Text
                    .Text = GT_tb.Text
                    .Tag = GID_tb.Text
                End With
                If GID_tb.Text.Length = 13 AndAlso tv.Nodes.Count > 0 AndAlso tv.Nodes(0).Text = "(M)" Then
                    Dim save As New save_db
                    Dim bytesData As Byte() = Encoding.GetEncoding(1252).GetBytes(GID_tb.Text.Remove(4, 1))
                    Dim gidst = ""
                    gidst = save.cvtsceid2cf(bytesData)
                    gidst &= GID_tb.Text.Remove(0, 5) 'CFID

                    tv.Nodes(0).Tag = "0" & vbCrLf & "0x" & gidst.Insert(8, " 0x") & vbCrLf

                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Sub save_cc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_cc.Click

        changed.Text = ""
        Try

            Dim b1 As String = cl_tb.Text
            Dim buffer As String = Nothing
            Dim i As Integer = 0
            Dim b5 As String = cmt_tb.Text
            cl_tb.Text = Nothing
            cmt_tb.Text = Nothing
            If off_rd.Checked = True Then
                If PSPAR.Checked = True Then
                    buffer = "2" & vbCrLf
                ElseIf TEMP.Checked = True Then
                    buffer = "4" & vbCrLf
                Else
                    buffer = "0" & vbCrLf
                End If
            Else
                If PSPAR.Checked = True Then
                    buffer = "3" & vbCrLf
                ElseIf TEMP.Checked = True Then
                    buffer = "5" & vbCrLf
                Else
                    buffer = "1" & vbCrLf
                End If
            End If

            If PSX = True Then
                Dim r As New System.Text.RegularExpressions.Regex("[0-9a-fA-F]{8} [0-9a-zA-Z?]{4}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                Dim m As System.Text.RegularExpressions.Match = r.Match(b1)

                While m.Success
                    buffer &= (m.Value) & vbCrLf
                    cl_tb.Text &= (m.Value) & vbCrLf
                    m = m.NextMatch()
                End While
            Else
                b1 = b1.Replace("_L ", "")
                Dim r As New System.Text.RegularExpressions.Regex("0x........ 0x........", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                Dim m As System.Text.RegularExpressions.Match = r.Match(b1)

                While m.Success
                    buffer &= (m.Value) & vbCrLf
                    cl_tb.Text &= (m.Value) & vbCrLf
                    m = m.NextMatch()
                End While

                buffer = System.Text.RegularExpressions.Regex.Replace(buffer, "[g-zG-Z]", "A")
                buffer = buffer.ToUpper
                buffer = System.Text.RegularExpressions.Regex.Replace(buffer, "^0A", "0x")
                buffer = System.Text.RegularExpressions.Regex.Replace(buffer, "(\r|\n)0A", vbCrLf & "0x")
                buffer = buffer.Replace(" 0A", " 0x")
            End If

            If codetree.SelectedNode.Level = 2 Then
                codetree.BeginUpdate()
                codetree.SelectedNode.Name = CT_tb.Text.Replace("_C0 ", "")
                codetree.SelectedNode.Text = CT_tb.Text.Replace("_C0 ", "")
                codetree.SelectedNode.Name = codetree.SelectedNode.Name.Replace("_C1 ", "")
                codetree.SelectedNode.Text = codetree.SelectedNode.Text.Replace("_C1 ", "")
                CT_tb.Text = codetree.SelectedNode.Name

                If b5 <> Nothing Then
                    Dim b3 As String() = b5.Split(CChar(vbLf))
                    For Each s As String In b3
                        s = s.Replace("#", "")
                        If i = 0 Then
                            If s.Substring(0, 1) >= "!" Then
                                buffer &= "#" & s.Trim & vbCrLf
                                cmt_tb.Text &= s.Trim & vbCrLf

                            End If
                        End If

                        If i > 0 And s.Length > 1 Then
                            buffer &= "#" & s.Trim & vbCrLf
                            cmt_tb.Text &= s.Trim & vbCrLf
                        End If
                        i += 1
                    Next
                End If

                buffer &= "#" & dgtext.Text.Trim & vbCrLf
                buffer &= "#" & dmtext.Text.Trim & vbCrLf


                If codetree.SelectedNode.Index = 0 AndAlso codetree.SelectedNode.Text = "(M)" Then
                    '0\r\n
                    '0x12345678 0x12345000
                    Dim tag As String = codetree.SelectedNode.Parent.Tag.ToString.PadRight(10, CChar("0")).Substring(0, 10)
                    codetree.SelectedNode.Parent.Tag = tag & buffer.Substring(22, 3)
                    GID_tb.Text = tag & buffer.Substring(22, 3)

                End If


                codetree.SelectedNode.Tag = buffer
                codetree.EndUpdate()
            End If


        Catch ex As Exception

        End Try

    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles move_up.Click

        Try

            Dim newcode As New TreeNode

            If codetree.SelectedNode.Level = 1 Then
                codetree.BeginUpdate()
                Dim cln As TreeNode = CType(codetree.SelectedNode.Clone(), TreeNode)
                If move_up.Text.Contains("☆") = True Then
                    codetree.SelectedNode.Parent.Nodes.Insert(0, cln)
                Else
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index - 1, cln)
                End If
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = cln
                codetree.EndUpdate()
                Dim z As Integer = codetree.SelectedNode.Index - 15
                If z < 0 Then
                    codetree.TopNode = codetree.SelectedNode.Parent
                Else
                    codetree.TopNode = codetree.SelectedNode.Parent.Nodes(z)
                End If
                codetree.Focus()
            End If

            If codetree.SelectedNode.Level = 2 Then

                With newcode
                    .ImageIndex = 2
                    .SelectedImageIndex = 3
                    .Name = codetree.SelectedNode.Name
                    .Text = codetree.SelectedNode.Text
                    .Tag = codetree.SelectedNode.Tag
                End With

                codetree.BeginUpdate()
                If move_up.Text.Contains("☆") = True Then
                    codetree.SelectedNode.Parent.Nodes.Insert(0, newcode)
                Else
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index - 1, newcode)
                End If
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = newcode
                codetree.EndUpdate()
                Dim z As Integer = codetree.SelectedNode.Index - 15
                If z < 0 Then
                    codetree.TopNode = codetree.SelectedNode.Parent
                Else
                    codetree.TopNode = codetree.SelectedNode.Parent.Nodes(z)
                End If
                codetree.Focus()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles move_down.Click

        Try
            Dim newcode As New TreeNode


            If codetree.SelectedNode.Level = 1 Then
                codetree.BeginUpdate()
                Dim cln As TreeNode = CType(codetree.SelectedNode.Clone(), TreeNode)
                If move_up.Text.Contains("☆") = True Then
                    codetree.SelectedNode.Parent.Nodes.Add(cln)
                Else
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index + 2, cln)
                End If
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = cln
                codetree.EndUpdate()
                Dim z As Integer = codetree.SelectedNode.Index - 15
                If z < 0 Then
                    codetree.TopNode = codetree.SelectedNode.Parent
                Else
                    codetree.TopNode = codetree.SelectedNode.Parent.Nodes(z)
                End If
                codetree.Focus()
            End If

            If codetree.SelectedNode.Level = 2 Then

                With newcode
                    .ImageIndex = 2
                    .SelectedImageIndex = 3
                    .Name = codetree.SelectedNode.Name
                    .Text = codetree.SelectedNode.Text
                    .Tag = codetree.SelectedNode.Tag
                End With

                codetree.BeginUpdate()
                If move_up.Text.Contains("☆") = True Then
                    codetree.SelectedNode.Parent.Nodes.Add(newcode)
                Else
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index + 2, newcode)
                End If
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = newcode
                codetree.EndUpdate()
                Dim z As Integer = codetree.SelectedNode.Index - 15
                If z < 0 Then
                    codetree.TopNode = codetree.SelectedNode.Parent
                Else
                    codetree.TopNode = codetree.SelectedNode.Parent.Nodes(z)
                End If
                codetree.Focus()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, _
        ByVal e As KeyEventArgs) Handles Me.KeyDown

        If (e.KeyData And Keys.Control) = Keys.Control Then
            If Not move_down.Text.Contains("☆") Then
                move_down.Text &= "☆"
                move_up.Text &= "☆"
            End If
        Else
            move_down.Text = move_down.Text.Replace("☆", "")
            move_up.Text = move_up.Text.Replace("☆", "")
        End If

    End Sub

    Private Sub TextBoxm_KeyDown(ByVal sender As Object, _
        ByVal e As KeyEventArgs) Handles Me.KeyUp
        move_down.Text = move_down.Text.Replace("☆", "")
        move_up.Text = move_up.Text.Replace("☆", "")
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles merge_codes.Click

        Try
            Dim newcode As New TreeNode
            Dim newcode2 As New TreeNode

            If codetree.SelectedNode.Level = 1 Then
                codetree.BeginUpdate()

                Dim i As Integer = 0
                Dim z As Integer = codetree.SelectedNode.Index
                Dim x As Integer = codetree.SelectedNode.Parent.Index
                Dim rr As TreeNode = codetree.SelectedNode
                Dim kk As Integer = rr.Nodes.Count

                If z > 0 Then
                    While kk > 0
                        merge_prevtreeview(z)
                        kk -= 1
                    End While
                End If

                codetree.EndUpdate()
            End If

            If codetree.SelectedNode.Level = 2 Then

                With newcode
                    .ImageIndex = 2
                    .SelectedImageIndex = 3
                    .Name = codetree.SelectedNode.Name
                    .Text = codetree.SelectedNode.Text
                    .Tag = codetree.SelectedNode.Tag
                End With
                Dim z As Integer = codetree.SelectedNode.Index
                Dim x As Integer = codetree.SelectedNode.Parent.Index
                With newcode2
                    .ImageIndex = 2
                    .SelectedImageIndex = 3
                    .Name = codetree.Nodes(0).Nodes(x).Nodes(z - 1).Name
                    .Text = codetree.Nodes(0).Nodes(x).Nodes(z - 1).Text
                    .Tag = codetree.Nodes(0).Nodes(x).Nodes(z - 1).Tag
                End With

                codetree.BeginUpdate()
                Dim b1 As String = newcode.Tag.ToString
                Dim b2 As String = newcode2.Tag.ToString
                b2 &= b1.Remove(0, 1) & vbCrLf
                newcode.Name &= "'"
                newcode.Text &= "'"
                newcode.Tag = b2
                Dim a As Integer = CInt(b1.Substring(0, 1))
                Dim b As Integer = CInt(b2.Substring(0, 1))

                If z = 0 Then

                ElseIf (b And &HE) = (a And &HE) Then
                    codetree.Nodes(0).Nodes(x).Nodes(z - 1).Remove()
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index, newcode)
                    codetree.SelectedNode.Remove()
                    codetree.SelectedNode = newcode

                ElseIf MessageBox.Show("コード形式が一致してません。このまま合成しますか？", "コード合成の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then
                    codetree.Nodes(0).Nodes(x).Nodes(z - 1).Remove()
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index, newcode)
                    codetree.SelectedNode.Remove()
                    codetree.SelectedNode = newcode
                End If
                codetree.EndUpdate()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Function merge_prevtreeview(ByVal z As Integer) As Boolean

        Dim newcode As New TreeNode

        With newcode
            .ImageIndex = 2
            .SelectedImageIndex = 3
            .Name = codetree.Nodes(0).Nodes(z).Nodes(0).Name
            .Text = codetree.Nodes(0).Nodes(z).Nodes(0).Text
            .Tag = codetree.Nodes(0).Nodes(z).Nodes(0).Tag
        End With
        codetree.Nodes(0).Nodes(z - 1).Nodes.Add(newcode)
        codetree.Nodes(0).Nodes(z).Nodes(0).Remove()
        Return True
    End Function

#End Region
    'ツリー操作
#Region "Code tree procedures"

    Private Sub codetree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles codetree.KeyUp
        If e.KeyCode = Keys.Delete Then
            Try
                If codetree.SelectedNode.Level = 1 Then
                    If MessageBox.Show("選択しているゲームとコードをすべて削除しますか？", "削除の確認", _
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        codetree.SelectedNode.Remove()
                    End If
                ElseIf codetree.SelectedNode.Level = 2 Then

                    If MessageBox.Show("選択しているコードを削除しますか?", "削除の確認", MessageBoxButtons.OKCancel, _
                       MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        codetree.SelectedNode.Remove()
                    End If
                End If

            Catch ex As Exception

            End Try

        End If
    End Sub

    'ツリー選択時
    Private Sub codetree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles codetree.AfterSelect
        Dim j As New joker

        Me.AutoSize = False
        changed.Text = ""
        dgtext.Text = ""
        dmtext.Text = ""
        move_up.Enabled = True
        move_down.Enabled = True
        merge_codes.Enabled = True
        Dim sdg As New StringBuilder
        Dim sdm As New StringBuilder
        Dim scmt As New StringBuilder
        Dim scode As New StringBuilder

        Select Case codetree.SelectedNode.Level

            Case Is = 0
                codetree.SelectedNode.SelectedImageIndex = 0
                resets_level1() ' Sets appropriate access to code editing
            Case Is = 1
                codetree.SelectedNode.SelectedImageIndex = 1
                GID_tb.Text = codetree.SelectedNode.Tag.ToString.Trim
                GT_tb.Text = codetree.SelectedNode.Text.Trim
                resets_level2() ' Sets appropriate access to code editing
            Case Is = 2
                Dim b1 As String = codetree.SelectedNode.Tag.ToString
                Dim b2 As String() = b1.Split(CChar(vbCrLf))
                Dim i As Integer = 0
                Dim skip As Boolean = False

                codetree.SelectedNode.SelectedImageIndex = 3
                CT_tb.Text = codetree.SelectedNode.Text.Trim
                GID_tb.Text = codetree.SelectedNode.Parent.Tag.ToString.Trim
                GT_tb.Text = codetree.SelectedNode.Parent.Text.Trim
                resets_level3() ' Sets appropriate access to code editing

                For Each s As String In b2

                    skip = False

                    s = s.Trim ' Remove the new line character so it doesn't interfere with checks

                    If i = 0 Then ' If on the first line, check if the code is enabled by default

                        If s = "1" Or s = "3" Or s = "5" Then
                            on_rd.Checked = True
                        Else
                            off_rd.Checked = True
                        End If

                        If s = "4" Or s = "5" Then
                            TEMP.Checked = True
                        ElseIf s = "2" Or s = "3" Then
                            PSPAR.Checked = True
                        ElseIf s = "0" Or s = "1" Then
                            CWC.Checked = True
                        End If

                        skip = True

                    End If

                    i += 1

                    Try

                        If s <> Nothing And skip = False Then

                            ' Check for a joker
                            If s.Trim.Length = 21 Then

                                If s.Substring(2, 1).ToUpper = "D" And s.Substring(13, 1) = "1" Then
                                    j.button_value(s)
                                ElseIf s.Substring(2, 1).ToUpper = "D" And s.Substring(13, 1) = "3" Then
                                    inverse_chk.Checked = True
                                    j.button_value(s)
                                End If

                            End If

                            If s.Length >= 2 Then

                                If s.Contains("#<DGLINE") Then
                                    sdg.AppendLine(s.Remove(0, 1))
                                ElseIf s.Contains("#<DGMODE") Then
                                    sdm.AppendLine(s.Remove(0, 1))
                                ElseIf s.Contains("#") Then
                                    scmt.AppendLine(s.Remove(0, 1))
                                Else
                                    scode.AppendLine(s)
                                End If

                            End If

                        End If

                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                Next

                dgtext.Text = sdg.ToString
                dmtext.Text = sdm.ToString
                cmt_tb.Text = scmt.ToString
                cl_tb.Text = scode.ToString

        End Select

    End Sub

    Private Sub codetree_DragEnter(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles codetree.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    'ドラッグ
    Private Sub codetree_ItemDrag(ByVal sender As Object, _
        ByVal e As ItemDragEventArgs)
        Dim tv As TreeView = CType(sender, TreeView)
        tv.SelectedNode = CType(e.Item, TreeNode)
        tv.Focus()

        'ノードのドラッグを開始する
        Dim dde As DragDropEffects = _
            tv.DoDragDrop(e.Item, DragDropEffects.All)

    End Sub

    Private Sub codetree_1DragDrop(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.DragEventArgs) _
            Handles codetree.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim fileName As String() = CType( _
                e.Data.GetData(DataFormats.FileDrop, False), _
                String())
            Dim psf As New psf
            If psf.video(fileName(0)) <> "" Then
                Dim id As String = psf.GETID(fileName(0))
                Dim add As Boolean = True
                For Each n As TreeNode In codetree.Nodes(0).Nodes
                    If n.Tag.ToString = id Then
                        add = False
                    End If
                Next
                If add = True Then
                    Dim newnode As New TreeNode
                    newnode.Text = psf.GETNAME(fileName(0))
                    newnode.Name = psf.GETNAME(fileName(0))
                    newnode.Tag = id
                    codetree.Nodes(0).Nodes.Insert(0, newnode)
                Else
                    MessageBox.Show(Me, id & "," & psf.GETNAME(fileName(0)) & vbCrLf & "はすでにIDが登録されてます", "ID重複")
                End If
                Exit Sub
            End If

            Dim open As New load_db
            If codetree.Nodes IsNot Nothing AndAlso MessageBox.Show("ドロップされたデータベースを開くと現在のデータベースが消えてしまいます。このまま開いてもよろしいですか？", _
                                                                    "データベース保存の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
            Me.AutoSize = False
            DBLOAD(fileName(0))


        End If
    End Sub

    'ドラッグしている時
    Private Sub codetree_DragOver(ByVal sender As Object, _
            ByVal e As DragEventArgs)
        'ドラッグされているデータがTreeNodeか調べる
        If e.Data.GetDataPresent(GetType(TreeNode)) Then
            If (e.KeyState And 8) = 8 And _
                (e.AllowedEffect And DragDropEffects.Copy) = _
                    DragDropEffects.Copy Then
                'Ctrlキーが押されていればCopy
                '"8"はCtrlキーを表す
                e.Effect = DragDropEffects.Copy
            ElseIf (e.AllowedEffect And DragDropEffects.Move) = _
                DragDropEffects.Move Then
                '何も押されていなければMove
                e.Effect = DragDropEffects.Move
            Else
                e.Effect = DragDropEffects.None
            End If
        ElseIf e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'TreeNodeでなければ受け入れない
            e.Effect = DragDropEffects.None
        End If

        'マウス下のNodeを選択する
        If e.Effect <> DragDropEffects.None AndAlso e.Effect <> DragDropEffects.Copy Then
            Dim tv As TreeView = CType(sender, TreeView)
            'マウスのあるNodeを取得する
            Dim target As TreeNode = _
                tv.GetNodeAt(tv.PointToClient(New Point(e.X, e.Y)))
            'ドラッグされているNodeを取得する
            Dim [source] As TreeNode = _
                CType(e.Data.GetData(GetType(TreeNode)), TreeNode)
            'マウス下のNodeがドロップ先として適切か調べる
            If Not target Is Nothing AndAlso _
                target.Level = [source].Level AndAlso _
                Not target Is [source] AndAlso _
                Not IsChildNode([source], target) AndAlso _
        move_up.Enabled = True Then
                'Nodeを選択する
                If target.IsSelected = False Then
                    tv.SelectedNode = target
                End If
            Else
                e.Effect = DragDropEffects.None
            End If
        End If
    End Sub

    'ドロップされたとき
    Private Sub codetree_DragDrop(ByVal sender As Object, _
            ByVal e As DragEventArgs)
        'ドロップされたデータがTreeNodeか調べる
        If e.Data.GetDataPresent(GetType(TreeNode)) Then
            Dim tv As TreeView = CType(sender, TreeView)
            'ドロップされたデータ(TreeNode)を取得
            Dim [source] As TreeNode = _
                CType(e.Data.GetData(GetType(TreeNode)), TreeNode)
            'ドロップ先のTreeNodeを取得する
            Dim target As TreeNode = _
                tv.GetNodeAt(tv.PointToClient(New Point(e.X, e.Y)))
            'マウス下のNodeがドロップ先として適切か調べる
            If Not target Is Nothing AndAlso _
                target.Level = [source].Level AndAlso _
                Not target Is [source] AndAlso _
                Not IsChildNode([source], target) And
        move_up.Enabled = True Then
                'ドロップされたNodeのコピーを作成
                Dim cln As TreeNode = CType([source].Clone(), TreeNode)
                'Nodeを追加
                If target.Index < [source].Index Then
                    target.Parent.Nodes.Insert(target.Index, cln)
                Else
                    target.Parent.Nodes.Insert(target.Index + 1, cln)
                End If
                If e.Effect = DragDropEffects.Move Then
                    [source].Remove()
                End If
                '追加されたNodeを選択
                tv.SelectedNode = cln
            Else
                e.Effect = DragDropEffects.None
            End If
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    '禁止状態
    Private Shared Function IsChildNode( _
            ByVal parentNode As TreeNode, _
            ByVal childNode As TreeNode) As Boolean
        If Not childNode.Parent Is parentNode.Parent Then
            Return True
        ElseIf childNode.Parent Is parentNode Then
            Return True 'IsChildNode(parentNode, childNode.Parent)
        Else
            Return False
        End If
    End Function

#End Region

    'パッドボタン
#Region "Joker code procedures"

    Private Sub button_list_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_list.DoubleClick

        ' For some reason, when clicking quickly on the checkbox list it will not fire 
        ' off the SelectedIndexChanged event  so this is used to capture any changes 
        ' when the user clicks on the control quickly.

        Dim x As Integer = 0
        Dim proceed As Boolean = False
        Dim j As New joker

        If cl_tb.Text.Trim.Length >= 21 Then ' If the code text box contains at least one code or more

            For x = 0 To 19  ' Check if any joker buttons were selected
                If button_list.GetItemChecked(x) = True Then
                    proceed = True
                    Exit For ' No need to continue since we know something is checked
                End If
            Next

        End If

        If proceed = True Then ' If a joker was selected, calculate the code
            j.add_joker()
        Else ' If not, remove any jokers if they exist
            'j.remove_joker()
        End If

    End Sub

    Private Sub inverse_chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles inverse_chk.CheckedChanged

        Dim x As Integer = 0
        Dim proceed As Boolean = False
        Dim j As New joker

        If cl_tb.Text.Trim.Length >= 21 Then ' If the code text box contains at least one code or more

            For x = 0 To 19  ' Check if any joker buttons were selected
                If button_list.GetItemChecked(x) = True Then
                    proceed = True
                End If
            Next

        End If

        If proceed = True Then ' If a joker was selected, calculate the code
            j.add_joker()
        Else ' If not, remove any jokers if they exist
            'j.remove_joker()
        End If

    End Sub

    Private Sub button_list_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles button_list.ItemCheck


        ' Restricts the amount of checked items to 3 since CWcheat 
        ' only supports a 3 button press combination for joker codes

        If button_list.CheckedItems.Count >= 3 Then

            e.NewValue = CheckState.Unchecked

        End If

    End Sub

#End Region

#Region "Window control"

    ' This makes sure the error list window always ends up below the main window
    Private Sub Main_locationchanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.LocationChanged

        If error_window.Visible = True Then

            Dim point As New Point
            point.X = Me.Location.X
            point.Y = Me.Location.Y + Me.Height
            error_window.Location = point

        End If

    End Sub

    Private Sub Main_resized(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        If error_window.Visible = True Then

            Dim point As New Point
            error_window.Width = Me.Width
            point.X = Me.Location.X
            point.Y = Me.Location.Y + Me.Height
            error_window.Location = point

        End If

    End Sub

#End Region

#Region "Hotkeys"

    Private Sub main_key_down(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown

        ' CTRL + V
        If e.Control = True AndAlso e.KeyCode = Keys.V Then
            'to do
        End If

        ' CTRL + C
        If e.Control = True AndAlso e.KeyCode = Keys.C Then
            'to do
        End If

    End Sub

#End Region

    'リスト連携
#Region "LISTVIEW"
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles USELIST.Click
        Dim f As New list
        Dim backup As String = cmt_tb.Text
        Dim line As Integer = 1
        Dim type As String = Nothing
        Dim bit As Integer = 1
        Dim lslen As Integer = 23
        Dim rmlen As Integer = 0
        Dim i As Integer = 0
        Dim z As Integer = 0
        Dim truelist As Boolean = True
        Dim b3 As String = cl_tb.Text
        Dim r As New System.Text.RegularExpressions.Regex("LIST/.+\.txt\([AV]B?,([1-9]|[1-9][0-9]),[1-8],[1-8]\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim m As System.Text.RegularExpressions.Match = r.Match(backup)
        Dim len As Integer = 20
        If PSX = True Then
            lslen = 15
            len = 13
        End If

        While m.Success
            Dim b1 As String = m.Value
            b1 = b1.Substring(b1.Length - 9, 9)
            If b1.Substring(0, 1) = "," Then
                b1 = b1.Remove(0, 1)
            End If
            i = 0
            Dim b2 As String() = b1.Split(CChar(","c))
            For Each s In b2
                Select Case i
                    Case 0
                        type = s.Substring(s.Length - 1, 1)
                        If type = "B" Then
                            type = s.Substring(s.Length - 2, 1)
                        End If
                    Case 1
                        s = s.Replace(",", "")
                        line = CType(s, Integer)
                    Case 2
                        s = s.Substring(0, 1)
                        bit = CType(s, Integer)
                    Case 3
                        s = s.Substring(0, 1)
                        rmlen = CType(s, Integer)
                End Select
                i += 1
            Next
            If type = "V" Then
                i = 11
                If PSX = True Then
                    i = 7
                End If
            Else
                i = 0
            End If

            m = m.NextMatch()
            z += 1
            i += (line - 1) * lslen + bit + 1
            If PSX = False Then
                If truelist = True AndAlso i + rmlen < b3.Length AndAlso rmlen + bit <= 9 Then
                    truelist = True
                Else
                    truelist = False
                End If
            ElseIf PSX = True Then
                If type = "A" AndAlso i + rmlen < b3.Length AndAlso rmlen + bit <= 9 Then
                    truelist = True
                ElseIf type = "V" AndAlso i + rmlen < b3.Length AndAlso rmlen + bit <= 5 Then
                    truelist = True
                Else
                    truelist = False
                End If
            End If
        End While

        If truelist = True And changed.Text <> "コード内容が変更されました。" And backup <> Nothing And z > 0 Then
            f.ShowDialog(Me)
            f.Dispose()
        ElseIf cl_tb.Text.Length < len Then
            changed.Text = "コード内容が空か文字数が足りません。"
        Else
            changed.Text = "リスト定義がないか,範囲が異常です。"
        End If
        cmt_tb.Text = backup
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SHIFLIST.Click
        Dim len As Integer = 20
        If PSX = True Then
            Exit Sub
        End If
        If cl_tb.Text.Length > len Then
            changed.Text = "簡易シフトが追加されました,行を合わせてください。"
            Dim z As String = "0"
            If cmt_tb.Text.Contains("840") Then
                z = "8"
            End If

            Dim tl As New textline
            Dim s As String = tl.linec(cl_tb.Text, cl_tb.SelectionStart).ToString
            cmt_tb.Text = "LIST/shift" & z & ".txt" & "(V," & s & ",6,3)シフト倍" & vbCrLf & cmt_tb.Text
        Else
            changed.Text = "コード内容が空か文字数が足りません。"
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SELECTLIST.Click

        Dim len As Integer = 20
        If PSX = True Then
            len = 12
        End If
        Dim ofd As New OpenFileDialog()
        Dim lspath As String = Nothing
        ofd.InitialDirectory = My.Application.Info.DirectoryPath.ToString() & "\LIST\"
        ofd.Filter = _
    "txtファイル(*.txt)|*.txt"
        ofd.Title = "追加するリストのTXTを選んでください"
        If cl_tb.Text.Length > len Then
            If ofd.ShowDialog() = DialogResult.OK Then
                changed.Text = "リストが追加されました,行を合わせてください。"
                lspath = ofd.FileName
                lspath = lspath.Replace(My.Application.Info.DirectoryPath.ToString(), "")
                lspath = lspath.Replace("\", "/")
                lspath = lspath.Remove(0, 1)
                Dim tl As New textline
                Dim s As String = tl.linec(cl_tb.Text, cl_tb.SelectionStart).ToString
                cmt_tb.Text = lspath & "(V," & s & ",1,8)" & vbCrLf & cmt_tb.Text
            End If
        Else
            changed.Text = "コード内容が空か文字数が足りません。"
        End If
    End Sub
#End Region

    '保存警告
#Region "ALERTTXT"
    Private Sub RadioButton4_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CWC.Click
        changed.Text = "コード形式が変更されました。"
    End Sub
    Private Sub RadioButton5_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PSPAR.Click
        changed.Text = "コード形式が変更されました。"
    End Sub

    Private Sub RadioButton6_clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMP.Click
        changed.Text = "コード形式が変更されました。"
    End Sub

    Private Sub GT_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GT_tb.KeyPress
        changed.Text = "タイトル/IDが変更されました。"
    End Sub

    Private Sub GID_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GID_tb.KeyPress
        changed.Text = "タイトル/IDが変更されました。"
    End Sub

    Private Sub CT_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CT_tb.KeyPress
        changed.Text = "コード名が変更されました。"
    End Sub

    Private Sub cl_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cl_tb.KeyPress
        changed.Text = "コード内容が変更されました。"
    End Sub

    Private Sub cmt_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmt_tb.KeyPress
        changed.Text = "コードコメントが変更されました。"
    End Sub

    Private Sub on_rd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles on_rd.Click
        changed.Text = "コード実行状態が変更されました。"
    End Sub

    Private Sub off_rd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles off_rd.Click
        changed.Text = "コード実行状態が変更されました。"
    End Sub
#End Region

#Region "SETTNG"
    'ぐっりど値えｄぃた
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DATAGRID.Click, dgedit.Click
        Dim f As New datagrid

        If TEMP.Checked = True Then
            f.edmode = "_N "
        ElseIf PSPAR.Checked = True Then
            f.edmode = "_M "
        ElseIf CWC.Checked = True Then
            f.edmode = "_L "
        End If

        f.ShowDialog(Me)
        f.Dispose()
    End Sub

    Private Sub クリップボードToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clipboad.Click
        Dim s As New save_db
        s.clipboad("CLIP")
    End Sub

    Private Sub CMF出力ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMFexport.Click
        Dim s As New save_db
        s.clipboad("CMF")
    End Sub

    Private Sub txt出力ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FCTXT.Click
        Dim s As New save_db
        s.clipboad("TXT")
    End Sub

    Private Sub SCM出力ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SCMexport.Click
        Dim s As New save_db
        s.clipboad("SCM")
    End Sub

    Private Sub TAB出力ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TABexport.Click
        Dim s As New save_db
        s.save_tab("TAB")
    End Sub

    Private Sub FTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTPDsetting.Click
        Dim f As New ftp
        f.ShowDialog()
    End Sub

    Private Function FTPMODE(ByVal MODE As String) As Boolean
        Dim s As New save_db
        Dim f As New ftp
        Dim id As String = ""
        Dim lv As Integer = codetree.SelectedNode.Level
        If lv > 0 Then
            If lv = 1 Then
                id = codetree.SelectedNode.Tag.ToString
            Else
                id = codetree.SelectedNode.Parent.Tag.ToString
            End If

            If MODE <> "TAB" Then
                s.clipboad(MODE)
            Else
                s.save_tab("TAB")
            End If
            f.SENDDB_PSPFTPD(Application.StartupPath & "\" & id & "." & MODE)
        End If
        Return True
    End Function

    Private Sub SCM転送ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ftpscm.Click
        FTPMODE("SCM")
    End Sub

    Private Sub TAB転送ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ftptab.Click
        FTPMODE("TAB")
    End Sub

    Private Sub 編集DBを転送ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ftpdb.Click
        Dim f As New ftp
        f.SENDDB_PSPFTPD(database)
    End Sub

    Private Sub CMF転送ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ftpcmf.Click
        FTPMODE("CMF")
    End Sub

    Private Sub gameid_dragendter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles GID_tb.DragEnter, PSF.DragEnter, GT_tb.DragEnter, cmt_tb.DragEnter, USELIST.DragEnter, SHIFLIST.DragEnter, SELECTLIST.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub get_filepath_filedropped(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles GID_tb.DragDrop, PSF.DragDrop, GT_tb.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する
        Dim fileName As String() = CType( _
            e.Data.GetData(DataFormats.FileDrop, False), _
            String())
        GETPSF(fileName(0))
    End Sub

    Private Sub get_listpath_filedropped(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles cmt_tb.DragDrop, USELIST.DragDrop, SHIFLIST.DragDrop, SELECTLIST.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する
        Dim fileName As String() = CType( _
            e.Data.GetData(DataFormats.FileDrop, False), _
            String())
        Dim sb As New StringBuilder
        Dim s As String() = {"LIST/", "", "(V,1,1,8)", ""}
        s(3) = Application.StartupPath & "\LIST\"
        For i = 0 To fileName.Length - 1
            If fileName(i).Contains(s(3)) Then
                s(1) = fileName(i).Replace(s(3), "")
                s(1) = s(1).Replace("\", "/")
                sb.Append(s(0))
                sb.Append(s(1))
                sb.AppendLine(s(2))
            Else
                sb.AppendLine("LISTディレクトリに入ってません")
            End If
        Next
        sb.Append(cmt_tb.Text)
        cmt_tb.Text = sb.ToString
    End Sub

    Private Sub PSF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PSF.Click

        Me.open_file.Filter = "PBP/ISOファイル(*.pbp;*iso)|*.pbp;*.iso"
        Me.open_file.Title = "PBP/ISOファイルを選んでください"

        If open_file.ShowDialog = Windows.Forms.DialogResult.OK And open_file.FileName <> Nothing Then

            GETPSF(open_file.FileName)

        End If
    End Sub

    Function GETPSF(ByVal fn As String) As Boolean

        Dim psf As New psf
        Dim str As String = psf.GETNAME(fn)
        Dim gid As String = psf.GETID(fn)

        If str <> "" AndAlso gid <> "" Then
            If str = "NOTILE" Then
                changed.Text = "PSFにTITLEがありません"
                GID_tb.Text = gid
            ElseIf str = "NULL" Then
                changed.Text = "PSFにTITLEはありますが空文字のようです"
                GID_tb.Text = gid
            ElseIf str = "UMDVIDEO" Then
                changed.Text = "UMDVIDEOイメージなので取得しませんでした"
            ElseIf str = "DAX" Then
                changed.Text = "Deflate圧縮イメージDAXは対応してません"
            ElseIf str = "JSO" Then
                changed.Text = "LZ0圧縮イメージJSOは対応してません"
            Else
                changed.Text = "ゲームタイトル/IDが変更されました"
                GT_tb.Text = str
                GID_tb.Text = gid
            End If
        Else
            changed.Text = "PBP/ISOではありません"
        End If
        Return True
    End Function

    Private Sub PBPHBHASH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PBPHBHASH.Click

        If PBPHBHASH.Checked = True Then
            My.Settings.hbhash = False
            PBPHBHASH.Checked = False
        Else
            My.Settings.hbhash = True
            PBPHBHASH.Checked = True
        End If
    End Sub

    Private Sub 同名でToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles samename.Click
        If samename.Checked = True Then
            samename.Checked = False
            releasedate.Checked = True
            My.Settings.updatemode = False
        Else
            releasedate.Checked = False
            samename.Checked = True
            My.Settings.updatemode = True
        End If
    End Sub

    Private Sub 別名で保存ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles releasedate.Click
        If releasedate.Checked = False Then
            samename.Checked = False
            releasedate.Checked = True
            My.Settings.updatemode = False
        Else
            releasedate.Checked = False
            samename.Checked = True
            My.Settings.updatemode = True
        End If

    End Sub

    Private Sub GOOGLE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GOOGLESVN.Click

        GOOGLESVN.Checked = Not GOOGLESVN.Checked
        GITHUB.Checked = Not GOOGLESVN.Checked
        My.Settings.updatesever = GOOGLESVN.Checked

    End Sub

    Private Sub GBKOP_Click(sender As System.Object, e As System.EventArgs) Handles GBKOP.Click

        GBKOP.Checked = Not GBKOP.Checked
        My.Settings.GBKOP = GBKOP.Checked

    End Sub

    Private Sub GITHUB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GITHUB.Click

        GITHUB.Checked = Not GITHUB.Checked
        GOOGLESVN.Checked = Not GITHUB.Checked
        My.Settings.updatesever = GITHUB.Checked

    End Sub

    Private Sub cl_tb_TextChanged_1(sender As System.Object, e As System.EventArgs) Handles cl_tb.MouseClick, cl_tb.Validated, cl_tb.TextChanged
        Dim tl As New textline
        curr_line.Text = tl.linec(cl_tb.Text, cl_tb.SelectionStart).ToString & "行目"
    End Sub

    Private Sub cl_tbTextChanged_1(sender As System.Object, e As KeyEventArgs) Handles cl_tb.KeyDown
        Dim tl As New textline
        Dim temp As Integer = cl_tb.SelectionStart
        Dim j = temp
        Dim k = cl_tb.Text.Length
        temp = tl.linec(cl_tb.Text, temp)
        If e.KeyData = Keys.Up AndAlso temp > 1 Then
            temp -= 1
        ElseIf e.KeyData = Keys.Down Then
            If j >= k Then
            Else
                temp += 1
            End If
        End If
        If e.KeyData = Keys.Left AndAlso temp > 1 AndAlso cl_tb.Text.Substring(cl_tb.SelectionStart - 2, 2) = vbCrLf Then
            temp -= 1
        ElseIf e.KeyData = Keys.Right AndAlso cl_tb.SelectionStart < cl_tb.Text.Length AndAlso cl_tb.Text.Substring(cl_tb.SelectionStart, 2) = vbCrLf Then
            temp += 1
        End If
        curr_line.Text = temp.ToString & "行目"
    End Sub

    Private Sub DBENCODE_Click(sender As System.Object, e As System.EventArgs)
        If My.Settings.saveencode = False Then
            DBENCODE.Checked = True
            My.Settings.saveencode = True
        Else
            DBENCODE.Checked = False
            My.Settings.saveencode = False
        End If

    End Sub

    Private Sub ENCTRING_Click(sender As System.Object, e As System.EventArgs)
        If My.Settings.savetype = False Then
            ENCTRING.Checked = False
            CPENC.Checked = True
            My.Settings.savetype = True
        Else
            ENCTRING.Checked = True
            CPENC.Checked = False
            My.Settings.savetype = False
        End If
    End Sub

    Private Sub cpstring_Click(sender As System.Object, e As System.EventArgs) Handles cpstring.Click
        If My.Settings.checkcpstr = False Then
            cpstring.Checked = True
            My.Settings.checkcpstr = True
        Else
            cpstring.Checked = False
            My.Settings.checkcpstr = False
        End If
    End Sub

    Private Sub CFEDIT_Click(sender As System.Object, e As System.EventArgs) Handles CFEDIT.Click
        CFEDIT.Checked = Not CFEDIT.Checked
        My.Settings.cfid = CFEDIT.Checked
    End Sub

    Private Sub ARBINhanzen_Click(sender As Object, e As EventArgs) Handles ARBINhanzen.Click
        ARBINhanzen.Checked = Not ARBINhanzen.Checked
        My.Settings.arbinhanzen = ARBINhanzen.Checked

    End Sub

    Private Sub ARCUT_Click(sender As Object, e As EventArgs) Handles ARCUT.Click

        ARCUT.Checked = Not ARCUT.Checked
        My.Settings.arbincut = ARCUT.Checked
    End Sub
#End Region

    '入力マスク
#Region "inputmask"
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GID_tb.KeyPress
        Dim mask As New Regex("[0-9A-Z\-\u0008]", RegexOptions.IgnoreCase)
        Dim ok As Match = mask.Match(e.KeyChar)
        If ok.Success = False Then
            e.Handled = True
        ElseIf GID_tb.SelectionStart <> 4 AndAlso e.KeyChar = "-"c Then
            e.Handled = True
        ElseIf GID_tb.SelectionStart >= 10 AndAlso (e.KeyChar < "0"c Or e.KeyChar > "9"c) AndAlso e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cl_tb.KeyPress
        Dim mk As String = "[0-9A-Fa-fx \u000D\u0008]"
        If PSX = True Then
            mk = "[0-9A-Fa-f \u000D\u0008]"
        End If
        Dim i As Integer = CInt(AscW(e.KeyChar))

        Dim mask As New Regex(mk, RegexOptions.ECMAScript)
        Dim ok As Match = mask.Match(e.KeyChar)

        If ok.Success = False Then
            e.Handled = True
        ElseIf e.KeyChar <> "x"c Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If
    End Sub
#End Region

End Class