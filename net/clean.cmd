del /s *.vcxproj.filters
del /s *.vcxproj.user
for /f "usebackq" %%A in (`dir /b /s /AD ^| findstr /e \bin`) do rd /s /q %%A
for /f "usebackq" %%A in (`dir /b /s /AD ^| findstr /e \obj`) do rd /s /q %%A
for /f "usebackq" %%A in (`dir /b /s /AD ^| findstr /e \x64`) do rd /s /q %%A
for /f "usebackq" %%A in (`dir /b /s /AD ^| findstr /e \.ionide`) do rd /s /q %%A
for /f "usebackq" %%A in (`dir /b /s /AD ^| findstr /e \.vs`) do rd /s /q %%A

pause