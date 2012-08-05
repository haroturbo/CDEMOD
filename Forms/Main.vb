Imports System.IO       'Stream、StreamWriter、StreamReader、FileStream用
Imports System.Text     'Encoding用
Imports System.Diagnostics
Imports System.Collections
Imports System.Linq
Imports System.Net
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

#Region "ini"

    Private Sub main_Load(ByVal sender As Object, _
        ByVal e As EventArgs) Handles MyBase.Load
        'http://dobon.net/vb/dotnet/control/tvdraganddrop.html
        'TreeView1へのドラッグを受け入れる
        codetree.AllowDrop = True


        If My.Settings.fixedform = True Then
            Me.AutoSize = True
            fixedform.Checked = True
        Else
            Me.Width = My.Settings.mainyoko
            Me.Height = My.Settings.maintate

        End If

        If My.Settings.checkcpstr = True Then
            cpstring.Checked = True
        End If
        If My.Settings.autocp = True Then
            zenkakuwitherror.Checked = True
        End If

        If My.Settings.GBKOP = True Then
            GBKOP.Checked = True
        End If

        If My.Settings.cfid = True Then
            CFEDIT.Checked = True
        End If

        If My.Settings.saveencode = True Then
            DBENCODE.Checked = True
        End If

        If My.Settings.savetype = True Then
            CPENC.Checked = True
        Else
            ENCTRING.Checked = True
        End If

        If My.Settings.codepathwhensave = True Then
            update_save_filepass.Checked = True
        End If

        If My.Settings.updater = True Then
            Dim check As New checkupdate
            check.CDEupater("start")
            autoupdater.Checked = True
        Else
            autoupdater.Checked = False
        End If

        If My.Settings.hbhash = True Then
            PBPHBHASH.Checked = True
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

        If My.Settings.app8 <> "" Then
            APP8.Text = exename(My.Settings.app8)
            APP8custom.Text = APP8.Text
        End If
        If My.Settings.app9 <> "" Then
            APP9.Text = exename(My.Settings.app9)
            APP9custom.Text = APP9.Text
        End If
        If My.Settings.app10 <> "" Then
            APP10.Text = exename(My.Settings.app10)
            APP10custom.Text = APP10.Text
        End If

        If My.Settings.url8 <> "" Then
            URL8.Text = urltrim(My.Settings.url8)
            URL8custom.Text = URL8.Text
        End If
        If My.Settings.url9 <> "" Then
            URL9.Text = urltrim(My.Settings.url9)
            URL9custom.Text = URL9.Text
        End If
        If My.Settings.url10 <> "" Then
            URL10.Text = urltrim(My.Settings.url10)
            URL10custom.Text = URL8.Text
        End If

        For Each cmd As String In My.Application.CommandLineArgs
            My.Settings.lastcodepath = cmd
        Next

        If System.IO.File.Exists(My.Settings.lastcodepath) = True Then
            Dim open As New load_db
            database = My.Settings.lastcodepath
            PSX = open.check_db(database, 932) ' Check the file's format
            CODEFREAK = open.check2_db(database, 1201)
            DATEL = open.check3_db(database, 932)
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

            If codetree.Nodes IsNot Nothing Then
                If enc1 = 1201 Then
                    saveas_cwcheat.Enabled = True
                    saveas_psx.Enabled = False
                    saveas_codefreak.Enabled = True
                    UTF16BE.Enabled = True
                Else
                    If PSX = False Then
                        saveas_cwcheat.Enabled = True
                        saveas_psx.Enabled = False
                    Else
                        saveas_cwcheat.Enabled = False
                        saveas_psx.Enabled = True
                    End If
                    saveas_codefreak.Enabled = False
                    UTF16BE.Enabled = False
                End If

                If codetree.Nodes.Count >= 1 Then
                    codetree.Nodes(0).Expand()
                End If
                resets_level1()
                loaded = True
                file_saveas.Enabled = True
                overwrite_db.Enabled = True
                overwrite_db.ToolTipText = "対象;" & database
            End If

            error_window.list_load_error.EndUpdate()
            codetree.EndUpdate()
        Else
            codetree.Nodes.Add("NEW_DB").ImageIndex = 0
        End If

        reset_codepage()

        'イベントハンドラを追加する
        AddHandler codetree.ItemDrag, AddressOf codetree_ItemDrag
        AddHandler codetree.DragOver, AddressOf codetree_DragOver
        AddHandler codetree.DragDrop, AddressOf codetree_DragDrop

        CT_tb.Font = My.Settings.CT_tb
        GID_tb.Font = My.Settings.GID_tb
        GT_tb.Font = My.Settings.CT_tb
        cmt_tb.Font = My.Settings.cmt_tb
        cl_tb.Font = My.Settings.cl_tb
        codetree.Font = My.Settings.codetree

        GITHUB.Checked = My.Settings.updatesever
        GOOGLESVN.Checked = Not GITHUB.Checked

        If My.Settings.updatecomp = True Then
            MessageBox.Show("アップデートが完了しました", "アップデート完了")
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

            database = open_file.FileName

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
            My.Settings.lastcodepath = database
            overwrite_db.ToolTipText = "対象;" & database

        End If
    End Sub

    Private Sub overwrite_db_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles overwrite_db.Click
        Dim s As New save_db
        If My.Settings.lastcodepath <> "" Then

            If CODEFREAK = True Then
                s.save_cf(database, 1201)
            ElseIf DATEL = True Then
                s.save_ar(database, 932)
            ElseIf PSX = True Then
                s.save_psx(database, enc1)
            Else
                s.save_cwcheat(database, enc1)
            End If

            codetree.Nodes(0).Text = Path.GetFileNameWithoutExtension(database)
            If My.Settings.codepathwhensave = True Then
                My.Settings.lastcodepath = database
            End If

            DATEL = False
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

    Private Sub ACTONREPLAYToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_actionreplay.Click
        Dim open As New load_db
        Dim s As New save_db

        Me.save_file.Filter = "ACTIONREPLAY (*.bin)|*.bin"

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then

            database = save_file.FileName
            s.save_ar(database, 932)

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

    Private Sub codesite(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles codesite_browser.Click
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

    Private Sub nichsite(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nichannel_browser.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = "C:\Program Files"
        ofd.Filter = "EXEファイル(*.exe)|*.exe"
        ofd.Title = "2CHブラウザのEXEを選んでください"
        If ofd.ShowDialog() = DialogResult.OK Then
            'OKボタンがクリックされたとき
            '選択されたファイル名を表示する
            My.Settings.nichbrowser = ofd.FileName
        End If
    End Sub

    Private Sub URL8ToolStripMenuItem1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles URL8custom.Click
        Dim f As New url
        f.TextBox1.Text = My.Settings.url8
        f.ShowDialog(Me)
        My.Settings.url8 = f.TextBox1.Text
        URL8.Text = urltrim(f.TextBox1.Text)
        URL8custom.Text = urltrim(f.TextBox1.Text)
        f.Dispose()
    End Sub

    Private Sub URL9ToolStripMenuItem1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles URL9custom.Click
        Dim f As New url
        f.TextBox1.Text = My.Settings.url9
        f.ShowDialog(Me)
        My.Settings.url9 = f.TextBox1.Text
        URL9.Text = urltrim(f.TextBox1.Text)
        URL9custom.Text = urltrim(f.TextBox1.Text)
        f.Dispose()
    End Sub

    Private Sub URL10ToolStripMenuItem1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles URL10custom.Click
        Dim f As New url
        f.TextBox1.Text = My.Settings.url10
        f.ShowDialog(Me)
        My.Settings.url10 = f.TextBox1.Text
        URL10.Text = urltrim(f.TextBox1.Text)
        URL10custom.Text = urltrim(f.TextBox1.Text)
        f.Dispose()
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

    Private Sub APP8ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APP8custom.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = "C:\Program Files"
        ofd.Filter = _
    "EXEファイル(*.exe)|*.exe"
        ofd.Title = "EXEを選んでください"
        If ofd.ShowDialog() = DialogResult.OK Then
            'OKボタンがクリックされたとき
            '選択されたファイル名を表示する
            My.Settings.app8 = ofd.FileName
            APP8.Text = exename(ofd.FileName)
            APP8custom.Text = exename(ofd.FileName)
        End If
    End Sub
    Private Sub APP9ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APP9custom.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = "C:\Program Files"
        ofd.Filter = _
    "EXEファイル(*.exe)|*.exe"
        ofd.Title = "EXEを選んでください"
        If ofd.ShowDialog() = DialogResult.OK Then
            'OKボタンがクリックされたとき
            '選択されたファイル名を表示する
            My.Settings.app9 = ofd.FileName
            APP9.Text = exename(ofd.FileName)
            APP9custom.Text = exename(ofd.FileName)
        End If
    End Sub
    Private Sub APP10ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APP10custom.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = "C:\Program Files"
        ofd.Filter = _
    "EXEファイル(*.exe)|*.exe"
        ofd.Title = "EXEを選んでください"
        If ofd.ShowDialog() = DialogResult.OK Then
            'OKボタンがクリックされたとき
            '選択されたファイル名を表示する
            My.Settings.app10 = ofd.FileName
            APP10.Text = exename(ofd.FileName)
            APP10custom.Text = exename(ofd.FileName)
        End If
    End Sub

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
        cmt_tb.Text = Nothing
        f.ShowDialog(Me)
        Dim b1 As String = cmt_tb.Text
        Dim b2 As String() = b1.Split(CChar(vbLf))
        Dim gid As String = Nothing
        Dim gname As String = "(NULL)"
        Dim cname As String = "(NULL)"
        Dim code As String = Nothing
        Dim cname2 As String = Nothing
        Dim code2 As String = Nothing
        Dim coment As String = Nothing
        Dim add As Boolean = False
        Dim havegame As Boolean = False
        Dim nullcode As Boolean = False
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim z As Integer = 0
        Dim level2insert As Integer = 1
        Dim pos As Integer
        Dim parent As Integer
        Dim line As Integer = 0
        Dim erct As Integer = 0

        If codetree.Nodes.Count >= 1 And b1 <> Nothing Then
            codetree.BeginUpdate()

            Dim selnode1stlv As Integer = codetree.SelectedNode.Level
            If selnode1stlv = 2 Then
                pos = codetree.SelectedNode.Index
                parent = codetree.SelectedNode.Parent.Index
            End If

            For Each s As String In b2

                line += 1

                If s.Length >= 2 Then
                    If selnode1stlv = 0 AndAlso s.Substring(0, 2) = "_S" Then
                        If havegame = True AndAlso nullcode = False Then
                            add = True
                            i = 0
                        End If
                        s = s.PadRight(4)
                        gid = s.Substring(3, s.Length - 3).Trim
                    ElseIf selnode1stlv = 0 AndAlso s.Substring(0, 2) = "_G" Then
                        s = s.PadRight(4)
                        gname = s.Substring(3, s.Length - 3).Trim
                        Dim gnode = New TreeNode(gname)
                        With gnode
                            .Name = gname
                            .Tag = gid
                            .ImageIndex = 1
                        End With
                        codetree.Nodes(0).Nodes.Insert(k, gnode)
                        k += 1
                        codetree.SelectedNode = gnode
                        havegame = True
                        nullcode = True

                    ElseIf s.Substring(0, 2) = "_C" Then
                        nullcode = True
                        s = s.PadRight(3, "0"c)
                        If i = 0 Then
                            If s.Substring(2, 1) = "0" Then
                                code = "0" & vbCrLf
                            Else
                                code = "1" & vbCrLf
                            End If
                            cname = s.Substring(3, s.Length - 3).Trim
                        Else
                            add = True
                            If nullcode = True Then
                                code2 &= "0" & vbCrLf
                            End If
                            code = code & coment
                            If s.Substring(2, 1) = "0" Then
                                code2 = "0" & vbCrLf
                            Else
                                code2 = "1" & vbCrLf
                            End If
                            cname2 = s.Substring(3, s.Length - 3).Trim
                        End If
                        i += 1

                    ElseIf s.Substring(0, 2) = "_L" Or s.Substring(0, 2) = "_M" Or s.Substring(0, 2) = "_N" Then
                        nullcode = False
                        s = s.Replace(vbCr, "")
                        If PSX = True Then
                            s = s.PadRight(17, "0"c)
                            '_L 12345678 1234
                            If s.Substring(2, 1) = " " And s.Substring(11, 1) = " " Then
                                code &= s.Substring(3, 13).Trim & vbCrLf
                            End If
                        Else
                            s = s.PadRight(24, "0"c)
                            '_L 0x12345678 0x12345678
                            If s.Substring(3, 2) = "0x" And s.Substring(14, 2) = "0x" Then
                                If s.Substring(0, 2) = "_M" Then
                                    z = Integer.Parse(code.Substring(0, 1))
                                    code = code.Remove(0, 1)
                                    z = 2 Or z
                                    code = code.Insert(0, z.ToString())
                                ElseIf s.Substring(0, 2) = "_N" Then
                                    z = Integer.Parse(code.Substring(0, 1))
                                    code = code.Remove(0, 1)
                                    z = 4 Or z
                                    code = code.Insert(0, z.ToString())
                                End If
                                code &= s.Substring(3, 21).Trim & vbCrLf
                            End If

                        End If

                    ElseIf s.Substring(0, 1) = "#" Then

                        s = s.Replace("#", "")
                        coment &= "#" & s.Trim & vbCrLf

                    End If
                End If


                If add = True Then
                    Try
                        Dim newcode As New TreeNode

                        With newcode
                            .ImageIndex = 2
                            .SelectedImageIndex = 3
                            .Name = cname
                            .Text = cname
                            .Tag = code
                        End With

                        Select Case codetree.SelectedNode.Level

                            Case Is = 1
                                codetree.SelectedNode.Nodes.Add(newcode)
                            Case Is = 2
                                codetree.Nodes(0).Nodes(parent).Nodes.Insert(pos + level2insert, newcode)
                                level2insert += 1
                        End Select

                    Catch ex As Exception

                    End Try

                    code = code2
                    cname = cname2
                    coment = Nothing
                    add = False
                End If
            Next
            codetree.EndUpdate()

            file_saveas.Enabled = True
            overwrite_db.Enabled = True

        End If

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

    Public Function ConvANK(ByVal moto As String) As String
        '-- 半角カタカナ(Unicodeで\uFF61-\uFF9Fが範囲)を全角に --
        Dim re2 As Regex = New Regex("[\uFF61-\uFF9F]+")
        Dim output2 As String = re2.Replace(moto, AddressOf myReplacer2)
        Return output2
    End Function

    Shared Function myReplacer2(ByVal m As Match) As String
        Return Strings.StrConv(m.Value, VbStrConv.Wide, 0)
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
    Private Sub wikiToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wikiToolStripMenuItem1.Click
        Process.Start(browser, "http://www21.atwiki.jp/cwcwiki/")
    End Sub

    Private Sub OHGToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OHGToolStripMenuItem.Click
        Process.Start(browser, "http://www.onehitgamer.com/forum/")
    End Sub

    Private Sub HAXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HAXToolStripMenuItem.Click
        Process.Start(browser, "http://haxcommunity.org/")
    End Sub

    Private Sub CNGBAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CNGBAToolStripMenuItem.Click
        Process.Start(browser, "http://www.cngba.com/forum-988-1.html")
    End Sub

    Private Sub GOOGLEToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GOOGLEToolStripMenuItem.Click
        Process.Start(browser, "http://www.google.co.jp/")
    End Sub

    Private Sub CMF暗号復元ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmf_decript.Click
        Process.Start(browser, "http://raing3.gshi.org/psp-utilities/#index.php?action=cmf-decrypter")
    End Sub

    Private Sub cwcToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cwcToolStripMenuItem1.Click
        Process.Start(browser, "http://www.myconsole.it/143-cwcheat-official-support-forum/98-english-support-board/")
    End Sub

    Private Sub url8ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles URL8.Click
        Process.Start(browser, My.Settings.url8)
    End Sub
    Private Sub url9ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles URL9.Click
        Process.Start(browser, My.Settings.url9)
    End Sub
    Private Sub url10ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles URL10.Click
        Process.Start(browser, My.Settings.url10)
    End Sub

    Private Sub CDEMODToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Process.Start(browser, "http://unzu127xp.pa.land.to/data/IJIRO/CDEMOD/bin/Release/index.html")
    End Sub

#End Region

#Region "EXECUTE"
    Private Sub KAKASI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KAKASI.Click, cntkakasi.Click
        boot("APP\kakasi.bat")
    End Sub

    Private Sub MECAB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MECABk.Click
        boot("APP\kanahenkan.bat")
    End Sub

    Private Sub PMETAN変換ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pme_cnv.Click
        boot("APP\pme.bat")
    End Sub

    Private Sub TEMPAR鶴ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles temparutility.Click
        boot("APP\temp.bat")
    End Sub

    Private Sub WgetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Wget.Click
        boot("APP\wget.bat")
    End Sub

    Private Sub JaneStyleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nichanbrowser.Click
        boot(My.Settings.nichbrowser)
    End Sub

    Private Sub PSPへコードコピーToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles copy_to_psp.Click, cntdbcopy.Click
        boot("APP\cp.bat")
    End Sub

    Private Sub 登録なし8ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APP8.Click
        boot(My.Settings.app8)
    End Sub
    Private Sub 登録なし9ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APP9.Click
        boot(My.Settings.app9)
    End Sub
    Private Sub 登録なし10ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APP10.Click
        boot(My.Settings.app10)
    End Sub

    Function boot(ByVal exe As String) As Boolean

        If exe = "" Then
            MessageBox.Show("アプリケーションが登録されてません。", "アプリ未登録")
            Return False
        ElseIf Not exe.Contains(":") AndAlso Not exe.Contains(Application.StartupPath) _
            AndAlso exe.Contains("APP\") AndAlso exe.Contains(".bat") Then
            exe = Application.StartupPath & "\" & exe
        End If

        If System.IO.File.Exists(exe) Then
            Process.Start(exe)
        Else
            MessageBox.Show("'" + exe + "'が見つかりませんでした。")
        End If

        Return True
    End Function

#End Region

#Region "HELP"

    Private Sub オンラインヘルプToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles オンラインヘルプToolStripMenuItem.Click
        System.Diagnostics.Process.Start(browser, "http://unzu127xp.pa.land.to/data/CDE.html")
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
            With newgame
                .Name = "新規ゲーム"
                .Text = "新規ゲーム"
                .ImageIndex = 1
                .Tag = "0000-00000"
            End With
            codetree.Nodes(0).Nodes.Insert(0, newgame)
            codetree.SelectedNode = newgame
            GT_tb.Enabled = True
            GT_tb.Text = "新規ゲーム"

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

            With newcode
                .ImageIndex = 2
                .SelectedImageIndex = 3
                .Name = "新規コード"
                .Text = "新規コード"
                .Tag = "0"
            End With

            Select Case codetree.SelectedNode.Level

                Case Is = 1

                    off_rd.Checked = True
                    CT_tb.Enabled = True
                    CT_tb.Text = "新規コード"
                    cmt_tb.Enabled = True
                    cl_tb.Enabled = True
                    codetree.SelectedNode.Nodes.Insert(0, newcode)
                    codetree.SelectedNode = newcode
                Case Is = 2

                    off_rd.Checked = True
                    CT_tb.Enabled = True
                    CT_tb.Text = "新規コード"
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
            GID_tb.Text = System.Text.RegularExpressions.Regex.Replace(GID_tb.Text, "[^\-0-9A-Za-z]", "0").ToUpper
            Select Case codetree.SelectedNode.Level

                Case Is = 1
                    With codetree.SelectedNode
                        .Name = GT_tb.Text
                        .Text = GT_tb.Text
                        .Tag = GID_tb.Text
                    End With

                Case Is = 2
                    With codetree.SelectedNode.Parent
                        .Name = GT_tb.Text
                        .Text = GT_tb.Text
                        .Tag = GID_tb.Text
                    End With
            End Select

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
                Dim r As New System.Text.RegularExpressions.Regex( _
        "[0-9a-fA-F]{8} [0-9a-zA-Z?]{4}", _
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                Dim m As System.Text.RegularExpressions.Match = r.Match(b1)

                While m.Success
                    buffer &= (m.Value) & vbCrLf
                    cl_tb.Text &= (m.Value) & vbCrLf
                    m = m.NextMatch()
                End While
            Else
                b1 = b1.Replace("_L ", "")
                Dim r As New System.Text.RegularExpressions.Regex( _
        "0x........ 0x........", _
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                Dim m As System.Text.RegularExpressions.Match = r.Match(b1)

                While m.Success
                    buffer &= (m.Value) & vbCrLf
                    cl_tb.Text &= (m.Value) & vbCrLf
                    m = m.NextMatch()
                End While

                '        b1 = cl_tb.Text.Replace("_L ", "")
                '        b1 = System.Text.RegularExpressions.Regex.Replace( _
                '            b1, "_C.+\n", vbCrLf)
                '        b1 = System.Text.RegularExpressions.Regex.Replace( _
                '        b1, "[!-/;-@\u005B-`\u007B-\uFFFF].+\n", vbCrLf)
                buffer = System.Text.RegularExpressions.Regex.Replace( _
        buffer, "[g-zG-Z]", "A")
                buffer = buffer.ToUpper
                buffer = System.Text.RegularExpressions.Regex.Replace( _
        buffer, "^0A", "0x")
                buffer = System.Text.RegularExpressions.Regex.Replace( _
        buffer, "(\r|\n)0A", vbCrLf & "0x")
                buffer = buffer.Replace(" 0A", " 0x")
                '        b1 = System.Text.RegularExpressions.Regex.Replace( _
                'b1, "[!-/;-@\u005B-`\u007B-\uFFFF].+[^0-9A-F]$", "")
                '        Dim b2 As String() = b1.Split(CChar(vbCrLf))
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
                    MessageBox.Show(id & "," & psf.GETNAME(fileName(0)) & vbCrLf & "はすでにIDが登録されてます", "ID重複")
                End If
                Exit Sub
            End If

            Dim open As New load_db
            If codetree.Nodes IsNot Nothing AndAlso MessageBox.Show("ドロップされたデータベースを開くと現在のデータベースが消えてしまいます。このまま開いてもよろしいですか？", _
                                                                    "データベース保存の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
            Me.AutoSize = False
            database = fileName(0)

            error_window.list_save_error.Items.Clear() 'Clear any save errors from a previous database
            PSX = open.check_db(database, 932) ' Check the file's format
            CODEFREAK = open.check2_db(database, 1201)
            DATEL = open.check3_db(database, 932)
            codetree.Nodes.Clear()
            codetree.BeginUpdate()

            error_window.list_load_error.BeginUpdate()

            UTF16BE.Enabled = False
            saveas_codefreak.Enabled = False

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
                saveas_actionreplay.Enabled = True
            ElseIf PSX = True Then
                enc1 = open.check_enc(database)
                reset_PSX()
                Application.DoEvents()
                open.read_PSX(database, enc1)
                PSX = True
            ElseIf open.no_db(database, enc1) = False Then
                enc1 = open.check_enc(database)
                reset_PSP()
                Application.DoEvents()
                open.read_PSP(database, enc1)
            End If
            If codetree.Nodes.Count >= 1 Then
                codetree.Nodes(0).Expand()
            End If
            If enc1 = 1201 Then
                UTF16BE.Enabled = True
                saveas_codefreak.Enabled = True
            End If
            resets_level1()
            codetree.EndUpdate()
            reset_codepage()
            error_window.list_load_error.EndUpdate()
            loaded = True
            file_saveas.Enabled = True
            overwrite_db.Enabled = True
            My.Settings.lastcodepath = database
            overwrite_db.ToolTipText = "対象;" & database


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
        Dim r As New System.Text.RegularExpressions.Regex( _
    "LIST/.+\.txt\((A|V),([1-9]|[1-9][0-9]),[1-8],[1-8]\)", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
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

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTPDsetting.Click
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
                'ElseIf str = "CSO" Then
                '    'changed.Text = "Deflate圧縮イメージCSOは対応してません"
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

    Private Sub DBENCODE_Click(sender As System.Object, e As System.EventArgs) Handles DBENCODE.Click
        If My.Settings.saveencode = False Then
            DBENCODE.Checked = True
            My.Settings.saveencode = True
        Else
            DBENCODE.Checked = False
            My.Settings.saveencode = False
        End If

    End Sub

    Private Sub ENCTRING_Click(sender As System.Object, e As System.EventArgs) Handles ENCTRING.Click, CPENC.Click
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

    Private Sub zenkakuwitherror_Click(sender As System.Object, e As System.EventArgs) Handles zenkakuwitherror.Click

        If My.Settings.autocp = False Then
            zenkakuwitherror.Checked = True
            My.Settings.autocp = True
        Else
            zenkakuwitherror.Checked = False
            My.Settings.autocp = False
        End If
    End Sub

    Private Sub CFEDIT_Click(sender As System.Object, e As System.EventArgs) Handles CFEDIT.Click
        CFEDIT.Checked = Not CFEDIT.Checked
        My.Settings.cfid = CFEDIT.Checked
    End Sub
End Class