Public Class Form2
    Dim i As Integer
    Dim Fblood(19) As Label
    Dim FDtag(19) As Integer
    Dim FD(3) As Integer
    Dim bang(3) As Integer '殭屍有沒有撞到
    Dim nownum(19) As Integer '紀錄現在第幾個食物生成
    Dim k As Integer 'nownum的第幾格
    Dim yesno As Integer 'msgbox選項
    Dim j As Integer '可隨意用
    Dim x As Integer '第幾隻殭屍
    Dim m As Integer = 0 '第幾個money
    Dim time As Integer
    Dim heart As Integer = 3 '生命
    Dim die(3) As Integer '已死掉
    Dim number As Integer '第幾隻殭屍要死掉
    Dim zblood(3) As Integer '殭屍血量
    Dim zbloodLB(3) As Label '殭屍血量label
    Dim pic(19) As PictureBox '選擇放置位子的框框
    Dim zombie(3) As PictureBox '殭屍
    Dim bullet(19) As PictureBox '子彈
    Dim newfood(19) As PictureBox '食物砲塔
    Dim bulletTimer(19) As Timer '控制子彈的timer
    Dim foodchoose As Integer '選上方列表第幾個食物角色
    Dim money(100) As PictureBox '隨機生成的錢
    Dim moneytime As Integer '錢生成時間
    Dim moneyX, moneyY As Integer '錢的XY座標

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Randomize()
        i = 0
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
        For j = 0 To 3
            zblood(j) = 15
        Next

        For j = 0 To 3
            bang(j) = 0 '0=沒撞到
        Next
        For j = 0 To 3
            FD(j) = 0
        Next

        For j = 0 To 3
            zombie(j) = New PictureBox
            zombie(j).Size = New Size(70, 80)
            'zombie(j).BackColor = BackColor.Transparent
            zombie(j).Left = 770
            zombie(j).SizeMode = PictureBoxSizeMode.StretchImage
            zombie(j).Image = My.Resources.茄子
            zombie(j).BackColor = BackColor.DarkRed
            Me.Controls.Add(zombie(j))

            zbloodLB(j) = New Label
            zbloodLB(j).Left = 770
            zbloodLB(j).BackColor = BackColor.Transparent
            Me.Controls.Add(zbloodLB(j))
            zbloodLB(j).BringToFront()
            zombie(j).BringToFront()
            zombie(j).Tag = 0
            If j = 0 Then
                zombie(j).Top = 141
                zbloodLB(j).Top = 123
            ElseIf j = 1 Then
                zombie(j).Top = 250
                zbloodLB(j).Top = 235
            ElseIf j = 2 Then
                zombie(j).Top = 357
                zbloodLB(j).Top = 342
            ElseIf j = 3 Then
                zombie(j).Top = 472
                zbloodLB(j).Top = 457
            End If
            zbloodLB(j).Text = zblood(j)
        Next

        moneytime = Int(Rnd() * 6 + 3) '3~8秒
        moneyTimer.Interval = 1000 * moneytime
        moneyTimer.Enabled = True

        moneyLB.Text = 50
        heartLB.Text = heart
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
            foodchoose = 1
        ElseIf Target.Equals(food1) Then
            foodchoose = 2
            moneyLB.Text -= 50
        ElseIf Target.Equals(food2) Then
            moneyLB.Text -= 100
            foodchoose = 3
        End If

        'tag>4 表示且位子有放食物不能選
        For j = 0 To 19
            If pic(j).Tag Mod 10 <= 5 Then
                pic(j).BorderStyle = BorderStyle.FixedSingle
                pic(j).Visible = True
                pic(j).Enabled = True
            End If
        Next


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

        If foodchoose = 1 Then
            newfood(i).Image = My.Resources.漢堡
        ElseIf foodchoose = 2 Then
            newfood(i).Image = My.Resources.薯條
        ElseIf foodchoose = 3 Then
            newfood(i).Image = My.Resources.可樂
        End If

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
        bullet(i).SizeMode = PictureBoxSizeMode.AutoSize
        bullet(i).Left = newfood(i).Left + newfood(i).Width
        bullet(i).Top = newfood(i).Top + 30
        bullet(i).Image = My.Resources.bullet_1
        bullet(i).BringToFront()
        Me.Controls.Add(bullet(i))
        bullet(i).Tag = newfood(i).Tag
        bullet(i).BackColor = Color.Transparent
        Fblood(i) = New Label
        Me.Controls.Add(Fblood(i))
        Fblood(i).Text = 6
