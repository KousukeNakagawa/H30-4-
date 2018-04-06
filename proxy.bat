@echo off
echo "0:プロキシー解除 1:プロキシーセット"
set /P I="入力:"
if %I% == 0 (
setx HTTP_Proxy ""
setx HTTPS_Proxy ""
) else if %I% == 1 (
setx HTTP_Proxy "cache9.hac.neec.ac.jp:8080"
setx HTTPS_Proxy "cache9.hac.neec.ac.jp:8080"
)
pause