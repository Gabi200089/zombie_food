Public Class Form1
    Dim pausetrue As Integer = 0 '0=沒暫停
    Dim t(24) As Integer 'timer陣列
    Dim win As Integer = 0
    Dim i, y, c(5) As Integer
    Dim Fblood(19) As Label
    Dim FDtag(19) As Integer
    Dim FD(11) As Integer
    Dim bang(5) As Integer '殭屍有沒有撞到
    Dim nownum(19) As Integer '紀錄現在第幾個食物生成
    Dim k As Integer 'nownum的第幾格
    Dim yesno As Integer 'msgbox選項
    Dim j As Integer '可隨意用
    Dim x As Integer '第幾隻殭屍
    Dim m As Integer = 0 '第幾個money
    Dim time As Integer = 0
    Dim heart As Integer = 3 '生命
    Dim die(5) As Integer '已死掉
    Dim number As Integer '第幾隻殭屍要死掉
    Dim zblood(5) As Integer '殭屍血量
    Dim zbloodLB(5) As Label '殭屍血量label
    Dim pic(19) As PictureBox '選擇放置位子的框框
    Dim zombie(5) As PictureBox '殭屍
    Dim bullet(19) As PictureBox '子彈
    Dim newfood(19) As PictureBox '食物砲塔
    Dim bulletTimer(19) As Timer '控制子彈的timer
    Dim foodchoose As Integer '選上方列表第幾個食物角色
    Dim money(100) As PictureBox '隨機生成的錢
    Dim moneytime As Integer '錢生成時間
    Dim moneyX, moneyY As Integer '錢的XY座標
    Dim attack As Integer '食物攻擊1,2,3
    Dim soundvolume As Integer = 50 '初始音量大小
    Dim QAYN As Integer '是否有機智問答
    Dim zw(5) As Integer
    Dim ze(5) As Integer
    Dim z(5) As Timer '放殭屍變換圖片的timer
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        moneyLB.Text = 50
        forever.Enabled = True

        AxWindowsMediaPlayer1.settings.volume = soundvolume
        AxWindowsMediaPlayer1.settings.autoStart = True
        AxWindowsMediaPlayer1.settings.setMode("Loop", True)
        AxWindowsMediaPlayer1.URL = Application.StartupPath & "\sounds\gameplay1.mp3"

        Randomize()
        i = 0

        z(0) = Z1
        z(1) = Z2
        z(2) = Z3
        z(3) = Z4
        z(4) = Z5
        z(5) = Z6

        For j = 0 To 5
            zw(j) = 0
            c(j) = 0
        Next
        For j = 0 To 19
            pic(j) = CType(Me.Controls.Find("PictureBox" & CStr(j + 1), False)(0), PictureBox)
        Next

#Region "pic tag"
        pic(0).Tag = 15
        pic(1).Tag = 14
        pic(2).Tag = 13
        pic(3).Tag = 12
        pic(4).Tag = 11
        pic(5).Tag = 25
        pic(6).Tag = 24
        pic(7).Tag = 23
        pic(8).Tag = 22
        pic(9).Tag = 21
        pic(10).Tag = 35
        pic(11).Tag = 34
        pic(12).Tag = 33
        pic(13).Tag = 32
        pic(14).Tag = 31
        pic(15).Tag = 45
        pic(16).Tag = 44
        pic(17).Tag = 43
        pic(18).Tag = 42
        pic(19).Tag = 41
#End Region
        For j = 0 To 5
            zblood(j) = 12
        Next

        For j = 0 To 5
            bang(j) = 0 '0=沒撞到
        Next
        For j = 0 To 5
            FD(j) = 0
        Next

        For j = 0 To 5
            zombie(j) = New PictureBox
            zombie(j).Size = New Size(70, 80)
            zombie(j).BackColor = BackColor.Transparent
            zombie(j).Left = 800
            zombie(j).SizeMode = PictureBoxSizeMode.StretchImage
            z(j).Enabled = True
            'zombie(j).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇.gif"))
            ' zombie(j).Image = My.Resources.香菇
            ' zombie(j).BackColor = BackColor.DarkRed
            Me.Controls.Add(zombie(j))

            zbloodLB(j) = New Label
            zbloodLB(j).Left = 800
            zbloodLB(j).BackColor = BackColor.Transparent
            Me.Controls.Add(zbloodLB(j))
            zbloodLB(j).BringToFront()
            zombie(j).BringToFront()
            zombie(j).Tag = 0
            If j = 0 Then
                zombie(j).Top = 250
                zbloodLB(j).Top = 235
            ElseIf j = 1 Then
                zombie(j).Top = 472
                zbloodLB(j).Top = 457
            ElseIf j = 2 Then
                zombie(j).Top = 123
                zbloodLB(j).Top = 141
            ElseIf j = 3 Then
                zombie(j).Top = 357
                zbloodLB(j).Top = 342
            ElseIf j = 4 Then
                zombie(j).Top = 141
                zbloodLB(j).Top = 123
            ElseIf j = 5 Then
                zombie(j).Top = 357
                zbloodLB(j).Top = 342
            End If
            zbloodLB(j).Text = zblood(j)
        Next

        moneytime = Int(Rnd() * 3 + 3) '3~5秒
        moneyTimer.Interval = 1000 * moneytime
        moneyTimer.Enabled = True

        ''heartLB.Text = heart
        ZBappear.Enabled = True

    End Sub

#Region "選擇角色"
    Private Sub food_mouseclick(sender As Object, e As EventArgs) Handles food3.MouseClick, food1.MouseClick, food2.MouseClick

        Dim Target As PictureBox = CType(sender, PictureBox)
        food3.Enabled = False
        food1.Enabled = False
        food2.Enabled = False

        If Target.Equals(food3) Then
            moneyLB.Text -= 150
            foodchoose = 3
        ElseIf Target.Equals(food1) Then
            foodchoose = 1
            moneyLB.Text -= 50
        ElseIf Target.Equals(food2) Then
            moneyLB.Text -= 100
            foodchoose = 2
        End If

        'tag>4 表示且位子有放食物不能選
        For j = 0 To 19
            If pic(j).Tag Mod 10 <= 5 Then
                pic(j).BorderStyle = BorderStyle.FixedSingle
                pic(j).Visible = True
                pic(j).Enabled = True
            End If
        Next

        QAYN = Int(Rnd() * 2) '0為不產生 1為產生 QA
        If QAYN = 1 Then
            allstop()
            rndQA()
        End If
    End Sub