#Region "食物and子彈的tag"
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

        '一放食物砲塔之後子彈和殭屍同時開始
        Timer1.Enabled = True

    End Sub
    Function PZ(A As PictureBox, B As PictureBox) '碰撞模型
        Dim r As Boolean
        r = False
        If (A.Left + A.Width > B.Left + 30 And A.Left < B.Left + B.Width) _
           And (A.Top + A.Height > B.Top And A.Top < B.Top + B.Height) Then

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
            If (die(0) = 0 And newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 11 And zombie(0).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 12 And zombie(0).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 13 And zombie(0).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 14 And zombie(0).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(0).Left And newfood(nownum(k)).Tag = 15 And zombie(0).Tag < 5) Then
                zombie(0).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(0).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zombie(0).Image = My.Resources.茄子吃
                FDtag(FD(0)) = newfood(nownum(k)).Tag
                Z1eat.Enabled = True
                bang(0) = 1 '殭屍撞到
            ElseIf bang(0) = 0 And zombie(0).Left > 0 Then '沒撞過的就是<>7
                If zombie(0).Left < pic(4).Left + pic(4).Width + 30 And pic(4).Tag = 11 And zombie(0).Tag < 1 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(3).Left + pic(3).Width + 30 And pic(3).Tag = 12 And zombie(0).Tag < 2 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(2).Left + pic(2).Width + 30 And pic(2).Tag = 13 And zombie(0).Tag < 3 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(1).Left + pic(1).Width + 30 And pic(1).Tag = 14 And zombie(0).Tag < 4 Then
                    zombie(0).Tag += 1
                End If
                If zombie(0).Left < pic(0).Left + pic(0).Width + 30 And pic(0).Tag = 15 And zombie(0).Tag < 5 Then
                    zombie(0).Tag += 1
                End If
                zombie(0).Left = zombie(0).Left - 3
                zbloodLB(0).Left = zbloodLB(0).Left - 3

                If zombie(0).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    heartLB.Text = heart
                    zombie(0).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                        If yesno = 6 Then
                            Application.Restart()
                        ElseIf yesno = 7 Then
                            Me.Close()
                        End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(0).Left > 0 Then '解決k第一次不能跑的問題
            zombie(0).Left = zombie(0).Left - 30
            zbloodLB(0).Left = zbloodLB(0).Left - 30
            If zombie(0).Left < pic(4).Left + pic(4).Width + 30 And pic(4).Tag = 11 And zombie(0).Tag < 1 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(3).Left + pic(3).Width + 30 And pic(3).Tag = 12 And zombie(0).Tag < 2 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(2).Left + pic(2).Width + 30 And pic(2).Tag = 13 And zombie(0).Tag < 3 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(1).Left + pic(1).Width + 30 And pic(1).Tag = 14 And zombie(0).Tag < 4 Then
                zombie(0).Tag += 1
            End If
            If zombie(0).Left < pic(0).Left + pic(0).Width + 30 And pic(0).Tag = 15 And zombie(0).Tag < 5 Then
                zombie(0).Tag += 1
            End If
            zombie(0).Left = zombie(0).Left - 3
            zbloodLB(0).Left = zbloodLB(0).Left - 3

            If zombie(0).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                heartLB.Text = heart
                zombie(0).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                    If yesno = 6 Then
                        Application.Restart()
                    ElseIf yesno = 7 Then
                        Me.Close()
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW2_Tick(sender As Object, e As EventArgs) Handles ZW2.Tick

        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 21 And zombie(1).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 22 And zombie(1).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 23 And zombie(1).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 24 And zombie(0).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(1).Left And newfood(nownum(k)).Tag = 25 And zombie(0).Tag < 5) Then
                zombie(1).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(1).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zombie(1).Image = My.Resources.茄子吃
                FDtag(FD(1)) = newfood(nownum(k)).Tag
                Z2eat.Enabled = True
                bang(1) = 1 '殭屍撞到
            ElseIf bang(1) = 0 And zombie(1).Left > 0 Then '沒撞過的就是<>7
                If zombie(1).Left < pic(9).Left + pic(9).Width + 30 And pic(9).Tag = 21 And zombie(1).Tag < 1 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(8).Left + pic(8).Width + 30 And pic(8).Tag = 22 And zombie(1).Tag < 2 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(7).Left + pic(7).Width + 30 And pic(7).Tag = 23 And zombie(1).Tag < 3 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(6).Left + pic(6).Width + 30 And pic(6).Tag = 24 And zombie(1).Tag < 4 Then
                    zombie(1).Tag += 1
                End If
                If zombie(1).Left < pic(5).Left + pic(5).Width + 30 And pic(5).Tag = 25 And zombie(1).Tag < 5 Then
                    zombie(1).Tag += 1
                End If
                zombie(1).Left = zombie(1).Left - 3
                zbloodLB(1).Left = zbloodLB(1).Left - 3

                If zombie(1).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    heartLB.Text = heart
                    zombie(1).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                        If yesno = 6 Then
                            Application.Restart()
                        ElseIf yesno = 7 Then
                            Me.Close()
                        End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(1).Left > 0 Then '解決k第一次不能跑的問題
            zombie(1).Left = zombie(1).Left - 30
            zbloodLB(1).Left = zbloodLB(1).Left - 30
            If zombie(1).Left < pic(9).Left + pic(9).Width + 30 And pic(9).Tag = 21 And zombie(1).Tag < 1 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(8).Left + pic(8).Width + 30 And pic(8).Tag = 22 And zombie(1).Tag < 2 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(7).Left + pic(7).Width + 30 And pic(7).Tag = 23 And zombie(1).Tag < 3 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(6).Left + pic(6).Width + 30 And pic(6).Tag = 24 And zombie(1).Tag < 4 Then
                zombie(1).Tag += 1
            End If
            If zombie(1).Left < pic(5).Left + pic(5).Width + 30 And pic(5).Tag = 25 And zombie(1).Tag < 5 Then
                zombie(1).Tag += 1
            End If
            zombie(1).Left = zombie(1).Left - 3
            zbloodLB(1).Left = zbloodLB(1).Left - 3

            If zombie(1).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                heartLB.Text = heart
                zombie(1).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                    If yesno = 6 Then
                        Application.Restart()
                    ElseIf yesno = 7 Then
                        Me.Close()
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW3_Tick(sender As Object, e As EventArgs) Handles ZW3.Tick
        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 31 And zombie(2).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 32 And zombie(2).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 33 And zombie(2).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 34 And zombie(2).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(2).Left And newfood(nownum(k)).Tag = 35 And zombie(2).Tag < 5) Then
                zombie(2).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(2).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zombie(2).Image = My.Resources.茄子吃
                FDtag(FD(2)) = newfood(nownum(k)).Tag
                Z3eat.Enabled = True
                bang(2) = 1 '殭屍撞到
            ElseIf bang(2) = 0 And zombie(2).Left > 0 Then '沒撞過的就是<>7
                If zombie(2).Left < pic(14).Left + pic(14).Width + 30 And pic(14).Tag = 31 And zombie(2).Tag < 1 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(3).Left + pic(13).Width + 30 And pic(13).Tag = 32 And zombie(2).Tag < 2 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(2).Left + pic(12).Width + 30 And pic(12).Tag = 33 And zombie(2).Tag < 3 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(1).Left + pic(11).Width + 30 And pic(11).Tag = 34 And zombie(2).Tag < 4 Then
                    zombie(2).Tag += 1
                End If
                If zombie(2).Left < pic(0).Left + pic(10).Width + 30 And pic(10).Tag = 35 And zombie(2).Tag < 5 Then
                    zombie(2).Tag += 1
                End If
                zombie(2).Left = zombie(2).Left - 3
                zbloodLB(2).Left = zbloodLB(2).Left - 3

                If zombie(2).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    heartLB.Text = heart
                    zombie(2).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                        If yesno = 6 Then
                            Application.Restart()
                        ElseIf yesno = 7 Then
                            Me.Close()
                        End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(2).Left > 0 Then '解決k第一次不能跑的問題
            zombie(2).Left = zombie(2).Left - 30
            zbloodLB(2).Left = zbloodLB(2).Left - 30
            If zombie(2).Left < pic(4).Left + pic(14).Width + 30 And pic(4).Tag = 31 And zombie(2).Tag < 1 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(3).Left + pic(13).Width + 30 And pic(3).Tag = 32 And zombie(2).Tag < 2 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(2).Left + pic(12).Width + 30 And pic(2).Tag = 33 And zombie(2).Tag < 3 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(1).Left + pic(11).Width + 30 And pic(1).Tag = 34 And zombie(2).Tag < 4 Then
                zombie(2).Tag += 1
            End If
            If zombie(2).Left < pic(0).Left + pic(10).Width + 30 And pic(0).Tag = 35 And zombie(2).Tag < 5 Then
                zombie(2).Tag += 1
            End If
            zombie(2).Left = zombie(2).Left - 3
            zbloodLB(2).Left = zbloodLB(2).Left - 3

            If zombie(2).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                heartLB.Text = heart
                zombie(2).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                    If yesno = 6 Then
                        Application.Restart()
                    ElseIf yesno = 7 Then
                        Me.Close()
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub ZW4_Tick(sender As Object, e As EventArgs) Handles ZW4.Tick
        For k = 0 To i - 1 ' Step -1
            If (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 41 And zombie(3).Tag < 1) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 42 And zombie(3).Tag < 2) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 43 And zombie(3).Tag < 3) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 44 And zombie(3).Tag < 4) Or (newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30 > zombie(3).Left And newfood(nownum(k)).Tag = 45 And zombie(3).Tag < 5) Then
                zombie(3).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zbloodLB(3).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width + 30
                zombie(3).Image = My.Resources.茄子吃
                FDtag(FD(3)) = newfood(nownum(k)).Tag
                Z4eat.Enabled = True
                bang(3) = 1 '殭屍撞到
            ElseIf bang(3) = 0 And zombie(3).Left > 0 Then '沒撞過的就是<>7
                If zombie(3).Left < pic(19).Left + pic(19).Width + 30 And pic(19).Tag = 41 And zombie(3).Tag < 1 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(18).Left + pic(18).Width + 30 And pic(18).Tag = 42 And zombie(3).Tag < 2 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(17).Left + pic(17).Width + 30 And pic(17).Tag = 43 And zombie(3).Tag < 3 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(16).Left + pic(16).Width + 30 And pic(16).Tag = 44 And zombie(3).Tag < 4 Then
                    zombie(3).Tag += 1
                End If
                If zombie(3).Left < pic(15).Left + pic(15).Width + 30 And pic(15).Tag = 45 And zombie(3).Tag < 5 Then
                    zombie(3).Tag += 1
                End If
                zombie(3).Left = zombie(3).Left - 3
                zbloodLB(3).Left = zbloodLB(3).Left - 3

                If zombie(3).Left <= 0 Then '碰邊界減一命
                    heart = heart - 1
                    heartLB.Text = heart
                    zombie(3).Size = New Size(0, 0)
                    If heart = 0 Then
                        yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                        If yesno = 6 Then
                            Application.Restart()
                        ElseIf yesno = 7 Then
                            Me.Close()
                        End If
                    End If
                End If
            End If
        Next
        If i = 0 And zombie(3).Left > 0 Then '解決k第一次不能跑的問題
            If zombie(3).Left < pic(19).Left + pic(19).Width + 30 And pic(19).Tag = 41 And zombie(3).Tag < 1 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(18).Left + pic(18).Width + 30 And pic(18).Tag = 42 And zombie(3).Tag < 2 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(17).Left + pic(17).Width + 30 And pic(17).Tag = 43 And zombie(3).Tag < 3 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(16).Left + pic(16).Width + 30 And pic(16).Tag = 44 And zombie(3).Tag < 4 Then
                zombie(3).Tag += 1
            End If
            If zombie(3).Left < pic(15).Left + pic(15).Width + 30 And pic(15).Tag = 45 And zombie(3).Tag < 5 Then
                zombie(3).Tag += 1
            End If
            zombie(3).Left = zombie(3).Left - 3
            zbloodLB(3).Left = zbloodLB(3).Left - 3


            If zombie(3).Left <= 0 Then '碰邊界減一命
                heart = heart - 1
                heartLB.Text = heart
                zombie(3).Size = New Size(0, 0)
                If heart = 0 Then
                    yesno = MsgBox("你輸了!要重新開始嗎", vbYesNo, "訊息")
                    If yesno = 6 Then
                        Application.Restart()
                    ElseIf yesno = 7 Then
                        Me.Close()
                    End If
                End If
            End If
        End If

    End Sub
