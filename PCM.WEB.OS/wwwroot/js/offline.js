// PCM.WEB.OS - página offline: botão de nova tentativa (sem script inline por causa da CSP)
document.getElementById('btnTentarNovamente').addEventListener('click', function () {
    window.location.reload();
});
