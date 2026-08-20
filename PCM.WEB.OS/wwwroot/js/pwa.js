// ============================================================
//  PCM.WEB.OS - PWA (registro do service worker, instalação e atualização)
//  Arquivo externo e sem estilos inline, para respeitar a CSP do projeto.
// ============================================================

(function () {
    'use strict';

    if (!('serviceWorker' in navigator)) return;

    var deferredPrompt = null;
    var refreshing = false;

    // Raiz da aplicação derivada do próprio src deste arquivo (.../js/pwa.js).
    // Assim o service worker funciona tanto na raiz do site quanto em um
    // diretório virtual, sem precisar fixar "/" no código.
    var thisScript = document.currentScript;
    var appRoot = thisScript
        ? thisScript.src.replace(/js\/pwa\.js(\?.*)?$/, '')
        : '/';

    // ---------- Utilitários de UI ----------
    function criarBanner(texto, textoBotao, onClick) {
        var banner = document.createElement('div');
        banner.className = 'pwa-banner';

        var span = document.createElement('span');
        span.className = 'pwa-banner-text';
        span.textContent = texto;

        var btn = document.createElement('button');
        btn.type = 'button';
        btn.className = 'pwa-banner-btn';
        btn.textContent = textoBotao;
        btn.addEventListener('click', function () {
            banner.remove();
            onClick();
        });

        var fechar = document.createElement('button');
        fechar.type = 'button';
        fechar.className = 'pwa-banner-close';
        fechar.setAttribute('aria-label', 'Fechar');
        fechar.textContent = '×';
        fechar.addEventListener('click', function () {
            banner.remove();
        });

        banner.appendChild(span);
        banner.appendChild(btn);
        banner.appendChild(fechar);
        document.body.appendChild(banner);

        return banner;
    }

    // ---------- Registro do service worker ----------
    window.addEventListener('load', function () {

        navigator.serviceWorker.register(appRoot + 'sw.js', { scope: appRoot }).then(function (reg) {

            // Já existe uma versão nova aguardando (aba anterior deixou pendente)
            if (reg.waiting && navigator.serviceWorker.controller) {
                avisarAtualizacao(reg.waiting);
            }

            // Nova versão detectada durante o uso
            reg.addEventListener('updatefound', function () {
                var novo = reg.installing;
                if (!novo) return;

                novo.addEventListener('statechange', function () {
                    // Só avisa se já havia um SW controlando (ou seja, é atualização, não 1ª instalação)
                    if (novo.state === 'installed' && navigator.serviceWorker.controller) {
                        avisarAtualizacao(novo);
                    }
                });
            });

        }).catch(function (err) {
            console.error('Falha ao registrar o service worker:', err);
        });

    });

    // A atualização NÃO é aplicada sozinha: durante um inventário um reload
    // inesperado descartaria o código que está sendo digitado.
    function avisarAtualizacao(worker) {
        criarBanner('Nova versão disponível.', 'Atualizar', function () {
            worker.postMessage('SKIP_WAITING');
        });
    }

    navigator.serviceWorker.addEventListener('controllerchange', function () {
        if (refreshing) return;
        refreshing = true;
        window.location.reload();
    });

    // ---------- Convite para instalar ----------
    window.addEventListener('beforeinstallprompt', function (e) {
        e.preventDefault();
        deferredPrompt = e;

        // Não insiste se o usuário já dispensou nesta sessão
        try {
            if (sessionStorage.getItem('pwaInstallDispensado') === '1') return;
        } catch (err) { /* modo privado: segue exibindo */ }

        criarBanner('Instale o app para usar em tela cheia.', 'Instalar', function () {
            if (!deferredPrompt) return;
            deferredPrompt.prompt();
            deferredPrompt.userChoice.finally(function () {
                deferredPrompt = null;
            });
        });

        try {
            sessionStorage.setItem('pwaInstallDispensado', '1');
        } catch (err) { /* ignora */ }
    });

})();
