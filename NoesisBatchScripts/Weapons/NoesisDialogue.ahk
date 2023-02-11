#NoEnv
#SingleInstance,Force
DetectHiddenWindows, on

; Ensure mouse coords are always fetched via screen size 
; and not relative to the active window
CoordMode Mouse, Screen
CoordMode Pixel, Screen

; Send, {%key%}
 
F5::
	; Output the current mouse coordinates
	MouseGetPos, MouseX, MouseY
	PixelGetColor, DetectedColour, %MouseX%, %MouseY%, Slow RGB
	Log("Position: " MouseX ", " MouseY " --- Colour: " DetectedColour)
	return

F3::
	; reset log file
	LogReset()
	Log("Starting noesis window detection loop...")

	; Start the bot
	SetTimer, WindowLoop, 500
	return

F8::
	ExitApp
	Return

WindowLoop:
	; Logic loop
	Loop,
	{
		DetectNoesisWindow()
		Sleep, 200
	}

; --------------------------------------------------------------------------------
; CUSTOM FUNCTIONS
; --------------------------------------------------------------------------------

Log(text) {
	global

	FormatTime, CurrentDateTime,, dd-MM-yy HH:mm:ss
	FileAppend, [%CurrentDateTime%] %text% `n, ./log.txt
}

LogReset() {
	FileDelete, ./log.txt
}

DetectNoesisWindow() {
	AddSkeleton := true

	; Check if the open window is .. open
	if (WinExist("Open"))
	{
		WinActivate, Open
		Sleep, 300

		if (AddSkeleton) {
			SendInput {Raw}E:\SquareEnix\SquareEnix\FINAL FANTASY XI\ROM\27\82.DAT
			Sleep, 300
			Send {Enter}
			Log("Inserted rom file to the open window.")
		} else {
			Send {Esc}
			Log("Skipped the log file!")
		}

		Sleep, 800
	}
}