#End Region
#Region "確認子彈和殭屍的碰撞"
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        For k = 0 To i - 1
            For x = 0 To 3
                ' If zombie(x).Tag <= 10 Then
                If PZ(bullet(nownum(k)), zombie(x)) Then
                    bullet(nownum(k)).Left = newfood(nownum(k)).Left + newfood(nownum(k)).Width
                    'Timer4.Enabled = True
                    If x = 0 Then
                        If zblood(0) <> 0 Then
                            If (zombie(0).Tag <> 1 And bullet(nownum(k)).Tag <> 11) Or (zombie(0).Tag <> 2 And bullet(nownum(k)).Tag <> 12) Or (zombie(0).Tag <> 3 And bullet(nownum(k)).Tag <> 13) Or (zombie(0).Tag <> 4 And bullet(nownum(k)).Tag <> 14) Or (zombie(0).Tag <> 5 And bullet(nownum(k)).Tag <> 15) Then
                                zblood(0) = zblood(0) - 1 '減血
                                zbloodLB(0).Text = zblood(0)
                            End If
                        ElseIf zblood(0) = 0 And die(0) <> 1 Then
                            zombie(0).Image = My.Resources.茄子死 '死掉的樣子
                            Z1eat.Enabled = False
                            die(0) = 1
                            ZD1.Enabled = True '等1秒消失
                        End If

                    ElseIf x = 1 Then
                        If zblood(1) <> 0 Then
                            If (zombie(1).Tag <> 1 And bullet(nownum(k)).Tag <> 21) Or (zombie(1).Tag <> 2 And bullet(nownum(k)).Tag <> 22) Or (zombie(1).Tag <> 3 And bullet(nownum(k)).Tag <> 23) Or (zombie(1).Tag <> 4 And bullet(nownum(k)).Tag <> 24) Or (zombie(1).Tag <> 5 And bullet(nownum(k)).Tag <> 25) Then
                                zblood(1) = zblood(1) - 1
                                zbloodLB(1).Text = zblood(1)
                            End If
                        ElseIf zblood(1) = 0 And die(1) <> 1 Then
                            zombie(1).Image = My.Resources.茄子死
                            Z2eat.Enabled = False
                            die(1) = 1
                            ZD2.Enabled = True
                        End If

                    ElseIf x = 2 Then
                        If zblood(2) <> 0 Then
                            If (zombie(2).Tag <> 1 And bullet(nownum(k)).Tag <> 31) Or (zombie(2).Tag <> 2 And bullet(nownum(k)).Tag <> 32) Or (zombie(2).Tag <> 3 And bullet(nownum(k)).Tag <> 33) Or (zombie(2).Tag <> 4 And bullet(nownum(k)).Tag <> 34) Or (zombie(2).Tag <> 5 And bullet(nownum(k)).Tag <> 35) Then
                                zblood(2) = zblood(2) - 1
                                zbloodLB(2).Text = zblood(2)
                            End If
                        ElseIf zblood(2) = 0 And die(2) <> 1 Then
                            zombie(2).Image = My.Resources.茄子死
                            Z3eat.Enabled = False
                            die(2) = 1
                            ZD3.Enabled = True
                        End If

                    ElseIf x = 3 Then

                        If zblood(3) <> 0 Then
                            If (zombie(3).Tag <> 1 And bullet(nownum(k)).Tag <> 41) Or (zombie(3).Tag <> 2 And bullet(nownum(k)).Tag <> 42) Or (zombie(3).Tag <> 3 And bullet(nownum(k)).Tag <> 43) Or (zombie(3).Tag <> 4 And bullet(nownum(k)).Tag <> 44) Or (zombie(3).Tag <> 5 And bullet(nownum(k)).Tag <> 45) Then
                                zblood(3) = zblood(3) - 1
                                zbloodLB(3).Text = zblood(3)
                            End If
                        ElseIf zblood(3) = 0 And die(3) <> 1 Then
                            zombie(3).Image = My.Resources.茄子死
                            Z4eat.Enabled = False
                            die(3) = 1
                            ZD4.Enabled = True
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
        zombie(0).Size = New Size(0, 0)
        zbloodLB(0).Size = New Size(0, 0)
        ZD1.Enabled = False
        Z1eat.Enabled = False
    End Sub
    Private Sub ZD2_Tick(sender As Object, e As EventArgs) Handles ZD2.Tick '第二隻殭屍消失
        ZW2.Enabled = False
        zombie(1).Size = New Size(0, 0)
        zbloodLB(1).Size = New Size(0, 0)
        ZD2.Enabled = False
        Z2eat.Enabled = False
    End Sub
    Private Sub ZD3_Tick(sender As Object, e As EventArgs) Handles ZD3.Tick '第三隻殭屍消失
        zombie(2).Size = New Size(0, 0)
        zbloodLB(2).Size = New Size(0, 0)
        ZD3.Enabled = False
        Z3eat.Enabled = False
        ZW3.Enabled = False
    End Sub
    Private Sub ZD4_Tick(sender As Object, e As EventArgs) Handles ZD4.Tick '第四隻殭屍消失
        zombie(3).Size = New Size(0, 0)
        zbloodLB(3).Size = New Size(0, 0)
        ZD4.Enabled = False
        Z4eat.Enabled = False
        ZW4.Enabled = False
    End Sub