#End Region
#Region "選擇格子位置"
    Private Sub PictureBox_MouseClick(sender As System.Object, e As MouseEventArgs) Handles PictureBox1.MouseClick, PictureBox2.MouseClick, PictureBox3.MouseClick, PictureBox4.MouseClick, PictureBox5.MouseClick, PictureBox6.MouseClick, PictureBox7.MouseClick, PictureBox8.MouseClick, PictureBox9.MouseClick, PictureBox10.MouseClick, PictureBox11.MouseClick, PictureBox12.MouseClick, PictureBox13.MouseClick, PictureBox14.MouseClick, PictureBox15.MouseClick, PictureBox16.MouseClick, PictureBox17.MouseClick, PictureBox18.MouseClick, PictureBox19.MouseClick, PictureBox20.MouseClick
        Dim Target As PictureBox = CType(sender, PictureBox)

        Target.Tag = Target.Tag + 5 'tag=5=第一排且位子有放食物,tag=6=第二排且位子有放食物,tag=7=第三排且位子有放食物,tag=8=第四排且位子有放食物


        '動態生成食物砲塔
        newfood(i) = New PictureBox
        newfood(i).Width = 70
        newfood(i).Height = 80
        newfood(i).Left = Target.Left
        newfood(i).Top = Target.Top


        newfood(i).SizeMode = PictureBoxSizeMode.StretchImage
        newfood(i).Visible = True

        '把選擇框框關閉
        For j = 0 To 19
            pic(j).Visible = False
            pic(j).Enabled = False
        Next
        newfood(i).SendToBack()
        Me.Controls.Add(newfood(i))
        newfood(i).BackColor = Color.Transparent

        'If Target.Tag = 5 Then
        'newfood(i).Tag = 1
        'ElseIf Target.Tag = 6 Then
        '    newfood(i).Tag = 2
        'ElseIf Target.Tag = 7 Then
        '    newfood(i).Tag = 3
        'ElseIf Target.Tag = 8 Then
        '    newfood(i).Tag = 4
        'End If


        '動態生成子彈
        bullet(i) = New PictureBox
        bullet(i).SizeMode = PictureBoxSizeMode.StretchImage
        bullet(i).Left = newfood(i).Left + newfood(i).Width
        bullet(i).Top = newfood(i).Top + 30
        ' bullet(i).Image = My.Resources.bullet_1
        bullet(i).BringToFront()
        Me.Controls.Add(bullet(i))
        bullet(i).Tag = newfood(i).Tag
        bullet(i).BackColor = Color.Transparent
        Fblood(i) = New Label
        Fblood(i).Visible = False
        Me.Controls.Add(Fblood(i))
        Fblood(i).Text = 6


        If foodchoose = 1 Then
            newfood(i).Image = My.Resources.薯條
            bullet(i).Image = My.Resources.射薯條
            bullet(i).Width = 24
            bullet(i).Height = 25
        ElseIf foodchoose = 2 Then
            newfood(i).Image = My.Resources.可樂
            bullet(i).Image = My.Resources.bullet_2
            bullet(i).Width = 25
            bullet(i).Height = 25
        ElseIf foodchoose = 3 Then
            newfood(i).Image = My.Resources.漢堡
            bullet(i).Image = My.Resources.射黃瓜
            bullet(i).Width = 26
            bullet(i).Height = 25
        End If
        'If foodchoose = 1 Then
        '    bullet(i).Tag = 1 '攻擊力1
        'ElseIf foodchoose = 2 Then
        '    bullet(i).Tag = 2 '攻擊力2
        'ElseIf foodchoose = 3 Then
        '    bullet(i).Tag = 3 '攻擊力3
        'End If
#Region "食物的tag"
        If Target.Equals(pic(0)) Then
            newfood(i).Tag = 15
            bullet(i).Tag = 15
            Fblood(i).Tag = 15
        End If
        If Target.Equals(pic(1)) Then
            newfood(i).Tag = 14
            bullet(i).Tag = 14
            Fblood(i).Tag = 14
        End If
        If Target.Equals(pic(2)) Then
            newfood(i).Tag = 13
            bullet(i).Tag = 13
            Fblood(i).Tag = 13
        End If
        If Target.Equals(pic(3)) Then
            newfood(i).Tag = 12
            bullet(i).Tag = 12
            Fblood(i).Tag = 12
        End If
        If Target.Equals(pic(4)) Then
            newfood(i).Tag = 11
            bullet(i).Tag = 11
            Fblood(i).Tag = 11
        End If
        If Target.Equals(pic(5)) Then
            newfood(i).Tag = 25
            bullet(i).Tag = 25
            Fblood(i).Tag = 25
        End If
        If Target.Equals(pic(6)) Then
            newfood(i).Tag = 24
            bullet(i).Tag = 24
            Fblood(i).Tag = 24
        End If
        If Target.Equals(pic(7)) Then
            newfood(i).Tag = 23
            bullet(i).Tag = 23
            Fblood(i).Tag = 23
        End If
        If Target.Equals(pic(8)) Then
            newfood(i).Tag = 22
            bullet(i).Tag = 22
            Fblood(i).Tag = 22
        End If
        If Target.Equals(pic(9)) Then
            newfood(i).Tag = 21
            bullet(i).Tag = 21
            Fblood(i).Tag = 21
        End If
        If Target.Equals(pic(10)) Then
            newfood(i).Tag = 35
            bullet(i).Tag = 35
            Fblood(i).Tag = 35
        End If
        If Target.Equals(pic(11)) Then
            newfood(i).Tag = 34
            bullet(i).Tag = 34
            Fblood(i).Tag = 34
        End If
        If Target.Equals(pic(12)) Then
            newfood(i).Tag = 33
            bullet(i).Tag = 33
            Fblood(i).Tag = 33
        End If
        If Target.Equals(pic(13)) Then
            newfood(i).Tag = 32
            bullet(i).Tag = 32
            Fblood(i).Tag = 32
        End If
        If Target.Equals(pic(14)) Then
            newfood(i).Tag = 31
            bullet(i).Tag = 31
            Fblood(i).Tag = 31
        End If
        If Target.Equals(pic(15)) Then
            newfood(i).Tag = 45
            bullet(i).Tag = 45
            Fblood(i).Tag = 45
        End If
        If Target.Equals(pic(16)) Then
            newfood(i).Tag = 44
            bullet(i).Tag = 44
            Fblood(i).Tag = 44
        End If
        If Target.Equals(pic(17)) Then
            newfood(i).Tag = 43
            bullet(i).Tag = 43
            Fblood(i).Tag = 43
        End If
        If Target.Equals(pic(18)) Then
            newfood(i).Tag = 42
            bullet(i).Tag = 42
            Fblood(i).Tag = 42
        End If
        If Target.Equals(pic(19)) Then
            newfood(i).Tag = 41
            bullet(i).Tag = 41
            Fblood(i).Tag = 41
        End If
#End Region

        nownum(i) = i
        i = i + 1 '往下一格食物陣列放

        food3.Enabled = True
        food1.Enabled = True
        food2.Enabled = True

        '一放食物砲塔之後子彈timer開始
        If i = 1 Then
            Timer1.Enabled = True
        End If

    End Sub
    Function PZ(A As PictureBox, B As PictureBox) '碰撞模型
        Dim r As Boolean
        r = False
        If (A.Left + A.Width > B.Left + 30 And A.Left < B.Left + B.Width) _
           And (A.Top + A.Height > B.Top And A.Top < B.Top + B.Height) And A.Tag Mod 10 <> B.Tag Then

            r = True
        End If

        PZ = r

    End Function
#End Region
#Region "子彈前進且碰邊框回位"
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        For k = 0 To i - 1
            If bullet(nownum(k)).Left < 800 And bullet(nownum(k)).Visible = True Then
                bullet(nownum(k)).Left = bullet(nownum(k)).Left + 2
            ElseIf bullet(nownum(k)).Left >= 800 And Fblood(nownum(k)).Text <> 0 Then
                bullet(nownum(k)).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width - 10
                bullet(nownum(k)).Top = newfood(nownum(k)).Top + 30
            End If
        Next

    End Sub
