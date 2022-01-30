cd "C:\Root\Scripts\Projects\ExpenseTracker\src\ConsoleApp\bin\Debug\net6.0"

.\ConsoleApp.exe income ContaCorrente2 PixNB 10000
.\ConsoleApp.exe income Poupança1 PoupançaCaixa 10000

@echo off
set counter=0
:loop
if %counter% equ 15 goto :EOF
set /a counter+=1
.\ConsoleApp.exe expense Poupança1 PoupançaCaixa 250 --tags Eletronics --note "HD Externo" --date 26/09/2021
.\ConsoleApp.exe expense ContaCorrente1 PixBB 5 --tags Food --note Chocolate
.\ConsoleApp.exe expense ContaCorrente2 PixNB 99 --tags Medicine --note "Remédio para ansiedade"
.\ConsoleApp.exe income ContaCorrente1 PixBB 1500 --tags Bolsa --note "Bolsa Mestrado"
goto loop