#End Region
    Private Sub ZBappear_Tick(sender As Object, e As EventArgs) Handles ZBappear.Tick
        time = time + 1
        If time = 1 Then
            ZW1.Enabled = True
        End If
        If time = 3 Then
            ZW2.Enabled = True
        End If
        If time = 5 Then
            ZW3.Enabled = True
        End If
        If time = 7 Then
            ZW4.Enabled = True
        End If

    End Sub

    Private Sub moneydis_Tick(sender As Object, e As EventArgs) Handles moneydis.Tick
        money(m - 1).Visible = False
        moneydis.Enabled = False
    End Sub

    Private Sub forever_Tick(sender As Object, e As EventArgs) Handles forever.Tick
        Label2.Text = zombie(0).Tag
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
        'If zblood(0) = 0 And die(0) <> 1 Then
        '    zombie(0).Image = My.Resources.茄子死 '死掉的樣子
        '    Z1eat.Enabled = FalseFD
        '    die(0) = 1
        '    ZD1.Enabled = True '等1秒消失
        'End If
    End Sub
#Region "食物血量的Timer"
    Private Sub Z1eat_Tick(sender As Object, e As EventArgs) Handles Z1eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(0)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(0)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            'bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = True
                            zombie(0).Image = My.Resources.茄子
                            bang(0) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    FD(0) = FD(0) + 1
                                End If
                            Next
                        End If
                    Next
                Else
                    Fblood(y).Text = Fblood(y).Text - 1
                End If
            End If
        Next

    End Sub
    Private Sub Z2eat_Tick(sender As Object, e As EventArgs) Handles Z2eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(1)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(1)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = False
                            zombie(1).Image = My.Resources.茄子
                            bang(1) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    FD(1) = FD(1) + 1
                                End If
                            Next
                        End If
                    Next
                Else
                    Fblood(y).Text = Fblood(y).Text - 1
                End If
            End If
        Next

    End Sub
    Private Sub Z3eat_Tick(sender As Object, e As EventArgs) Handles Z3eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(2)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(2)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = False
                            zombie(2).Image = My.Resources.茄子
                            bang(2) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    FD(2) = FD(2) + 1
                                End If
                            Next
                        End If
                    Next
                Else
                    Fblood(y).Text = Fblood(y).Text - 1
                End If
            End If
        Next

    End Sub
    Private Sub Z4eat_Tick(sender As Object, e As EventArgs) Handles Z4eat.Tick
        For y = 0 To i - 1
            If Fblood(y).Tag = FDtag(FD(3)) Then
                If Fblood(y).Text = 0 Then
                    For j = 0 To i - 1
                        If newfood(nownum(j)).Tag = FDtag(FD(3)) Then
                            newfood(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Size = New Size(0, 0)
                            bullet(nownum(j)).Location = New Point(780, 1000)
                            bullet(nownum(j)).Visible = False
                            zombie(3).Image = My.Resources.茄子
                            bang(3) = 0
                            For a = 0 To 19
                                If pic(a).Tag = newfood(nownum(j)).Tag + 5 Then
                                    pic(a).Tag = pic(a).Tag - 5
                                    FD(3) = FD(3) + 1
                                End If
                            Next
                        End If
                    Next
                Else
                    Fblood(y).Text = Fblood(y).Text - 1
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
        moneyX = Int(Rnd() * 731 + 40) 'X座標40~770
        moneyY = Int(Rnd() * 331 + 140) 'Y座標140~470
        money(m).Location = New Point(moneyX, moneyY)
        money(m).Image = My.Resources.money
        money(m).SizeMode = PictureBoxSizeMode.StretchImage
        AddHandler money(m).Click, AddressOf moneypic_Click '新增圖片控制項
        Me.Controls.Add(money(m))
        money(m).BringToFront()
        moneyTimer.Interval = 1000 * moneytime
        moneydis.Enabled = True
        m = m + 1


    End Sub

    Private Sub FDdie_Tick(sender As Object, e As EventArgs) Handles FDdie.Tick

    End Sub

    Private Sub moneypic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        sender.visible = False
        moneyLB.Text = moneyLB.Text + 25 '一個錢25分

    End Sub
    'Function FD(A As Label, B As Timer)
    '    B.Enabled = True
    '    A.Text = A.Text - 1
    'End Function
End Class