#End Region
#Region "殭屍前進且碰食物暫停"
    Private Sub ZW1_Tick(sender As Object, e As EventArgs) Handles ZW1.Tick
        For k = 0 To i - 1 ' Step -1
            If (die(0) = 0 And newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 21 And zombie(0).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 22 And zombie(0).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 23 And zombie(0).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 24 And zombie(0).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 25 And zombie(0).Tag < 5) Then
                zombie(0).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(0).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                Z1.Enabled = False
                zombie(0).Image = My.Resources.香菇吃
                If FD(0) <> 0 Then
                    FDtag(FD(0)) = newfood(nownum(k)).Tag
                Else
                    FDtag(FD(6)) = newfood(nownum(k)).Tag
                End If
                bang(0) = 1 '殭屍撞到
                Z1eat.Enabled = True
            ElseIf bang(0) = 0 And zombie(0).Left > 0 Then '沒撞過的就是<>7
                If zombie(0).Left < pic(9).Left + pic(9).Width + 30 And pic(9).Tag = 21 And zombie(0).Tag < 1 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(8).Left + pic(8).Width + 30 And pic(8).Tag = 22 And zombie(0).Tag < 2 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(7).Left + pic(7).Width + 30 And pic(7).Tag = 23 And zombie(0).Tag < 3 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(6).Left + pic(6).Width + 30 And pic(6).Tag = 24 And zombie(0).Tag < 4 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(5).Left + pic(5).Width + 30 And pic(5).Tag = 25 And zombie(0).Tag < 5 Then
                    zombie(0).Tag += 1
                End If

                If zw(0) = 0 Then
                    zombie(0).Left = zombie(0).Left - 3
                    zbloodLB(0).Left = zbloodLB(0).Left - 3
                End If
                zw(0) = 1
                If k = i - 1 Then
                    zw(0) = 0
                End If
                If zombie(0).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    ''heartLB.Text = heart
                    zombie(0).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                        'If yesno = 6 Then
                        '    Dim frm1 = New Form1
                        '    frm1.Show()
                        '    Me.Close()
                        'ElseIf yesno = 7 Then
                        '第一頁.Show()
                        選單.Show()
                        Me.Close()
                        ' End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(0).Left > 0 Then '解決k第一次不能跑的問題
            zombie(0).Left = zombie(0).Left - 3
            zbloodLB(0).Left = zbloodLB(0).Left - 3
            If zombie(0).Left < pic(9).Left + pic(9).Width + 30 And pic(9).Tag = 21 And zombie(0).Tag < 1 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(8).Left + pic(8).Width + 30 And pic(8).Tag = 22 And zombie(0).Tag < 2 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(7).Left + pic(7).Width + 30 And pic(7).Tag = 23 And zombie(0).Tag < 3 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(6).Left + pic(6).Width + 30 And pic(6).Tag = 24 And zombie(0).Tag < 4 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(5).Left + pic(5).Width + 30 And pic(5).Tag = 25 And zombie(0).Tag < 5 Then
                zombie(0).Tag += 1
            End If
            zombie(0).Left = zombie(0).Left - 3
            zbloodLB(0).Left = zbloodLB(0).Left - 3

            If zombie(0).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                ''heartLB.Text = heart
                zombie(0).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                    'If yesno = 6 Then
                    '    Dim frm1 = New Form1
                    '    frm1.Show()
                    '    Me.Close()
                    'ElseIf yesno = 7 Then
                    '第一頁.Show()
                    選單.Show()
                    Me.Close()
                    ' End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW2_Tick(sender As Object, e As EventArgs) Handles ZW2.Tick
        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 41 And zombie(1).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 42 And zombie(1).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 43 And zombie(1).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 44 And zombie(0).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 45 And zombie(0).Tag < 5) Then
                zombie(1).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(1).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                Z2.Enabled = False
                zombie(1).Image = My.Resources.香菇吃
                If FD(1) <> 0 Then
                    FDtag(FD(1)) = newfood(nownum(k)).Tag
                Else
                    FDtag(FD(7)) = newfood(nownum(k)).Tag
                End If
                Z2eat.Enabled = True
                bang(1) = 1 '殭屍撞到
            ElseIf bang(1) = 0 And zombie(1).Left > 0 Then '沒撞過的就是<>7
                If zombie(1).Left < pic(19).Left + pic(19).Width + 30 And pic(19).Tag = 41 And zombie(1).Tag < 1 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(18).Left + pic(18).Width + 30 And pic(18).Tag = 42 And zombie(1).Tag < 2 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(17).Left + pic(17).Width + 30 And pic(17).Tag = 43 And zombie(1).Tag < 3 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(16).Left + pic(16).Width + 30 And pic(16).Tag = 44 And zombie(1).Tag < 4 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(15).Left + pic(15).Width + 30 And pic(15).Tag = 45 And zombie(1).Tag < 5 Then
                    zombie(1).Tag += 1
                End If


                If zw(1) = 0 Then
                    zombie(1).Left = zombie(1).Left - 3
                    zbloodLB(1).Left = zbloodLB(1).Left - 3
                End If
                zw(1) = 1
                If k = i - 1 Then
                    zw(1) = 0
                End If


                If zombie(1).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    ''heartLB.Text = heart
                    zombie(1).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                        'If yesno = 6 Then
                        '    Dim frm1 = New Form1
                        '    frm1.Show()
                        '    Me.Close()
                        'ElseIf yesno = 7 Then
                        '第一頁.Show()
                        選單.Show()
                        Me.Close()
                        ' End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(1).Left > 0 Then '解決k第一次不能跑的問題
            zombie(1).Left = zombie(1).Left - 3
            zbloodLB(1).Left = zbloodLB(1).Left - 3
            If zombie(1).Left < pic(19).Left + pic(19).Width + 30 And pic(19).Tag = 41 And zombie(1).Tag < 1 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(18).Left + pic(18).Width + 30 And pic(18).Tag = 42 And zombie(1).Tag < 2 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(17).Left + pic(17).Width + 30 And pic(17).Tag = 43 And zombie(1).Tag < 3 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(16).Left + pic(16).Width + 30 And pic(16).Tag = 44 And zombie(1).Tag < 4 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(15).Left + pic(15).Width + 30 And pic(15).Tag = 45 And zombie(1).Tag < 5 Then
                zombie(1).Tag += 1
            End If
            zombie(1).Left = zombie(1).Left - 3
            zbloodLB(1).Left = zbloodLB(1).Left - 3

            If zombie(1).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                ''heartLB.Text = heart
                zombie(1).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                    'If yesno = 6 Then
                    '    Dim frm1 = New Form1
                    '    frm1.Show()
                    '    Me.Close()
                    'ElseIf yesno = 7 Then
                    '第一頁.Show()
                    選單.Show()
                    Me.Close()
                    ' End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW3_Tick(sender As Object, e As EventArgs) Handles ZW3.Tick
        ZW3.Dispose()
        ZW3.Start()
        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 11 And zombie(2).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 12 And zombie(2).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 13 And zombie(2).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 14 And zombie(2).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 15 And zombie(2).Tag < 5) Then
                zombie(2).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(2).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                Z3.Enabled = False
                zombie(2).Image = My.Resources.香菇吃
                If FD(2) <> 0 Then
                    FDtag(FD(2)) = newfood(nownum(k)).Tag
                Else
                    FDtag(FD(8)) = newfood(nownum(k)).Tag
                End If
                Z3eat.Enabled = True
                bang(2) = 1 '殭屍撞到
            ElseIf bang(2) = 0 And zombie(2).Left > 0 Then '沒撞過的就是<>7
                If zombie(2).Left < pic(4).Left + pic(4).Width + 30 And pic(4).Tag = 11 And zombie(2).Tag < 1 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(3).Left + pic(3).Width + 30 And pic(3).Tag = 12 And zombie(2).Tag < 2 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(2).Left + pic(2).Width + 30 And pic(2).Tag = 13 And zombie(2).Tag < 3 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(1).Left + pic(1).Width + 30 And pic(1).Tag = 14 And zombie(2).Tag < 4 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(0).Left + pic(0).Width + 30 And pic(0).Tag = 15 And zombie(2).Tag < 5 Then
                    zombie(2).Tag += 1
                End If

                If zw(2) = 0 Then
                    zombie(2).Left = zombie(2).Left - 3
                    zbloodLB(2).Left = zbloodLB(2).Left - 3
                End If
                zw(2) = 1
                If k = i - 1 Then
                    zw(2) = 0
                End If

                If zombie(2).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    ''heartLB.Text = heart
                    zombie(2).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                        'If yesno = 6 Then
                        '    Dim frm1 = New Form1
                        '    frm1.Show()
                        '    Me.Close()
                        'ElseIf yesno = 7 Then
                        '第一頁.Show()
                        選單.Show()
                        Me.Close()
                        ' End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(2).Left > 0 Then '解決k第一次不能跑的問題
            zombie(2).Left = zombie(2).Left - 3
            zbloodLB(2).Left = zbloodLB(2).Left - 3
            If zombie(2).Left < pic(4).Left + pic(4).Width + 30 And pic(4).Tag = 11 And zombie(2).Tag < 1 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(3).Left + pic(3).Width + 30 And pic(3).Tag = 12 And zombie(2).Tag < 2 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(2).Left + pic(2).Width + 30 And pic(2).Tag = 13 And zombie(2).Tag < 3 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(1).Left + pic(1).Width + 30 And pic(1).Tag = 14 And zombie(2).Tag < 4 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(0).Left + pic(0).Width + 30 And pic(0).Tag = 15 And zombie(2).Tag < 5 Then
                zombie(2).Tag += 1
            End If
            zombie(2).Left = zombie(2).Left - 3
            zbloodLB(2).Left = zbloodLB(2).Left - 3

            If zombie(2).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                ''heartLB.Text = heart
                zombie(2).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                    'If yesno = 6 Then
                    '    Dim frm1 = New Form1
                    '    frm1.Show()
                    '    Me.Close()
                    'ElseIf yesno = 7 Then
                    '第一頁.Show()
                    選單.Show()
                    Me.Close()
                    ' End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW4_Tick(sender As Object, e As EventArgs) Handles ZW4.Tick
        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 31 And zombie(3).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 32 And zombie(3).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 33 And zombie(3).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 34 And zombie(3).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 35 And zombie(3).Tag < 5) Then
                zombie(3).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(3).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                Z4.Enabled = False
                zombie(3).Image = My.Resources.香菇吃
                If FD(3) <> 0 Then
                    FDtag(FD(3)) = newfood(nownum(k)).Tag
                Else
                    FDtag(FD(9)) = newfood(nownum(k)).Tag
                End If
                Z4eat.Enabled = True
                bang(3) = 1 '殭屍撞到
            ElseIf bang(3) = 0 And zombie(3).Left > 0 Then '沒撞過的就是<>7
                If zombie(3).Left < pic(14).Left + pic(14).Width + 30 And pic(14).Tag = 31 And zombie(3).Tag < 1 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(13).Left + pic(13).Width + 30 And pic(13).Tag = 32 And zombie(3).Tag < 2 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(12).Left + pic(12).Width + 30 And pic(12).Tag = 33 And zombie(3).Tag < 3 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(11).Left + pic(11).Width + 30 And pic(11).Tag = 34 And zombie(3).Tag < 4 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(10).Left + pic(10).Width + 30 And pic(10).Tag = 35 And zombie(3).Tag < 5 Then
                    zombie(3).Tag += 1
                End If

                If zw(3) = 0 Then
                    zombie(3).Left = zombie(3).Left - 3
                    zbloodLB(3).Left = zbloodLB(3).Left - 3
                End If
                zw(3) = 1
                If k = i - 1 Then
                    zw(3) = 0
                End If

                If zombie(3).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    ''heartLB.Text = heart
                    zombie(3).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                        'If yesno = 6 Then
                        '    Dim frm1 = New Form1
                        '    frm1.Show()
                        '    Me.Close()
                        'ElseIf yesno = 7 Then
                        '第一頁.Show()
                        選單.Show()
                        Me.Close()
                        ' End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(3).Left > 0 Then '解決k第一次不能跑的問題
            If zombie(3).Left < pic(14).Left + pic(14).Width + 30 And pic(14).Tag = 31 And zombie(3).Tag < 1 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(13).Left + pic(13).Width + 30 And pic(13).Tag = 32 And zombie(3).Tag < 2 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(12).Left + pic(12).Width + 30 And pic(12).Tag = 33 And zombie(3).Tag < 3 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(11).Left + pic(11).Width + 30 And pic(11).Tag = 34 And zombie(3).Tag < 4 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(10).Left + pic(10).Width + 30 And pic(10).Tag = 35 And zombie(3).Tag < 5 Then
                zombie(3).Tag += 1
            End If
            zombie(3).Left = zombie(3).Left - 3
            zbloodLB(3).Left = zbloodLB(3).Left - 3


            If zombie(3).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                ''heartLB.Text = heart
                zombie(3).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                    'If yesno = 6 Then
                    '    Dim frm1 = New Form1
                    '    frm1.Show()
                    '    Me.Close()
                    'ElseIf yesno = 7 Then
                    '第一頁.Show()
                    選單.Show()
                    Me.Close()
                    ' End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW5_Tick(sender As Object, e As EventArgs) Handles ZW5.Tick
        ZW5.Dispose()
        ZW5.Start()
        For k = 0 To i - 1 ' Step -1
            If (die(4) = 0 And newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(4).Left And newfood(nownum(k)).Tag = 11 And zombie(4).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(4).Left And newfood(nownum(k)).Tag = 12 And zombie(4).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(4).Left And newfood(nownum(k)).Tag = 13 And zombie(4).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(4).Left And newfood(nownum(k)).Tag = 14 And zombie(4).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(4).Left And newfood(nownum(k)).Tag = 15 And zombie(4).Tag < 5) Then
                zombie(4).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(4).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                Z5.Enabled = False
                zombie(4).Image = My.Resources.香菇吃
                If FD(4) <> 0 Then
                    FDtag(FD(4)) = newfood(nownum(k)).Tag
                Else
                    FDtag(FD(10)) = newfood(nownum(k)).Tag
                End If
                Z5eat.Enabled = True
                bang(4) = 1 '殭屍撞到
            ElseIf bang(4) = 0 And zombie(4).Left > 0 Then '沒撞過的就是<>7
                If zombie(4).Left < pic(4).Left + pic(4).Width + 30 And pic(4).Tag = 11 And zombie(4).Tag < 1 Then
                    zombie(4).Tag += 1
                End If
                If zombie(4).Left < pic(3).Left + pic(3).Width + 30 And pic(3).Tag = 12 And zombie(4).Tag < 2 Then
                    zombie(4).Tag += 1
                End If
                If zombie(4).Left < pic(2).Left + pic(2).Width + 30 And pic(2).Tag = 13 And zombie(4).Tag < 3 Then
                    zombie(4).Tag += 1
                End If
                If zombie(4).Left < pic(1).Left + pic(1).Width + 30 And pic(1).Tag = 14 And zombie(4).Tag < 4 Then
                    zombie(4).Tag += 1
                End If
                If zombie(4).Left < pic(0).Left + pic(0).Width + 30 And pic(0).Tag = 15 And zombie(4).Tag < 5 Then
                    zombie(4).Tag += 1
                End If

                If zw(4) = 0 Then
                    zombie(4).Left = zombie(4).Left - 3
                    zbloodLB(4).Left = zbloodLB(4).Left - 3
                End If
                zw(4) = 1
                If k = i - 1 Then
                    zw(4) = 0
                End If

                If zombie(4).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    'heartLB.Text = heart
                    zombie(4).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                        'If yesno = 6 Then
                        '    Dim frm1 = New Form1
                        '    frm1.Show()
                        '    Me.Close()
                        'ElseIf yesno = 7 Then
                        '第一頁.Show()
                        選單.Show()
                        Me.Close()
                        ' End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(4).Left > 0 Then '解決k第一次不能跑的問題
            zombie(4).Left = zombie(4).Left - 3
            zbloodLB(4).Left = zbloodLB(4).Left - 3
            If zombie(4).Left < pic(4).Left + pic(4).Width + 30 And pic(4).Tag = 11 And zombie(4).Tag < 1 Then
                zombie(4).Tag += 1
            End If
            If zombie(4).Left < pic(3).Left + pic(3).Width + 30 And pic(3).Tag = 12 And zombie(4).Tag < 2 Then
                zombie(4).Tag += 1
            End If
            If zombie(4).Left < pic(2).Left + pic(2).Width + 30 And pic(2).Tag = 13 And zombie(4).Tag < 3 Then
                zombie(4).Tag += 1
            End If
            If zombie(4).Left < pic(1).Left + pic(1).Width + 30 And pic(1).Tag = 14 And zombie(4).Tag < 4 Then
                zombie(4).Tag += 1
            End If
            If zombie(4).Left < pic(0).Left + pic(0).Width + 30 And pic(0).Tag = 15 And zombie(4).Tag < 5 Then
                zombie(4).Tag += 1
            End If
            zombie(4).Left = zombie(4).Left - 3
            zbloodLB(4).Left = zbloodLB(4).Left - 3

            If zombie(4).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                ''heartLB.Text = heart
                zombie(4).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                    'If yesno = 6 Then
                    '    Dim frm1 = New Form1
                    '    frm1.Show()
                    '    Me.Close()
                    'ElseIf yesno = 7 Then
                    '第一頁.Show()
                    選單.Show()
                    Me.Close()
                    ' End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW6_Tick(sender As Object, e As EventArgs) Handles ZW6.Tick
        ZW6.Dispose()
        ZW6.Start()
        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(5).Left And newfood(nownum(k)).Tag = 31 And zombie(5).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(5).Left And newfood(nownum(k)).Tag = 32 And zombie(5).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(5).Left And newfood(nownum(k)).Tag = 33 And zombie(5).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(5).Left And newfood(nownum(k)).Tag = 34 And zombie(5).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(5).Left And newfood(nownum(k)).Tag = 35 And zombie(5).Tag < 5) Then
                zombie(5).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(5).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                Z6.Enabled = False
                zombie(5).Image = My.Resources.香菇吃
                If FD(5) <> 0 Then
                    FDtag(FD(5)) = newfood(nownum(k)).Tag
                Else
                    FDtag(FD(11)) = newfood(nownum(k)).Tag
                End If
                Z6eat.Enabled = True
                bang(5) = 1 '殭屍撞到
            ElseIf bang(5) = 0 And zombie(5).Left > 0 Then '沒撞過的就是<>7
                If zombie(5).Left < pic(14).Left + pic(14).Width + 30 And pic(14).Tag = 31 And zombie(5).Tag < 1 Then
                    zombie(5).Tag += 1
                End If
                If zombie(5).Left < pic(13).Left + pic(13).Width + 30 And pic(13).Tag = 32 And zombie(5).Tag < 2 Then
                    zombie(5).Tag += 1
                End If
                If zombie(5).Left < pic(12).Left + pic(12).Width + 30 And pic(12).Tag = 33 And zombie(5).Tag < 3 Then
                    zombie(5).Tag += 1
                End If
                If zombie(5).Left < pic(11).Left + pic(11).Width + 30 And pic(11).Tag = 34 And zombie(5).Tag < 4 Then
                    zombie(5).Tag += 1
                End If
                If zombie(5).Left < pic(10).Left + pic(10).Width + 30 And pic(10).Tag = 35 And zombie(5).Tag < 5 Then
                    zombie(5).Tag += 1
                End If

                If zw(5) = 0 Then
                    zombie(5).Left = zombie(5).Left - 3
                    zbloodLB(5).Left = zbloodLB(5).Left - 3
                End If
                zw(5) = 1
                If k = i - 1 Then
                    zw(5) = 0
                End If

                If zombie(5).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    ''heartLB.Text = heart
                    zombie(5).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                        'If yesno = 6 Then
                        '    Dim frm1 = New Form1
                        '    frm1.Show()
                        '    Me.Close()
                        'ElseIf yesno = 7 Then
                        '第一頁.Show()
                        選單.Show()
                        Me.Close()
                        ' End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(5).Left > 0 Then '解決k第一次不能跑的問題
            If zombie(5).Left < pic(14).Left + pic(14).Width + 30 And pic(14).Tag = 31 And zombie(5).Tag < 1 Then
                zombie(5).Tag += 1
            End If
            If zombie(5).Left < pic(13).Left + pic(13).Width + 30 And pic(13).Tag = 32 And zombie(5).Tag < 2 Then
                zombie(5).Tag += 1
            End If
            If zombie(5).Left < pic(12).Left + pic(12).Width + 30 And pic(12).Tag = 33 And zombie(5).Tag < 3 Then
                zombie(5).Tag += 1
            End If
            If zombie(5).Left < pic(11).Left + pic(11).Width + 30 And pic(11).Tag = 34 And zombie(5).Tag < 4 Then
                zombie(5).Tag += 1
            End If
            If zombie(5).Left < pic(10).Left + pic(10).Width + 30 And pic(10).Tag = 35 And zombie(5).Tag < 5 Then
                zombie(5).Tag += 1
            End If
            zombie(5).Left = zombie(5).Left - 3
            zbloodLB(5).Left = zbloodLB(5).Left - 3


            If zombie(5).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                ''heartLB.Text = heart
                zombie(5).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!請回主選單!", , "訊息")
                    'If yesno = 6 Then
                    '    Dim frm1 = New Form1
                    '    frm1.Show()
                    '    Me.Close()
                    'ElseIf yesno = 7 Then
                    '第一頁.Show()
                    選單.Show()
                    Me.Close()
                    ' End If
                End If
            End If
        End If

    End Sub

