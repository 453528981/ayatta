@echo off
@for /r . %%a in (.) do @if exist "%%a\obj" rmdir /s /q "%%a\obj"
@for /r . %%a in (.) do @if exist "%%a\bin" rmdir /s /q "%%a\bin"
rd /q /s .vs
rd /q /s .git
del /a /f /s /q *.user