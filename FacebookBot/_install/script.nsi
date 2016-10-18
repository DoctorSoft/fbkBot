;Подключаем современный интерфейс
!include "MUI.nsh"

;Название кнопки инсталлятора на панели задач
Name "Установка FacebookBot 1.1"

;Имя файла инсталлятора
OutFile "FacebookBot 1.1.exe"

;Определяем константы
!define pkgdir "d:\Projects\Freelance\FacebookBot\FacebookBot\bin\Debug\"

;Папка установки
InstallDir "$PROGRAMFILES\FacebookBot"

;Устанавливаем наиболее оптимальный метод сжатия, опция SOLID - поместить все данные в один блок (увеличивает степень сжатия)
SetCompressor /SOLID lzma

;Настройка подключенного современного интерфейса -----------------------------------------------------------------------------------------
;Выдавать ли предупреждение при закрытии инсталлятора
!define MUI_ABORTWARNING
;Страницы установки
Page directory
Page instfiles
;Страницы удаления
UninstPage uninstConfirm
UninstPage instfiles
;Русский язык инсталлятора
!insertmacro MUI_LANGUAGE "Russian"
;Заголовок окна инсталлятора
Caption "Установка FacebookBot 1.1"
;Показывать протокол действий в окне хода выполнения
ShowInstDetails show
;-----------------------------------------------------------------------------------------------------------------------------------------

Section "Receipt"
	;Папка для распаковки файлов
	SetOutPath "$INSTDIR"
	;Файлы, которые подлежат распаковке в эту папку
	File /r "${pkgdir}"
	
	;Ключи реестра для удаления программы	
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "DisplayName" "FacebookBot"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FacebookBot" "NoRepair" 1
	WriteUninstaller "uninstall.exe"

	CreateDirectory "$SMPROGRAMS\FacebookBot"
	CreateShortCut "$SMPROGRAMS\FacebookBot\Удалить FacebookBot.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
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