#End Region
#Region "確認子彈和殭屍的碰撞"
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        For k = 0 To i - 1
            For x = 0 To 5
                ' If zombie(x).Tag <= 10 Then
                If PZ(bullet(nownum(k)), zombie(x)) Then
                    If bullet(nownum(k)).Width = 24 Then
                        attack = 1
                    ElseIf bullet(nownum(k)).Width = 25 Then
                        attack = 2
                    ElseIf bullet(nownum(k)).Width = 26 Then
                        attack = 3
                    End If
                    bullet(nownum(k)).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width
                    'Timer4.Enabled = True
                    If x = 0 Then
                        If zblood(0) >= 1 Then
                            If (zombie(0).Tag < 1 And bullet(nownum(k)).Tag = 21) Or (zombie(0).Tag < 2 And bullet(nownum(k)).Tag = 22) Or (zombie(0).Tag < 3 And bullet(nownum(k)).Tag = 23) Or (zombie(0).Tag < 4 And bullet(nownum(k)).Tag = 24) Or (zombie(0).Tag < 5 And bullet(nownum(k)).Tag = 25) Then
                                zblood(0) = zblood(0) - attack '減血
                                zbloodLB(0).Text = zblood(0)
                            End If
                        ElseIf zblood(0) < 1 And die(0) <> 1 Then
                            Z1.Enabled = False
                            zombie(0).Image = My.Resources.香菇死 '死掉的樣子
                            Z1eat.Enabled = False
                            die(0) = 1
                            ZD1.Enabled = True '等1秒消失
                        End If

                    ElseIf x = 1 Then
                        If zblood(1) >= 1 Then
                            If (zombie(1).Tag < 1 And bullet(nownum(k)).Tag = 41) Or (zombie(1).Tag < 2 And bullet(nownum(k)).Tag = 42) Or (zombie(1).Tag < 3 And bullet(nownum(k)).Tag = 43) Or (zombie(1).Tag < 4 And bullet(nownum(k)).Tag = 44) Or (zombie(1).Tag < 5 And bullet(nownum(k)).Tag = 45) Then
                                zblood(1) = zblood(1) - attack
                                zbloodLB(1).Text = zblood(1)
                            End If
                        ElseIf zblood(1) < 1 And die(1) <> 1 Then
                            Z2.Enabled = False
                            zombie(1).Image = My.Resources.香菇死
                            Z2eat.Enabled = False
                            die(1) = 1
                            ZD2.Enabled = True
                        End If

                    ElseIf x = 2 Then
                        If zblood(2) >= 1 Then
                            If (zombie(2).Tag < 1 And bullet(nownum(k)).Tag = 11) Or (zombie(2).Tag < 2 And bullet(nownum(k)).Tag = 12) Or (zombie(2).Tag < 3 And bullet(nownum(k)).Tag = 13) Or (zombie(2).Tag < 4 And bullet(nownum(k)).Tag = 14) Or (zombie(2).Tag < 5 And bullet(nownum(k)).Tag = 15) Then
                                zblood(2) = zblood(2) - attack
                                zbloodLB(2).Text = zblood(2)
                            End If
                        ElseIf zblood(2) < 1 And die(2) <> 1 Then
                            Z3.Enabled = False
                            zombie(2).Image = My.Resources.香菇死
                            Z3eat.Enabled = False
                            die(2) = 1
                            ZD3.Enabled = True
                        End If

                    ElseIf x = 3 Then

                        If zblood(3) >= 1 Then
                            If (zombie(3).Tag < 1 And bullet(nownum(k)).Tag = 31) Or (zombie(3).Tag < 2 And bullet(nownum(k)).Tag = 32) Or (zombie(3).Tag < 3 And bullet(nownum(k)).Tag = 33) Or (zombie(3).Tag < 4 And bullet(nownum(k)).Tag = 34) Or (zombie(3).Tag < 5 And bullet(nownum(k)).Tag = 35) Then
                                zblood(3) = zblood(3) - attack
                                zbloodLB(3).Text = zblood(3)
                            End If
                        ElseIf zblood(3) < 1 And die(3) <> 1 Then
                            Z4.Enabled = False
                            zombie(3).Image = My.Resources.香菇死
                            Z4eat.Enabled = False
                            die(3) = 1
                            ZD4.Enabled = True
                        End If
                    ElseIf x = 4 Then

                        If zblood(4) >= 1 Then
                            If (zombie(4).Tag < 1 And bullet(nownum(k)).Tag = 11) Or (zombie(4).Tag < 2 And bullet(nownum(k)).Tag = 12) Or (zombie(4).Tag < 3 And bullet(nownum(k)).Tag = 13) Or (zombie(4).Tag < 4 And bullet(nownum(k)).Tag = 14) Or (zombie(4).Tag < 5 And bullet(nownum(k)).Tag = 15) Then
                                zblood(4) = zblood(4) - attack
                            End If
                        ElseIf zblood(4) < 1 And die(4) <> 1 Then
                            Z5.Enabled = False
                            zombie(4).Image = My.Resources.香菇死
                            Z5eat.Enabled = False
                            die(4) = 1
                            ZD5.Enabled = True
                        End If
                    ElseIf x = 5 Then

                        If zblood(5) >= 1 Then
                            If (zombie(5).Tag < 1 And bullet(nownum(k)).Tag = 31) Or (zombie(5).Tag < 2 And bullet(nownum(k)).Tag = 32) Or (zombie(5).Tag < 3 And bullet(nownum(k)).Tag = 33) Or (zombie(5).Tag < 4 And bullet(nownum(k)).Tag = 34) Or (zombie(5).Tag < 5 And bullet(nownum(k)).Tag = 35) Then
                                zblood(5) = zblood(5) - attack
                                zbloodLB(5).Text = zblood(5)
                            End If
                        ElseIf zblood(5) < 1 And die(5) <> 1 Then
                            Z6.Enabled = False
                            zombie(5).Image = My.Resources.香菇死
                            Z6eat.Enabled = False
                            die(5) = 1
                            ZD6.Enabled = True
                        End If
                    End If
                End If
            Next
        Next
    End Sub
