Public Enum EnumDESKTOP
    DESKTOP_CREATEMENU = &H4                                           'Required to create a menu on the desktop.
    DESKTOP_CREATEWINDOW = &H2                                         'Required to create a window on the desktop.
    DESKTOP_ENUMERATE = &H40                                           'Required for the desktop to be enumerated.
    DESKTOP_HOOKCONTROL = &H8                                          'Required to establish any of the window hooks.
    DESKTOP_JOURNALPLAYBACK = &H20                                     'Required to perform journal playback on a desktop.
    DESKTOP_JOURNALRECORD = &H10                                       'Required to perform journal recording on a desktop.
    DESKTOP_READOBJECTS = &H1                                          'Required to read objects on the desktop.
    DESKTOP_SWITCHDESKTOP = &H100                                      'Required to activate the desktop using the SwitchDesktop function.
    DESKTOP_WRITEOBJECTS = &H80                                        'Required to write objects on the desktop.
    STANDARD_RIGHTS_REQUIRED = &HF0000
    STANDARD_RIGHTS_WRITE = &H20000
    STANDARD_RIGHTS_ALL = &H1F0000
    READ_CONTROL = &H20000
    'STANDARD_RIGHTS_EXECUTE = (READ_CONTROL)
    'STANDARD_RIGHTS_READ = (READ_CONTROL)
    GENERIC_READ = DESKTOP_ENUMERATE Or DESKTOP_READOBJECTS Or READ_CONTROL 'STANDARD_RIGHTS_READ

    GENERIC_EXECUTE = DESKTOP_SWITCHDESKTOP Or READ_CONTROL 'STANDARD_RIGHTS_EXECUTE

    GENERIC_WRITE = DESKTOP_CREATEMENU Or DESKTOP_CREATEWINDOW Or DESKTOP_HOOKCONTROL Or
        DESKTOP_JOURNALPLAYBACK Or DESKTOP_JOURNALRECORD Or DESKTOP_WRITEOBJECTS Or STANDARD_RIGHTS_WRITE

    GENERIC_ALL = DESKTOP_CREATEMENU Or DESKTOP_CREATEWINDOW Or DESKTOP_ENUMERATE Or DESKTOP_HOOKCONTROL Or DESKTOP_JOURNALPLAYBACK Or
        DESKTOP_JOURNALRECORD Or DESKTOP_READOBJECTS Or DESKTOP_SWITCHDESKTOP Or DESKTOP_WRITEOBJECTS Or STANDARD_RIGHTS_REQUIRED
End Enum

Public Structure STARTUPINFO
    Public cb As Integer
    Public lpReserved As String
    Public lpDesktop As String
    Public lpTitle As Integer
    Public dwX As Integer
    Public dwY As Integer
    Public dwXSize As Integer
    Public dwYSize As Integer
    Public dwXCountChars As Integer
    Public dwYCountChars As Integer
    Public dwFillAttribute As Integer
    Public dwFlags As Integer
    Public wShowWindow As Integer
    Public cbReserved2 As Integer
    Public lpReserved2 As Byte
    Public hStdInput As IntPtr
    Public hStdOutput As IntPtr
    Public hStdError As IntPtr
End Structure

Public Structure PROCESS_INFORMATION
    Public hProcess As IntPtr
    Public hThread As IntPtr
    Public dwProcessId As Integer
    Public dwThreadId As Integer
End Structure

Public Enum EnumUOI
    UOI_FLAGS = 1
    UOI_HEAPSIZE = 5
    UOI_IO = 6
    UOI_NAME = 2
    UOI_TYPE = 3
    UOI_USER_SID = 4
End Enum
