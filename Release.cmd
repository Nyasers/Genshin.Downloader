@cd "%~dp0"&&(rmdir/s/q Release&(dotnet publish -o Release --no-self-contained||(pause&exit))&start "" 7zG a -sdel -ms -mx9 -mmt Release/Genshin.Downloader.7z ./Release/*)