#End Region
#Region "等一秒消失Timer"
    Private Sub ZD1_Tick(sender As Object, e As EventArgs) Handles ZD1.Tick '第一隻殭屍消失
        ZW1.Enabled = False
        'zombie(0).Size = New Size(0, 0)
        zombie(0).Left = 801
        zbloodLB(0).Visible = False
        ZD1.Enabled = False
        Z1eat.Enabled = False
    End Sub
    Private Sub ZD2_Tick(sender As Object, e As EventArgs) Handles ZD2.Tick '第二隻殭屍消失
        ZW2.Enabled = False
        'zombie(1).Size = New Size(0, 0)
        zombie(1).Left = 801
        zbloodLB(1).Visible = False
        ZD2.Enabled = False
        Z2eat.Enabled = False
    End Sub
    Private Sub ZD3_Tick(sender As Object, e As EventArgs) Handles ZD3.Tick '第三隻殭屍消失
        zombie(2).Size = New Size(0, 0)
        zbloodLB(2).Visible = False
        zombie(2).Left = 801
        ZD3.Enabled = False
        Z3eat.Enabled = False
        ZW3.Enabled = False
    End Sub
    Private Sub ZD4_Tick(sender As Object, e As EventArgs) Handles ZD4.Tick '第四隻殭屍消失
        'zombie(3).Size = New Size(0, 0)
        zbloodLB(3).Visible = False
        zombie(3).Left = 801
        ZD4.Enabled = False
        Z4eat.Enabled = False
        ZW4.Enabled = False
    End Sub
    Private Sub ZD5_Tick(sender As Object, e As EventArgs) Handles ZD5.Tick '第一隻殭屍消失
        ZW5.Enabled = False
        'zombie(4).Size = New Size(0, 0)
        zombie(4).Left = 801
        zbloodLB(4).Visible = False
        ZD5.Enabled = False
        Z5eat.Enabled = False
    End Sub
    Private Sub ZD6_Tick(sender As Object, e As EventArgs) Handles ZD6.Tick '第一隻殭屍消失
        ZW6.Enabled = False
        'zombie(5).Size = New Size(0, 0)
        zombie(5).Left = 801
        zbloodLB(5).Visible = False
        ZD6.Enabled = False
        Z6eat.Enabled = False
    End Sub
