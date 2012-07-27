SET WG="C:\C:\Program Files\GnuWin32\bin"
%WG%\wget http://www.datel-japan.co.jp/pspar/codelists/japan/pspar_codes1.bin
APP\pspar.exe pspar_codes1.bin ar.db
del *.bin