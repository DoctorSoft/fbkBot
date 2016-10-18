;���������� ����������� ���������
!include "MUI.nsh"

;�������� ������ ������������ �� ������ �����
Name "��������� FacebookBot 1.1"

;��� ����� ������������
OutFile "FacebookBot 1.1.exe"

;���������� ���������
!define pkgdir "d:\Projects\Freelance\FacebookBot\FacebookBot\bin\Debug\"

;����� ���������
InstallDir "$PROGRAMFILES\FacebookBot"

;������������� �������� ����������� ����� ������, ����� SOLID - ��������� ��� ������ � ���� ���� (����������� ������� ������)
SetCompressor /SOLID lzma

;��������� ������������� ������������ ���������� -----------------------------------------------------------------------------------------
;�������� �� �������������� ��� �������� ������������
!define MUI_ABORTWARNING
;�������� ���������
Page directory
Page instfiles
;�������� ��������
UninstPage uninstConfirm
UninstPage instfiles
;������� ���� ������������
!insertmacro MUI_LANGUAGE "Russian"
;��������� ���� ������������
Caption "��������� FacebookBot 1.1"
;���������� �������� �������� � ���� ���� ����������
ShowInstDetails show
;-----------------------------------------------------------------------------------------------------------------------------------------

Section "Receipt"
	;����� ��� ���������� ������
	SetOutPath "$INSTDIR"
	;�����, ������� �������� ���������� � ��� �����
	File /r "${pkgdir}"
	
	;����� ������� ��� �������� ���������	
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "DisplayName" "FacebookBot"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "NoRepair" 1
	WriteUninstaller "uninstall.exe"

	CreateDirectory "$SMPROGRAMS\FacebookBot"
	CreateShortCut "$SMPROGRAMS\FacebookBot\������� FacebookBot.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
	CreateShortCut "$SMPROGRAMS\FacebookBot\FacebookBot 1.1.lnk" "$INSTDIR\FacebookBot.exe" "" "$INSTDIR\FacebookBot.exe" 0
	SetShellVarContext current
	CreateShortCut "$DESKTOP\FacebookBot 1.1.lnk" "$INSTDIR\FacebookBot.exe" "" "$INSTDIR\FacebookBot.exe" 0
	
SectionEnd

Section "Uninstall"
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot"
	Delete "$DESKTOP\FacebookBot 1.1.lnk"
	Delete "$SMPROGRAMS\FacebookBot\*.*"
	Delete "$DESKTOP\FacebookBot 1.1.lnk"
	RMDir "$SMPROGRAMS\FacebookBot"
	RMDir /r "$INSTDIR"
SectionEnd