#End Region
    Private Sub ZBappear_Tick(sender As Object, e As EventArgs) Handles ZBappear.Tick
        time = time + 1
        If time = 1 Then
            ZW1.Enabled = True
        End If
        If time = 8 Then
            ZW2.Enabled = True
        End If
        If time = 11 Then
            ZW3.Enabled = True
        End If
        If time = 15 Then
            ZW4.Enabled = True
        End If
        If time = 26 Then
            ZW5.Enabled = True
        End If
        If time = 22 Then
            ZW6.Enabled = True
        End If

    End Sub

    Private Sub moneydis_Tick(sender As Object, e As EventArgs) Handles moneydis.Tick
        money(m - 1).Visible = False
        moneydis.Enabled = False
    End Sub

    Private Sub forever_Tick(sender As Object, e As EventArgs) Handles forever.Tick
        Label1.Text = zombie(1).Tag
        '判斷愛心圖片
        ' Label1.Text = zombie(0).Tag
        If heart = 2 Then
            heart3.Visible = False
        ElseIf heart = 1 Then
            heart2.Visible = False
        ElseIf heart = 0 Then
            heart1.Visible = False
        End If

        'Label2.Text = zombie(0).Tag
        Select Case CInt(moneyLB.Text)
            Case Is < 50
                food3.Enabled = False
                food1.Enabled = False
                food2.Enabled = False
                food3.Image = My.Resources.漢堡灰
                food1.Image = My.Resources.薯條灰
                food2.Image = My.Resources.可樂灰
            Case 50 To 99
                food3.Enabled = False
                food1.Enabled = True
                food2.Enabled = False
                food3.Image = My.Resources.漢堡灰
                food1.Image = My.Resources.薯條正面
                food2.Image = My.Resources.可樂灰
            Case 100 To 149
                food3.Enabled = False
                food1.Enabled = True
                food2.Enabled = True
                food3.Image = My.Resources.漢堡灰
                food1.Image = My.Resources.薯條正面
                food2.Image = My.Resources.可樂正面
            Case Is >= 150
                food3.Enabled = True
                food1.Enabled = True
                food2.Enabled = True
                food3.Image = My.Resources.漢堡1
                food1.Image = My.Resources.薯條正面
                food2.Image = My.Resources.可樂正面
        End Select

        '判斷是否有贏
        If win = 0 And heart > 0 And ((zombie(0).Left < 1 Or zombie(0).Left > 800) And (zombie(1).Left < 1 Or zombie(1).Left > 800) And (zombie(2).Left < 1 Or zombie(2).Left > 800) And (zombie(3).Left < 1 Or zombie(3).Left > 800) And (zombie(4).Left < 1 Or zombie(4).Left > 800) And (zombie(5).Left < 1 Or zombie(5).Left > 800)) Or (zblood(0) <= 0 And zblood(1) <= 0 And zblood(2) <= 0 And zblood(3) <= 0 And zblood(4) <= 0 And zblood(5) <= 0) Then
            win = 1
            forever.Enabled = False
            moneyTimer.Enabled = False
            x = MsgBox("要繼續下一關嗎?(若選擇「否」則會回到主選單)", 4, "你贏了!!!")
            'msg已出現過
            If x = 6 Then
                第二關.Show()
                Me.Close()
            ElseIf x = 7 Then
                選單.Show()
                Me.Close()
            End If
        End If
        'If zblood(0) = 0 And die(0) <> 1 Then
        '    zombie(0).Image = My.Resources.香菇死 '死掉的樣子
        '    Z1eat.Enabled = FalseFD
        '    die(0) = 1
        '    ZD1.Enabled = True '等1秒消失
        'End If
    End Sub
#Region "食物血量的Timer"
    Private Sub Z1eat_Tick(sender As Object, e As EventArgs) Handles Z1eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(0)) Or Fblood(y).Tag = FDtag(FD(6)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(0)) Or newfood(nownum(j)).Tag = FDtag(FD(6)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            'bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = True
                            Z1.Enabled = True
                            'zombie(0).Image = My.Resources.香菇
                            bang(0) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    If newfood(nownum(j)).Tag = FDtag(FD(0)) Then
                                        FD(0) = FD(0) + 1
                                    Else
                                        FD(6) = FD(6) + 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    If ze(0) = 0 Then
                        Fblood(y).Text = Fblood(y).Text - 1
                    End If
                    ze(0) = 1
                    If y = i - 1 Then
                        ze(0) = 0
                    End If

                End If
            End If
        Next

    End Sub
    Private Sub Z2eat_Tick(sender As Object, e As EventArgs) Handles Z2eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(1)) Or Fblood(y).Tag = FDtag(FD(7)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(1)) Or newfood(nownum(j)).Tag = FDtag(FD(7)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = False
                            Z2.Enabled = True
                            'zombie(1).Image = My.Resources.香菇
                            bang(1) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    If Fblood(y).Tag = FDtag(FD(1)) Then
                                        FD(1) = FD(1) + 1
                                    Else
                                        FD(7) = FD(7) + 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    If ze(1) = 0 Then
                        Fblood(y).Text = Fblood(y).Text - 1
                    End If
                    ze(1) = 1
                    If y = i - 1 Then
                        ze(1) = 0
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub Z3eat_Tick(sender As Object, e As EventArgs) Handles Z3eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(2)) Or Fblood(y).Tag = FDtag(FD(8)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(2)) Or newfood(nownum(j)).Tag = FDtag(FD(8)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = False
                            Z3.Enabled = True
                            'zombie(2).Image = My.Resources.香菇
                            bang(2) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    If Fblood(y).Tag = FDtag(FD(2)) Then
                                        FD(2) = FD(2) + 1
                                    Else
                                        FD(8) = FD(8) + 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    If ze(2) = 0 Then
                        Fblood(y).Text = Fblood(y).Text - 1
                    End If
                    ze(2) = 1
                    If y = i - 1 Then
                        ze(2) = 0
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub Z4eat_Tick(sender As Object, e As EventArgs) Handles Z4eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(3)) Or Fblood(y).Tag = FDtag(FD(9)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(3)) Or newfood(nownum(j)).Tag = FDtag(FD(9)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = False
                            Z4.Enabled = True
                            'zombie(3).Image = My.Resources.香菇
                            bang(3) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    If Fblood(y).Tag = FDtag(FD(3)) Then
                                        FD(3) = FD(3) + 1
                                    Else
                                        FD(9) = FD(9) + 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    If ze(3) = 0 Then
                        Fblood(y).Text = Fblood(y).Text - 1
                    End If
                    ze(3) = 1
                    If y = i - 1 Then
                        ze(3) = 0
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub Z5eat_Tick(sender As Object, e As EventArgs) Handles Z5eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(4)) Or Fblood(y).Tag = FDtag(FD(10)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(4)) Or newfood(nownum(j)).Tag = FDtag(FD(10)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            'bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = True
                            Z5.Enabled = True
                            'zombie(4).Image = My.Resources.香菇
                            bang(4) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    If Fblood(y).Tag = FDtag(FD(4)) Then
                                        FD(4) = FD(4) + 1
                                    Else
                                        FD(10) = FD(10) + 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    If ze(4) = 0 Then
                        Fblood(y).Text = Fblood(y).Text - 1
                    End If
                    ze(4) = 1
                    If y = i - 1 Then
                        ze(4) = 0
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub Z6eat_Tick(sender As Object, e As EventArgs) Handles Z6eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(5)) Or Fblood(y).Tag = FDtag(FD(11)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(5)) Or newfood(nownum(j)).Tag = FDtag(FD(11)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            'bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = True
                            Z6.Enabled = True
                            'zombie(5).Image = My.Resources.香菇
                            bang(5) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    If Fblood(y).Tag = FDtag(FD(5)) Then
                                        FD(5) = FD(5) + 1
                                    Else
                                        FD(11) = FD(11) + 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    If ze(5) = 0 Then
                        Fblood(y).Text = Fblood(y).Text - 1
                    End If
                    ze(5) = 1
                    If y = i - 1 Then
                        ze(5) = 0
                    End If
                End If
            End If
        Next

    End Sub
#End Region
    Private Sub moneyTimer_Tick(sender As Object, e As EventArgs) Handles moneyTimer.Tick
        money(m) = New PictureBox
        money(m).Visible = True
        money(m).Size = New Size(40, 40)
        money(m).Name = "moneypic" & m.ToString
        moneyX = Int(Rnd() * 701 + 40) 'X座標40~740
        moneyY = Int(Rnd() * 331 + 140) 'Y座標140~470
        money(m).Location = New Point(moneyX, moneyY)
        money(m).BackColor = Color.Transparent
        money(m).Image = My.Resources.money
        money(m).SizeMode = PictureBoxSizeMode.StretchImage
        AddHandler money(m).Click, AddressOf moneypic_Click '新增圖片控制項
        Me.Controls.Add(money(m))
        money(m).BringToFront()
        moneyTimer.Interval = 1000 * moneytime
        moneydis.Enabled = True
        m = m + 1

    End Sub
    'Private Sub restart_Click(sender As Object, e As EventArgs) Handles restart.Click
    '    If MsgBox("是否要重新開始?", vbYesNo, "訊息") = 6 Then
    '        Dim frm1 = New Form1
    '        frm1.Show()
    '        Me.Close()
    '        'Application.Restart()
    '        'Form1_Load(sender, e)
    '        'Me.Refresh()
    '        'Me.OnLoad()
    '        'Exit Sub
    '    End If
    'End Sub
    Private Sub pause_Click(sender As Object, e As EventArgs) Handles pause.Click
        allstop()
        Pausepanel.Visible = True
        Pausepanel.BringToFront()
    End Sub

    Private Sub play_Click(sender As Object, e As EventArgs) Handles play.Click
        allstart()
        Pausepanel.Visible = False
    End Sub

    Private Sub home_Click(sender As Object, e As EventArgs) Handles home.click
        第一頁.Show()
        Me.Close()
    End Sub
#Region "音量"
    Private Sub volumedown_Click(sender As Object, e As EventArgs) Handles volumedown.Click
        If soundvolume > 0 Then
            soundvolume -= 10
            AxWindowsMediaPlayer1.settings.volume = soundvolume
        End If
    End Sub

    Private Sub volumeup_Click(sender As Object, e As EventArgs) Handles volumeup.Click
        If soundvolume < 100 Then
            soundvolume += 10
            AxWindowsMediaPlayer1.settings.volume = soundvolume
        End If
    End Sub

    Private Sub volumeoff_Click(sender As Object, e As EventArgs) Handles volumeoff.Click
        AxWindowsMediaPlayer1.Ctlcontrols.pause()
        volumeoff.Visible = False
        volumeon.Visible = True
    End Sub

    Private Sub volumeon_Click(sender As Object, e As EventArgs) Handles volumeon.Click
        AxWindowsMediaPlayer1.Ctlcontrols.play()
        volumeon.Visible = False
        volumeoff.Visible = True
    End Sub

#End Region
    Private Sub moneypic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        sender.visible = False
        moneyLB.Text = moneyLB.Text + 25 '一個錢25分

    End Sub

    Private Function allstop()
        pausetrue = 1
        If Timer3.Enabled = True Then
            t(0) = 1
            Timer3.Enabled = False
        Else
            t(0) = 0
        End If
        If Timer1.Enabled = True Then
            t(1) = 1
            Timer1.Enabled = False
        Else
            t(1) = 0
        End If
        If FDdie.Enabled = True Then
            t(2) = 1
            FDdie.Enabled = False
        Else
            t(2) = 0
        End If
        If forever.Enabled = True Then
            t(3) = 1
            forever.Enabled = False
        Else
            t(3) = 0
        End If
        If moneydis.Enabled = True Then
            t(4) = 1
            moneydis.Enabled = False
        Else
            t(4) = 0
        End If
        If moneyTimer.Enabled = True Then
            t(5) = 1
            moneyTimer.Enabled = False
        Else
            t(5) = 0
        End If
        If ZW1.Enabled = True Then
            t(6) = 1
            ZW1.Enabled = False
        Else
            t(6) = 0
        End If
        If ZW2.Enabled = True Then
            t(7) = 1
            ZW2.Enabled = False
        Else
            t(7) = 0
        End If
        If ZW3.Enabled = True Then
            t(8) = 1
            ZW3.Enabled = False
        Else
            t(8) = 0
        End If
        If ZW4.Enabled = True Then
            t(9) = 1
            ZW4.Enabled = False
        Else
            t(9) = 0
        End If
        If ZW5.Enabled = True Then
            t(10) = 1
            ZW5.Enabled = False
        Else
            t(10) = 0
        End If
        If ZW6.Enabled = True Then
            t(11) = 1
            ZW6.Enabled = False
        Else
            t(11) = 0
        End If
        If ZBappear.Enabled = True Then
            t(12) = 1
            ZBappear.Enabled = False
        Else
            t(12) = 0
        End If
        If ZD1.Enabled = True Then
            t(13) = 1
            ZD1.Enabled = False
        Else
            t(13) = 0
        End If
        If ZD2.Enabled = True Then
            t(14) = 1
            ZD2.Enabled = False
        Else
            t(14) = 0
        End If
        If ZD3.Enabled = True Then
            t(15) = 1
            ZD3.Enabled = False
        Else
            t(15) = 0
        End If
        If ZD4.Enabled = True Then
            t(16) = 1
            ZD4.Enabled = False
        Else
            t(16) = 0
        End If
        If ZD5.Enabled = True Then
            t(17) = 1
            ZD5.Enabled = False
        Else
            t(17) = 0
        End If
        If ZD6.Enabled = True Then
            t(18) = 1
            ZD6.Enabled = False
        Else
            t(18) = 0
        End If
        If Z1eat.Enabled = True Then
            t(19) = 1
            Z1eat.Enabled = False
        Else
            t(19) = 0
        End If
        If Z2eat.Enabled = True Then
            t(20) = 1
            Z2eat.Enabled = False
        Else
            t(20) = 0
        End If
        If Z3eat.Enabled = True Then
            t(21) = 1
            Z3eat.Enabled = False
        Else
            t(21) = 0
        End If
        If Z4eat.Enabled = True Then
            t(22) = 1
            Z4eat.Enabled = False
        Else
            t(22) = 0
        End If
        If Z5eat.Enabled = True Then
            t(23) = 1
            Z5eat.Enabled = False
        Else
            t(23) = 0
        End If
        If Z6eat.Enabled = True Then
            t(24) = 1
            Z6eat.Enabled = False
        Else
            t(24) = 0
        End If
    End Function

    Private Sub Z1_Tick(sender As Object, e As EventArgs) Handles Z1.Tick
        c(0) = c(0) + 1
        If c(0) > 9 Then c(0) = 1
        zombie(0).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇" & c(0) & ".png"))
    End Sub
    Private Sub Z2_Tick(sender As Object, e As EventArgs) Handles Z2.Tick
        c(1) = c(1) + 1
        If c(1) > 9 Then c(1) = 1
        zombie(1).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇" & c(1) & ".png"))
    End Sub
    Private Sub Z3_Tick(sender As Object, e As EventArgs) Handles Z3.Tick
        c(2) = c(2) + 1
        If c(2) > 9 Then c(2) = 1
        zombie(2).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇" & c(2) & ".png"))
    End Sub
    Private Sub Z4_Tick(sender As Object, e As EventArgs) Handles Z4.Tick
        c(3) = c(3) + 1
        If c(3) > 9 Then c(3) = 1
        zombie(3).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇" & c(3) & ".png"))
    End Sub
    Private Sub Z5_Tick(sender As Object, e As EventArgs) Handles Z5.Tick
        c(4) = c(4) + 1
        If c(4) > 9 Then c(4) = 1
        zombie(4).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇" & c(4) & ".png"))
    End Sub

    Private Sub Z6_Tick(sender As Object, e As EventArgs) Handles Z6.Tick
        c(5) = c(5) + 1
        If c(5) > 9 Then c(5) = 1
        zombie(5).Image = Image.FromFile((Application.StartupPath & "\殭屍\香菇" & c(5) & ".png"))
    End Sub

    Private Sub food1_Click(sender As Object, e As EventArgs) Handles food1.Click

    End Sub

    Public Function allstart()
        pausetrue = 0
        If t(0) = 1 Then
            Timer3.Enabled = True
        End If
        If t(1) = 1 Then
            Timer1.Enabled = True
        End If
        If t(2) = 1 Then
            FDdie.Enabled = True
        End If
        If t(3) = 1 Then
            forever.Enabled = True
        End If
        If t(4) = 1 Then
            moneydis.Enabled = True
        End If
        If t(5) = 1 Then
            moneyTimer.Enabled = True
        End If
        If t(6) = 1 Then
            ZW1.Enabled = True
        End If
        If t(7) = 1 Then
            ZW2.Enabled = True
        End If
        If t(8) = 1 Then
            ZW3.Enabled = True
        End If
        If t(9) = 1 Then
            ZW4.Enabled = True
        End If
        If t(10) = 1 Then
            ZW5.Enabled = True
        End If
        If t(11) = 1 Then
            ZW6.Enabled = True
        End If
        If t(12) = 1 Then
            ZBappear.Enabled = True
        End If
        If t(13) = 1 Then
            ZD1.Enabled = True
        End If
        If t(14) = 1 Then
            ZD2.Enabled = True
        End If
        If t(15) = 1 Then
            ZD3.Enabled = True
        End If
        If t(16) = 1 Then
            ZD4.Enabled = True
        End If
        If t(17) = 1 Then
            ZD5.Enabled = True
        End If
        If t(18) = 1 Then
            ZD6.Enabled = True
        End If
        If t(19) = 1 Then
            Z1eat.Enabled = True
        End If
        If t(20) = 1 Then
            Z2eat.Enabled = True
        End If
        If t(21) = 1 Then
            Z3eat.Enabled = True
        End If
        If t(22) = 1 Then
            Z4eat.Enabled = True
        End If
        If t(23) = 1 Then
            Z5eat.Enabled = True
        End If
        If t(24) = 1 Then
            Z6eat.Enabled = True
        End If

    End Function
    Private Sub rndQA()
        Dim QA_num As Integer
        QA_num = Int(Rnd() * 10) + 1

        Select Case QA_num
            Case = 1
                Q1.Show()
                Q1.StartPosition = StartPosition.Manual
                Q1.Left = Me.Left + 20
                Q1.Top = Me.Top + 45
                Q1.Label2.Text = "1"
            Case = 2
                Q2.Show()
                Q2.StartPosition = StartPosition.Manual
                Q2.Left = Me.Left + 20
                Q2.Top = Me.Top + 45
                Q2.Label1.Text = "1"
            Case = 3
                Q3.Show()
                Q3.StartPosition = StartPosition.Manual
                Q3.Left = Me.Left + 20
                Q3.Top = Me.Top + 45
                Q3.Label1.Text = "1"
            Case = 4
                Q4.Show()
                Q4.StartPosition = StartPosition.Manual
                Q4.Left = Me.Left + 20
                Q4.Top = Me.Top + 45
                Q4.Label1.Text = "1"
            Case = 5
                Q5.Show()
                Q5.StartPosition = StartPosition.Manual
                Q5.Left = Me.Left + 20
                Q5.Top = Me.Top + 45
                Q5.Label1.Text = "1"
            Case = 6
                Q6.Show()
                Q6.StartPosition = StartPosition.Manual
                Q6.Left = Me.Left + 20
                Q6.Top = Me.Top + 45
                Q6.Label1.Text = "1"
            Case = 7
                Q7.Show()
                Q7.StartPosition = StartPosition.Manual
                Q7.Left = Me.Left + 20
                Q7.Top = Me.Top + 45
                Q7.Label1.Text = "1"
            Case = 8
                Q8.Show()
                Q8.StartPosition = StartPosition.Manual
                Q8.Left = Me.Left + 20
                Q8.Top = Me.Top + 45
                Q8.Label1.Text = "1"
            Case = 9
                Q9.Show()
                Q9.StartPosition = StartPosition.Manual
                Q9.Left = Me.Left + 20
                Q9.Top = Me.Top + 45
                Q9.Label2.Text = "1"
            Case = 10
                Q10.Show()
                Q10.StartPosition = StartPosition.Manual
                Q10.Left = Me.Left + 20
                Q10.Top = Me.Top + 45
                Q10.Label2.Text = "1"
        End Select
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Q1.Close()
        Q2.Close()
        Q3.Close()
        Q4.Close()
        Q5.Close()
        Q6.Close()
        Q7.Close()
        Q8.Close()
        Q9.Close()
        Q10.Close()
    End Sub
End Class
